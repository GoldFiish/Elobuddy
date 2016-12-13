using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

namespace NebulaNasus.Modes
{
    internal class Mode_Item : Nasus
    {

        public static readonly Item Bilgewater  = new Item((int)ItemId.Bilgewater_Cutlass, 550f);
        public static readonly Item BladeKing   = new Item((int)ItemId.Blade_of_the_Ruined_King, 550f);
        public static readonly Item Youmuu      = new Item((int)ItemId.Youmuus_Ghostblade);
        public static readonly Item Sheen       = new Item((int)ItemId.Sheen);              //광휘의 검
        public static readonly Item IceGauntlet = new Item((int)ItemId.Iceborn_Gauntlet);   //얼어붙은 건틀릿
        public static readonly Item TriniForce  = new Item((int)ItemId.Trinity_Force);      //삼위일체   
        public static readonly Item Redemption  = new Item((int)ItemId.Redemption);         //구원
        static readonly Item Quicksilver        = new Item((int)ItemId.Quicksilver_Sash);
        static readonly Item Mercurial          = new Item((int)ItemId.Mercurial_Scimitar);

        public static void Items_Use()
        {
            if (Player.Instance.IsDead) return;
            if (Player.Instance.CountEnemiesInRange(1500) == 0) return;

            if (Status_CheckBox(M_Item, "Item.BK") && (Bilgewater.IsOwned() || BladeKing.IsOwned()))
            {
                var Botrk_Target = TargetSelector.GetTarget(550, DamageType.Physical);

                if (Botrk_Target != null)
                {
                    if (Bilgewater.IsReady())
                    {
                        Bilgewater.Cast(Botrk_Target);
                    }

                    if (BladeKing.IsReady() && Player.Instance.HealthPercent <= Status_Slider(M_Item, "Item.BK.Hp"))
                    {
                        BladeKing.Cast(Botrk_Target);
                    }
                }
            }            

            if (Youmuu.IsOwned() && Youmuu.IsReady() && Player.Instance.CountEnemiesInRange(1000) >= 1)
            {
                Youmuu.Cast();
            }           
            Active_Item();
        }

        public static void Active_Redemption()
        {
            if (Redemption.IsOwned() && Redemption.IsReady() && Status_CheckBox(M_Item, "Item.Redemption"))
            {
                var Redemption_Target = TargetSelector.GetTarget(5500, DamageType.Physical);

                if (Redemption_Target == null) return;

                if (Player.Instance.Distance(Redemption_Target) <= 850)
                {
                    if (Redemption_Target.IsAttackingPlayer && Player.Instance.HealthPercent <= Status_Slider(M_Item, "Item.Redemption.MyHp"))
                    {
                        var ipRedemption = Prediction.Position.PredictCircularMissile(Player.Instance, 5500, 275, 2500, 3200); //범위 540

                        if (ipRedemption.HitChancePercent >= 50)
                        {
                            Redemption.Cast(ipRedemption.CastPosition);
                        }
                    }
                }
                else if(Redemption_Target.Health <= Damage.DmgRedemption(Redemption_Target))
                {
                    var ieRedemption = Prediction.Position.PredictCircularMissile(Redemption_Target, 5500, 275, 2500, 3200);

                    if (ieRedemption.HitChancePercent >= 50)
                    {
                        Redemption.Cast(ieRedemption.CastPosition);
                    }
                }

                foreach (var team in EntityManager.Heroes.Allies.Where(x => x.IsValidTarget(5500) && !x.IsMe))
                {
                    if ((team.CountAlliesInRange(550) >= 2 && team.HealthPercent <= 55) )
                    {
                        var ipRedemption = Prediction.Position.PredictCircularMissile(team, 5500, 275, 2500, 3200);

                        if (ipRedemption.HitChancePercent >= 50)
                        {
                            Redemption.Cast(ipRedemption.CastPosition);
                        }
                    }
                    else if (team.CountAlliesInRange(350) >= 2 && Redemption_Target.HealthPercent <= 50)
                    {
                        var ieRedemption = Prediction.Position.PredictCircularMissile(Redemption_Target, 5500, 275, 2500, 3200);

                        if (ieRedemption.HitChancePercent >= 50)
                        {
                            Redemption.Cast(ieRedemption.CastPosition);
                        }
                    }
                }
            }

        }

        private static void Active_Item()
        {
            var Delay_Time = Status_Slider(M_Item, "CastDelay");

            if (Quicksilver.IsOwned() && Quicksilver.IsReady() && Status_CheckBox(M_Item, "QSS"))
            {
                if (Status_CheckBox(M_Item, "Poisons") && Player.Instance.HasBuffOfType(BuffType.Poison))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Supression") && Player.Instance.HasBuffOfType(BuffType.Suppression))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Blind") && Player.Instance.HasBuffOfType(BuffType.Blind))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Charm") && Player.Instance.HasBuffOfType(BuffType.Charm))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Fear") && Player.Instance.HasBuffOfType(BuffType.Fear))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Polymorph") && Player.Instance.HasBuffOfType(BuffType.Polymorph))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Silence") && Player.Instance.HasBuffOfType(BuffType.Silence))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Slow") && Player.Instance.HasBuffOfType(BuffType.Slow))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Stun") && Player.Instance.HasBuffOfType(BuffType.Stun))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Snare") && Player.Instance.HasBuffOfType(BuffType.Snare))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Taunt") && Player.Instance.HasBuffOfType(BuffType.Taunt))
                { Core.DelayAction(() => Quicksilver.Cast(), Delay_Time); }
            }

            if (Mercurial.IsOwned() && Mercurial.IsReady() && Status_CheckBox(M_Item, "Scimitar"))
            {
                if (Status_CheckBox(M_Item, "Poisons") && Player.Instance.HasBuffOfType(BuffType.Poison))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Supression") && Player.Instance.HasBuffOfType(BuffType.Suppression))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Blind") && Player.Instance.HasBuffOfType(BuffType.Blind))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Charm") && Player.Instance.HasBuffOfType(BuffType.Charm))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Fear") && Player.Instance.HasBuffOfType(BuffType.Fear))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Polymorph") && Player.Instance.HasBuffOfType(BuffType.Polymorph))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Silence") && Player.Instance.HasBuffOfType(BuffType.Silence))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Slow") && Player.Instance.HasBuffOfType(BuffType.Slow))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Stun") && Player.Instance.HasBuffOfType(BuffType.Stun))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Snare") && Player.Instance.HasBuffOfType(BuffType.Snare))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }

                if (Status_CheckBox(M_Item, "Taunt") && Player.Instance.HasBuffOfType(BuffType.Taunt))
                { Core.DelayAction(() => Mercurial.Cast(), Delay_Time); }
            }
        }   //End Active_Item
    }
}
