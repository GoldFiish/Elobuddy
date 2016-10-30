using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaKalista
{
   internal class Mode_Lane : Kalista 
    {
        public static void LaneClear()
        {
            if (Player.Instance.IsDead) return;
           
            var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget() && Player.Instance.Distance(x) <= 1500);

            if (minion != null)
            {
                //Lnae Q
                if (MenuLane["Lane.Q"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuLane["Lane.Q.Mana"].Cast<Slider>().CurrentValue && SpellManager.Q.IsReady())
                {
                    foreach (var target in minion.Where(x => x.Health <= Extensions.Get_Q_Damage_Float(x)))
                    {
                        var Qtarget = SpellManager.Q.GetPrediction(target);
                        var Qtarget_num = Qtarget.GetCollisionObjects<Obj_AI_Minion>().Count(x => x.Health <= Extensions.Get_Q_Damage_Float(x));

                        if (Qtarget_num >= MenuLane["Lane.Q.Num"].Cast<Slider>().CurrentValue)
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
                if (MenuLane["Lane.E.All"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuLane["Lane.E.Mana"].Cast<Slider>().CurrentValue && SpellManager.E.IsReady())
                {
                    var minion_num = minion.Count(x => x.IsValidTarget(SpellManager.E.Range) && x.Health <= x.Get_E_Damage_Double());

                    if (minion_num >= MenuLane["Lane.E.Num"].Cast<Slider>().CurrentValue)
                    {
                        SpellManager.E.Cast();
                    }
                }
            }
        }   //  End LaneClear        
    }
}

