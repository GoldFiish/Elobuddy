using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace NebulaKalista.Modes
{
    internal class Mode_Always : Kalista
    {
        public static void Always()
        {
            if (Player.Instance.IsDead) return;

            if (SpellManager.R.IsReady())
            {
                var Partner = EntityManager.Heroes.Allies.Where(x => Player.Instance.Distance(x) <= 1150 && x.HasBuff("kalistacoopstrikeally") && !x.IsMe && !x.IsDead).FirstOrDefault();

                if (Partner == null) return;

                if (Status_CheckBox(MenuMisc, "R_Save") && SpellManager.R.IsInRange(Partner) && Partner.CountEnemyChampionsInRange(1100) >= 1)
                {
                    if (Partner.HealthPercent <= Status_Slider(MenuMisc, "R_Save_Hp"))
                    {
                        SpellManager.R.Cast();
                    }
                }

                if (Status_CheckBox(MenuMisc, "R_LongGrap") && (Partner.ChampionName == ("Blitzcrank") || Partner.ChampionName == ("Skarner") || Partner.ChampionName == ("TahmKench")))
                {
                    if (Player.Instance.Distance(Partner) >= Status_Slider(MenuMisc, "R_LongGrap_Dis"))
                    {
                        foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget() && (x.HasBuff("rocketgrab2") || x.HasBuff("skarnerimpale") || x.HasBuff("tahmkenchwdevoured"))))
                        {
                            if (Status_CheckBox(MenuMisc, "R_" + enemy.ChampionName))
                            {
                                SpellManager.R.Cast();
                            }
                        }
                    }
                }
            }

            //Auto Killsteal
            if (Status_CheckBox(MenuMisc, "E_KillSteal"))
            {
                var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);

                if (SpellManager.E.IsReady() && EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(1200) && Extensions.IsRendKillable(x)))
                {
                    SpellManager.E.Cast();
                }

                if (Qtarget != null && SpellManager.Q.IsLearned && SpellManager.Q.IsReady() && (!Qtarget.IsInvulnerable || !Qtarget.HasUndyingBuff()))
                {
                    if (Qtarget.TotalShieldHealth() <= Extensions.Get_Q_Damage_Float(Qtarget))
                    {
                        var QPrediction = SpellManager.Q.GetPrediction(Qtarget);
                        var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget(SpellManager.Q.Range));

                        if (QPrediction.HitChancePercent >= 50)
                        {
                            SpellManager.Q.Cast(QPrediction.CastPosition);
                        }

                        foreach (var m in (from m in minion
                                           let p1 = new Geometry.Polygon.Rectangle((Vector2)Player.Instance.Position, Player.Instance.Position.Extend(m.Position, SpellManager.Q.Range), SpellManager.Q.Width)
                                           where
                                           minion.Count(x => p1.IsInside(x.Position) && x.Health >= Extensions.Get_Q_Damage_Float(x)) <= 0 &&
                                           minion.Count(x => p1.IsInside(x.Position) && x.Health <= Extensions.Get_Q_Damage_Float(x)) >= 1 &&
                                           p1.IsInside(QPrediction.CastPosition)
                                           select m))
                        {
                            SpellManager.Q.Cast(QPrediction.CastPosition);
                        }
                    }
                }
            }

            //Auto Monster steal
            if (Status_CheckBox(MenuMisc, "E_MonsterSteal"))
            {
                var target = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(1200) && !x.Name.Contains("Mini") &&
                (x.BaseSkinName.ToLower().Contains("dragon") || x.BaseSkinName.ToLower().Contains("herald") || x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault();

                if (target == null) return;

                if (SpellManager.Q.IsReady() && target.Health <= Extensions.Get_Q_Damage_Float(target))
                {
                    var QPrediction = SpellManager.Q.GetPrediction(target);

                    if (QPrediction.HitChancePercent >= 50)
                    {
                        SpellManager.Q.Cast(QPrediction.UnitPosition);
                    }
                }

                if (SpellManager.E.IsReady() && Extensions.IsRendKillable(target))
                {
                    SpellManager.E.Cast();
                }
            }

            //Auto before death
            if (Status_CheckBox(MenuMisc, "E_Death") && SpellManager.E.IsReady() && Player.Instance.HealthPercent <= Status_Slider(MenuMisc, "E_Death_Hp"))
            {
                if(Player.Instance.CountAllyChampionsInRange(1100) >= 1)
                {
                    SpellManager.E.Cast();
                }
            }
        }   //End Always
    }
}
