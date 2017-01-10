using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace NebulaKalista.Modes
{
   internal class Mode_Lane : Kalista 
    {
        public static void LaneClear()
        {
            if (Player.Instance.IsDead) return;
           
            var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= 1250);

            if (minion != null)
            {
                //Lnae Q
                if (Status_CheckBox(MenuLane, "Lane_Q") && Player.Instance.ManaPercent > Status_Slider(MenuLane, "Lane_Q_Mana") && SpellManager.Q.IsReady())
                {
                    foreach (var target in minion.Where(x => x.Health <= Extensions.Get_Q_Damage_Float(x)))
                    {
                        var Qtarget = SpellManager.Q.GetPrediction(target);
                        var Qtarget_num = Qtarget.GetCollisionObjects<Obj_AI_Minion>().Count(x => x.Health <= Extensions.Get_Q_Damage_Float(x));

                        if (Qtarget_num >= Status_Slider(MenuLane, "Lane_Q_Num"))
                        {
                            SpellManager.Q.Cast(Qtarget.CastPosition);
                        }
                    }
                }

                //Lnae E_Siege, SuperMinion [ Auto ]
                if (SpellManager.E.IsReady() && minion.Any(x => x.IsValidTarget(SpellManager.E.Range) && (x.BaseSkinName.ToLower().Contains("siege") || x.BaseSkinName.ToLower().Contains("super")) &&
                x.Health <= x.Get_E_Damage_Double()))
                {
                    SpellManager.E.Cast();
                }

                //Lnae E_All Minions
                if (Status_CheckBox(MenuLane, "Lane_E_All") && Player.Instance.ManaPercent > Status_Slider(MenuLane, "Lane_E_Mana") && SpellManager.E.IsReady())
                {
                    var minion_num = minion.Count(x => x.IsValidTarget(SpellManager.E.Range) && x.Health <= x.Get_E_Damage_Double());

                    if (minion_num >= Status_Slider(MenuLane, "Lane_E_Num"))
                    {
                        SpellManager.E.Cast();
                    }
                }
            }
        }   //  End LaneClear        
    }
}

