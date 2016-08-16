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
            
            if (SpellManager.Q.IsReady() && MenuMain["Harass.Q"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuMain["Harass.Mana"].Cast<Slider>().CurrentValue)
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

            if (SpellManager.E.IsReady() && MenuMain["Harass.E"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuMain["Harass.Mana"].Cast<Slider>().CurrentValue)
            {
                if (EntityManager.MinionsAndMonsters.Minions.Any(x => x.IsValidTarget(1200) && x.Health <= x.Get_E_Damage_Double()))
                {
                    if (EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(SpellManager.E.Range) && x.Distance(Player.Instance.ServerPosition) > 900 && x.GetRendBuff().Count >= 1))
                    {
                        SpellManager.E.Cast();
                    }
                }                

                if (EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(SpellManager.E.Range) && x.Distance(Player.Instance.ServerPosition) > 900 && x.GetRendBuff().Count >= 3))
                {
                    SpellManager.E.Cast();
                }
            }
        }   //End Harass
    }
}
