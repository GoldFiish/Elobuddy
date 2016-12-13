using System.Linq;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;

namespace NebulaNasus.Modes
{
    class Mode_Actives : Nasus
    {
        public static void Active()
        {
            if (Player.Instance.IsDead) return;

            var target = TargetSelector.GetTarget(850, DamageType.True);
            var Jmonster = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= 850 && !x.Name.Contains("Mini") &&
               (x.BaseSkinName.ToLower().Contains("dragon") || x.BaseSkinName.ToLower().Contains("herald") || x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault();

            if (target != null)
            {
                if (Player.Instance.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown)
                {
                    if (!Status_CheckBox(M_Misc, "Misc_Ignite") || !SpellManager.Ignite.IsReady()) return;

                    var Ignite_Damage = Damage.DmgIgnite(target);

                    if (SpellManager.Ignite.IsInRange(target))
                    {
                        if (target.Health <= Ignite_Damage + Player.Instance.GetAutoAttackDamage(target) ||
                            target.Health <= Ignite_Damage + Damage.DmgQ(target) + Player.Instance.GetAutoAttackDamage(target))
                        {
                            SpellManager.Ignite.Cast(target);
                        }

                        if (SpellManager.E.IsReady() && target.Health <= Ignite_Damage + Damage.DmgQ(target) + Damage.DmgE(target) + Player.Instance.GetAutoAttackDamage(target))
                        {
                            SpellManager.Ignite.Cast(target);
                        }
                    }
                }

                if (Status_CheckBox(M_Misc, "Misc_KillSt"))
                {
                    if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target))
                    {
                        if (target.TotalShieldHealth() <= Damage.DmgQ(target))
                        {
                            SpellManager.Q.Cast(target);
                        }
                    }

                    if (SpellManager.E.IsReady() && SpellManager.E.IsInRange(target))
                    {
                        var Epredicticon = SpellManager.E.GetPrediction(target);

                        if (target.TotalShieldHealth() <= Player.Instance.GetSpellDamage(target, SpellSlot.E))
                        {
                            if (Epredicticon.HitChancePercent >= 50)
                            {
                                SpellManager.E.Cast(Epredicticon.CastPosition);
                            }
                        }
                    }
                }
            }

            if (Jmonster != null && target != null && Status_CheckBox(M_Misc, "Misc_JungleSt"))
            {
                if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(Jmonster))
                {
                    if (Jmonster.Health <= Damage.DmgQ(Jmonster))
                    {
                        SpellManager.Q.Cast(Jmonster);
                    }
                }

                if (SpellManager.E.IsReady() && SpellManager.E.IsInRange(Jmonster))
                {
                    if (Jmonster.Distance(target) <= 550)
                    {
                        if (Jmonster.Health <= Player.Instance.GetSpellDamage(target, SpellSlot.E))
                        {
                            SpellManager.E.Cast(Jmonster);
                        }
                    }
                }
            }
        }
    }
}
