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
                    if(target_enemy.Health <= Damage.DmgCalSteal(target_enemy))
                    {
                        if (Player.Instance.Distance(target_enemy) <= Player.Instance.AttackRange)
                        {
                            Player.IssueOrder(GameObjectOrder.AttackTo, target_enemy);
                        }

                        if (Mode_Item.Ignite.IsReady())
                        {
                            Mode_Item.Ignite.Cast(target_enemy);
                        }

                        if (SpellManager.Q.IsReady())
                        {
                            SpellManager.Q.Cast(target_enemy);
                        }

                        if (Mode_Item.Bilgewater.IsReady())
                        {
                            Mode_Item.Bilgewater.Cast(target_enemy);
                        }

                        if (Mode_Item.BladeKing.IsReady())
                        {
                            Mode_Item.BladeKing.Cast(target_enemy);
                        }

                        if (Mode_Item.Hextech.IsReady())
                        {
                            Mode_Item.Hextech.Cast(target_enemy);
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
