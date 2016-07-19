﻿using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaKalista
{
    internal class Mode_Always : Kalista
    {
        public static void Always()
        {
            if (MenuMisc["E.KillSteal"].Cast<CheckBox>().CurrentValue)
            {
                if (SpellManager.E.IsReady() && EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(1200) && Extensions.IsRendKillable(x)))
                {
                    SpellManager.E.Cast();
                }

                foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(1200) && x.Health <= x.Get_Q_Damage_Float() && !x.IsDead))
                {
                    if (SpellManager.Q.IsReady())
                    {
                        SpellManager.Q.Cast(target);
                    }
                }
            }

            if (MenuMisc["E.MonsterSteal"].Cast<CheckBox>().CurrentValue)
            {
                if (SpellManager.E.IsReady() && EntityManager.MinionsAndMonsters.Monsters.Any(x => Extensions.IsRendKillable(x) &&
                        x.IsValidTarget(1200) && ((x.BaseSkinName.Contains("dragon") || x.BaseSkinName.Contains("herald") || x.BaseSkinName.Contains("baron")) || !x.Name.Contains("Mini"))))
                {
                    SpellManager.E.Cast();
                }
            }

            if (MenuMisc["E.Death"].Cast<CheckBox>().CurrentValue && Player.Instance.HealthPercent <= MenuMisc["E.Death.Hp"].Cast<Slider>().CurrentValue)
                {
                    if (SpellManager.E.IsReady() && EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(SpellManager.E.Range)))
                    {
                        SpellManager.E.Cast();
                    }
                }

            if (MenuMisc["R.Save"].Cast<CheckBox>().CurrentValue && SpellManager.R.IsReady())
            {                
                var ppp = EntityManager.Heroes.Allies.LastOrDefault(x => !x.IsMe && !x.IsDead && x.Distance(ObjectManager.Player.Position) < 1500 && x.HasBuff("kalistacoopstrikeally"));

                if (ppp == null) return;

                if (ppp.HealthPercent < MenuMisc["R.Save.Hp"].Cast<Slider>().CurrentValue && ppp.CountEnemiesInRange(1500) > 0)
                {
                    SpellManager.R.Cast();
                }        
            }
        }   //End Always
    }
}