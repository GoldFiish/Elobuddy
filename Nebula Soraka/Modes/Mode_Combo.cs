using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace NebulaSoraka.Modes
{
    class Mode_Combo : Soraka
    {
        public static PredictionResult GetQPrediction(AIHeroClient target)
        {
            float divider = target.Position.Distance(Player.Instance.Position) / SpellManager.Q.Range;
            SpellManager.Q.CastDelay = (int)(0.2f + 0.8f * divider);
            var prediction = SpellManager.Q.GetPrediction(target);
            return prediction;
        }

        public static void Combo()
        {
            if (Player.Instance.IsDead) return;

            var target = TargetSelector.GetTarget(925, DamageType.Magical);

            if (target != null)
            {
                if (Status_CheckBox(M_Main, "Combo_Q") && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target))
                {
                    if (!target.IsInvulnerable || !target.HasUndyingBuff() || !target.IsZombie)
                    {
                        //var Qprediction = SpellManager.Q.GetPrediction(target);

                        //if (Qprediction.HitChance >= HitChance.High)
                        //{
                        //    SpellManager.Q.Cast(target);
                        //}

                        //Test
                        var prediction = GetQPrediction(target);

                        if (prediction.HitChance >= HitChance.High)
                        {
                            SpellManager.Q.Cast(prediction.CastPosition);
                        }
                    }
                }
                
                if (Status_CheckBox(M_Main, "Combo_E") && SpellManager.E.IsReady() && SpellManager.E.IsInRange(target))
                {
                    var Eprediction = SpellManager.E.GetPrediction(target);

                    if (Eprediction.HitChance >= HitChance.High)
                    {
                        SpellManager.E.Cast(Eprediction.CastPosition);
                       
                    }
                }
            }
        }
    }
}