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
            if (Player.Instance.CountEnemiesInRange(750) == 0) return;

            if (SpellManager.Q.IsReady())
            {
                var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Magical);

                if (MenuHarass["Harass.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuHarass["Harass.Q.Mana"].Cast<Slider>().CurrentValue && 
                    Qtarget.IsValidTarget(MenuHarass["Harass.Q.Range"].Cast<Slider>().CurrentValue))
                {
                    SpellManager.Q.Cast(Qtarget);
                }
            }

            if (SpellManager.W.IsReady())
            {
                var enemy = EntityManager.Heroes.Enemies.FirstOrDefault(x => !x.IsDead);

                if (MenuHarass["Harass.W.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuHarass["Harass.W.Mana"].Cast<Slider>().CurrentValue &&
                    Player.Instance.CountEnemiesInRange(MenuHarass["Harass.W.Range"].Cast<Slider>().CurrentValue) >= 1)
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

                    if (Player.Instance.Distance(Rtarget) < MenuHarass["Harass.R.Range"].Cast<Slider>().CurrentValue)
                    {
                        var RPrediction = SpellManager.R.GetPrediction(Rtarget);

                        if (RPrediction.HitChance >= HitChance.High)
                        {
                            SpellManager.R.Cast(RPrediction.CastPosition);
                        }
                    }
                }
            }
        }
    }   //End Mode_Harass
}
