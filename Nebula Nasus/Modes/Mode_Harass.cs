using EloBuddy;
using EloBuddy.SDK;

namespace NebulaNasus.Modes
{
    class Mode_Harass : Nasus
    {
        public static void Harass()
        {
            if (Player.Instance.IsDead) return;

            var target = TargetSelector.GetTarget(750, DamageType.Mixed);

            if( target != null)
            {
                if (Status_CheckBox(M_Main, "Harass_Q") && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(target) && Player.Instance.ManaPercent > Status_Slider(M_Main, "Harass_Q_Mana"))
                {
                    SpellManager.Q.Cast(target);
                }

                if (Status_CheckBox(M_Main, "Harass_E") && SpellManager.E.IsReady() && Player.Instance.Distance(target) <= 220 && Player.Instance.ManaPercent > Status_Slider(M_Main, "Harass_E_Mana"))
                {
                    SpellManager.E.Cast(target);
                }
            }
        }   //End Harass
    }   //End Class Mode_Harass
}
