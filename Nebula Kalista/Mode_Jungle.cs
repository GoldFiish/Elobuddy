using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaKalista
{  
    internal class Mode_Jungle : Kalista
    {
        public static void JungleClear()
        {
            var monster = EntityManager.MinionsAndMonsters.Get(EntityManager.MinionsAndMonsters.EntityType.Monster, EntityManager.UnitTeam.Both, Player.Instance.ServerPosition, 1200);

            if (monster == null) return;
            
            if (SpellManager.E.IsReady())
            {
                if (MenuFarm["Jungle.E.All"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuFarm["Jungle.Mana"].Cast<Slider>().CurrentValue)
                {
                    if (monster.Any(x => Extensions.IsRendKillable(x)))
                    {
                        SpellManager.E.Cast();
                    }

                }

                if (MenuFarm["Jungle.E.Big"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuFarm["Jungle.Mana"].Cast<Slider>().CurrentValue)
                {
                    if (monster.Any(x => Extensions.IsRendKillable(x) && !x.Name.Contains("Mini")))
                    {
                        SpellManager.E.Cast();
                    }
                }
            }
                        
            if(SpellManager.Q.IsReady())
            {
                if(MenuFarm["Jungle.Q.All"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuFarm["Jungle.Mana"].Cast<Slider>().CurrentValue)
                {
                    foreach (var Qmonster in monster.Where(x => x.Health <= x.Get_Q_Damage_Float() && SpellManager.Q.GetPrediction(x).HitChance >= HitChance.High))
                    {
                        SpellManager.Q.Cast(Qmonster);
                    }
                }

                if (MenuFarm["Jungle.Q.Big"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuFarm["Jungle.Mana"].Cast<Slider>().CurrentValue)
                {
                    foreach (var Qmonster in monster.Where(x => !x.Name.Contains("Mini") && x.Health <= x.Get_Q_Damage_Float() && SpellManager.Q.GetPrediction(x).HitChance >= HitChance.High))
                    {
                        SpellManager.Q.Cast(Qmonster);
                    }
                }
            }
        }   //End JungleClear
    }
}
