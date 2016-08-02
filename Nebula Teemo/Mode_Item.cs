using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaTeemo
{
    internal class Mode_Item : Teemo
    {
        public static readonly Spell.Targeted Ignite = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("summonerdot"), 600);

        static readonly Item BladeKing = new Item((int)ItemId.Blade_of_the_Ruined_King, 550f);
        static readonly Item Bilgewater = new Item((int)ItemId.Bilgewater_Cutlass, 550f);
        static readonly Item Youmuu = new Item((int)ItemId.Youmuus_Ghostblade);
        static readonly Item Hextech = new Item((int)ItemId.Hextech_Gunblade, 700f);
        static readonly Item Quicksilver = new Item((int)ItemId.Quicksilver_Sash);
        static readonly Item Mercurial = new Item((int)ItemId.Mercurial_Scimitar);
       

        public static void Items_Use()
        {
            if (Player.Instance.IsDead) return;
            if (Player.Instance.CountEnemiesInRange(1500) == 0) return;
                    
            if (Bilgewater.IsOwned() || BladeKing.IsOwned())
            {
                var Botrk_Target = TargetSelector.GetTarget(550, DamageType.Physical);

                if (Bilgewater.IsReady())
                {
                    Bilgewater.Cast(Botrk_Target);
                }

                if (BladeKing.IsReady() && Player.Instance.HealthPercent <= MenuItem["BladeKing.Use"].Cast<Slider>().CurrentValue)
                {
                    BladeKing.Cast(Botrk_Target);
                }
            }

            if (Hextech.IsOwned() && Hextech.IsReady())
            {
                var Hextech_Target = TargetSelector.GetTarget(700, DamageType.Magical);

                Hextech.Cast(Hextech_Target);
            }

            if (Youmuu.IsOwned() && Youmuu.IsReady() && Player.Instance.CountEnemiesInRange(1000) >= 1)
            {
                Youmuu.Cast();
            }

            if (Ignite.Slot != SpellSlot.Unknown && Ignite.IsReady())
            {
                var Ignite_Target = TargetSelector.GetTarget(600, DamageType.True);
                //var Ignite_Damage = Player.Instance.GetSummonerSpellDamage(Ignite_Target, DamageLibrary.SummonerSpells.Ignite);

               if(MenuCombo["Combo.Ignite"].Cast<CheckBox>().CurrentValue)
                {
                    if (Ignite_Target.TotalShieldHealth() <= Damage.DmgIgnite + (Damage.DmgIgnite * 0.15))
                    {
                        Ignite.Cast(Ignite_Target);
                    }
                }                
            }
            Active_Item();
        }

        private static void Active_Item()
        {
            var Delay_Time = MenuItem["CastDelay"].Cast<Slider>().CurrentValue;

            if (Quicksilver.IsOwned() && Quicksilver.IsReady() && MenuItem["QSS"].Cast<CheckBox>().CurrentValue)
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

                if (MenuItem["Snare"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Snare))
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

                if (MenuItem["Snare"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Snare))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (MenuItem["Taunt"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Taunt))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }
            }
        }   //End Active_Item
    }
}
