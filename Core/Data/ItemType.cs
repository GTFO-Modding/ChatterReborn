using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Data
{
    public enum ItemType : uint
    {
        Ammo = GD.Item.AmmoPackWeapon,
        Tool = GD.Item.AmmoPackTool,
        DisInfect = GD.Item.DisinfectionPack,
        MedPack = GD.Item.MediPack,
        GlowStick = GD.Item.CONSUMABLE_GlowStick,
        GlowStick_Xmas = GD.Item.CONSUMABLE_GlowStick_Christmas,
        GlowStick_HW = GD.Item.CONSUMABLE_GlowStick_Halloween,
        GlueGrenade = GD.Item.CONSUMABLE_GlueGrenade,
        HealthSyringe = GD.Item.CONSUMABLE_Syringe_Health,
        BuffSyringe = GD.Item.CONSUMABLE_Syringe_MeleeBuff,
        FogRepeller = GD.Item.CONSUMABLE_FogRepeller,
        Tripmine_Explosive = GD.Item.CONSUMABLE_Tripmine_Explosive,
        Tripmine_Glue = GD.Item.CONSUMABLE_Tripmine_Glue,
        LongRangeFlashLight = GD.Item.CONSUMABLE_FlashlightMedium,
        HeavyFogReller = GD.Item.Carry_HeavyFogRepeller
    }
}
