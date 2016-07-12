using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;

namespace NebulaKalista
{
    public class Mode_Main
    {
        public static IEnumerable<AIHeroClient> ValidTargets { get { return EntityManager.Heroes.Enemies.Where(enemy => enemy.IsHPBarRendered); } }

        public static void Combo()
        {
            if (!Orbwalker.CanMove) { return; }
            
            if (Kalista.MenuMain["Combo.Q"].Cast<CheckBox>().CurrentValue)
            {
                var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);
      
                if (SpellManager.Q.IsReady() && SpellManager.Q.GetPrediction(Qtarget).HitChance >= HitChance.High)
                {                
                    if (Qtarget.IsValidTarget(SpellManager.E.Range) && !Player.Instance.IsDashing())
                    {
                        SpellManager.Q.Cast(Qtarget);
                    }
                }
            } 

            if (Kalista.MenuMain["Combo.E"].Cast<CheckBox>().CurrentValue)
            {
                if (ValidTargets.Any(t => Extensions.IsRendKillable(t)))
                {
                    SpellManager.E.Cast();
                }
            }
        }
        
        public static void Harass()
        {
            if (Kalista.MenuMain["Harass.Q"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.ManaPercent > Kalista.MenuMain["Harass.Mana"].Cast<Slider>().CurrentValue)
            {
                var Qtarget = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);
                
                if (SpellManager.Q.IsReady() && SpellManager.Q.GetPrediction(Qtarget).HitChance >= HitChance.High)
                {
                    if (Qtarget.IsValidTarget(SpellManager.E.Range) && !Player.Instance.IsDashing())
                    {
                        SpellManager.Q.Cast(Qtarget);
                    }
                }
            }

            if (Kalista.MenuMain["Harass.E"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.ManaPercent > Kalista.MenuMain["Harass.Mana"].Cast<Slider>().CurrentValue)
            {               
                if (SpellManager.E.IsReady())
                {
                    if (EntityManager.MinionsAndMonsters.Minions.Any(x => x.Health > x.Get_E_Damage_Double()))
                    {
                        if (EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(SpellManager.E.Range) && x.Distance(ObjectManager.Player.ServerPosition) < 900 && x.GetRendBuff().Count >= 1))
                        {
                            SpellManager.E.Cast();
                        }
                    }

                    if (EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(SpellManager.E.Range) && x.Distance(ObjectManager.Player.ServerPosition) < 900 && x.GetRendBuff().Count >= 3))
                    {
                        SpellManager.E.Cast();
                    }
                }
            }
        }
    }
}
