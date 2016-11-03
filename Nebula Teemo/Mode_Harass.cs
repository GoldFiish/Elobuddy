using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaTeemo
{
    internal class Mode_Harass : Teemo
    {       
        public static void Harass()
        {
            if (Player.Instance.IsDead) return;

            var enemy = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(1500)).FirstOrDefault();            

            if (enemy != null)
            {
                if (SpellManager.Q.IsReady())
                {
                    if (MenuHarass["Harass.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuHarass["Harass.Q.Mana"].Cast<Slider>().CurrentValue)
                    {
                        if (Player.Instance.Distance(enemy) > Player.Instance.AttackRange && Player.Instance.Distance(enemy) <= SpellManager.Q.Range)
                        {
                            SpellManager.Q.Cast(enemy);
                        }
                    }
                }

                if (SpellManager.W.IsReady())
                {
                    if (MenuHarass["Harass.W.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuHarass["Harass.W.Mana"].Cast<Slider>().CurrentValue &&
                          Player.Instance.Distance(enemy) >= MenuHarass["Harass.W.Range"].Cast<Slider>().CurrentValue && Player.Instance.Distance(enemy) <= 800)
                    {
                        SpellManager.W.Cast();
                    }
                }

                if (SpellManager.R.IsReady())
                {
                    if (MenuHarass["Harass.R.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuHarass["Harass.R.Mana"].Cast<Slider>().CurrentValue &&
                        Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > MenuHarass["Harass.R.Count"].Cast<Slider>().CurrentValue)
                    {
                        var Rtarget = TargetSelector.GetTarget(SpellManager.R.Range, DamageType.Magical);
                        var RCastPrediction = Prediction.Position.PredictCircularMissile(enemy, SpellManager.R.Range, 135, 1000, 1000);
                        if ((enemy.Spellbook.IsCastingSpell && Player.Instance.IsTargetable) || enemy.IsAttackingPlayer)
                        {
                            if (SpellManager.R.IsInRange(enemy))
                            {
                                if (RCastPrediction.HitChance >= HitChance.High)
                                {
                                    SpellManager.R.Cast(RCastPrediction.CastPosition);
                                }
                            }
                        }

                        if (SpellManager.R.IsInRange(Rtarget))
                        {
                            var RPrediction = Prediction.Position.PredictCircularMissile(Rtarget, SpellManager.R.Range, 135, 1000, 1000);

                            if (RPrediction.HitChance >= HitChance.High)
                            {
                                SpellManager.R.Cast(RPrediction.CastPosition);
                            }
                        }
                    }
                }
            }
        }   //End Harass
    }
}
