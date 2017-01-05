using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace NebulaSoraka.Modes
{
    class Mode_Jungle : Soraka
    {
        public static PredictionResult GetQPrediction(Obj_AI_Base target)
        {
            float divider = Player.Instance.Distance(target) / SpellManager.Q.Range;
            SpellManager.Q.CastDelay = (int)(0.2f + 0.8f * divider);
            var prediction = SpellManager.Q.GetPrediction(target);
            return prediction;
        }

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
                var target = monster.OrderBy(x => x.Health).FirstOrDefault();

                if ( target != null)
                {
                    var Qprediction = GetQPrediction(target);

                    if (Qprediction.HitChancePercent >= 80)
                    {
                        SpellManager.Q.Cast(Qprediction.CastPosition);
                    }
                }
            }
        }   //End Static Jungle
    }   //End Class Mode_Jungle
}
