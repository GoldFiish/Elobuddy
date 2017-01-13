using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaTwistedFate.Modes
{
    internal class Mode_Item : TwistedFate
    {
        public static readonly Item Bilgewater  = new Item((int)ItemId.Bilgewater_Cutlass, 550f);
        public static readonly Item BladeKing   = new Item((int)ItemId.Blade_of_the_Ruined_King, 550f);
        public static readonly Item Youmuu      = new Item((int)ItemId.Youmuus_Ghostblade);
        public static readonly Item Hextech     = new Item((int)ItemId.Hextech_Gunblade, 700f);
        public static readonly Item Quicksilver = new Item((int)ItemId.Quicksilver_Sash);
        public static readonly Item Mercurial   = new Item((int)ItemId.Mercurial_Scimitar);
        public static readonly Item Zhonyas     = new Item((int)ItemId.Zhonyas_Hourglass);
        public static readonly Item Sheen       = new Item((int)ItemId.Sheen);
        public static readonly Item IceGauntlet = new Item((int)ItemId.Iceborn_Gauntlet);
        public static readonly Item TriniForce  = new Item((int)ItemId.Trinity_Force);
        public static readonly Item LichBane    = new Item((int)ItemId.Lich_Bane);

        public static void Items_Use()
        {
            if (Player.Instance.IsDead) return;
            if (Player.Instance.CountEnemyChampionsInRange(1500) == 0) return;

            if (Status_CheckBox(M_Item, "Item.BK") && (Bilgewater.IsOwned() || BladeKing.IsOwned()))
            {
                var Botrk_Target = TargetSelector.GetTarget(550, DamageType.Physical);

                if (Botrk_Target != null)
                {
                    if (Bilgewater.IsReady() || (BladeKing.IsReady() && Player.Instance.HealthPercent <= Status_Slider(M_Item, "Item.BK.Hp")))
                    {
                        Bilgewater.Cast(Botrk_Target);
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

            if (Youmuu.IsOwned() && Youmuu.IsReady() && Player.Instance.CountEnemyChampionsInRange(1000) >= 1)
            {
                Youmuu.Cast();
            }
            Active_Item();
        }

        public static void UltBuffUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead) return;
            if (Status_CheckBox(M_Item, "Item.Zy") == false) return;
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
                        (Player.Instance.CountEnemyChampionsInRange(1200) >= 1 && Player.Instance.Health <= enemy.GetSpellDamage(Player.Instance, SpellSlot.R) + (Player.Instance.Health / 2)))
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
            
            if (sender.IsMe && args.SData.Name.ToLower() == "gate" && Status_CheckBox(M_Misc, "Misc_Auto_Yel"))
            {
                CardSelect.StartSelecting(Cards.Yellow);
            }
            
            if (Status_CheckBox(M_Item, "Item_Zy") == false) return;
            if (!Zhonyas.IsOwned() || !Zhonyas.IsReady()) return;
            if (!(args.Target is AIHeroClient)) return;

            var hero = sender as AIHeroClient;

            if (hero.IsEnemy)
            {
                var hitme = Player.Instance.Distance(args.End + (args.SData.CastRadius / 2)) < 100;

                if (Status_CheckBox(M_Item, "R." + hero.ChampionName.ToLower()) && args.Slot == SpellSlot.R)
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

                    if (Player.Instance.HealthPercent <= Status_Slider(M_Item, "Item.Zy.SHp") || death || damagepercent >= Status_Slider(M_Item, "Item.Zy.SDmg"))
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
            if (Status_CheckBox(M_Item, "Item.Zy") == false) return;
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
                if (Player.Instance.HealthPercent <= Status_Slider(M_Item, "Item.Zy.BHp") || death || aaprecent >= Status_Slider(M_Item, "Item.Zy.BDmg"))
                {
                    Core.DelayAction(() => Zhonyas.Cast(), (int)args.SData.MissileSpeed + 100);
                }
            }
        }

        private static void Active_Item()
        {
            var Delay_Time = M_Item["CastDelay"].Cast<Slider>().CurrentValue;

            if (Quicksilver.IsOwned() && Quicksilver.IsReady() && M_Item["QSS"].Cast<CheckBox>().CurrentValue)
            {
                if (M_Item["Poisons"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Poison))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (M_Item["Supression"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Suppression))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (M_Item["Blind"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Blind))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (M_Item["Charm"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Charm))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (M_Item["Fear"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Fear))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (M_Item["Polymorph"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Polymorph))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (M_Item["Silence"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Silence))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (M_Item["Slow"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Slow))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (M_Item["Stun"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Stun))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (M_Item["Snare"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Snare))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (M_Item["Taunt"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Taunt))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }
            }

            if (Mercurial.IsOwned() && Mercurial.IsReady() && M_Item["Scimitar"].Cast<CheckBox>().CurrentValue)
            {
                if (M_Item["Poisons"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Poison))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (M_Item["Supression"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Suppression))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (M_Item["Blind"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Blind))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (M_Item["Charm"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Charm))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (M_Item["Fear"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Fear))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (M_Item["Polymorph"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Polymorph))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (M_Item["Silence"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Silence))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (M_Item["Slow"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Slow))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (M_Item["Stun"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Stun))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (M_Item["Snare"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Snare))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (M_Item["Taunt"].Cast<CheckBox>().CurrentValue && Player.Instance.HasBuffOfType(BuffType.Taunt))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }
            }
        }   //End Active_Item
    }
}
