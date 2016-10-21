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

            var minion = EntityManager.MinionsAndMonsters.Get(EntityManager.MinionsAndMonsters.EntityType.Minion, EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, 1200);
           
            if (minion == null) return;

            if (SpellManager.Q.IsReady() && MenuLane["Lane.Minions.Big"].Cast<CheckBox>().CurrentValue)
            {
                var target = minion.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) < 1200 && (x.BaseSkinName.ToLower().Contains("siege") || x.BaseSkinName.ToLower().Contains("super"))).FirstOrDefault();

                if(target != null)
                {
                    if (target.Distance(Player.Instance.Position) > 450 && target.Distance(Player.Instance.Position) <= 680 && target.Health <= Damage.DmgQ(target) && Player.Instance.CountEnemiesInRange(850) >= 1)
                    {
                        SpellManager.Q.Cast(target);
                    }
                }
            }

            if (SpellManager.R.IsReady() && MenuLane["Lane.R.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuLane["Lane.R.Mana"].Cast<Slider>().CurrentValue)
            {
                if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > MenuLane["Lane.R.RCount"].Cast<Slider>().CurrentValue)
                {
                    foreach (var m in minion.Where(x => x.HealthPercent >= 25))
                    {
                        var RPrediction = Prediction.Position.PredictCircularMissile(m, SpellManager.R.Range, 135, 1000, 1000);
                        var PoisonCount = RPrediction.GetCollisionObjects<Obj_AI_Minion>().Count(x => x.HealthPercent >= 35);

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
