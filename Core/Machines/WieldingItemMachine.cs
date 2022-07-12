using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using ChatterReborn.Utils.Machine;
using ChatterReborn.WieldingItemStates;
using GameData;
using Gear;
using Player;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Machines
{
    public class WieldingItemMachine : StateMachine<WI_Base>
    {

        public void Setup(LocalPlayerAgent owner)
        {
            this.m_owner = owner;
            this.SetupEnum<WI_State>();
            this.AddState(WI_State.Deciding, new WI_Deciding());
            this.AddState(WI_State.EnemyScanning, new WI_EnemyScanning());
            this.AddState(WI_State.EnemyScanResults, new WI_EnemyScanResults());                            
            this.StartState = this.GetState(WI_State.Deciding);
            this.SetupLocalInventory();
        }


        private CallBackUtils.CallBack _friendlyFireCallback;
        private void FriendlyFireComment()
        {
            if (ConfigurationManager.FriendlyFireApologyEnabled)
            {
                this.Owner.WantToStartDialog(GD.PlayerDialog.CL_Sorry, true);                
            }
            _friendlyFireCallback = null;
        }

        private void SetupLocalInventory()
        {
            this.m_inventory = this.Owner.Inventory.TryCast<PlayerInventoryLocal>();
            m_bulletWeaponDialogCooldown = 30f;
        }

        private LocalPlayerAgent m_owner;

        private PlayerInventoryLocal m_inventory;

        public LocalPlayerAgent Owner => m_owner;

        public bool GetWieldingItem<T>(out T wieldItem) where T : ItemEquippable
        {
            wieldItem = m_currentItemWielded?.TryCast<T>();
            return wieldItem != null;
        }

        private bool IsAnyOneHoldingAmmo
        {
            get
            {
                foreach (var player in ExtendedPlayerManager.AllPlayersInLevel)
                {
                    var backPack = PlayerBackpackManager.GetBackpack(player.Owner);

                    if (backPack != null)
                    {
                        List<BackpackItem> backpackItems = backPack.BackpackItems.ToSystemList();
                        foreach (var backpackItem in backpackItems)
                        {
                            if (backpackItem.Instance != null)
                            {
                                var resourcePack = backpackItem.Instance.TryCast<ResourcePackPickup>();
                                if (resourcePack != null && resourcePack.m_packType == eResourceContainerSpawnType.AmmoWeapon)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
        }


        private void CheckLastBulletHit()
        {
            if (GetWieldingItem<BulletWeapon>(out var weapon))
            {
                if (weapon.m_lastFireTime + 2f > Time.time)
                {
                    if (Weapon.s_weaponRayData != null && Weapon.s_weaponRayData.rayHit.collider != null && Weapon.s_weaponRayData.rayHit.collider.gameObject != null)
                    {
                        if (Weapon.s_weaponRayData.owner == this.Owner)
                        {
                            var playerAgent = Weapon.s_weaponRayData.rayHit.collider.gameObject.GetComponent<PlayerAgent>();
                            if (playerAgent == null)
                            {
                                playerAgent = Weapon.s_weaponRayData.rayHit.collider.gameObject.GetComponentInParent<PlayerAgent>();
                            }
                            if (playerAgent != null || Weapon.s_weaponRayData.rayHit.collider.gameObject.layer == LayerManager.LAYER_PLAYER_SYNCED)
                            {
                                if (_friendlyFireCallback == null && !CoolDownManager.HasCooldown(CoolDownType.FriendlyFireApology))
                                {
                                    _friendlyFireCallback = new CallBackUtils.CallBack(FriendlyFireComment);
                                    _friendlyFireCallback.QueueCallBack(1.35f);
                                    CoolDownManager.ApplyCooldown(CoolDownType.FriendlyFireApology, 10f);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateItemWielded()
        {
            ItemEquippable wieldedItem = null;
            if (this.m_inventory.WieldedItem != null)
            {
                wieldedItem = this.m_inventory.WieldedItem.TryCast<ItemEquippable>();
            }
            if (wieldedItem != null)
            {
                bool noPreviousItemWielded = this.m_currentItemWielded == null;
                if (noPreviousItemWielded || this.m_currentItemWielded != wieldedItem)
                {                    
                    m_currentItemWielded = wieldedItem;
                    if (!noPreviousItemWielded)
                    {
                        ChatterDebug.LogMessage("UpdateItemWielded, Detected a new wielded item..");
                        this.OnWieldedItemChanged();
                    }                    
                }
            }            
        }

        private void StartAmmoDialog(uint dialogID)
        {
            if (ConfigurationManager.AmmoCommentsEnabled && !CoolDownManager.HasCooldown(CoolDownType.BulletWeaponDialog))
            {
                if (DramaManager.CurrentStateEnum != DRAMA_State.Combat || DramaManager.CurrentStateEnum != DRAMA_State.Sneaking)
                {
                    this.Owner.WantToStartDialog(dialogID, true);
                    CoolDownManager.ApplyCooldown(CoolDownType.BulletWeaponDialog, m_bulletWeaponDialogCooldown);
                    if (m_bulletWeaponDialogCooldown < 180f)
                    {
                        m_bulletWeaponDialogCooldown += 15f;
                    }
                }                
            }
        }

        private float m_bulletWeaponDialogCooldown;

        private bool HasAmmoPack
        {
            get
            {
                var backpack = PlayerBackpackManager.GetBackpack(this.Owner.Owner);
                if (backpack != null && backpack.TryGetBackpackItem(InventorySlot.ResourcePack, out var backpackItem))
                {
                    if (backpackItem.ItemID == GD.Item.AmmoPackWeapon)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private void OnBulletWeaponWield(BulletWeapon bulletWeapon)
        {
            var chatterMachine = DramaChatterManager.GetMachine(this.Owner);
            if (chatterMachine != null)
            {                
                ExtendedPlayerManager.GetWeaponStats(this.Owner, out float totalAmmoRel, out _);
                if (totalAmmoRel < 0.15f)
                {
                    uint dialogID = GD.PlayerDialog.on_ammo_low;
                    if (totalAmmoRel == 0f)
                    {
                        dialogID = GD.PlayerDialog.ammo_depleted_reminder;
                    }
                    if (!HasAmmoPack)
                    {
                        this.StartAmmoDialog(dialogID);
                    }                    
                }
                else
                {
                    this.m_bulletWeaponDialogCooldown = 30f;
                }
            }            
        }

        private void OnWieldedItemChanged()
        {
            if (m_currentItemWielded.Convert(out BulletWeapon bulletWeapon))
            {
                this.OnBulletWeaponWield(bulletWeapon);
            }
        }


        public void Update()
        {
            this.UpdateItemWielded();
            this.CheckLastBulletHit();
            this.UpdateState();
        }

        private ItemEquippable m_currentItemWielded;


    }
}
