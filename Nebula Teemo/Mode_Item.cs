using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.Sandbox;
using SharpDX;

namespace NebulaTeemo
{
    internal class Mode_Item : Teemo
    {
        public static readonly Spell.Targeted Ignite = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("summonerdot"), 600);

        public static readonly Item BladeKing = new Item((int)ItemId.Blade_of_the_Ruined_King, 550f);
        public static readonly Item Bilgewater = new Item((int)ItemId.Bilgewater_Cutlass, 550f);
        static readonly Item Youmuu = new Item((int)ItemId.Youmuus_Ghostblade);
        public static readonly Item Hextech = new Item((int)ItemId.Hextech_Gunblade, 700f);
        static readonly Item Quicksilver = new Item((int)ItemId.Quicksilver_Sash);
        static readonly Item Mercurial = new Item((int)ItemId.Mercurial_Scimitar);
        static readonly Item Zhonyas = new Item((int)ItemId.Zhonyas_Hourglass);

        public static void Items_Use()
        {
            if (Player.Instance.IsDead) return;
            if (Player.Instance.CountEnemiesInRange(1500) == 0) return;

            if (Bilgewater.IsOwned() || BladeKing.IsOwned())
            {
                var Botrk_Target = TargetSelector.GetTarget(550, DamageType.Physical);

                if (Botrk_Target != null && !Botrk_Target.IsInvulnerable && !Botrk_Target.HasUndyingBuff() && !Botrk_Target.IsZombie)
                {
                    if (Bilgewater.IsReady())
                    {
                        Bilgewater.Cast(Botrk_Target);
                    }

                    if (BladeKing.IsReady() && Player.Instance.HealthPercent <= MenuItem["Item.BK.Hp"].Cast<Slider>().CurrentValue)
                    {
                        BladeKing.Cast(Botrk_Target);
                    }
                }
            }

            if (Hextech.IsOwned() && Hextech.IsReady())
            {
                var Hextech_Target = TargetSelector.GetTarget(700, DamageType.Magical);

                if (Hextech_Target != null && !Hextech_Target.IsInvulnerable && !Hextech_Target.HasUndyingBuff() && !Hextech_Target.IsZombie)
                {
                    Hextech.Cast(Hextech_Target);
                }
            }

            if (Youmuu.IsOwned() && Youmuu.IsReady() && Player.Instance.CountEnemiesInRange(1000) >= 1)
            {
                Youmuu.Cast();
            }

            if (Ignite.Slot != SpellSlot.Unknown && Ignite.IsReady())
            {
                var Ignite_Target = TargetSelector.GetTarget(580, DamageType.True);

                if (Ignite_Target != null && MenuCombo["Combo.Ignite"].Cast<CheckBox>().CurrentValue)
                {
                    if (Ignite_Target.Health <= Damage.DmgCal(Ignite_Target))
                    {
                        Ignite.Cast(Ignite_Target);
                    }
                }
            }
            Active_Item();
        }

