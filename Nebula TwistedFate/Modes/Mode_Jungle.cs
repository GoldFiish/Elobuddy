using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace NebulaTwistedFate.Modes
{
    class Mode_Jungle : TwistedFate
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

            if (Status_CheckBox(M_Clear, "Jungle_W") && SpellManager.W.IsReady())
            {
                if (Player.Instance.ManaPercent < Status_Slider(M_Clear, "Jungle_W_TotalMana"))
                {
                    CardSelect.StartSelecting(Cards.Blue);
                }
                else
                {
                    switch (Status_ComboBox(M_Clear, "Jungle_W_Pick"))
                    {
                        case 0:     //Auto
                            var MKillNum = monster.OrderBy(x => x.Health).Count(x => x.Distance(monster.FirstOrDefault()) <= 200 && x.Health <= Damage.PredictWDamage(x, Cards.Red));
                            var TotalNum = monster.Count(x => x.Distance(monster.FirstOrDefault()) <= 200);
                            var BigMonster = monster.Where(x => x.IsValidTarget(Player.Instance.AttackRange + 100) && !x.Name.Contains("Mini")).FirstOrDefault();

                            if (MKillNum >= 2 || TotalNum >= 2)
                            {
                                CardSelect.StartSelecting(Cards.Red);
                            }
                            
                            if (Player.Instance.Distance(BigMonster) <= BigMonster.AttackRange + 100 )
                            {
                                CardSelect.StartSelecting(Cards.Yellow);
                            }
                            break;
                        case 1:     //Red
                            CardSelect.StartSelecting(Cards.Red);
                            break;
                        case 2:     //Yellow
                            CardSelect.StartSelecting(Cards.Yellow);
                            break;
                    }
                }
            }

            if (Status_CheckBox(M_Clear, "Jungle_Q") && SpellManager.Q.IsReady() && Player.Instance.ManaPercent > Status_Slider(M_Clear, "Jungle_Q_Mana"))
            {
                var target = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(1450)).OrderBy(x => x.Health);

                if (target != null)
                {
                    var TargetNum = target.Count(x => x.Distance(target.FirstOrDefault()) <= 200);

                    var Qprediction = SpellManager.Q.GetPrediction(target.FirstOrDefault());

                    if (TargetNum >= 2)
                    {
                        SpellManager.Q.Cast(Qprediction.CastPosition);
                    }
                }              
            }
        }   //End Static Jungle
    }   //End Class Mode_Jungle
}
