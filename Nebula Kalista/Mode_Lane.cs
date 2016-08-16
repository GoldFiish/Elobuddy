using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaKalista
{
   internal class Mode_Lane : Kalista 
    {
        public static void LaneClear() //AttackableUnit target = null
        {
            var minion = EntityManager.MinionsAndMonsters.Get(EntityManager.MinionsAndMonsters.EntityType.Minion, EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, 1200);
            
            if (minion == null) return;
            
            if (SpellManager.E.IsReady() && minion.Any(x => x.IsValidTarget(SpellManager.E.Range) && (x.BaseSkinName.ToLower().Contains("siege") || x.BaseSkinName.ToLower().Contains("super")) &&
            x.Health <= x.Get_E_Damage_Double()))
            {
                SpellManager.E.Cast(); 
            }

            if (MenuFarm["Lane.E.All"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuFarm["Lane.E.Mana"].Cast<Slider>().CurrentValue && SpellManager.E.IsReady())
            {
                var minion_num = minion.Count(x => x.IsValidTarget(SpellManager.E.Range) && x.Health <= x.Get_E_Damage_Double());

                if (minion_num >= MenuFarm["Lane.E.Num"].Cast<Slider>().CurrentValue)
                {
                    SpellManager.E.Cast();
                }
            }

            if (MenuFarm["Lane.Q"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuFarm["Lane.Q.Mana"].Cast<Slider>().CurrentValue && SpellManager.Q.IsReady())
            {
                var Qminion = EntityManager.MinionsAndMonsters.Get(EntityManager.MinionsAndMonsters.EntityType.Minion, EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, SpellManager.Q.Range);

                foreach (var Qminions in Qminion.Where(x => x.Health <= x.Get_Q_Damage_Float()))
                {
                    var Qtarget = SpellManager.Q.GetPrediction(Qminions);
                    var Qtarget_num = Qtarget.GetCollisionObjects<Obj_AI_Minion>().Count(x => x.Health <= x.Get_Q_Damage_Float());
                    
                    if (Qtarget.HitChance >= HitChance.High) return;

                    if (Qtarget_num >= MenuFarm["Lane.Q.Num"].Cast<Slider>().CurrentValue)
                    {
                        SpellManager.Q.Cast(Qtarget.CastPosition);
                        //SpellManager.Q.CastOnBestFarmPosition(Qtarget_num);
                    }
                }
            }
        }   //  End LaneClear
    }
}

