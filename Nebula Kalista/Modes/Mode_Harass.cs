using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace NebulaKalista.Modes
{
    internal class Mode_Harass : Kalista
    {
        public static void Harass()
        {
            if (Player.Instance.IsDead) return;

            var target = TargetSelector.GetTarget(1350, DamageType.Physical);

            if(target != null)
            {
                if (SpellManager.E.IsLearned && SpellManager.E.IsReady() && Status_CheckBox(MenuHarass, "Harass_E") && Player.Instance.ManaPercent > Status_Slider(MenuHarass, "Harass_E_Mana"))
                {
                    if (target.IsValidTarget(1200) && Player.Instance.Distance(target) > 700)
                    {
                        if (EntityManager.MinionsAndMonsters.EnemyMinions.Count(x => x.IsValidTarget(1200) && x.Health <= Extensions.Get_E_Damage_Float(x)) >= Status_Slider(MenuHarass, "Harass_E_MCount"))
                        {
                            if (target.GetBuffCount("kalistaexpungemarker") >= Status_Slider(MenuHarass, "Harass_E_CStack"))
                            {
                                SpellManager.E.Cast();
                            }
                        }
                    }
                }

                if (SpellManager.Q.IsLearned && SpellManager.Q.IsReady() && Status_CheckBox(MenuHarass, "Harass_Q") && Player.Instance.ManaPercent > Status_Slider(MenuHarass, "Harass_Q_Mana"))
                {
                    if(target.IsValidTarget() && Player.Instance.Distance(target) <= 1200)
                    {
                        var QPrediction = SpellManager.Q.GetPrediction(target);
                        var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget(SpellManager.Q.Range));

                        if (QPrediction.HitChance >= HitChance.High)
                        {
                            SpellManager.Q.Cast(QPrediction.CastPosition);
                        }

                        foreach (var m in (from m in minion
                                           let p1 = new Geometry.Polygon.Rectangle((Vector2)Player.Instance.Position, Player.Instance.Position.Extend(m.Position, SpellManager.Q.Range), SpellManager.Q.Width)
                                           where
                                           minion.Count(x => p1.IsInside(x.Position) && x.Health >= Extensions.Get_Q_Damage_Float(x)) <= 0 &&
                                           minion.Count(x => p1.IsInside(x.Position) && x.Health <= Extensions.Get_Q_Damage_Float(x)) >= Status_Slider(MenuHarass, "Harass_Q_MCount") &&
                                           p1.IsInside(QPrediction.CastPosition)
                                           select m))
                        {
                            SpellManager.Q.Cast(QPrediction.CastPosition);
                        }
                    }
                }               
            }
        }   //End Harass
    }
}
