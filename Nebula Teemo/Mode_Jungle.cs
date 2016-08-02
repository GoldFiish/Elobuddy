using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaTeemo
{
    internal class Mode_Jungle : Teemo
    {
        public static void Jungle()
        {
            if (Player.Instance.IsDead) return;

            var JungleMonster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, 750);
          
           
            if (JungleMonster == null) return;

            var AttackStyle = MenuJungle["Jungle.AA"].Cast<Slider>().CurrentValue;
            var MiniMonster = JungleMonster.Where(x => x.IsValidTarget(680) && x.Name.Contains("Mini"));
            var BigMonster = JungleMonster.Where(x => x.IsValidTarget(680) && !x.Name.Contains("Mini") && 
            (!x.BaseSkinName.ToLower().Contains("dragon") && !x.BaseSkinName.ToLower().Contains("herald") && !x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault(x => x.HealthPercent >= 15);
            var EpicMonster = JungleMonster.Where(x => x.IsValidTarget(680) && !x.Name.Contains("Mini") &&
                (x.BaseSkinName.ToLower().Contains("dragon") || x.BaseSkinName.ToLower().Contains("herald") || x.BaseSkinName.ToLower().Contains("baron"))).FirstOrDefault(x => x.HealthPercent >= 50);

            if (AttackStyle == 0)
            {
                if (SpellManager.Q.IsReady() && MenuJungle["Jungle.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuJungle["Jungle.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if (EpicMonster != null)
                    {
                        SpellManager.Q.Cast(EpicMonster);
                    }

                    if (BigMonster != null)
                    {
                        SpellManager.Q.Cast(BigMonster);
                    }

                    if (Player.Instance.CountEnemiesInRange(800) >= 1 && BigMonster.Health <= Damage.DmgQ(BigMonster))
                    {
                        SpellManager.Q.Cast(BigMonster);
                    }
                }
            }

            if (AttackStyle == 1)
            {
                if (SpellManager.Q.IsReady() && MenuJungle["Jungle.Q.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuJungle["Jungle.Q.Mana"].Cast<Slider>().CurrentValue)
                {
                    if (EpicMonster != null)
                    {
                        SpellManager.Q.Cast(EpicMonster);
                    }

                    if (BigMonster != null)
                    {
                        SpellManager.Q.Cast(BigMonster);
                    }

                    if (Player.Instance.CountEnemiesInRange(800) >= 1 && BigMonster.Health <= Damage.DmgQ(BigMonster))
                    {
                        SpellManager.Q.Cast(BigMonster);
                    }
                }

                if (MiniMonster.FirstOrDefault(m => m.Distance(Player.Instance.ServerPosition) <= Player.Instance.AttackRange) != null)
                {
                    Orbwalker.ForcedTarget = MiniMonster.FirstOrDefault();
                    JungleMonster = MiniMonster;
                }
            }
                        
            if (SpellManager.R.IsReady())
            {
                if (MenuJungle["Jungle.R.Use"].Cast<CheckBox>().CurrentValue && Player.Instance.ManaPercent > MenuJungle["Jungle.R.Mana"].Cast<Slider>().CurrentValue &&
                    Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > MenuJungle["Jungle.R.Count"].Cast<Slider>().CurrentValue)
                {
                    var RPrediction = Prediction.Position.PredictCircularMissile(BigMonster, SpellManager.R.Range, 135, 0, 1000);

                    if (RPrediction.HitChance >= HitChance.High)
                    {
                        SpellManager.R.Cast(RPrediction.CastPosition);
                    }
                }
            }
        }   //End Jungle
    }
}
