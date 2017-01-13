using EloBuddy;
using EloBuddy.SDK;

namespace NebulaTwistedFate.Modes
{
    class Mode_Combo : TwistedFate
    {

        public static void Combo()
        {
            if (Player.Instance.IsDead) return;

            var target = TargetSelector.GetTarget(1500, DamageType.Magical);

            if (target != null)
            {
                if (Status_CheckBox(M_Combo, "Combo_W") && Player.Instance.Distance(target) <= Player.Instance.AttackRange + 100)
                {
                    if (Player.Instance.ManaPercent < Status_Slider(M_Combo, "Combo_W_Blue"))
                    {
                        if (Player.Instance.HealthPercent >= 50)
                        {
                            CardSelect.StartSelecting(Cards.Blue);
                        }
                    }

                    switch (Status_ComboBox(M_Combo, "Combo_W_Pick"))
                    {
                        case 0:     //Auto
                            if (target.CountAllyChampionsInRange(200) >= Status_Slider(M_Combo, "Combo_W_Red"))
                            {
                                CardSelect.StartSelecting(Cards.Red);
                            }
                            else
                            {
                                if (Player.Instance.CountAllyChampionsInRange(Player.Instance.AttackRange) >= 1)
                                {
                                    CardSelect.StartSelecting(Cards.Yellow);
                                }
                                else if (Player.Instance.Distance(target) <= Player.Instance.AttackRange - 150)
                                {
                                    CardSelect.StartSelecting(Cards.Yellow);
                                }
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

                if (Status_CheckBox(M_Combo, "Combo_Q") && Player.Instance.ManaPercent >= Status_Slider(M_Combo, "Combo_Q_Mana"))
                {
                    if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target))
                    {
                        switch (Status_ComboBox(M_Combo, "Combo_Q_Stun"))
                        {
                            case 0:     //아무떄나
                                var Qprediction = SpellManager.Q.GetPrediction(target);

                                if (Qprediction.HitChancePercent >= Status_Slider(M_Combo, "Combo_Q_Pre"))
                                {
                                    SpellManager.Q.Cast(Qprediction.CastPosition);
                                }
                                break;
                            case 1:
                                if (target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Knockup) || target.HasBuffOfType(BuffType.Suppression) ||
                                    target.IsRecalling())
                                {
                                    SpellManager.Q.Cast(target);
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}