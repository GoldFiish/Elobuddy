using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaTeemo
{
    internal class Mode_Steal : Teemo
    {
        public static void KillSteal()
        {
            if (Player.Instance.IsDead) return;

            var target_enemy = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= 1200).FirstOrDefault();
            
            if(target_enemy != null)
            {
                if (MenuMisc["Steal.K.0"].Cast<CheckBox>().CurrentValue)
                {
                    if (Player.Instance.Distance(target_enemy) <= Player.Instance.AttackRange)
                    {
                        //E || AD
                        if (target_enemy.Health <= Damage.DmgE(target_enemy) ||
                            target_enemy.Health <= Player.Instance.TotalAttackDamage)
                        {
                            Player.IssueOrder(GameObjectOrder.AttackTo, target_enemy);
                        }

                        if (Mode_Item.Hextech.IsOwned() && Mode_Item.Hextech.IsReady())
                        {
                            //E + Hextech || AD + Hextech
                            if (target_enemy.Health <= Damage.DmgE(target_enemy) + Damage.DmgHextech(target_enemy) ||
                                target_enemy.Health <= Player.Instance.TotalAttackDamage + Damage.DmgHextech(target_enemy))
                            {
                                Mode_Item.Hextech.Cast(target_enemy);
                                Player.IssueOrder(GameObjectOrder.AttackTo, target_enemy);
                            }
                        }

                        if (Mode_Item.Bilgewater.IsOwned() && Mode_Item.Bilgewater.IsReady())
                        {
                            //E + Bilgewater || AD + Bilgewater
                            if (target_enemy.Health <= Damage.DmgE(target_enemy) + Damage.Bilgewater(target_enemy) ||
                                target_enemy.Health <= Player.Instance.TotalAttackDamage + Damage.Bilgewater(target_enemy))
                            {
                                Mode_Item.Bilgewater.Cast(target_enemy);
                                Player.IssueOrder(GameObjectOrder.AttackTo, target_enemy);
                            }
                        }
                    }

                    if (Mode_Item.Ignite.Slot != SpellSlot.Unknown && Mode_Item.Ignite.IsReady() && Player.Instance.Distance(target_enemy) <= 600)
                    {
                        //Ignite
                        if (target_enemy.Health <= Damage.DmgIgnite)
                        {
                            Mode_Item.Ignite.Cast(target_enemy);
                        }

                        //Ignite + E || Ignite + AD || Ignite + E + AD
                        if (Player.Instance.Distance(target_enemy) <= Player.Instance.AttackRange)
                        {
                            if (target_enemy.Health <= Damage.DmgIgnite + Damage.DmgE(target_enemy) ||
                            target_enemy.Health <= Damage.DmgIgnite + Player.Instance.TotalAttackDamage ||
                            target_enemy.Health <= Damage.DmgIgnite + Damage.DmgE(target_enemy) + Player.Instance.TotalAttackDamage)
                            {
                                Mode_Item.Ignite.Cast(target_enemy);
                                Player.IssueOrder(GameObjectOrder.AttackTo, target_enemy);
                            }
                        }

                        if (SpellManager.Q.IsReady())
                        {
                            //Ignite + Q
                            if (target_enemy.Health <= Damage.DmgIgnite + Damage.DmgQ(target_enemy))
                            {
                                Mode_Item.Ignite.Cast(target_enemy);
                                SpellManager.Q.Cast(target_enemy);
                            }

                            //Ignite + Q + E || Ignite + Q + AD
                            if (Player.Instance.Distance(target_enemy) <= Player.Instance.AttackRange)
                            {
                                if (target_enemy.Health <= Damage.DmgIgnite + Damage.DmgQ(target_enemy) + Damage.DmgE(target_enemy) ||
                                target_enemy.Health <= Damage.DmgIgnite + Damage.DmgQ(target_enemy) + Player.Instance.TotalAttackDamage)
                                {
                                    Mode_Item.Ignite.Cast(target_enemy);
                                    Player.IssueOrder(GameObjectOrder.AttackTo, target_enemy);
                                    SpellManager.Q.Cast(target_enemy);
                                }
                            }
                        }

                        if (Mode_Item.Hextech.IsOwned() && Mode_Item.Hextech.IsReady())
                        {
                            //Ignite + Hextech
                            if (target_enemy.Health <= Damage.DmgIgnite + Damage.DmgHextech(target_enemy))
                            {
                                Mode_Item.Hextech.Cast(target_enemy);
                                Mode_Item.Ignite.Cast(target_enemy);
                            }

                            if (SpellManager.Q.IsReady())
                            {
                                //Ignite + Q + Hextech
                                if (target_enemy.Health <= Damage.DmgIgnite + Damage.DmgQ(target_enemy) + Damage.DmgHextech(target_enemy))
                                {
                                    Mode_Item.Hextech.Cast(target_enemy);
                                    Mode_Item.Ignite.Cast(target_enemy);
                                    SpellManager.Q.Cast(target_enemy);
                                }

                                //Ignite + Q + E + Hextech || Ignite + Q + AD + Hextech
                                if (Player.Instance.Distance(target_enemy) <= Player.Instance.AttackRange)
                                {
                                    if (target_enemy.Health <= Damage.DmgIgnite + Damage.DmgQ(target_enemy) + Damage.DmgE(target_enemy) + Damage.DmgHextech(target_enemy) ||
                                    target_enemy.Health <= Damage.DmgIgnite + Damage.DmgQ(target_enemy) + Player.Instance.TotalAttackDamage + Damage.DmgHextech(target_enemy))
                                    {
                                        Mode_Item.Hextech.Cast(target_enemy);
                                        Mode_Item.Ignite.Cast(target_enemy);
                                        Player.IssueOrder(GameObjectOrder.AttackTo, target_enemy);
                                        SpellManager.Q.Cast(target_enemy);
                                    }
                                }
                            }
                        }

                        if (Mode_Item.Bilgewater.IsOwned() && Mode_Item.Bilgewater.IsReady() && Player.Instance.Distance(target_enemy) <= 550)
                        {
                            //Ignite + Bilgewater
                            if (target_enemy.Health <= Damage.DmgIgnite + Damage.Bilgewater(target_enemy))
                            {
                                Mode_Item.Bilgewater.Cast(target_enemy);
                                Mode_Item.Ignite.Cast(target_enemy);
                            }

                            //Ignite + Q + Bilgewater
                            if (target_enemy.Health <= Damage.DmgIgnite + Damage.DmgQ(target_enemy) + Damage.Bilgewater(target_enemy))
                            {
                                Mode_Item.Bilgewater.Cast(target_enemy);
                                Mode_Item.Ignite.Cast(target_enemy);
                                SpellManager.Q.Cast(target_enemy);
                            }

                            //Ignite + Q + E + Bilgewater || Ignite + Q + AD + Bilgewater
                            if (Player.Instance.Distance(target_enemy) <= Player.Instance.AttackRange)
                            {
                                if (target_enemy.Health <= Damage.DmgIgnite + Damage.DmgQ(target_enemy) + Damage.DmgE(target_enemy) + Damage.Bilgewater(target_enemy) ||
                                target_enemy.Health <= Damage.DmgIgnite + Damage.DmgQ(target_enemy) + Player.Instance.TotalAttackDamage + Damage.Bilgewater(target_enemy))
                                {
                                    Mode_Item.Bilgewater.Cast(target_enemy);
                                    Mode_Item.Ignite.Cast(target_enemy);
                                    Player.IssueOrder(GameObjectOrder.AttackTo, target_enemy);
                                    SpellManager.Q.Cast(target_enemy);
                                }
                            }
                        }
                    }
                    
                    if(SpellManager.Q.IsReady())
                    {
                        //Q
                        if (target_enemy.Health <= Damage.DmgQ(target_enemy))
                        {
                            SpellManager.Q.Cast(target_enemy);
                        }

                        //Q + E || Q + AD
                        if (Player.Instance.Distance(target_enemy) <= Player.Instance.AttackRange)
                        {
                            if (target_enemy.Health <= Damage.DmgQ(target_enemy) + Damage.DmgE(target_enemy) ||
                            target_enemy.Health <= Damage.DmgQ(target_enemy) + Player.Instance.TotalAttackDamage)
                            {
                                SpellManager.Q.Cast(target_enemy);
                                Player.IssueOrder(GameObjectOrder.AttackTo, target_enemy);
                            }
                        }

                        if (Mode_Item.Hextech.IsOwned() && Mode_Item.Hextech.IsReady())
                        {
                            //Q + Hextech
                            if (target_enemy.Health <= Damage.DmgQ(target_enemy) + Damage.DmgHextech(target_enemy))
                            {
                                Mode_Item.Hextech.Cast(target_enemy);
                            }

                            //Q + E + Hextech || Q + AD + Hextech
                            if (Player.Instance.Distance(target_enemy) <= Player.Instance.AttackRange)
                            {
                                if (target_enemy.Health <= Damage.DmgQ(target_enemy) + Damage.DmgE(target_enemy) + Damage.DmgHextech(target_enemy) ||
                                target_enemy.Health <= Damage.DmgQ(target_enemy) + Player.Instance.TotalAttackDamage + Damage.DmgHextech(target_enemy))
                                {
                                    Mode_Item.Hextech.Cast(target_enemy);
                                    SpellManager.Q.Cast(target_enemy);
                                    Player.IssueOrder(GameObjectOrder.AttackTo, target_enemy);
                                }
                            }
                        }

                        if (Mode_Item.Bilgewater.IsOwned() && Mode_Item.Bilgewater.IsReady())
                        {
                            //Q + Bilgewater
                            if (target_enemy.Health <= Damage.DmgQ(target_enemy) + Damage.Bilgewater(target_enemy))
                            {
                                Mode_Item.Bilgewater.Cast(target_enemy);
                            }

                            //Q + E + Bilgewater || Q + AD + Bilgewater
                            if (Player.Instance.Distance(target_enemy) <= Player.Instance.AttackRange)
                            {
                                if (target_enemy.Health <= Damage.DmgQ(target_enemy) + Damage.DmgE(target_enemy) + Damage.Bilgewater(target_enemy) ||
                                target_enemy.Health <= Damage.DmgQ(target_enemy) + Player.Instance.TotalAttackDamage + Damage.Bilgewater(target_enemy))
                                {
                                    Mode_Item.Bilgewater.Cast(target_enemy);
                                    SpellManager.Q.Cast(target_enemy);
                                    Player.IssueOrder(GameObjectOrder.AttackTo, target_enemy);
                                }
                            }
                        }
                    }
                }
            }
        }   //End KillSteal


        public static void JungleSteal()
        {
            if (Player.Instance.IsDead) return;

            var target_monster = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= 800 && !x.Name.Contains("Mini") &&
               (x.BaseSkinName.ToLower().Contains("dragon") || x.BaseSkinName.ToLower().Contains("herald") || x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault();

            if (target_monster != null && SpellManager.Q.IsInRange(target_monster) && SpellManager.Q.IsReady())
            {
                if (MenuMisc["Steal.J.0"].Cast<CheckBox>().CurrentValue)
                {
                    if (target_monster.Health <= Damage.DmgQ(target_monster))
                    {
                        SpellManager.Q.Cast(target_monster);
                    }

                    if (Player.Instance.Distance(target_monster) <= Player.Instance.AttackRange && target_monster.Health <= Damage.DmgQ(target_monster) + Damage.DmgE(target_monster))
                    {
                        SpellManager.Q.Cast(target_monster);
                        Orbwalker.ForcedTarget = target_monster;
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target_monster);
                        Orbwalker.ResetAutoAttack();
                    }
                }
            }
        }   //End JungleSteal
    }
}
