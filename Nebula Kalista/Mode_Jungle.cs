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
            var monster = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= 1200);

            if (monster != null)
            {
                //Jungle E
                if (SpellManager.E.IsLearned && SpellManager.E.IsReady())
                {
                    //Jungle E_All Type 
                    if (MenuJungle["Jungle.E.All"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuJungle["Jungle.Mana"].Cast<Slider>().CurrentValue)
                    {
                        if (monster.Any(x => Extensions.IsRendKillable(x)))
                        {
                            SpellManager.E.Cast();
                        }
                    }

                    //Jungle E_Big Type
                    if (MenuJungle["Jungle.E.Big"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuJungle["Jungle.Mana"].Cast<Slider>().CurrentValue)
                    {
                        if (monster.Any(x => Extensions.IsRendKillable(x) && !x.Name.Contains("Mini")))
                        {
                            SpellManager.E.Cast();
                        }
                    }
                }

                //Jungle Q
                if (SpellManager.Q.IsLearned && SpellManager.Q.IsReady())
                {
                    //Jungle Q_All Type 
                    if (MenuJungle["Jungle.Q.All"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuJungle["Jungle.Mana"].Cast<Slider>().CurrentValue)
                    {
                        foreach (var Qmonster in monster.Where(x => x.Health <= x.Get_Q_Damage_Float() && SpellManager.Q.GetPrediction(x).HitChance >= HitChance.High))
                        {
                            SpellManager.Q.Cast(Qmonster);
                        }
                    }

                    //Jungle Q_Big Type
                    if (MenuJungle["Jungle.Q.Big"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuJungle["Jungle.Mana"].Cast<Slider>().CurrentValue)
                    {
                        foreach (var Qmonster in monster.Where(x => !x.Name.Contains("Mini") && x.Health <= x.Get_Q_Damage_Float() && SpellManager.Q.GetPrediction(x).HitChance >= HitChance.High))
                        {
                            SpellManager.Q.Cast(Qmonster);
                        }
                    }
                }
            }
        }   //End JungleClear
    }
}
