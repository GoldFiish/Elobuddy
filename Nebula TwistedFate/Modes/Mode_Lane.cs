using EloBuddy;
using EloBuddy.SDK;
using System.Linq;

namespace NebulaTwistedFate.Modes
{
    class Mode_Lane : TwistedFate
    {
        public static void Lane()
        {
            if (Player.Instance.IsDead) return;

            var targetW = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget(Player.Instance.AttackRange + 100)).OrderBy(x => x.Health);

            if (targetW != null)
            {
                if (Status_CheckBox(M_Clear, "Lane_W_Blue") && Player.Instance.ManaPercent < Status_Slider(M_Clear, "Lane_W_TotalMana"))
                {
                    CardSelect.StartSelecting(Cards.Blue);
                }
                else if (Status_CheckBox(M_Clear, "Lane_W_Red") && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Lane_W_TotalMana"))
                {
                    var TeamTurret = EntityManager.Turrets.Allies.Where(x => Player.Instance.Distance(x) <= 370).FirstOrDefault();
                    
                    if ((TeamTurret != null && Player.Instance.Distance(TeamTurret) <= 370 && TeamTurret.CountEnemyMinionsInRange(420) >= 3) && Player.Instance.CountEnemyChampionsInRange(1700) == 0)
                    {
                        var PushNum = targetW.Count(x => x.Distance(targetW.FirstOrDefault()) <= 200);

                        if (PushNum >= Status_Slider(M_Clear, "Lane_W_RedHit"))
                        {
                            CardSelect.StartSelecting(Cards.Red);
                        }
                    }
                    else if (Player.Instance.CountEnemyChampionsInRange(1700) >= 1)
                    {
                        var KillNum = targetW.Count(x => x.Distance(targetW.FirstOrDefault()) <= 200 && x.Health <= Damage.DmgW(x, Cards.Red));

                        if (KillNum >= Status_Slider(M_Clear, "Lane_W_RedKill"))
                        {
                            CardSelect.StartSelecting(Cards.Red);
                        }
                    }
                }
            }

            if (Status_CheckBox(M_Clear, "Lane_Q") && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Lane_Q_Mana"))
            {
                var target = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget(SpellManager.Q.Range)).OrderBy(x => x.Health);

                if (target != null)
                {
                    switch (Status_ComboBox(M_Clear, "Lane_Q_Mode"))
                    {
                        case 0: //Auto
                            var TeamTurret = EntityManager.Turrets.Allies.Where(x => Player.Instance.Distance(x) <= 350).FirstOrDefault();
                            
                            if ((TeamTurret != null && Player.Instance.Distance(TeamTurret) <= 370 && TeamTurret.CountEnemyMinionsInRange(420) >= 3) &&
                                Player.Instance.CountEnemyChampionsInRange(1700) == 0)
                            {
                                target = target.OrderBy(x => x.Health);
                            }
                            
                            if (Player.Instance.CountEnemyChampionsInRange(SpellManager.Q.Range) >= 1)
                            {
                                target = target.OrderBy(x => x.Health <= Damage.DmgQ(x));
                            }
                            break;
                        case 1: //Falst clear"
                            target = target.OrderBy(x => x.Health);
                            break;
                        case 2: //Killable clear
                            target = target.OrderBy(x => x.Health <= Damage.DmgQ(x));
                            break;
                    }

                    var targetPre = EntityManager.MinionsAndMonsters.GetLineFarmLocation(target, SpellManager.Q.Width, (int)SpellManager.Q.Range);

                    if (targetPre.HitNumber >= Status_Slider(M_Clear, "Lane_Q_Hit"))
                    {
                        SpellManager.Q.Cast(targetPre.CastPosition);
                    }
                }
            }
        }   //End Static Lane
    }   //End Class Mode_Lane
}
