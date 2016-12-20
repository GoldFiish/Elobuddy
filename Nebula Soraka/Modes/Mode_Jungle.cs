using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace NebulaSoraka.Modes
{
    class Mode_Jungle : Soraka
    {
        public static void Jungle()
        {
            if (Player.Instance.IsDead) return;

            var monster = EntityManager.MinionsAndMonsters.Monsters.Where(m => m.IsValidTarget(850));

            if (monster == null) return;

            var MiniMonster = monster.Where(x => x.IsValidTarget(SpellManager.Q.Range) && x.Name.Contains("Mini"));

            if (MiniMonster != null && MiniMonster.FirstOrDefault(x => Player.Instance.Distance(x) <= Player.Instance.AttackRange) != null)
            {
                Orbwalker.ForcedTarget = MiniMonster.FirstOrDefault();
                monster = MiniMonster;
            }

            if (Status_CheckBox(M_Clear, "Jungle_Q") && SpellManager.Q.IsReady() && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Jungle_Q_Mana"))
            {
                foreach (var target in EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(800)).OrderBy(x => x.Health))
                {
                    SpellManager.Q.Cast(target);
                }
            }
        }   //End Static Jungle
    }   //End Class Mode_Jungle
}
