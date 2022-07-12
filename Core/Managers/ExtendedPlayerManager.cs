using AIGraph;
using ChatterReborn.Data;
using ChatterReborn.Utils;
using GameData;
using LevelGeneration;
using Player;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Managers
{
    public class ExtendedPlayerManager : ChatterManager<ExtendedPlayerManager>
    {
        public static float GetOverallHealth
        {
            get
            {
                float num = 0f;
                float num2 = 0f;
                List<PlayerAgent> list = AllPlayersInLevel;
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        PlayerAgent playerAgent = list[i];
                        if (playerAgent != null)
                        {
                            num2 += playerAgent.Damage.GetHealthRel();
                            num += 1f;
                        }
                    }
                }
                return num2 / num;
            }
        }

        public static bool IsPlayerAgentAlive(PlayerAgent playerAgent)
        {
            return playerAgent != null && playerAgent.Alive;
        }

        public static bool LightsAvailable(PlayerAgent player)
        {

            LG_LightCollection lg_LightCollection = LG_LightCollection.Create(player.CourseNode, player.Position, LG_LightCollectionSorting.Distance, 3f);
            if (lg_LightCollection != null)
            {
                var collected = lg_LightCollection.collectedLights;
                for (int i = 0; i < collected.Count; i++)
                {
                    var coll = collected[i];
                    if (coll.light.isActiveAndEnabled && coll.light.GetIntensity() > 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public static void GetWeaponStats(PlayerAgent playerAgent, out float totalAmmoRel, out int weaponCount)
        {
            totalAmmoRel = 0f;
            weaponCount = 0;

            PlayerBackpack backpack = PlayerBackpackManager.GetBackpack(playerAgent.Owner);

            BackpackItem backpackItem;
            if (backpack.TryGetBackpackItem(InventorySlot.GearStandard, out backpackItem))
            {
                weaponCount++;
                totalAmmoRel += backpack.AmmoStorage.StandardAmmo.RelInPack;
            }
            BackpackItem backpackItem2;
            if (backpack.TryGetBackpackItem(InventorySlot.GearSpecial, out backpackItem2))
            {
                weaponCount++;
                totalAmmoRel += backpack.AmmoStorage.SpecialAmmo.RelInPack;
            }

            totalAmmoRel /= weaponCount;
        }


        public static void GetToolAmmo(PlayerAgent playerAgent, out float toolAmmoRel)
        {
            toolAmmoRel = 0f;
            PlayerBackpack backpack = PlayerBackpackManager.GetBackpack(playerAgent.Owner);
            if (backpack.TryGetBackpackItem(InventorySlot.GearClass, out _))
            {
                toolAmmoRel += backpack.AmmoStorage.ClassAmmo.RelInPack;
            }
        }


        public static int GetTotalAlivePlayers
        {
            get
            {
                int amount = 0;
                for (int i = 0; i < AllPlayersInLevel.Count; i++)
                {
                    PlayerAgent playerAgent = AllPlayersInLevel[i];
                    if (playerAgent != null && playerAgent.Alive)
                    {
                        amount++;
                    }
                }
                return amount;

            }
        }

        public static List<PlayerAgent> GetNonLocalPlayers
        {
            get
            {
                List<PlayerAgent> playerAgents = Il2cppUtils.ToSystemList(PlayerManager.PlayerAgentsInLevel);

                for (int i = 0; i < playerAgents.Count; i++)
                {
                    if (playerAgents[i].IsLocallyOwned)
                    {
                        playerAgents.RemoveAt(i);
                    }
                }

                return playerAgents;
            }
        }

        public static List<PlayerAgent> GetAllPlayers
        {
            get
            {
                return Il2cppUtils.ToSystemList(PlayerManager.PlayerAgentsInLevel);
            }
        }
        public static bool IsCharacterIDLocal(int CharacterID)
        {
            PlayerAgent playerAgent;
            return PlayerManager.TryGetLocalPlayerAgent(out playerAgent) && playerAgent.CharacterID == CharacterID;
        }

        public static bool IscharacterIDBot(int CharacterID)
        {
            foreach (var player in PlayerManager.PlayerAgentsInLevel)
            {
                if (player.CharacterID == CharacterID)
                {
                    var bot = player.GetComponent<PlayerBotAI>();
                    if (bot != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsPlayerSeperated(PlayerAgent playerAgent)
        {
            for (int i = 0; i < AllPlayersInLevel.Count; i++)
            {
                PlayerAgent playerAgent2 = AllPlayersInLevel[i];
                if (playerAgent2 != null && playerAgent2.Alive)
                {
                    Vector3 vector3 = (playerAgent2.Position - playerAgent.Position);
                    if (vector3.magnitude < 40f)
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        public static List<PlayerAgent> AllPlayersInLevel
        {
            get
            {
                return Il2cppUtils.ToSystemList(PlayerManager.PlayerAgentsInLevel);
            }
        }


        public static void StartApplyResourcePackDialog(PlayerAgent playerAgent)
        {
            string cooldownKey = CoolDownType.GiveResource.ToString() + playerAgent.CharacterID;
            if (!CoolDownManager.HasCooldown(cooldownKey))
            {
                WeightHandler<uint> weightHandler = WeightHandler<uint>.CreateWeightHandler();
                weightHandler.AddValue(GD.PlayerDialog.CL_Wait, 3f);
                weightHandler.AddValue(GD.PlayerDialog.CL_GoSlow, 2f);
                weightHandler.AddValue(GD.PlayerDialog.heal_spray_apply_teammate, 5f);             

                if (weightHandler.HasAny)
                {
                    playerAgent.WantToStartDialog(weightHandler.Best.Value, true);
                }
                CoolDownManager.ApplyCooldown(cooldownKey, 2f);
            }
        }


        public static void OnItemTakeDialog(PlayerAgent interactionSourceAgent, uint itemID)
        {
            string cooldownKey = CoolDownType.PickUpResource.ToString() + interactionSourceAgent.CharacterID;
            if (CoolDownManager.HasCooldown(cooldownKey))
            {
                return;
            }

            PlayerBackpack backPack = PlayerBackpackManager.GetLocalOrSyncBackpack(interactionSourceAgent.Owner);
            WeightHandler<uint> dialogHandler = WeightHandler<uint>.CreateWeightHandler();
            if (itemID == GD.Item.AmmoPackWeapon || itemID == GD.Item.AmmoPackTool)
            {
                dialogHandler.AddValue(GD.PlayerDialog.on_pick_up_ammo, 1f);
                if (itemID == GD.Item.AmmoPackWeapon)
                {
                    if (backPack != null && (backPack.AmmoStorage.StandardAmmo.RelInPack + backPack.AmmoStorage.SpecialAmmo.RelInPack) / 2f < 0.2f)
                    {
                        dialogHandler.AddValue(GD.PlayerDialog.picked_up_ammo_depleted, 3f);
                    }
                }
            }
            else if (itemID == GD.Item.MediPack || itemID == GD.Item.DisinfectionPack)
            {
                dialogHandler.AddValue(GD.PlayerDialog.on_pick_up_health, 2f);
                float healthRel = interactionSourceAgent.Damage.GetHealthRel();
                if (itemID == GD.Item.MediPack)
                {
                    if (healthRel <= 0.3f)
                    {
                        dialogHandler.AddValue(GD.PlayerDialog.on_pick_up_health_when_low, 3f);
                        if (healthRel <= 0.2f)
                        {
                            dialogHandler.AddValue(GD.PlayerDialog.found_health_station_response, 1f);
                        }
                    }
                }                
            }

            if (dialogHandler.HasAny)
            {
                uint pickUpDialogID = dialogHandler.Best.Value;
                ChatterDebug.LogWarning("ExtendedPlayerManager.OnItemTakeDialog, triggering resource pickUpDialogID " + pickUpDialogID);
                if (UnityEngine.Random.value < 0.75f)
                {
                    float randomDelay = UnityEngine.Random.Range(0.5f, 0.8f);
                    PrisonerDialogManager.DelayDialogForced(randomDelay, dialogHandler.Best.Value, interactionSourceAgent);
                }
                CoolDownManager.ApplyCooldown(cooldownKey, 5f);
            }
        }
    }
}
