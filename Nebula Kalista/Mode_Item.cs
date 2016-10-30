using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaKalista
{
    internal class Mode_Item : Kalista
    {
        static readonly Item BladeKing = new Item((int)ItemId.Blade_of_the_Ruined_King, 550f);
        static readonly Item Bilgewater = new Item((int)ItemId.Bilgewater_Cutlass, 550f);
        static readonly Item Youmuu = new Item((int)ItemId.Youmuus_Ghostblade);
        static readonly Item Hextech = new Item((int)ItemId.Hextech_Gunblade, 700f);
        static readonly Item Quicksilver = new Item((int)ItemId.Quicksilver_Sash);
        static readonly Item Mercurial = new Item((int)ItemId.Mercurial_Scimitar);

        public static void Items_Use()
        {
            if (Player.Instance.CountEnemiesInRange(1500) == 0) return;

            //BladeKing, Bilgewater
            if (Bilgewater.IsOwned() || BladeKing.IsOwned())
            {
                var Botrk_Target = TargetSelector.GetTarget(Player.Instance.AttackRange + Player.Instance.BoundingRadius + 65, DamageType.Physical);

                if (Botrk_Target != null)
                {
                    if (Bilgewater.IsReady())
                    {
                        Bilgewater.Cast(Botrk_Target);
                    }

                    if (BladeKing.IsReady() && Player.Instance.HealthPercent <= MenuItem["BladeKing.Use"].Cast<Slider>().CurrentValue)
                    {
                        BladeKing.Cast(Botrk_Target);
                    }
                }
            }

            //Youmuu
            if (Youmuu.IsOwned() && Youmuu.IsReady() && Player.Instance.CountEnemiesInRange(1500) >= 1)
            {
                Youmuu.Cast();
            }

            //Hextech
            if (Hextech.IsOwned() && Hextech.IsReady())
            {
                var Hextech_Target = TargetSelector.GetTarget(700, DamageType.Magical);

                if (Hextech_Target != null)
                {
                    Hextech.Cast(Hextech_Target);
                }
            }

            //Quicksilver, Scimitar
            Active_Item();
        }

        private static void Active_Item()
        {
            var Delay_Time = MenuItem["Cast.Delay"].Cast<Slider>().CurrentValue;

            if (Quicksilver.IsOwned() && Quicksilver.IsReady() && MenuItem["Quicksilver"].Cast<CheckBox>().CurrentValue)
            {
                if (MenuItem["Poisons"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Poison))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (MenuItem["Supression"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Suppression))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (MenuItem["Blind"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Blind))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (MenuItem["Charm"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Charm))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (MenuItem["Fear"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Fear))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (MenuItem["Polymorph"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Polymorph))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (MenuItem["Silence"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Silence))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (MenuItem["Slow"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Slow))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (MenuItem["Stun"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Stun))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (MenuItem["Knockup"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Knockup))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (MenuItem["Taunt"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Taunt))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }              
            }
          
            if (Mercurial.IsOwned() && Mercurial.IsReady() && MenuItem["Scimitar"].Cast<CheckBox>().CurrentValue)
            {
                if (MenuItem["Poisons"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Poison))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (MenuItem["Supression"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Suppression))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (MenuItem["Blind"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Blind))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (MenuItem["Charm"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Charm))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (MenuItem["Fear"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Fear))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (MenuItem["Polymorph"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Polymorph))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (MenuItem["Silence"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Silence))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (MenuItem["Slow"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Slow))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (MenuItem["Stun"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Stun))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (MenuItem["Knockup"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Knockup))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (MenuItem["Taunt"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Taunt))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }
            }
        }   //End Active_Item
    }
}
