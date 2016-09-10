using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace NebulaTeemo
{
    internal class Mode_Steal : Teemo
    {
        public static void KillSteal()
        {
            if (Player.Instance.IsDead) return;
            if (Player.Instance.CountEnemiesInRange(1300) == 0) return;

            var target_enemy = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= 1200).FirstOrDefault();
            
            if (target_enemy.IsValidTarget() && Player.Instance.Distance(target_enemy) <= 680)
            {
                if (MenuMisc["Steal.K.5"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsReady() && Mode_Item.Hextech.IsOwned() && Mode_Item.Hextech.IsReady())
                {
                    if (target_enemy.Health <= Damage.DmgQ(target_enemy) + Damage.DmgE(target_enemy) + Damage.DmgHextech(target_enemy))
                    {
                        Mode_Item.Hextech.Cast(target_enemy);
                        Orbwalker.ForcedTarget = target_enemy;
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target_enemy);
                        SpellManager.Q.Cast(target_enemy);
                        Orbwalker.ResetAutoAttack();
                    }
                }

                if (MenuMisc["Steal.K.4"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsReady() && target_enemy.Health <= Damage.DmgQ(target_enemy) + Damage.DmgE(target_enemy))
                {
                    SpellManager.W.Cast();
                    Orbwalker.ForcedTarget = target_enemy;
                    Player.IssueOrder(GameObjectOrder.AttackUnit, target_enemy);
                    SpellManager.Q.Cast(target_enemy);
                    Orbwalker.ResetAutoAttack();
                }

                if (MenuMisc["Steal.K.3"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsReady() && target_enemy.Health <= Damage.DmgQ(target_enemy))
                {
                    SpellManager.Q.Cast(target_enemy);
                }
            }
            
            if (target_enemy.IsValidTarget() && Player.Instance.Distance(target_enemy) <= 600)
            {
                if (MenuMisc["Steal.K.2"].Cast<CheckBox>().CurrentValue && Mode_Item.Ignite.IsReady() && SpellManager.Q.IsReady() && Mode_Item.Hextech.IsOwned() && Mode_Item.Hextech.IsReady())
                {
                    if (target_enemy.Health <= Damage.DmgIgnite + Damage.DmgHextech(target_enemy) + Damage.DmgQ(target_enemy))
                    {
                        Mode_Item.Hextech.Cast(target_enemy);
                        SpellManager.Q.Cast(target_enemy);
                        Mode_Item.Ignite.Cast(target_enemy);
                    }
                }

                if (MenuMisc["Steal.K.1"].Cast<CheckBox>().CurrentValue && Mode_Item.Ignite.IsReady() && SpellManager.Q.IsReady() && target_enemy.Health <= Damage.DmgIgnite + Damage.DmgQ(target_enemy))
                {
                    SpellManager.Q.Cast(target_enemy);
                    Mode_Item.Ignite.Cast(target_enemy);
                    
                }

                if (MenuMisc["Steal.K.0"].Cast<CheckBox>().CurrentValue && Mode_Item.Ignite.IsReady() && target_enemy.Health <= Damage.DmgIgnite)
                {
                    Mode_Item.Ignite.Cast(target_enemy);                   
                }
            }

            if (target_enemy.IsValidTarget() && Player.Instance.Distance(target_enemy) <= Player.Instance.AttackRange)
            {
                if (MenuMisc["Steal.K.7"].Cast<CheckBox>().CurrentValue && Mode_Item.Hextech.IsOwned() && Mode_Item.Hextech.IsReady())
                {
                    if (target_enemy.Health <= Damage.DmgE(target_enemy) + Damage.DmgHextech(target_enemy))
                    {
                        Mode_Item.Hextech.Cast(target_enemy);
                        Orbwalker.ForcedTarget = target_enemy;
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target_enemy);
                        Orbwalker.ResetAutoAttack();
                    }
                }

                if (MenuMisc["Steal.K.6"].Cast<CheckBox>().CurrentValue && target_enemy.Health <= Damage.DmgE(target_enemy))
                {
                    Orbwalker.ForcedTarget = target_enemy;
                    Player.IssueOrder(GameObjectOrder.AttackUnit, target_enemy);
                    Orbwalker.ResetAutoAttack();
                }

                
            }
        }   //End KillSteal


        public static void JungleSteal()
        {
            if (Player.Instance.IsDead) return;

            var target_monster = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= 800 && !x.Name.Contains("Mini") &&
               (x.BaseSkinName.ToLower().Contains("dragon") || x.BaseSkinName.ToLower().Contains("herald") || x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault();

            if (target_monster.IsValidTarget() && Player.Instance.Distance(target_monster) <= SpellManager.Q.Range)
            {
                if (MenuMisc["Steal.J.1"].Cast<CheckBox>().CurrentValue && Player.Instance.Distance(target_monster) <= Player.Instance.AttackRange && SpellManager.Q.IsReady())
                {
                    if (target_monster.Health <= Damage.DmgQ(target_monster) + Damage.DmgE(target_monster))
                    {
                        SpellManager.Q.Cast(target_monster);
                        Orbwalker.ForcedTarget = target_monster;
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target_monster);
                        Orbwalker.ResetAutoAttack();
                    }
                }

                if (MenuMisc["Steal.J.0"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsReady() && target_monster.Health <= Damage.DmgQ(target_monster))
                {
                    SpellManager.Q.Cast(target_monster);
                }                
            }
        }   //End JungleSteal
    }
}
