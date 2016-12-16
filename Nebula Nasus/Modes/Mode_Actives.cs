using System.Linq;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaNasus.Modes
{
    class Mode_Actives : Nasus
    {
        public static void Active()
        {
            if (Player.Instance.IsDead) return;

            if (Player.Instance.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown && Status_CheckBox(M_Misc, "Misc_Ignite"))
            {
                var Ignite_target = TargetSelector.GetTarget(600, DamageType.True);

                if (Ignite_target != null && SpellManager.Ignite.IsReady())
                {
                    if (Player.Instance.Distance(Ignite_target) <= 220 && Ignite_target .Health <= Damage.DmgCla(Ignite_target))
                    {
                        SpellManager.Ignite.Cast(Ignite_target);
                    }
                    else if (Player.Instance.Distance(Ignite_target) > 220 && SpellManager.Ignite.IsInRange(Ignite_target))
                    {
                        if(Ignite_target.Health <= Damage.DmgIgnite(Ignite_target) + Damage.DmgE(Ignite_target))
                        {
                            SpellManager.Ignite.Cast(Ignite_target);
                        }
                    }
                }
            }

            if (Status_CheckBox(M_Misc, "Misc_KillSt"))
            {
                var KStarget = TargetSelector.GetTarget(850, DamageType.Mixed);

                if (KStarget != null)
                {
                    if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(KStarget))
                    {
                        if (KStarget.TotalShieldHealth() <= Damage.DmgQ(KStarget))
                        {
                            SpellManager.Q.Cast(KStarget);
                        }
                    }

                    if (SpellManager.E.IsReady() && SpellManager.E.IsInRange(KStarget))
                    {
                        var Epredicticon = SpellManager.E.GetPrediction(KStarget);

                        if (KStarget.TotalShieldHealth() <= Damage.DmgQ(KStarget) && Epredicticon.HitChancePercent >= 50)
                        {
                            switch (M_Misc["Misc_KillStE"].Cast<ComboBox>().CurrentValue)
                            {
                                case 0:
                                    SpellManager.E.Cast(Epredicticon.CastPosition);
                                    break;

                                case 1:
                                    if (Player.Instance.Distance(KStarget) > 220)
                                    {
                                        SpellManager.E.Cast(Epredicticon.CastPosition);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            if (Status_CheckBox(M_Misc, "Misc_JungleSt"))
            {
                var Jmonster = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= 850 && !x.Name.Contains("Mini") &&
                 (x.BaseSkinName.ToLower().Contains("dragon") || x.BaseSkinName.ToLower().Contains("herald") || x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault();

                if (Jmonster != null)
                {
                    if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(Jmonster))
                    {
                        if (Jmonster.Health <= Damage.DmgQ(Jmonster))
                        {
                            SpellManager.Q.Cast(Jmonster);
                        }
                    }

                    if (SpellManager.E.IsReady() && SpellManager.E.IsInRange(Jmonster) && Player.Instance.CountAlliesInRange(1200) >= 1)
                    {
                        if (Jmonster.Health <= Damage.DmgE(Jmonster))
                        {
                            SpellManager.E.Cast(Jmonster);
                        }
                    }
                }
            }
        }
    }
}
