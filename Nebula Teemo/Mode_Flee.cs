using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;

namespace NebulaTeemo
{
    internal class Mode_Flee : Teemo
    {
        public static void Flee()
        {
            if (Player.Instance.IsDead) return;

            var Qtarget = TargetSelector.GetTarget(MenuFlee["Flee.Q.Range"].Cast<Slider>().CurrentValue, DamageType.Magical);
            var Rtarget = TargetSelector.GetTarget(SpellManager.R.Range, DamageType.Magical);

            if (Qtarget != null && SpellManager.Q.IsReady() && MenuFlee["Flee.Q.Use"].Cast<CheckBox>().CurrentValue)
            {
                if (Qtarget.IsValidTarget())
                {
                    SpellManager.Q.Cast(Qtarget);
                }
            }

            if (Rtarget != null && SpellManager.R.IsReady() && MenuFlee["Flee.R.Use"].Cast<CheckBox>().CurrentValue)
            {
                if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > 1)
                {
                    if (Rtarget.IsValidTarget() && Player.Instance.Distance(Rtarget) <= SpellManager.R.Range)
                    {
                        var RPrediction = Prediction.Position.PredictCircularMissile(Rtarget, SpellManager.R.Range, 135, 1000, 1000);

                        if (RPrediction.HitChance >= HitChance.Medium)
                        {
                            SpellManager.R.Cast(RPrediction.CastPosition);
                        }
                    }
                }
            }

            if (SpellManager.W.IsReady() && MenuFlee["Flee.W.Use"].Cast<CheckBox>().CurrentValue)
            {
                SpellManager.W.Cast();
            }
        }
    }
}
