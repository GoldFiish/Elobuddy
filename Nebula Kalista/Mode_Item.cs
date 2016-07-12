using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaKalista
{
    public class Mode_Item
    {
        static readonly Item Botrk = new Item((int)ItemId.Blade_of_the_Ruined_King, 550f);
        static readonly Item Bilgewater = new Item((int)ItemId.Bilgewater_Cutlass, 550f);
        static readonly Item Youmuu = new Item((int)ItemId.Youmuus_Ghostblade);
        static readonly Item Quicksilver = new Item((int)ItemId.Quicksilver_Sash);
        static readonly Item Mercurial = new Item((int)ItemId.Mercurial_Scimitar);
       
        public static void Items_Use()
        {
            var Botrk_Target = TargetSelector.GetTarget(ObjectManager.Player.AttackRange + ObjectManager.Player.BoundingRadius + 65, DamageType.Physical);
            if (Botrk_Target.IsValidTarget() && Botrk_Target.Distance(ObjectManager.Player) < 550)
            {
                if (Bilgewater.Slots.Any() && Bilgewater.IsReady()) { Bilgewater.Cast(Botrk_Target); }
                
                if (Botrk.Slots.Any() && Botrk.IsReady() &&ObjectManager.Player.HealthPercent <= Kalista.MenuItem["BladeKing.Use"].Cast<Slider>().CurrentValue) { Botrk.Cast(Botrk_Target); }
            }
            
            if (Youmuu.Slots.Any() && Youmuu.IsReady() && ObjectManager.Player.CountEnemiesInRange(1500) == 1)
            {
                Youmuu.Cast();
            }
                       
            if (Kalista.MenuItem["Poisons"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HasBuffOfType(BuffType.Poison))
            { Activ_Item(); }

            if (Kalista.MenuItem["Supression"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HasBuffOfType(BuffType.Suppression))
            { Activ_Item(); }

            if (Kalista.MenuItem["Blind"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HasBuffOfType(BuffType.Blind))
            { Activ_Item(); }

            if (Kalista.MenuItem["Charm"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HasBuffOfType(BuffType.Charm))
            { Activ_Item(); }

            if(Kalista.MenuItem["Fear"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HasBuffOfType(BuffType.Fear))
            { Activ_Item(); }

            if(Kalista.MenuItem["Polymorph"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HasBuffOfType(BuffType.Polymorph))
            { Activ_Item(); }

            if(Kalista.MenuItem["Silence"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HasBuffOfType(BuffType.Silence))
            { Activ_Item(); }

            if(Kalista.MenuItem["Slow"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HasBuffOfType(BuffType.Slow))
            { Activ_Item(); }

            if(Kalista.MenuItem["Stun"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HasBuffOfType(BuffType.Stun))
            { Activ_Item(); }

            if(Kalista.MenuItem["Knockup"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HasBuffOfType(BuffType.Knockup))
            { Activ_Item(); }

            if(Kalista.MenuItem["Taunt"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HasBuffOfType(BuffType.Taunt))
            { Activ_Item(); }
        }

        private static void Activ_Item()
        {
            if (Quicksilver.IsOwned() && Quicksilver.IsReady() && ObjectManager.Player.CountEnemiesInRange(1500) > 0)
            {
                Core.DelayAction(() => Quicksilver.Cast(), Kalista.MenuItem["Cast.Delay"].Cast<Slider>().CurrentValue);
            }

            if (Mercurial.IsOwned() && Mercurial.IsReady() && ObjectManager.Player.CountEnemiesInRange(1500) > 0)
            {
                Core.DelayAction(() => Mercurial.Cast(), Kalista.MenuItem["Cast.Delay"].Cast<Slider>().CurrentValue);
            }
        }
    }
}
