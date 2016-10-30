using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace NebulaKalista
{
    internal class Mode_Always : Kalista
    {
        public static void Always()
        {
            if (Player.Instance.IsDead) return;

            if (SpellManager.R.IsLearned && SpellManager.R.IsReady())
            {
                var Partner = EntityManager.Heroes.Allies.FirstOrDefault(x => !x.IsMe);

                if (Partner == null) return;

                if (Partner.HasBuff("kalistacoopstrikeally"))
                {
                    if (Partner.IsDead) return;

                    //Save partner
                    if (MenuMisc["R.Save"].Cast<CheckBox>().CurrentValue)
                    {
                        if (Partner.HealthPercent <= MenuMisc["R.Save.Hp"].Cast<Slider>().CurrentValue && Player.Instance.Distance(Partner.Position) <= SpellManager.R.Range && Partner.CountEnemiesInRange(1500) > 0)
                        {
                            SpellManager.R.Cast();
                        }
                    }

                    //Balista - Blitzcrank, Skarner, TahmKench
                    if (MenuMisc["R.LongGrap"].Cast<CheckBox>().CurrentValue)
                    {
                        if (Partner.ChampionName == ("Blitzcrank") || Partner.ChampionName == ("Skarner") || Partner.ChampionName == ("TahmKench"))
                        {
                            foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget() && x.IsHPBarRendered && Player.Instance.Distance(x) >= MenuMisc["R.LongGrap.Dis"].Cast<Slider>().CurrentValue))
                            {
                                if (MenuMisc["R." + enemy.ChampionName].Cast<CheckBox>().CurrentValue)
                                {
                                    if (enemy.HasBuff("rocketgrab2") || enemy.HasBuff("skarnerimpale") || enemy.HasBuff("tahmkenchwdevoured"))
                                    {
                                        SpellManager.R.Cast();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Auto Killsteal
            if (MenuMisc["E.KillSteal"].Cast<CheckBox>().CurrentValue)
            {
                var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);

                if (SpellManager.E.IsReady() && EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(1200) && Extensions.IsRendKillable(x)))
                {
                    SpellManager.E.Cast();
                }

                if (Qtarget != null && SpellManager.Q.IsLearned && Qtarget.TotalShieldHealth() <= Extensions.Get_Q_Damage_Float(Qtarget))
                {
                    var QPrediction = SpellManager.Q.GetPrediction(Qtarget);
                    var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= SpellManager.Q.Range);

                    if(QPrediction.HitChancePercent >= 50)
                    {
                        SpellManager.Q.Cast(QPrediction.CastPosition);
                    }

                    foreach (var m in (from m in minion let p1 = new Geometry.Polygon.Rectangle((Vector2)Player.Instance.Position, Player.Instance.Position.Extend(m.Position, SpellManager.Q.Range), SpellManager.Q.Width)
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

            //Auto Monster steal
            if (MenuMisc["E.MonsterSteal"].Cast<CheckBox>().CurrentValue)
            {
                var target = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(1200) && !x.Name.Contains("Mini") &&
                (x.BaseSkinName.ToLower().Contains("dragon") || x.BaseSkinName.ToLower().Contains("herald") || x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault();

                if (target == null) return;

                if (SpellManager.Q.IsReady() && target.Health <= Extensions.Get_Q_Damage_Float(target))
                {
                    var QPrediction = SpellManager.Q.GetPrediction(target);

                    if (QPrediction.HitChancePercent >= 70)
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
            if (MenuMisc["E.Death"].Cast<CheckBox>().CurrentValue && Player.Instance.HealthPercent <= MenuMisc["E.Death.Hp"].Cast<Slider>().CurrentValue)
            {
                if (SpellManager.E.IsReady())
                {
                    foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= 1200 && x.HasRendBuff()))
                    {
                        SpellManager.E.Cast();
                    }
                }
            } 
        }   //End Always
    }
}
