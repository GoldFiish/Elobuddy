using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;

namespace NebulaKalista
{
    public static class Extensions
    {
        private static readonly float[] RawRendDamage = { 20, 30, 40, 50, 60 };
        private static readonly float[] RawRendDamageMultiplier = { 0.6f, 0.6f, 0.6f, 0.6f, 0.6f };
        private static readonly float[] RawRendDamagePerSpear = { 10, 14, 19, 25, 32 };
        private static readonly float[] RawRendDamagePerSpearMultiplier = { 0.2f, 0.225f, 0.25f, 0.275f, 0.3f };

        public static float Get_Q_Damage_Float(this Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                new float[] {10, 70, 130, 190, 250 }[SpellManager.Q.Level - 1] + Player.Instance.TotalAttackDamage);
        }
        
        public static List<string> UndyingBuffs = new List<string>
        {
            "JudicatorIntervention",
            "UndyingRage",
            "FerociousHowl",
            "ChronoRevive",
            "ChronoShift",
            "lissandrarself",
            "kindredrnodeathbuff",
            "malzaharpassiveshield",
            "bansheesveil",
            "SivirE",
            "NocturneW",
            "BlackShield"
        };
        
        public static bool ShouldntRend(AIHeroClient target)
        {
            if (target == null || !target.IsHPBarRendered) return false;
           
            if (UndyingBuffs.Any(buff => target.HasBuff(buff))) return true;
           
            if (target.CharData.BaseSkinName == "Blitzcrank" && !target.HasBuff("BlitzcrankManaBarrierCD")
                && !target.HasBuff("ManaBarrier"))
            {
                return true;
            }
                       
            return target.HasBuffOfType(BuffType.SpellShield) || target.HasBuffOfType(BuffType.SpellImmunity);
        }

        public static BuffInstance GetRendBuff(this Obj_AI_Base target)
        {
            return target.Buffs.FirstOrDefault(b => b.Name == "kalistaexpungemarker");
        }

        public static bool HasRendBuff(this Obj_AI_Base target)
        {
            return GetRendBuff(target) != null;
        }

        public static double GetTotalHealthWithShieldsApplied(this Obj_AI_Base target)
        {
            return target.Health + target.AllShield;
        }

        public static bool IsRendKillable(this Obj_AI_Base target)
        {
            if (target == null) { return false; }
            if (!HasRendBuff(target)) { return false; }
            if (target is AIHeroClient && target.Health > 1)
            {
                if (ShouldntRend((AIHeroClient)target)) return false;
            }
       
            var totalHealth = GetTotalHealthWithShieldsApplied(target);

            var dmg = Get_E_Damage_Double(target);

            if (ObjectManager.Player.HasBuff("summonerexhaust"))
                dmg *= 0.6f;

            if (target.HasBuff("FerociousHowl"))
                dmg *= 0.3f;

            if (target.Name.Contains("Baron") && ObjectManager.Player.HasBuff("barontarget"))
            {
                dmg *= 0.5f;
            }
            
            if (target.Name.Contains("Dragon") && ObjectManager.Player.HasBuff("s5test_dragonslayerbuff"))
            {
                dmg *= (1f - (0.075f * ObjectManager.Player.GetBuffCount("s5test_dragonslayerbuff")));
            }
            return dmg > totalHealth;
            //return IsRendKillable_f(target) > totalHealth;         
        }

        //public static float IsRendKillable_f(this Obj_AI_Base target)
        //{
        //    //var dmg = Get_E_Damage_Double(target) + (Kalista.MenuMisc["E.Dmage"].Cast<CheckBox>().CurrentValue ? Kalista.MenuMisc["E.Dmage.Value"].Cast<Slider>().CurrentValue : 0);
        //    //return (float)dmg;
        //}

        public static float Get_E_Damage_Float(this Obj_AI_Base target)
        {
            return (float)Get_E_Damage(target, -1);
        }
        public static double Get_E_Damage_Double(this Obj_AI_Base target)
        {
            //return Get_E_Damage(target, -1);
            return Get_E_Damage(target, -1) + (Kalista.MenuMisc["E.Dmage"].Cast<CheckBox>().CurrentValue ? Kalista.MenuMisc["E.Dmage.Value"].Cast<Slider>().CurrentValue : 0);
        }
             
        public static double Get_E_Damage(this Obj_AI_Base target, int customStacks = -1, BuffInstance rendBuff = null)
        {
            return ObjectManager.Player.CalculateDamageOnUnit(target, DamageType.Physical, GetRawRendDamage(target, customStacks, rendBuff));
        }

        public static float GetRawRendDamage(Obj_AI_Base target, int customStacks = -1, BuffInstance rendBuff = null)
        {
            rendBuff = rendBuff ?? GetRendBuff(target);
            var stacks = (customStacks > -1 ? customStacks : rendBuff != null ? rendBuff.Count : 0) - 1;
            if (stacks > -1)
            {
                var index = SpellManager.E.Level - 1;
                return RawRendDamage[index] + stacks * RawRendDamagePerSpear[index] +
                       ObjectManager.Player.TotalAttackDamage * (RawRendDamageMultiplier[index] + stacks * RawRendDamagePerSpearMultiplier[index]);
            }
            return 0;
        }
    }
}
