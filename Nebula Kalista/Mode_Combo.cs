using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;

namespace NebulaKalista
{
    internal class Mode_Combo : Kalista 
    {
        public static void Combo()
        {
            var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);

            if (Qtarget == null) return;            

            if (MenuMain["Combo.Q"].Cast<CheckBox>().CurrentValue && SpellManager.Q.IsReady())
            {
                if(!Player.Instance.IsDashing() && Qtarget.IsValidTarget(SpellManager.Q.Range) && SpellManager.Q.GetPrediction(Qtarget).HitChance >= HitChance.High)
                {
                    SpellManager.Q.Cast(Qtarget);
                }
            }

            if (MenuMain["Combo.E"].Cast<CheckBox>().CurrentValue && SpellManager.E.IsReady())
            {
                if (EntityManager.Heroes.Enemies.Any(x => Extensions.IsRendKillable(x) && x.IsValidTarget(1200)))
                {
                    SpellManager.E.Cast();
                }
            }
        }   //End Combo
    }
}
