using EloBuddy;
using EloBuddy.SDK;
using System.Linq;

namespace NebulaNasus.Modes
{
    class Mode_Lane : Nasus
    {
        public static void Lane()
        {
            if (Player.Instance.IsDead) return;

            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.IsValidTarget(850));           

            foreach (var target in minions)
            {
                if (Status_CheckBox(M_Clear, "Lane_Q") && SpellManager.Q.IsReady() && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Lane_Q_Mana"))
                {
                    if (target.Health <= Damage.DmgQ(target) && SpellManager.Q.IsInRange(target))
                    {
                        SpellManager.Q.Cast(target);
                    }
                }

                if (Status_CheckBox(M_Clear, "Lane_E") && SpellManager.E.IsReady() && SpellManager.E.IsInRange(target) && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Lane_E_Mana"))
                {
                    if(Player.Instance.CountEnemiesInRange(1800) == 0)
                    {
                        SpellManager.E.Cast(target);
                    }
                }
            }
        }   //End Static Lane
    }   //End Class Mode_Lane
}
