using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaTeemo
{
    internal class Mode_Combo : Teemo
    {
        public static void Combo()
        {
            if (Player.Instance.IsDead) return;
            if (Player.Instance.CountEnemiesInRange(1500) == 0) return;

            var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Magical);
            var Rtarget = TargetSelector.GetTarget(SpellManager.R.Range, DamageType.Magical);

            if (SpellManager.Q.IsReady())
            {
                if (MenuCombo["Combo.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuCombo["Combo.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if (Qtarget != null)
                    {
                        if (Player.Instance.Distance(Qtarget) > 520 && Player.Instance.Distance(Qtarget) <= 680)
                        {
                            SpellManager.Q.Cast(Qtarget);
                        }
                    }
                }
            }            

            if(EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget() && 
            Player.Instance.Distance(x) >= MenuCombo["Combo.W.Range"].Cast<Slider>().CurrentValue &&
            Player.Instance.Distance(x) <= 800) != null && SpellManager.W.IsReady())
            {
                if(MenuCombo["Combo.W.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuCombo["Combo.W.Mana"].Cast<Slider>().CurrentValue)
                {
                    SpellManager.W.Cast();
                }
            }
            
            if (Rtarget != null && SpellManager.R.IsReady())
            {
                if (MenuCombo["Combo.R.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > MenuCombo["Combo.R.Count"].Cast<Slider>().CurrentValue)
                {
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
        }   //End Combo
    }
}