        public static void UltBuffUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead) return;
            if (MenuItem["Item.Zy"].Cast<CheckBox>().CurrentValue == false) return;
            if (!Zhonyas.IsOwned() || !Zhonyas.IsReady()) return;

            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                if (enemy.ChampionName == "Caitlyn" && Player.Instance.HasBuff("caitlynaceinthehole"))
                {
                    var TravelTime = Player.Instance.Distance(enemy.Position) / 3200;
                    var CastDelay = (TravelTime * 1000) + 900;

                    Core.DelayAction(() => Zhonyas.Cast(), (int)CastDelay);
                }
                else if (Player.Instance.HasBuff("zedrdeathmark") || Player.Instance.HasBuff("SoulShackles"))
                {
                    Core.DelayAction(() => Zhonyas.Cast(), 2500);
                }
                else if (enemy.ChampionName == "Karthus" && Player.Instance.HasBuff("karthusfallenonetarget"))
                {
                    if (Player.Instance.Health <= enemy.GetSpellDamage(Player.Instance, SpellSlot.R) + 150 ||
                        (Player.Instance.CountEnemiesInRange(1200) >= 1 && Player.Instance.Health <= enemy.GetSpellDamage(Player.Instance, SpellSlot.R) + (Player.Instance.Health / 2)))
                    {
                        Core.DelayAction(() => Zhonyas.Cast(), 2500);
                    }
                }
                else if (Player.Instance.HasBuff("fizzmarinerdoombomb"))
                {
                    Core.DelayAction(() => Zhonyas.Cast(), 1500);
                }
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.Instance.IsDead) return;
            if (MenuItem["Item.Zy"].Cast<CheckBox>().CurrentValue == false) return;
            if (!Zhonyas.IsOwned() || !Zhonyas.IsReady()) return;
            if (!(args.Target is AIHeroClient)) return;

            var hero = sender as AIHeroClient;

            if (hero.IsEnemy)
            {
                var hitme = Player.Instance.Distance(args.End + (args.SData.CastRadius / 2)) < 100;

                if (MenuItem["R." + hero.ChampionName.ToLower()].Cast<CheckBox>().CurrentValue && args.Slot == SpellSlot.R)
                {
                    if ((hero.ChampionName != "Caitlyn" || hero.ChampionName != "Zed" || hero.ChampionName != "Karthus" || hero.ChampionName != "Morgana" || hero.ChampionName != "Fizz") && args.Slot == SpellSlot.R)
                    {
                        if (hitme)
                        {
                            Core.DelayAction(() => Zhonyas.Cast(), (int)args.SData.SpellCastTime + 150);
                        }
                    }
                }

                if (args.Target.IsMe || hitme)
                {
                    var spelldamageme = hero.GetSpellDamage(Player.Instance, args.Slot);
                    var damagepercent = (spelldamageme / Player.Instance.TotalShieldHealth()) * 100;
                    var death = damagepercent >= Player.Instance.HealthPercent || spelldamageme >= Player.Instance.TotalShieldHealth() || hero.GetAutoAttackDamage(Player.Instance, true) >= Player.Instance.TotalShieldHealth();

                    if (Player.Instance.HealthPercent <= MenuItem["Item.Zy.SHp"].Cast<Slider>().CurrentValue || death || damagepercent >= MenuItem["Item.Zy.SDmg"].Cast<Slider>().CurrentValue)
                    {
                        if ((hero.ChampionName != "Caitlyn" || hero.ChampionName != "Zed" || hero.ChampionName != "Karthus" || hero.ChampionName != "Morgana" || hero.ChampionName != "Fizz") && args.Slot == SpellSlot.R)
                        {
                            Core.DelayAction(() => Zhonyas.Cast(), (int)args.SData.SpellCastTime + 150);
                        }
                    }
                }
            }
        }

        public static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.Instance.IsDead) return;
            if (MenuItem["Item.Zy"].Cast<CheckBox>().CurrentValue == false) return;
            if (!Zhonyas.IsOwned() || !Zhonyas.IsReady()) return;
            if (!(args.Target is AIHeroClient)) return;

            var hero = sender as AIHeroClient;
            var target = (AIHeroClient)args.Target;

            if (!(sender is AIHeroClient || sender is Obj_AI_Turret) || !sender.IsEnemy || target == null || sender == null)
            {
                return;
            }

            var aaprecent = (sender.GetAutoAttackDamage(Player.Instance, true) / Player.Instance.TotalShieldHealth()) * 100;
            var death = sender.GetAutoAttackDamage(Player.Instance, true) >= Player.Instance.TotalShieldHealth() || aaprecent >= Player.Instance.HealthPercent;

            if ((hero.IsEnemy || sender is Obj_AI_Turret) && target.IsMe)
            {
                if (Player.Instance.HealthPercent <= MenuItem["Item.Zy.BHp"].Cast<Slider>().CurrentValue || death || aaprecent >= MenuItem["Item.Zy.BDmg"].Cast<Slider>().CurrentValue)
                {
                    Core.DelayAction(() => Zhonyas.Cast(), (int)args.SData.MissileSpeed + 100);
                }
            }
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
