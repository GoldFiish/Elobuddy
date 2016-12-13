using EloBuddy;
using EloBuddy.SDK;
namespace NebulaNasus.Modes
{
    class Mode_Flee : Nasus
    {
        public static void Flee()
        {
            if (Player.Instance.IsDead) return;

            var target = TargetSelector.GetTarget(750, DamageType.Mixed);

            if (target != null)
            {
                if (Status_CheckBox(M_Main, "Flee_W") && SpellManager.W.IsReady() && SpellManager.W.IsInRange(target))
                {
                    SpellManager.W.Cast(target);
                }

                if (Status_CheckBox(M_Main, "Flee_E") && SpellManager.E.IsReady() && SpellManager.E.IsInRange(target))
                {
                    var Eprediction = SpellManager.E.GetPrediction(target);

                    if (Eprediction.HitChancePercent >= 50)
                    {
                        SpellManager.E.Cast(Eprediction.CastPosition);
                    }
                }

                if (Status_CheckBox(M_Main, "Flee_R") && SpellManager.R.IsReady() && Player.Instance.HealthPercent <= Status_Slider(M_Main, "Flee_R_Hp"))
                {
                    SpellManager.R.Cast();
                }
            }
        }
    }
}
