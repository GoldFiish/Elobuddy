using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace NebulaKalista.Modes
{
    internal class Mode_Combo : Kalista 
    {
        public static void Combo()
        {
            if (Player.Instance.IsDead) return;

            var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);
           
            if (Status_CheckBox(MenuCombo, "Combo_W") && Player.Instance.CountEnemyChampionsInRange(Player.Instance.AttackRange) >= 1)
            {
                foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(Player.Instance.AttackRange) && x.HasBuff("kalistacoopstrikemarkally")))
                {
                    Player.IssueOrder(GameObjectOrder.AttackTo, target);
                }
            }
            
            if (Qtarget != null && SpellManager.Q.IsLearned)
            {
                if (Status_CheckBox(MenuCombo, "Combo_Q") && SpellManager.Q.IsReady() && Player.Instance.ManaPercent > Status_Slider(MenuCombo, "Combo_Q_Mana"))
                {
                    if (Qtarget.IsValidTarget() && Player.Instance.Distance(Qtarget.Position) <= 1200)
                    {                        
                        var QPrediction = SpellManager.Q.GetPrediction(Qtarget);

                        if (QPrediction.HitChance >= HitChance.High)
                        {
                            switch (Status_ComboBox(MenuCombo, "Combo_Q_Style"))
                            {
                                case 0:     // [ Q ] activation
                                    SpellManager.Q.Cast(QPrediction.CastPosition);
                                    break;

                                case 1:     // [ Q ] + [ E ] Killable                                    
                                    if (!Qtarget.HasUndyingBuff() || !Qtarget.IsInvulnerable || !Qtarget.HasBuffOfType(BuffType.SpellShield))
                                    {
                                        if (Qtarget.TotalShieldHealth() <= Extensions.IsQEKillable(Qtarget))
                                        {
                                            SpellManager.Q.Cast(QPrediction.CastPosition);
                                        }
                                    }
                                    break;
                                case 2:     // [ E ] 5 stack
                                    if (Extensions.GetRendBuff(Qtarget).Count >= 5)
                                    {
                                        SpellManager.Q.Cast(QPrediction.CastPosition);
                                    }
                                    break;

                                case 3:     // [ E ] fail
                                    if (!SpellManager.E.IsReady() && (!Qtarget.HasUndyingBuff() || !Qtarget.IsInvulnerable || !Qtarget.HasBuffOfType(BuffType.SpellShield)))
                                    {
                                        if (Qtarget.TotalShieldHealth() <= Extensions.Get_Q_Damage_Float(Qtarget))
                                        {
                                            SpellManager.Q.Cast(QPrediction.CastPosition);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            
            if (SpellManager.E.IsLearned)
            {
                if (Status_CheckBox(MenuCombo, "Combo_E") && SpellManager.E.IsReady())
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