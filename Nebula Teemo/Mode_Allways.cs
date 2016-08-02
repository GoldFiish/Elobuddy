using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaTeemo
{
    internal class Mode_Allways : Teemo 
    {
        static readonly Item Zhonyas = new Item((int)ItemId.Zhonyas_Hourglass);
        //static readonly Item Biscuit = new Item((int)ItemId.Total_Biscuit_of_Rejuvenation);
        //static readonly Item Health = new Item((int)ItemId.Health_Potion );
        //static readonly Item Refillable = new Item((int)ItemId.Refillable_Potion);
        //static readonly Item Hunters = new Item((int)ItemId.Hunters_Potion);
        //static readonly Item Corrupting = new Item((int)ItemId.Corrupting_Potion);
        
        public static void Zy_Shield()
        {
            if (Player.Instance.IsDead) return;

            if (Player.Instance.CountEnemiesInRange(700) >= 1 && Zhonyas.IsOwned() && Zhonyas.IsReady())
            {
                if (Player.Instance.HealthPercent <= MenuItem["Item.Zy.Hp"].Cast<Slider>().CurrentValue)
                {
                    Zhonyas.Cast();
                }                
            }
        }   //End Zy_Shield

        public static void AutoR()
        {
            if (Player.Instance.IsDead) return;
            if (Player.Instance.CountEnemiesInRange(900) == 0) return;

            if (SpellManager.R.IsReady())
            { 
            if (MenuMisc["Auto.R"].Cast<CheckBox>().CurrentValue && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > 0)
                {
                    foreach (var Rtarget in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(SpellManager.R.Range) && !x.IsDead))
                    {
                        if (Rtarget.IsValidTarget(SpellManager.R.Range) && (Rtarget.HasBuff("zhonyasringshield") || Rtarget.HasBuff("Recall") || Rtarget.HasBuffOfType(BuffType.Stun) ||
                            Rtarget.HasBuffOfType(BuffType.Snare) || Rtarget.HasBuffOfType(BuffType.Taunt) || Rtarget.HasBuffOfType(BuffType.Charm) ||
                            Rtarget.HasBuffOfType(BuffType.Suppression) || Rtarget.HasBuffOfType(BuffType.Knockup)))                           
                        {
                            SpellManager.R.Cast(Rtarget.Position);
                        }
                    }
                }
            }
        }

        public static void Steal()
        {
            if (Player.Instance.IsDead) return;
            
            if (MenuMisc["Steal.Ignite"].Cast<CheckBox>().CurrentValue && Mode_Item.Ignite.IsReady())
            {
                foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(600) && x.TotalShieldHealth() <= Player.Instance.GetSummonerSpellDamage(x, DamageLibrary.SummonerSpells.Ignite) && !x.IsDead))
                {
                    Mode_Item.Ignite.Cast(target);                    
                }
            }

            if (MenuMisc["Steal.Skill"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(900) && !x.IsDead))
                {
                    if (target.IsValidTarget(550) && target.Health <= Damage.DmgE(target))
                    {
                        Player.IssueOrder(GameObjectOrder.AttackTo, target);
                    }
                   
                    if (SpellManager.R.IsReady() && target.IsValidTarget(SpellManager.R.Range) && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > 1)
                    {
                        var RPrediction = SpellManager.R.GetPrediction(target);

                        if (target.Health <= Damage.DmgR(target) + (Damage.DmgR(target) / 4) && RPrediction.HitChance >= HitChance.High)
                        {
                            SpellManager.R.Cast(RPrediction.CastPosition);
                        }
                    }

                    if (SpellManager.Q.IsReady() && target.IsValidTarget(680) && target.Health <= Damage.DmgQ(target))
                    {
                        SpellManager.Q.Cast(target);
                    }
                }
            }

            if (MenuMisc["Steal.Monster"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var target in EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(900) && !x.Name.Contains("Mini") &&
                (x.BaseSkinName.ToLower().Contains("dragon") || x.BaseSkinName.ToLower().Contains("herald") || x.BaseSkinName.ToLower().Contains("baron"))))
                {
                    Chat.Print(target.BaseSkinName);

                    if (target.IsValidTarget(550) && target.Health <= Damage.DmgE(target))
                    {
                        Player.IssueOrder(GameObjectOrder.AttackTo, target);
                    }

                    if (SpellManager.R.IsReady() && target.IsValidTarget(SpellManager.R.Range) && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > 2)
                    {
                        var RPrediction = SpellManager.R.GetPrediction(target);

                        if (target.Health <= Damage.DmgR(target) + (Damage.DmgR(target) / 4) && RPrediction.HitChance >= HitChance.High)
                        {
                            SpellManager.R.Cast(RPrediction.CastPosition);
                        }
                    }

                    if (SpellManager.Q.IsReady() && target.IsValidTarget(680) && target.Health <= Damage.DmgQ(target))
                    {                      
                        SpellManager.Q.Cast(target);
                    }   
                }
            }
        }
    }   //End Mode_Allways
}
