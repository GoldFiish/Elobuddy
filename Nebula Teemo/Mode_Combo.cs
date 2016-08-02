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
            if (Player.Instance.CountEnemiesInRange(750) == 0) return;

            var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Magical);
            var Rtarget = TargetSelector.GetTarget(SpellManager.R.Range, DamageType.Magical);

            if (SpellManager.Q.IsReady())
            {
                if (MenuCombo["Combo.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuCombo["Combo.Q.Mana"].Cast<Slider>().CurrentValue &&
                    Qtarget.IsValidTarget(SpellManager.Q.Range))
                {
                    SpellManager.Q.Cast(Qtarget);
                }          
            }

            if (SpellManager.W.IsReady())
            {
                var enemy = EntityManager.Heroes.Enemies.FirstOrDefault(x => !x.IsDead);

                if (MenuCombo["Combo.W.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuCombo["Combo.W.Mana"].Cast<Slider>().CurrentValue &&
                   Player.Instance.Distance(enemy) > MenuCombo["Combo.W.Range"].Cast<Slider>().CurrentValue && Player.Instance.Distance(enemy) < 720)
                {
                    SpellManager.W.Cast();
                }
            }

            if (SpellManager.R.IsReady())
            {
                if (MenuCombo["Combo.R.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > MenuCombo["Combo.R.Count"].Cast<Slider>().CurrentValue)
                {
                    if (Rtarget.IsValidTarget(SpellManager.R.Range)) // && !Rtarget.HasBuffOfType(BuffType.Poison))
                    {
                        var RPrediction = SpellManager.R.GetPrediction(Rtarget);

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
