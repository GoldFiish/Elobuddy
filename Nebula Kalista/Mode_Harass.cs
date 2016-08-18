using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;

namespace NebulaKalista
{
    internal class Mode_Harass : Kalista
    {
        public static void Harass()
        {
            var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);

            if (Qtarget == null) return;
            
            if (SpellManager.Q.IsReady() && MenuMain["Harass.Q"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuMain["Harass.Q.Mana"].Cast<Slider>().CurrentValue)
            {
                if (!Player.Instance.IsDashing() && Qtarget.IsValidTarget(SpellManager.Q.Range))
                {
                    var QPrediction = SpellManager.Q.GetPrediction(Qtarget);

                    if (QPrediction.HitChance >= HitChance.High)
                    {
                        SpellManager.Q.Cast(QPrediction.CastPosition);
                    }
                }
            }

            if (SpellManager.E.IsReady() && MenuMain["Harass.E"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuMain["Harass.E.Mana"].Cast<Slider>().CurrentValue)
            {
                var target = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(1200));

                if (target.FirstOrDefault(x => x.HasRendBuff()) == null) return;
               
                if (EntityManager.MinionsAndMonsters.Minions.Where(x => x.IsValidTarget(1200)).FirstOrDefault(x => x.Health <= x.Get_E_Damage_Float()) != null)
                {
                    if (target.FirstOrDefault(x => x.HasRendBuff()) != null)
                    {
                        SpellManager.E.Cast();
                    }
                }
                
                if (target.Where(x => x.Distance(Player.Instance.ServerPosition) > 700).FirstOrDefault(x => x.GetBuffCount("kalistaexpungemarker") >= MenuMain["Harass.E.Stack"].Cast<Slider>().CurrentValue) != null)
                {
                    SpellManager.E.Cast();
                }
            }
        }   //End Harass
    }
}
