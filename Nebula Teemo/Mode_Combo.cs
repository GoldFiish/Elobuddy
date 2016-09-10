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
            if (Player.Instance.CountEnemiesInRange(1200) == 0) return;            
            var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Magical);
            var Rtarget = TargetSelector.GetTarget(SpellManager.R.Range, DamageType.Magical);
            
            var ItsEnemy = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(1200)).FirstOrDefault();
            var ItsMe = EntityManager.Heroes.AllHeroes.Where(x => x.IsMe).FirstOrDefault();

            if (SpellManager.Q.IsReady())
            {
                if (MenuCombo["Combo.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuCombo["Combo.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if (Qtarget.IsValidTarget() && Player.Instance.Distance(Qtarget) > 500 && Player.Instance.Distance(Qtarget) <= 680)
                    {
                        SpellManager.Q.Cast(Qtarget);
                    }
                }
            }

            if (SpellManager.W.IsReady())
            {
                if (MenuCombo["Combo.W.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuCombo["Combo.W.Mana"].Cast<Slider>().CurrentValue &&
                   Player.Instance.Distance(ItsEnemy) >= MenuCombo["Combo.W.Range"].Cast<Slider>().CurrentValue && Player.Instance.Distance(ItsEnemy) <= 800)
                {
                    SpellManager.W.Cast();
                }
            }

            if (SpellManager.R.IsReady())
            {
                if (MenuCombo["Combo.R.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > MenuCombo["Combo.R.Count"].Cast<Slider>().CurrentValue)
                {
                    if (Rtarget.IsValidTarget() && Player.Instance.Distance(Rtarget) <= SpellManager.R.Range) // SpellManager.R.Range - 150? // && !Rtarget.HasBuffOfType(BuffType.Poison))
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
