using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace NebulaSoraka.Modes
{
    internal class Mode_Item : Soraka
    {
        public static readonly Item Bilgewater  = new Item((int)ItemId.Bilgewater_Cutlass, 550f);
        public static readonly Item BladeKing   = new Item((int)ItemId.Blade_of_the_Ruined_King, 550f);
        public static readonly Item Sheen       = new Item((int)ItemId.Sheen);              //광휘의 검
        public static readonly Item IceGauntlet = new Item((int)ItemId.Iceborn_Gauntlet);   //얼어붙은 건틀릿
        public static readonly Item TriniForce  = new Item((int)ItemId.Trinity_Force);      //삼위일체   
        public static readonly Item Redemption  = new Item((int)ItemId.Redemption);         //구원
        static readonly Item Mikaels            = new Item((int)ItemId.Mikaels_Crucible, 750f);
        static readonly Item Solari             = new Item((int)ItemId.Locket_of_the_Iron_Solari, 600f);

        public static void Items_Use()
        {
            if (Player.Instance.IsDead) return;
            if (Player.Instance.CountEnemiesInRange(1500) == 0) return;

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

            //미카엘
            if (Mikaels.IsOwned() && Mikaels.IsReady() && Status_CheckBox(M_Item, "Item.Mikael"))
            {
                switch (Status_ComboBox(M_Item, "Item.Mikael_Op"))
                {
                    case 0: //Ally
                        foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(750)))
                        {
                            if (Status_CheckBox(M_Item, "Fear") && target.HasBuffOfType(BuffType.Fear))
                            {
                                Mikaels.Cast(target);
                            }
                            else if (Status_CheckBox(M_Item, "Silence") && target.HasBuffOfType(BuffType.Silence))
                            {
                                Mikaels.Cast(target);
                            }
                            else if (Status_CheckBox(M_Item, "Slow") && target.HasBuffOfType(BuffType.Slow))
                            {
                                Mikaels.Cast(target);
                            }
                            else if (Status_CheckBox(M_Item, "Snare") && target.HasBuffOfType(BuffType.Snare))
                            {
                                Mikaels.Cast(target);
                            }
                            if (Status_CheckBox(M_Item, "Stun") && target.HasBuffOfType(BuffType.Stun))
                            {
                                Mikaels.Cast(target);
                            }
                            else if (Status_CheckBox(M_Item, "Taunt") && target.HasBuffOfType(BuffType.Taunt))
                            {
                                Mikaels.Cast(target);
                            }
                        }
                        break;
                    case 1: //Player
                        if (Status_CheckBox(M_Item, "Fear") && Player.Instance.HasBuffOfType(BuffType.Fear))
                        {
                            Mikaels.Cast(Player.Instance);
                        }
                        else if (Status_CheckBox(M_Item, "Silence") && Player.Instance.HasBuffOfType(BuffType.Silence))
                        {
                            Mikaels.Cast(Player.Instance);
                        }
                        else if (Status_CheckBox(M_Item, "Slow") && Player.Instance.HasBuffOfType(BuffType.Slow))
                        {
                            Mikaels.Cast(Player.Instance);
                        }
                        else if (Status_CheckBox(M_Item, "Snare") && Player.Instance.HasBuffOfType(BuffType.Snare))
                        {
                            Mikaels.Cast(Player.Instance);
                        }
                        if (Status_CheckBox(M_Item, "Stun") && Player.Instance.HasBuffOfType(BuffType.Stun))
                        {
                            Mikaels.Cast(Player.Instance);
                        }
                        else if (Status_CheckBox(M_Item, "Taunt") && Player.Instance.HasBuffOfType(BuffType.Taunt))
                        {
                            Mikaels.Cast(Player.Instance);
                        }
                        break;
                }
            }
        }

        public static void Active_Redemption()
        {
            //구원
            if (Redemption.IsOwned() && Redemption.IsReady() && Status_CheckBox(M_Item, "Item.Redemption"))
            {
                var Redemption_Target = TargetSelector.GetTarget(5500, DamageType.Physical);

                if (Redemption_Target == null) return;

                var MyRedemption = Prediction.Position.PredictCircularMissile(Player.Instance, 5500, 275, 2500, 2100); //Temp speed
                
                if (Redemption_Target.IsAttackingPlayer)
                {
                    if (Player.Instance.HealthPercent <= Status_Slider(M_Item, "Item.Redemption.MyHp"))
                    {
                        if (MyRedemption.HitChancePercent >= 50)
                        {
                            Redemption.Cast(MyRedemption.CastPosition);
                        }
                    }
                }

                foreach (var team in EntityManager.Heroes.Allies.Where(x => x.IsValidTarget(5500) && !x.IsMe))
                {
                    if (team.CountAlliesInRange(350) >= 1 && (Redemption_Target.HealthPercent <= 50 || team.HealthPercent <= 55))
                    {
                        var AlRedemption = Prediction.Position.PredictCircularMissile(Redemption_Target, 5500, 275, 2500, 2100); //Temp speed                

                        if (AlRedemption.HitChancePercent >= 80)
                        {
                            Redemption.Cast(AlRedemption.CastPosition);
                        }
                    }
                }
            }
        } 
      
        //솔라리
        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.Instance.IsDead) return;
            if (!Solari.IsOwned() || !Solari.IsReady()) return;
            if (!(args.Target is AIHeroClient) ) return;

            if (!(sender is AIHeroClient || sender is Obj_AI_Turret) || !sender.IsEnemy || sender == null)
            {
                return;
            }

            var team = EntityManager.Heroes.Allies.FirstOrDefault(a => a.IsInRange(args.End, 100) && a.IsValidTarget(600) && !a.IsMe);
            var hero = sender as AIHeroClient;
            var target = (AIHeroClient)args.Target;

            if (hero.IsEnemy && Player.Instance.CountEnemiesInRange(600) >= 1)
            {
                var HitMe = Player.Instance.Distance(args.End + (args.SData.CastRadius / 2)) < 100;

                if (target.IsMe || HitMe)
                {
                    var SpDmgToMe = hero.GetSpellDamage(Player.Instance, args.Slot);
                    var SpDmgPer = (SpDmgToMe / target.TotalShieldHealth()) * 100;
                    var ImDeath = SpDmgPer >= target.HealthPercent || SpDmgToMe >= target.TotalShieldHealth() || hero.GetAutoAttackDamage(Player.Instance, true) >= Player.Instance.TotalShieldHealth();

                    if (target.HealthPercent <= Status_Slider(M_Item, "Item.Solari.MyHp") || ImDeath || SpDmgPer >= Status_Slider(M_Item, "Item.Solari.AMyHp"))
                    {
                        Solari.Cast();
                    }
                }

                if (team != null)
                {
                    var HitTeam = team.Distance(args.End + (args.SData.CastRadius / 2)) < 100;

                    if ((target.IsAlly && !target.IsMe) || HitTeam)
                    {
                        var SpDmgToTeam = hero.GetSpellDamage(target, args.Slot);
                        var SpDmgTeamPer = (SpDmgToTeam / target.TotalShieldHealth()) * 100;
                        var TeamDeath = SpDmgTeamPer >= target.HealthPercent || SpDmgToTeam >= target.TotalShieldHealth() || hero.GetAutoAttackDamage(target, true) >= target.TotalShieldHealth();

                        if (Player.Instance.Distance(target) <= 600)
                        {
                            if (target.HealthPercent <= Status_Slider(M_Item, "Item.Solari.TeamHp") || TeamDeath)
                            {
                                Solari.Cast();
                            }
                        }
                    }
                }
            }
        }

        public static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.Instance.IsDead) return;
            if (!Solari.IsOwned() || !Solari.IsReady()) return;
            if (!(args.Target is AIHeroClient)) return;

            if (!(sender is AIHeroClient || sender is Obj_AI_Turret) || !sender.IsEnemy || sender == null)
            {
                return;
            }

            var hero = sender as AIHeroClient;
            var target = (AIHeroClient)args.Target;

            if (hero.IsEnemy && target != null)
            {
                var AttackPercent = (sender.GetAutoAttackDamage(target, true) / target.TotalShieldHealth()) * 100;
                var Death = sender.GetAutoAttackDamage(target, true) >= target.TotalShieldHealth() || AttackPercent >= target.HealthPercent;

                if (target.IsMe)
                {
                    if (target.HealthPercent <= Status_Slider(M_Item, "Item.Solari.MyHp") || Death || AttackPercent >= Status_Slider(M_Item, "Item.Solari.AMyHp"))
                    {
                        Solari.Cast();
                    }
                }

                if (target.IsAlly && !target.IsMe)
                {
                    if (Player.Instance.Distance(target) <= 600)
                    {
                        if (target.HealthPercent <= Status_Slider(M_Item, "Item.Solari.TeamHp") || Death)
                        {
                            Solari.Cast();
                        }
                    }
                }
            }
        }
    }
}
