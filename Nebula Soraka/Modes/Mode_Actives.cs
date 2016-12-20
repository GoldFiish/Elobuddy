using System.Linq;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaSoraka.Modes
{
    class Mode_Actives : Soraka
    {
        public static void Active()
        {
            if (Player.Instance.IsDead) return;

            if (Player.Instance.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown && Status_CheckBox(M_Misc, "Misc_Ignite"))
            {
                var Ignite_target = TargetSelector.GetTarget(600, DamageType.True);

                if (Ignite_target != null && SpellManager.Ignite.IsReady())
                {
                    if (Ignite_target.Health <= Damage.DmgCla(Ignite_target))
                    {
                        SpellManager.Ignite.Cast(Ignite_target);
                    }
                }
            }

            var target = TargetSelector.GetTarget(925, DamageType.Mixed);

            if (target != null)
            {
                if (Status_CheckBox(M_Misc, "Misc_KillSt"))
                {
                    if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target))
                    {
                        var Qpredicticon = SpellManager.E.GetPrediction(target);

                        if (target.TotalShieldHealth() <= Damage.DmgQ(target) && Qpredicticon.HitChancePercent >= 50)
                        {
                            SpellManager.Q.Cast(Qpredicticon.CastPosition);
                        }
                    }

                    if (SpellManager.E.IsReady() && SpellManager.E.IsInRange(target))
                    {
                        var Epredicticon = SpellManager.E.GetPrediction(target);

                        if (target.TotalShieldHealth() <= Damage.DmgQ(target) && Epredicticon.HitChancePercent >= 50)
                        {
                            SpellManager.E.Cast(Epredicticon.CastPosition);
                        }
                    }
                }
                
                if (Status_CheckBox(M_Auto, "Auto_Q") && Player.Instance.ManaPercent >= Status_Slider(M_Auto, "Auto_Q_Mana") && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target))
                {
                    if (!target.IsInvulnerable || !target.HasUndyingBuff() || !target.IsZombie)
                    {
                        var prediction = Mode_Combo.GetQPrediction(target);

                        if (prediction.HitChancePercent >= Status_Slider(M_Auto, "Auto_Q_Hit"))
                        {
                            SpellManager.Q.Cast(prediction.CastPosition);
                        }
                    }
                }                                
            }

            if (Status_CheckBox(M_Auto, "Auto_W") && SpellManager.W.IsReady() && Player.Instance.HealthPercent > Status_Slider(M_Auto, "Auto_W_MyHp"))
            {
                var team = ObjectManager.Get<AIHeroClient>().Where(x => x.IsAlly && x.IsValidTarget(SpellManager.W.Range + 100) && !x.IsMe &&
                                                                    x.HealthPercent <= Status_Slider(M_Auto, "Auto_W_TeamHp") && !x.IsRecalling() && !x.IsInShopRange());
                if (team != null)
                {
                    switch (Status_ComboBox(M_Auto, "Auto_W_Target"))
                    {
                        case 0: //Health  
                            team = team.OrderBy(x => x.Health);
                            break;
                        case 1: //AD"
                            team = team.OrderByDescending(x => x.TotalAttackDamage);
                            break;
                        case 2: //AP
                            team = team.OrderByDescending(x => x.TotalMagicalDamage);
                            break;
                    }

                    var Wtarget = team.FirstOrDefault();

                    if (Wtarget != null && SpellManager.W.IsInRange(Wtarget) && !Player.Instance.IsRecalling())
                    {
                        SpellManager.W.Cast(Wtarget);
                    }
                }
            }

            if (Status_CheckBox(M_Auto, "Auto_R") && SpellManager.R.IsReady())
            {
                foreach (var Rtarget in EntityManager.Heroes.Allies.Where(x => x.IsValidTarget() && x.IsHPBarRendered && !x.IsRecalling() && !x.IsInShopRange()))
                {
                    if (Rtarget.CountEnemiesInRange(450) >= 1 && Rtarget.HealthPercent < Status_Slider(M_Auto, "Auto_R_TeamHp"))
                    {
                        SpellManager.R.Cast();
                    }
                }

                if (target != null && target.IsAttackingPlayer)
                {
                    if (Player.Instance.HealthPercent < Status_Slider(M_Auto, "Auto_R_MyHp"))
                    {
                        SpellManager.R.Cast();
                    }
                }
            }
        }
    }
}
