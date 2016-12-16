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

            if (monster == null) return;

            var MiniMonster = monster.Where(x => x.IsValidTarget(SpellManager.Q.Range) && x.Name.Contains("Mini"));

            if (MiniMonster != null && MiniMonster.FirstOrDefault(x => Player.Instance.Distance(x) <= Player.Instance.AttackRange) != null)
            {
                Orbwalker.ForcedTarget = MiniMonster.FirstOrDefault();
                monster = MiniMonster;
            }

            if (Status_CheckBox(M_Clear, "Jungle_Q") && SpellManager.Q.IsReady() && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Jungle_Q_Mana"))
            {
                foreach (var target in EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(220)).OrderBy(x => x.Health))
                {
                    if (target.Health <= Damage.DmgQ(target))
                    {
                        SpellManager.Q.Cast(target);
                    }
                    else if (target.Health > 
                             (Player.Instance.Spellbook.GetSpell(SpellSlot.Q).Cooldown / Player.Instance.AttackDelay) * Player.Instance.GetAutoAttackDamage(target))
                    {
                        SpellManager.Q.Cast(target);
                    }
                }
            }

            if (Status_CheckBox(M_Clear, "Jungle_E") && SpellManager.E.IsReady() && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Jungle_E_Mana"))
            {
                var HitLocation = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(650)), 380, 650);

                if (HitLocation.HitNumber >= Status_Slider(M_Clear, "Jungle_E_Hit"))
                {
                    SpellManager.E.Cast(HitLocation.CastPosition);
                }
            }
        }   //End Static Jungle
    }   //End Class Mode_Jungle
}
