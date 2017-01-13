using EloBuddy;
using EloBuddy.SDK;

namespace NebulaTwistedFate.Modes
{
    class Mode_Harass : TwistedFate
    {
        public static void Harass()
        {
            if (Player.Instance.IsDead) return;

            var target = TargetSelector.GetTarget(1500, DamageType.Magical);

            if (target != null)
            {
                if (Status_CheckBox(M_Harras, "Harass_W") && Player.Instance.Distance(target) <= Player.Instance.AttackRange + 100)
                {
                    if (Player.Instance.ManaPercent < Status_Slider(M_Harras, "Harras_W_Blue"))
                    {
                        if (Player.Instance.HealthPercent >= 50)
                        {
                            CardSelect.StartSelecting(Cards.Blue);
                        }
                    }

                    switch (Status_ComboBox(M_Harras, "Harras_W_Pick"))
                    {
                        case 0:     //Auto
                            if (target.CountAllyMinionsInRange(200) >= 3 || target.CountAllyChampionsInRange(200) >= 1)
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
                            if (Player.Instance.CountAllyChampionsInRange(Player.Instance.AttackRange) >= 1)
                            {
                                CardSelect.StartSelecting(Cards.Yellow);
                            }
                            else if (Player.Instance.Distance(target) <= Player.Instance.AttackRange - 150)
                            {
                                CardSelect.StartSelecting(Cards.Yellow);
                            }
                            break;
                    }
                }

                if (Status_CheckBox(M_Harras, "Harass_Q") && Player.Instance.ManaPercent >= Status_Slider(M_Harras, "Harass_Q_Mana"))
                {
                    if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target))
                    {
                        var Qprediction = SpellManager.Q.GetPrediction(target);

                        if (Qprediction.HitChancePercent >= Status_Slider(M_Harras, "Harass_Q_Pre"))
                        {
                            SpellManager.Q.Cast(Qprediction.CastPosition);
                        }
                    }
                }
            }
        }   //End Harass
    }   //End Class Mode_Harass
}
