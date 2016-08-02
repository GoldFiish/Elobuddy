using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;

namespace NebulaTeemo
{
    internal class Mode_Lane : Teemo
    {        
        public static void Lane()
        {
            if (Player.Instance.IsDead) return;

            var minion = EntityManager.MinionsAndMonsters.Get(EntityManager.MinionsAndMonsters.EntityType.Minion, EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, 900);
           
            if (minion == null) return;

            if (SpellManager.Q.IsReady() && MenuLane["Lane.Minions.Big"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var Qtarget in EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => (x.BaseSkinName.ToLower().Contains("siege") || x.BaseSkinName.ToLower().Contains("super"))))
                {
                    if (Qtarget.Distance(Player.Instance.ServerPosition) > 450 && Qtarget.IsValidTarget(680) && Qtarget.Health <= Damage.DmgQ(Qtarget))
                    {                       
                        SpellManager.Q.Cast(Qtarget);
                    }

                    if (Qtarget.IsValidTarget(550) && Qtarget.Health <= Damage.DmgE(Qtarget))
                    {
                        Player.IssueOrder(GameObjectOrder.AttackTo, Qtarget);
                    }
                }
            }

            if (SpellManager.R.IsReady() && MenuLane["Lane.R.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuLane["Lane.R.Mana"].Cast<Slider>().CurrentValue)
            {
                if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > MenuLane["Lane.R.RCount"].Cast<Slider>().CurrentValue)
                {
                    foreach (var m in minion.Where(x => x.HealthPercent >= 90))
                    {
                        var RPrediction = Prediction.Position.PredictCircularMissile(m, SpellManager.R.Range, 135, 0, 1000);
                        var PoisonCount = RPrediction.GetCollisionObjects<Obj_AI_Minion>().Count(x => x.HealthPercent >= 70);

                        if (PoisonCount >= MenuLane["Lane.R.PCount"].Cast<Slider>().CurrentValue && RPrediction.HitChance >= HitChance.High)
                        {   
                            SpellManager.R.Cast(RPrediction.CastPosition);
                        }
                    }
                }
            }
        }
    }   //End Mode_Lane
}
