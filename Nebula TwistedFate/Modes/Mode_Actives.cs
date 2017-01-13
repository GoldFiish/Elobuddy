using System.Linq;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaTwistedFate.Modes
{
    class Mode_Actives : TwistedFate
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

            var target = TargetSelector.GetTarget(1450, DamageType.Mixed);

            if (target != null)
            {
                if (!target.HasUndyingBuff() || !target.IsInvulnerable || !target.IsZombie)
                {
                    if (Status_CheckBox(M_Misc, "Misc_KillSt"))
                    {
                        if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target))
                        {
                            var Qpredicticon = SpellManager.Q.GetPrediction(target);

                            if (target.TotalShieldHealth() <= Damage.DmgQ(target) && Qpredicticon.HitChancePercent >= 50)
                            {
                                SpellManager.Q.Cast(Qpredicticon.CastPosition);
                            }
                        }

                        if ((target.TotalShieldHealth() <= Damage.DmgECla(target) || target.TotalShieldHealth() <= Damage.DmgCla(target)) && Player.Instance.Distance(target) < Player.Instance.AttackRange)
                        {
                            Player.IssueOrder(GameObjectOrder.AttackTo, target);
                        }
                    }

                    if (Status_CheckBox(M_Misc, "Misc_Auto_Q") && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target))
                    {
                        if (target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Knockup) ||
                            target.HasBuffOfType(BuffType.Suppression) || target.HasBuffOfType(BuffType.Charm) || target.IsRecalling())
                        {
                            SpellManager.Q.Cast(target);
                        }
                    }
                }
            }

            var target_monster = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= 800 && !x.Name.Contains("Mini") &&
               (x.BaseSkinName.ToLower().Contains("dragon") || x.BaseSkinName.ToLower().Contains("herald") || x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault();

            if (target_monster != null && SpellManager.Q.IsInRange(target_monster) && SpellManager.Q.IsReady())
            {
                if (Status_CheckBox(M_Misc, "Misc_JungSt"))
                {
                    if (target_monster.Health <= Damage.DmgQ(target_monster))
                    {
                        SpellManager.Q.Cast(target_monster);
                    }

                    if (target_monster.Health <= Damage.DmgECla(target_monster))
                    {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target_monster);
                    }

                    if (target_monster.Health <= Damage.DmgCla(target_monster))
                    {
                        if (SpellManager.Q.IsReady())
                        {
                            SpellManager.Q.Cast(target_monster);
                        }

                        Player.IssueOrder(GameObjectOrder.AttackUnit, target_monster);
                        Orbwalker.ResetAutoAttack();
                    }
                }
            }
        }
    }
}
