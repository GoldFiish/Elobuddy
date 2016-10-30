using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace NebulaKalista
{
    internal class Mode_Harass : Kalista
    {
        public static void Harass()
        {
            if (Player.Instance.IsDead) return;

            var Qtarget = TargetSelector.GetTarget(1350, DamageType.Physical);

            //Harass Q
            if (Qtarget != null && SpellManager.Q.IsLearned)
            {
                if (SpellManager.Q.IsReady() && MenuHarass["Harass.Q"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuHarass["Harass.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if (Qtarget.IsValidTarget() && Player.Instance.Distance(Qtarget) <= SpellManager.Q.Range)
                    {
                        var QPrediction = SpellManager.Q.GetPrediction(Qtarget);
                        var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= SpellManager.Q.Range);

                        if (QPrediction.HitChance >= HitChance.High)
                        {
                            SpellManager.Q.Cast(QPrediction.CastPosition);
                        }

                        foreach (var m in (from m in minion
                                           let p1 = new Geometry.Polygon.Rectangle((Vector2)Player.Instance.Position, Player.Instance.Position.Extend(m.Position, SpellManager.Q.Range), SpellManager.Q.Width)
                                           where 
                                           minion.Count(x => p1.IsInside(x.Position) && x.Health >= Extensions.Get_Q_Damage_Float(x)) <= 0 &&
                                           minion.Count(x => p1.IsInside(x.Position) && x.Health <= Extensions.Get_Q_Damage_Float(x)) >= MenuHarass["Harass.Q.MCount"].Cast<Slider>().CurrentValue &&
                                           p1.IsInside(QPrediction.CastPosition)
                                           select m))
                        {
                            SpellManager.Q.Cast(QPrediction.CastPosition);
                        }
                    }
                }
            }

            //Harass E
            if (SpellManager.E.IsLearned)
            {
                if (SpellManager.E.IsReady() && MenuHarass["Harass.E"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuHarass["Harass.E.Mana"].Cast<Slider>().CurrentValue)
                {
                    var target = TargetSelector.GetTarget(1200, DamageType.Physical);

                    if (target != null) 
                    {
                        if (EntityManager.MinionsAndMonsters.EnemyMinions.Count(x => x.IsValidTarget(1200) && x.Health <= Extensions.Get_E_Damage_Float(x)) >= MenuHarass["Harass.E.MCount"].Cast<Slider>().CurrentValue)
                        {
                            if (Player.Instance.Distance(target) > 700 && target.GetBuffCount("kalistaexpungemarker") >= MenuHarass["Harass.E.CStack"].Cast<Slider>().CurrentValue)
                            {
                                SpellManager.E.Cast();
                            }
                        }
                    }
                }
            }
        }   //End Harass
    }
}
