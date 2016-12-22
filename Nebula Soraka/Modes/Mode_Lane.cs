using EloBuddy;
using EloBuddy.SDK;
using System.Linq;

namespace NebulaSoraka.Modes
{
    class Mode_Lane : Soraka
    {
        public static void Lane()
        {
            if (Player.Instance.IsDead) return;

            if (Status_CheckBox(M_Clear, "Lane_Q") && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Lane_Q_Mana"))
            {
                var HitLocation = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.IsValidTarget(800) && m.Health <= Damage.DmgQ(m)), 220, 800);
                                
                if (HitLocation.HitNumber >= Status_Slider(M_Clear, "Lane_Q_Hit"))
                {
                    SpellManager.Q.Cast(HitLocation.CastPosition);
                }
            }
        }   //End Static Lane
    }   //End Class Mode_Lane
}
