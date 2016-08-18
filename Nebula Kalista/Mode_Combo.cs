using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;

namespace NebulaKalista
{
    internal class Mode_Combo : Kalista 
    {
        public static void Combo()
        {
            var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);

            if (Qtarget == null) return;

            if (MenuMain["Combo.W"].Cast<CheckBox>().CurrentValue && Player.Instance.CountEnemiesInRange(Player.Instance.AttackRange) >= 1)
            {
                foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(Player.Instance.AttackRange) && x.HasBuff("kalistacoopstrikemarkally")))
                {
                    Player.IssueOrder(GameObjectOrder.AttackTo, target);
                }
            }

            if (MenuMain["Combo.Q"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsReady() && Player.Instance.ManaPercent > MenuMain["Combo.Q.Mana"].Cast<Slider>().CurrentValue)
            {
                if (!Player.Instance.IsDashing() && Qtarget.IsValidTarget(SpellManager.Q.Range))
                {
                    var QPrediction = SpellManager.Q.GetPrediction(Qtarget);

                    if (QPrediction.HitChance < HitChance.High)
                    {
                        if (MenuMain["Combo.Q.Style"].Cast<ComboBox>().SelectedIndex == 0)
                        {
                            SpellManager.Q.Cast(QPrediction.CastPosition);
                        }
                        else
                        {
                            if (Qtarget.Health <= Extensions.Get_Q_Damage_Float(Qtarget) + Extensions.Get_E_Damage_Double(Qtarget) && !Qtarget.HasBuffOfType(BuffType.SpellShield))
                            {
                                SpellManager.Q.Cast(QPrediction.CastPosition);
                            }
                        }
                    }

                    if (Qtarget.IsValidTarget(SpellManager.Q.Range) && Qtarget.Distance(Player.Instance.ServerPosition) > SpellManager.E.Range && !Qtarget.HasBuffOfType(BuffType.SpellShield))
                    {
                        if (QPrediction.HitChancePercent >= 70)
                        {
                            SpellManager.Q.Cast(QPrediction.CastPosition);
                        }
                    }
                }
            }

            if (MenuMain["Combo.E"].Cast<CheckBox>().CurrentValue && SpellManager.E.IsReady())
            {
                if (EntityManager.Heroes.Enemies.Any(x => Extensions.IsRendKillable(x) && x.IsValidTarget(1200)))
                {
                    SpellManager.E.Cast();
                }
            }           
        }   //End Combo
    }
}
