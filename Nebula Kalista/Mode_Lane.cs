using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaKalista
{
   public class Mode_Lane
    {
        public static void LaneClear(AttackableUnit target = null)
        {
            if (EntityManager.MinionsAndMonsters.Minions.Any(x => x.Health <= x.Get_E_Damage_Double() && (x.BaseSkinName.ToLower().Contains("siege") || x.BaseSkinName.ToLower().Contains("super"))))
            {
                SpellManager.E.Cast();
            }

            if (Kalista.MenuFarm["Lane.E"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.ManaPercent > Kalista.MenuFarm["Lane.E.Mana"].Cast<Slider>().CurrentValue)
            {
                var minion = EntityManager.MinionsAndMonsters.Get(EntityManager.MinionsAndMonsters.EntityType.Minion, EntityManager.UnitTeam.Enemy, ObjectManager.Player.ServerPosition, SpellManager.E.Range);
                var minion_count = minion.Count(x => x.Health <= x.Get_E_Damage_Double());
                if (minion_count >= Kalista.MenuFarm["Lane.E.Num"].Cast<Slider>().CurrentValue)
                {
                    SpellManager.E.Cast();
                }
            }

            if (Kalista.MenuFarm["Lane.Q"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.ManaPercent > Kalista.MenuFarm["Lane.Q.Mana"].Cast<Slider>().CurrentValue)
            {
                foreach (var minion in EntityManager.MinionsAndMonsters.Get(EntityManager.MinionsAndMonsters.EntityType.Minion, EntityManager.UnitTeam.Enemy,
                    ObjectManager.Player.ServerPosition, SpellManager.Q.Range))
                {
                    var hit_chance = SpellManager.Q.GetPrediction(minion);
                    var minion_count = hit_chance.GetCollisionObjects<Obj_AI_Minion>().Count(x => x.Health <= x.Get_Q_Damage_Float() && x.IsMinion);

                    if (hit_chance.HitChance >= HitChance.High) { return; }
                    if (minion_count >= Kalista.MenuFarm["Lane.Q.Num"].Cast<Slider>().CurrentValue)
                    {
                        SpellManager.Q.Cast(minion.ServerPosition);
                    }
                }
            }                     
        }
    }
}
