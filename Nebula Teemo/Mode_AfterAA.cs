using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaTeemo
{
    internal class Mode_AfterAA :Teemo
    {
        public static void AA_Combo()
        {
            if (Player.Instance.IsDead) return;
            if (Player.Instance.CountEnemiesInRange(1200) == 0) return;

            var ItsEnemy = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(1200)).FirstOrDefault();
            var ItsMe = EntityManager.Heroes.AllHeroes.Where(x => x.IsMe).FirstOrDefault();

            if (SpellManager.Q.IsReady())
            {
                if (MenuCombo["Combo.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuCombo["Combo.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if ((ItsEnemy.Spellbook.IsCastingSpell && ItsMe.IsTargetable) || ItsEnemy.IsAttackingPlayer)
                    {
                        if (ItsEnemy.IsValidTarget())
                        {
                            if (Player.Instance.Distance(ItsEnemy) <= 500)
                            {
                                SpellManager.Q.Cast(ItsEnemy);
                                Orbwalker.ResetAutoAttack();
                            }
                        }
                    }
                }
            }
        }   //End Combo

        public static void AA_Harass()
        {
            if (Player.Instance.IsDead) return;
            if (Player.Instance.CountEnemiesInRange(1200) == 0) return;

            var ItsEnemy = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(1200)).FirstOrDefault();
            var ItsMe = EntityManager.Heroes.AllHeroes.Where(x => x.IsMe).FirstOrDefault();

            if (SpellManager.Q.IsReady())
            {
                if (MenuHarass["Harass.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuHarass["Harass.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if ((ItsEnemy.Spellbook.IsCastingSpell && ItsMe.IsTargetable) || ItsEnemy.IsAttackingPlayer)
                    {
                        if (ItsEnemy.IsValidTarget())
                        {
                            if (Player.Instance.Distance(ItsEnemy) <= 500)
                            {
                                SpellManager.Q.Cast(ItsEnemy);
                                Orbwalker.ResetAutoAttack();
                            }
                        }
                    }                    
                }
            }
        }   //End Combo


        public static void AA_Jungle()
        {
            if (Player.Instance.IsDead) return;

            var JungleMonster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, 750);

            if (JungleMonster == null) return;

            var MiniMonster = JungleMonster.Where(x => x.IsValidTarget(680) && x.Name.Contains("Mini"));
            var BigMonster = JungleMonster.Where(x => x.IsValidTarget(680) && !x.Name.Contains("Mini") &&
            (!x.BaseSkinName.ToLower().Contains("dragon") && !x.BaseSkinName.ToLower().Contains("herald") && !x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault();
            var EpicMonster = JungleMonster.Where(x => x.IsValidTarget(680) && !x.Name.Contains("Mini") &&
                (x.BaseSkinName.ToLower().Contains("dragon") || x.BaseSkinName.ToLower().Contains("herald") || x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault();
            
            if (SpellManager.Q.IsReady())
            {
                if (MenuJungle["Jungle.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuJungle["Jungle.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if (Player.Instance.CountEnemiesInRange(1200) == 0)
                    {
                        if (BigMonster != null)
                        {
                            if (BigMonster.HealthPercent >= 35)
                            {
                                SpellManager.Q.Cast(BigMonster);
                                Orbwalker.ResetAutoAttack();
                            }                    
                        }

                        if (EpicMonster != null)
                        {
                            if (EpicMonster.HealthPercent >= 25)
                            {
                                SpellManager.Q.Cast(EpicMonster);
                                Orbwalker.ResetAutoAttack();
                            }              
                        }
                    }

                    if (Player.Instance.CountEnemiesInRange(1200) >= 1)
                    {
                        if (BigMonster != null)
                        {
                            if (BigMonster.Health <= Damage.DmgQ(BigMonster))
                            {
                                SpellManager.Q.Cast(BigMonster);
                                Orbwalker.ResetAutoAttack();
                            }

                            if (BigMonster.Health <= Damage.DmgQ(BigMonster) + Damage.DmgE(BigMonster))
                            {
                                SpellManager.Q.Cast(BigMonster);
                                Orbwalker.ResetAutoAttack();
                            }
                        }

                        if (EpicMonster != null)
                        {
                            if (EpicMonster.Health <= Damage.DmgQ(BigMonster))
                            {
                                SpellManager.Q.Cast(EpicMonster);
                                Orbwalker.ResetAutoAttack();
                            }

                            if (EpicMonster.Health <= Damage.DmgQ(BigMonster) + Damage.DmgE(EpicMonster))
                            {
                                SpellManager.Q.Cast(EpicMonster);
                                Orbwalker.ResetAutoAttack();
                            }
                        }
                    }
                }
            }

            if (MiniMonster.FirstOrDefault(m => m.Distance(Player.Instance.Position) <= Player.Instance.AttackRange) != null)
            {
                Orbwalker.ForcedTarget = MiniMonster.FirstOrDefault();
                JungleMonster = MiniMonster;
            }

            if (SpellManager.R.IsReady())
            {
                if (MenuJungle["Jungle.R.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuJungle["Jungle.R.Mana"].Cast<Slider>().CurrentValue &&
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > MenuJungle["Jungle.R.Count"].Cast<Slider>().CurrentValue)
                {
                    var RPrediction = Prediction.Position.PredictCircularMissile(BigMonster, SpellManager.R.Range, 135, 1000, 1000);

                    if (RPrediction.HitChance >= HitChance.High)
                    {
                        SpellManager.R.Cast(RPrediction.CastPosition);
                    }
                }
            }
        }   //End Jungle
    }
}
