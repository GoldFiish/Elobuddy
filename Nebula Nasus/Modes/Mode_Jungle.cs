using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace NebulaNasus.Modes
{
    class Mode_Jungle : Nasus
    {
        public static void Jungle()
        {
            if (Player.Instance.IsDead) return;

            var monster = EntityManager.MinionsAndMonsters.Monsters.Where(m => m.IsValidTarget(850));
            var MiniMonster = monster.Where(x => x.IsValidTarget(SpellManager.Q.Range) && x.Name.Contains("Mini"));
            
            if (MiniMonster != null && MiniMonster.FirstOrDefault(x => Player.Instance.Distance(x) <= Player.Instance.AttackRange) != null)
            {
                Orbwalker.ForcedTarget = MiniMonster.FirstOrDefault();
                monster = MiniMonster;
            }

            foreach (var target in EntityManager.MinionsAndMonsters.Monsters.Where(m => m.IsValidTarget(650)))
            {
                if (Status_CheckBox(M_Clear, "Jungle_Q") && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target) && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Jungle_Q_Mana"))
                {
                    if (target.Health <= Damage.DmgQ(target))
                    {
                        SpellManager.Q.Cast(target);
                    }
                    else if (target.Health > Damage.DmgQ(target) * 2)
                    {
                        SpellManager.Q.Cast(target);
                    }
                }
                
                if (Status_CheckBox(M_Clear, "Jungle_E") && SpellManager.E.IsReady() && SpellManager.E.IsInRange(target) && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Jungle_E_Mana"))
                {
                    SpellManager.E.Cast(target);                    
                }
            }
        }   //End Static Jungle
    }   //End Class Mode_Jungle
}
