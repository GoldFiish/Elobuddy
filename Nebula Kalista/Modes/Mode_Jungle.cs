using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace NebulaKalista.Modes
{  
    internal class Mode_Jungle : Kalista
    {
        public static void JungleClear()
        {
            var monster = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(1200));

            if (monster != null)
            {
                //Jungle E
                if (SpellManager.E.IsLearned && SpellManager.E.IsReady())
                {
                    if (Player.Instance.ManaPercent > Status_Slider(MenuLane, "Jungle_E_Mana"))
                    {
                        switch (Status_ComboBox(MenuLane, "Jungle_E_Mode"))
                        {
                            case 0:
                                break;
                            case 1:
                                if (monster.Any(x => Extensions.IsRendKillable(x)))
                                {
                                    SpellManager.E.Cast();
                                }
                                break;
                            case 2:
                                if (monster.Any(x => Extensions.IsRendKillable(x) && !x.Name.Contains("Mini")))
                                {
                                    SpellManager.E.Cast();
                                }
                                break;
                            case 3:
                                if (monster.Any(x => Extensions.IsRendKillable(x) && x.Name.Contains("Mini")))
                                {
                                    SpellManager.E.Cast();
                                }
                                break;
                        }
                    }
                }

                //Jungle Q
                if (SpellManager.Q.IsLearned && SpellManager.Q.IsReady())
                {
                    if (Player.Instance.ManaPercent > Status_Slider(MenuLane, "Jungle_Q_Mana"))
                    {
                        switch (Status_ComboBox(MenuLane, "Jungle_Q_Mode"))
                        {
                            case 0:
                                monster = null;
                                break;
                            case 1:
                                monster = monster.OrderBy(x => x.Health);
                                break;
                            case 2:
                                monster = monster.Where(x => !x.Name.Contains("Mini")).OrderBy(x => x.Health);
                                break;
                            case 3:
                                monster = monster.Where(x => x.Name.Contains("Mini")).OrderBy(x => x.Health);
                                break;
                        }

                        var Qtarget = monster.FirstOrDefault();

                        if (Qtarget.Health <= Extensions.Get_Q_Damage_Float(Qtarget) && SpellManager.Q.GetPrediction(Qtarget).HitChance >= HitChance.High)
                        {
                            SpellManager.Q.Cast(SpellManager.Q.GetPrediction(Qtarget).CastPosition);
                        }
                    }
                }
            }
        }   //End JungleClear
    }
}
