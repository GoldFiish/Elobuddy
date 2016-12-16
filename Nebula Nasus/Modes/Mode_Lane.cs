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
            
            if (Status_CheckBox(M_Clear, "Lane_Q"))
            {
                var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget(SpellManager.Q.Range)).OrderBy(x => x.Health).FirstOrDefault();

                if (SpellManager.Q.IsReady() && minion != null && minion.Health <= Damage.DmgQ(minion))
                {
                    SpellManager.Q.Cast(minion);
                }
            }

            if (Status_CheckBox(M_Clear, "Lane_E") && SpellManager.E.IsReady() && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Lane_E_Mana"))
            {
                if (Player.Instance.CountEnemiesInRange(1800) == 0)
                {
                    var HitLocation = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.IsValidTarget(650)), 380, 650);

                    if (HitLocation.HitNumber >= 3)
                    {
                        SpellManager.E.Cast(HitLocation.CastPosition);
                    }
                }
            }
        }   //End Static Lane
    }   //End Class Mode_Lane
}
