using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaKalista
{
    internal class Mode_Combo : Kalista 
    {
        public static void Combo()
        {
            if (Player.Instance.IsDead) return;

            var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);

            //Combo W [ Focus W passive target ]
            if (MenuCombo["Combo.W"].Cast<CheckBox>().CurrentValue && Player.Instance.CountEnemiesInRange(Player.Instance.AttackRange) >= 1)
            {
                foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(Player.Instance.AttackRange) && x.HasBuff("kalistacoopstrikemarkally")))
                {
                    Player.IssueOrder(GameObjectOrder.AttackTo, target);
                }
            }

            //Combo Q - Not Collision check 
            if (Qtarget != null && SpellManager.Q.IsLearned)
            {
                if (MenuCombo["Combo.Q"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsReady() && Player.Instance.ManaPercent > MenuCombo["Combo.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if (Qtarget.IsValidTarget() && Player.Instance.Distance(Qtarget.Position) <= 1200)
                    {                        
                        var QPrediction = SpellManager.Q.GetPrediction(Qtarget);

                        if (QPrediction.HitChance >= HitChance.High)
                        {
                            switch (Combo_QNum)
                            {
                                case 0:     // [ Q ] activation
                                    SpellManager.Q.Cast(QPrediction.CastPosition);
                                    break;

                                case 1:     // [ Q ] + [ E ] Killable
                                    if (Extensions.UndyingBuffs.Any(buff => Qtarget.HasBuff(buff))) return;

                                    if (Qtarget.TotalShieldHealth() <= Extensions.Get_Q_Damage_Float(Qtarget) + Extensions.Get_E_Damage_Float(Qtarget))
                                    {
                                        SpellManager.Q.Cast(QPrediction.CastPosition);
                                    }
                                    break;

                                case 2:     // [ E ] 5 stack
                                    if (Extensions.GetRendBuff(Qtarget).Count >= 5)
                                    {
                                        SpellManager.Q.Cast(QPrediction.CastPosition);
                                    }
                                    break;

                                case 3:     // [ E ] fail
                                    if (SpellManager.E.IsOnCooldown)
                                    {
                                        if (Qtarget.TotalShieldHealth() <= Extensions.Get_Q_Damage_Float(Qtarget))
                                        {
                                            SpellManager.Q.Cast(QPrediction.CastPosition);
                                        }
                                    }
                                    break;
                            }

                            if (Qtarget.TotalShieldHealth() <= Extensions.Get_Q_Damage_Float(Qtarget) && (!Qtarget.HasBuffOfType(BuffType.SpellShield) || !Qtarget.IsInvulnerable))
                            {
                                SpellManager.Q.Cast(QPrediction.CastPosition);
                            }
                        }
                    }
                }
            }

            //Combo E
            if (SpellManager.E.IsLearned)
            {
                if (MenuCombo["Combo.E"].Cast<CheckBox>().CurrentValue && SpellManager.E.IsReady())
                {
                    if (EntityManager.Heroes.Enemies.Any(x => Extensions.IsRendKillable(x) && x.IsValidTarget(1200)))
                    {
                        SpellManager.E.Cast();
                    }
                }
            }
        }   //End Combo
    }
}