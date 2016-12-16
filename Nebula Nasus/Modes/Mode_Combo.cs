using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace NebulaNasus.Modes
{
    class Mode_Combo : Nasus
    {
        public static void Combo()
        {
            if (Player.Instance.IsDead) return;

            var target = TargetSelector.GetTarget(750, DamageType.Mixed);

            if (target != null)
            {
                if (Status_CheckBox(M_Main, "Combo_W") && SpellManager.W.IsReady() && SpellManager.W.IsInRange(target))
                {
                    if (Player.Instance.Distance(target) <= Status_Slider(M_Main, "Combo_W_Dis"))
                    {
                        SpellManager.W.Cast(target);
                    }
                    else if (target.CountEnemiesInRange(450) >= 1 && Player.Instance.Distance(target) >= 450)
                    {
                        SpellManager.W.Cast(target);
                    }
                }

                if (Status_CheckBox(M_Main, "Combo_E") && SpellManager.E.IsReady() && SpellManager.E.IsInRange(target))
                {
                    var Eprediction = SpellManager.E.GetPrediction(target);

                    if (Eprediction.HitChance >= HitChance.High)
                    {
                        if (target.HasBuff("NasusW"))
                        {
                            SpellManager.E.Cast(Eprediction.CastPosition);
                        }
                        else if (target.CountAlliesInRange(550) >= 2)
                        {
                            SpellManager.E.Cast(Eprediction.CastPosition);
                        }
                        else if (target.CountEnemiesInRange(450) >= 1 && Player.Instance.Distance(target) >= 450)
                        {
                            SpellManager.E.Cast(Eprediction.CastPosition);
                        }
                        else if (Player.Instance.Distance(target) <= 450)
                        {
                            SpellManager.E.Cast(Eprediction.CastPosition);
                        }
                    }
                }

                if (Status_CheckBox(M_Main, "Combo_Q") && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target))
                {
                    if (!target.IsInvulnerable || !target.HasUndyingBuff() || !target.IsZombie)
                    {
                        if (target.TotalShieldHealth() <= Damage.DmgQ(target))
                        {
                            SpellManager.Q.Cast(target);
                        }
                        else if (target.Health >
                                (Player.Instance.Spellbook.GetSpell(SpellSlot.Q).Cooldown / Player.Instance.AttackDelay) * Player.Instance.GetAutoAttackDamage(target))
                        {
                            SpellManager.Q.Cast(target);
                        }
                    }
                }

                if (Status_CheckBox(M_Main, "Combo_R") && Player.Instance.HealthPercent <= Status_Slider(M_Main, "Combo_R_Hp"))
                {
                    SpellManager.R.Cast();
                }
            }
        }
    }
}
