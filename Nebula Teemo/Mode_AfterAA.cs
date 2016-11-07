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

            var ItsEnemy = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(1200)).FirstOrDefault();

            if (ItsEnemy != null && SpellManager.Q.IsReady())
            {
                if (MenuCombo["Combo.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuCombo["Combo.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if ((ItsEnemy.Spellbook.IsCastingSpell && Player.Instance.IsTargetable) || ItsEnemy.IsAttackingPlayer)
                    {
                        if (Player.Instance.Distance(ItsEnemy) <= 520)
                        {
                            SpellManager.Q.Cast(ItsEnemy);
                            Orbwalker.ResetAutoAttack();
                        }
                    }
                }
            }
        }   //End Combo

        public static void AA_Harass()
        {
            if (Player.Instance.IsDead) return;

            var ItsEnemy = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(1200)).FirstOrDefault();

            if (ItsEnemy != null && SpellManager.Q.IsReady())
            {
                if (MenuHarass["Harass.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuHarass["Harass.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if ((ItsEnemy.Spellbook.IsCastingSpell && Player.Instance.IsTargetable) || ItsEnemy.IsAttackingPlayer)
                    {
                        if (Player.Instance.Distance(ItsEnemy) <= 520)
                        {
                            SpellManager.Q.Cast(ItsEnemy);
                            Orbwalker.ResetAutoAttack();
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

            if (MiniMonster != null && MiniMonster.FirstOrDefault(m => m.Distance(Player.Instance.Position) <= Player.Instance.AttackRange) != null)
            {
                if (Player.Instance.HasBuff("krugstonefistcounter") && Player.Instance.GetBuffCount("krugstonefistcounter") >= 4)
                {                    
                    Orbwalker.ForcedTarget = BigMonster;
                    Player.IssueOrder(GameObjectOrder.AttackUnit, BigMonster);
                }
                else
                {
                    Orbwalker.ForcedTarget = MiniMonster.FirstOrDefault();
                    JungleMonster = MiniMonster;
                }
            }

            if (SpellManager.Q.IsReady())
            {
                if (MenuJungle["Jungle.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuJungle["Jungle.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if (Player.Instance.CountEnemiesInRange(1200) == 0)
                    {
                        Core.DelayAction(() =>
                        {
                            if (BigMonster != null && !BigMonster.HasBuff("Stun"))
                            {
                                if (BigMonster.HealthPercent >= 35)
                                {
                                    Player.IssueOrder(GameObjectOrder.AttackUnit, BigMonster);
                                    SpellManager.Q.Cast(BigMonster);
                                    //Orbwalker.ResetAutoAttack();
                                }
                            }
                        }, 180);

                        if (EpicMonster != null)
                        {
                            if (EpicMonster.HealthPercent >= 25)
                            {
                                Player.IssueOrder(GameObjectOrder.AttackUnit, EpicMonster);
                                SpellManager.Q.Cast(EpicMonster);
                                //Orbwalker.ResetAutoAttack();
                            }
                        }
                    }

                    if (Player.Instance.CountEnemiesInRange(1200) >= 1)
                    {
                        if (BigMonster != null)
                        {
                            if (BigMonster.Health <= Damage.DmgQ(BigMonster) ||
                                BigMonster.Health <= Damage.DmgQ(BigMonster) + Damage.DmgE(BigMonster) ||
                                BigMonster.Health <= Damage.DmgQ(BigMonster) + Player.Instance.TotalAttackDamage ||
                                BigMonster.Health <= Damage.DmgQ(BigMonster) + Damage.DmgE(BigMonster) + Player.Instance.TotalAttackDamage)
                            {
                                SpellManager.Q.Cast(BigMonster);
                                //Orbwalker.ResetAutoAttack();
                            }                            
                        }

                        if (EpicMonster != null)
                        {
                            if (EpicMonster.Health <= Damage.DmgQ(EpicMonster) ||
                                EpicMonster.Health <= Damage.DmgQ(EpicMonster) + Damage.DmgE(EpicMonster) ||
                                EpicMonster.Health <= Damage.DmgQ(EpicMonster) + Player.Instance.TotalAttackDamage ||
                                EpicMonster.Health <= Damage.DmgQ(EpicMonster) + Damage.DmgE(EpicMonster) + Player.Instance.TotalAttackDamage)
                            {
                                SpellManager.Q.Cast(EpicMonster);
                                //Orbwalker.ResetAutoAttack();
                            }
                        }
                    }
                }
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
