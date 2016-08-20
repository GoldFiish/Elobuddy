using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaKalista
{
    internal class Mode_Always : Kalista
    {
        public static void Always()
        {
            if (MenuMisc["R.Save"].Cast<CheckBox>().CurrentValue && SpellManager.R.IsReady())
            {
                var Partner = EntityManager.Heroes.Allies.Where(x => !x.IsMe && x.HasBuff("kalistacoopstrikeally")).FirstOrDefault();

                if (Partner == null || Partner.IsDead) return;

                if (Partner.HealthPercent <= MenuMisc["R.Save.Hp"].Cast<Slider>().CurrentValue && Player.Instance.Distance(Partner.Position) <= SpellManager.R.Range && Partner.CountEnemiesInRange(1500) > 0)
                {
                    SpellManager.R.Cast();
                }
            }

            if (MenuMisc["E.KillSteal"].Cast<CheckBox>().CurrentValue) 
            {
                if (SpellManager.E.IsReady() && EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(1200) && Extensions.IsRendKillable(x)))
                {
                    SpellManager.E.Cast();
                }

                var target = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget() && Player.Instance.Distance(x.Position) <= 1300 && x.Health <= Extensions.Get_Q_Damage_Float(x)).FirstOrDefault();              

                if (target != null)
                {
                    var QPrediction = SpellManager.Q.GetPrediction(target);

                    if (SpellManager.Q.IsReady() && QPrediction.HitChancePercent >= 70)
                    {
                        SpellManager.Q.Cast(QPrediction.CastPosition);
                    }
                }
            }

            if (MenuMisc["E.MonsterSteal"].Cast<CheckBox>().CurrentValue)
            {
                var target = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(1200) && !x.Name.Contains("Mini") &&
                (x.BaseSkinName.ToLower().Contains("dragon") || x.BaseSkinName.ToLower().Contains("herald") || x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault();
                
                if (target == null) return;

                if (SpellManager.Q.IsReady() && target.Health <= Extensions.Get_Q_Damage_Float(target))
                {
                    var QPrediction = SpellManager.Q.GetPrediction(target);

                    if (SpellManager.Q.IsReady() && QPrediction.HitChancePercent >= 80)
                    {
                        SpellManager.Q.Cast(QPrediction.CastPosition);
                    }
                }

                if (SpellManager.E.IsReady() && Extensions.IsRendKillable(target))
                {
                    SpellManager.E.Cast();
                }
            }
           
            if (MenuMisc["E.Death"].Cast<CheckBox>().CurrentValue && Player.Instance.HealthPercent <= MenuMisc["E.Death.Hp"].Cast<Slider>().CurrentValue)
            {
                if (!SpellManager.E.IsReady()) return;

                foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget() && Player.Instance.Distance(x.Position) <= 1200 && x.HasRendBuff()))
                {
                    SpellManager.E.Cast();
                }
            }           
        }   //End Always
    }
}
