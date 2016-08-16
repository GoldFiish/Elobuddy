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

            if (MenuMain["Combo.Q"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsReady() && Player.Instance.ManaPercent > MenuMain["Combo.Q.Mana"].Cast<Slider>().CurrentValue)
            {
                if (!Player.Instance.IsDashing() && Qtarget.IsValidTarget(SpellManager.Q.Range))
                {
                    var QPrediction = SpellManager.Q.GetPrediction(Qtarget);

                    if (QPrediction.HitChance >= HitChance.High)
                    {
                        SpellManager.Q.Cast(QPrediction.CastPosition);
                    }
                }
            }           

            if (MenuMain["Combo.W"].Cast<CheckBox>().CurrentValue && Player.Instance.CountEnemiesInRange(Player.Instance.AttackRange) >= 1)
            {               
                foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(Player.Instance.AttackRange) && x.HasBuff("kalistacoopstrikemarkally")))
                {
                    Player.IssueOrder(GameObjectOrder.AttackTo, target);                       
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
