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
            if (Player.Instance.CountEnemiesInRange(1700) == 0) return;

            var ItsEnemy = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(1500)).FirstOrDefault();
            var ItsMe = EntityManager.Heroes.AllHeroes.Where(x => x.IsMe).FirstOrDefault();

            if (ItsEnemy != null)
            {
                if (SpellManager.Q.IsReady())
                {
                    if (MenuHarass["Harass.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuHarass["Harass.Q.Mana"].Cast<Slider>().CurrentValue)
                    {
                        if (ItsEnemy.IsValidTarget() && Player.Instance.Distance(ItsEnemy) > Player.Instance.AttackRange && Player.Instance.Distance(ItsEnemy) <= SpellManager.Q.Range)
                        {
                            SpellManager.Q.Cast(ItsEnemy);
                        }
                    }
                }

                if (SpellManager.W.IsReady())
                {
                    if (MenuHarass["Harass.W.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuHarass["Harass.W.Mana"].Cast<Slider>().CurrentValue &&
                          Player.Instance.Distance(ItsEnemy) >= MenuHarass["Harass.W.Range"].Cast<Slider>().CurrentValue && Player.Instance.Distance(ItsEnemy) <= 800)
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
                        var RCastPrediction = Prediction.Position.PredictCircularMissile(ItsEnemy, SpellManager.R.Range, 135, 1000, 1000);

                        if ((ItsEnemy.Spellbook.IsCastingSpell && ItsMe.IsTargetable) || ItsEnemy.IsAttackingPlayer)
                        {
                            if (ItsEnemy.IsValidTarget() && Player.Instance.Distance(ItsEnemy) < SpellManager.R.Range)
                            {
                                if (RCastPrediction.HitChance >= HitChance.High)
                                {
                                    SpellManager.R.Cast(RCastPrediction.CastPosition);
                                }
                            }
                        }

                        if (Player.Instance.Distance(Rtarget) < SpellManager.R.Range)
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
