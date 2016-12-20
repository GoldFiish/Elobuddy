using EloBuddy;
using EloBuddy.SDK;
namespace NebulaSoraka.Modes
{
    class Mode_Flee : Soraka
    {
        public static void Flee()
        {
            if (Player.Instance.IsDead) return;

            var target = TargetSelector.GetTarget(925, DamageType.Magical);

            if (target != null)
            {
                if (Status_CheckBox(M_Main, "Flee_W") && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target))
                {
                    var Qprediction = SpellManager.Q.GetPrediction(target);

                    if (Qprediction.HitChancePercent >= 50)
                    {
                        SpellManager.Q.Cast(target);
                    }
                }
            }

            if (Status_CheckBox(M_Main, "Flee_E") && SpellManager.E.IsReady() && SpellManager.E.IsInRange(target))
            {
                var Eprediction = SpellManager.E.GetPrediction(target);

                if (Eprediction.HitChancePercent >= 50)
                {
                    SpellManager.E.Cast(Eprediction.CastPosition);

                }
            }
        }
    }
}