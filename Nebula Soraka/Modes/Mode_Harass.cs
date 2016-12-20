using EloBuddy;
using EloBuddy.SDK;

namespace NebulaSoraka.Modes
{
    class Mode_Harass : Soraka
    {
        public static void Harass()
        {
            if (Player.Instance.IsDead) return;

            var target = TargetSelector.GetTarget(925, DamageType.Magical);

            if (target != null)
            {
                if (Status_CheckBox(M_Main, "Harass_Q") && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target) && Player.Instance.ManaPercent > Status_Slider(M_Main, "Harass_Q_Mana"))
                {
                    if (!target.IsInvulnerable || !target.HasUndyingBuff() || !target.IsZombie)
                    {
                        var Qprediction = SpellManager.Q.GetPrediction(target);

                        if (Qprediction.HitChancePercent >= 50)
                        {
                            SpellManager.Q.Cast(target);
                        }
                    }
                }

                if (Status_CheckBox(M_Main, "Harass_E") && SpellManager.E.IsReady() && SpellManager.E.IsInRange(target) && Player.Instance.ManaPercent > Status_Slider(M_Main, "Harass_E_Mana"))
                {
                    var Eprediction = SpellManager.E.GetPrediction(target);

                    if (Eprediction.HitChancePercent >= 50)
                    {
                        SpellManager.E.Cast(Eprediction.CastPosition);

                    }
                }
            }
        }   //End Harass
    }   //End Class Mode_Harass
}
