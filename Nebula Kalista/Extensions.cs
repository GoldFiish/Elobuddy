using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
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
            if (SpellManager.Q.IsReady())
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                    new float[] { 10, 70, 130, 190, 250 }[SpellManager.Q.Level - 1] + Player.Instance.TotalAttackDamage);
            }
            return 0;
        }

        public static List<string> UndyingBuffs = new List<string>
        {
            "JudicatorIntervention",    //Kayle [ R ]
            "UndyingRage",              //Tryndamere [ R ] - "Undying Rage",
            "Chrono Revive",
            "Chrono Shift",             //Zilean [ R ] - "ChronoShift",    
            "lissandrarself",
            "KindredRNoDeathBuff",      //"kindredrnodeathbuff",
            "malzaharpassiveshield",
            "BansheesVeil",             //"bansheesveil",
            "SivirShield",              //Sivir [ E ] - "SivirE",  
            "ShroudofDarkness",         //Nocturne [ W ] - "NocturneW"
            "BlackShield",
            "zhonyasringshield"
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

            if (target.BaseSkinName == "Moredkaiser")
            {
                dmg -= target.Mana;
            }

            if (target.HasBuff("GarenW"))
            {
                dmg *= 0.7f;
            }

            if (Player.Instance.HasBuff("summonerexhaust"))
            {
                dmg *= 0.6f;
            }

            if (target.HasBuff("FerociousHowl"))
            {
                dmg *= 0.3f;
            }

            if (target.Name.Contains("Baron") && Player.Instance.HasBuff("barontarget"))
            {
                dmg *= 0.5f;
            }

            if (target.Name.Contains("Dragon") && Player.Instance.HasBuff("s5test_dragonslayerbuff"))
            {
                dmg *= (1f - (0.075f * Player.Instance.GetBuffCount("s5test_dragonslayerbuff")));
            }
            return dmg > totalHealth;
        }

        public static float IsQEKillable(Obj_AI_Base target)
        {
            var dmg = 0f;

            if (SpellManager.E.IsReady())
            {
                dmg = (float)Get_E_Damage_Double(target);
            }

            if (SpellManager.Q.IsReady())
            {
                dmg += Get_Q_Damage_Float(target);
            }

            if (target.BaseSkinName == "Moredkaiser")
            {
                dmg -= target.Mana;
            }

            if (target.HasBuff("BlitzcrankManaBarrierCD") && target.HasBuff("ManaBarrier"))
            {
                dmg -= target.Mana / 2f;
            }

            if (target.HasBuff("GarenW"))
            {
                dmg *= 0.7f;
            }

            if (Player.Instance.HasBuff("summonerexhaust"))
            {
                dmg *= 0.6f;
            }

            if (target.HasBuff("FerociousHowl"))
            {
                dmg *= 0.3f;
            }

            if (target.Name.Contains("Baron") && Player.Instance.HasBuff("barontarget"))
            {
                dmg *= 0.5f;
            }

            if (target.Name.Contains("Dragon") && Player.Instance.HasBuff("s5test_dragonslayerbuff"))
            {
                dmg *= (1f - (0.075f * Player.Instance.GetBuffCount("s5test_dragonslayerbuff")));
            }
            return dmg;
        }

        public static float Get_E_Damage_Float(this Obj_AI_Base target)
        {
            return (float)Get_E_Damage(target, -1) + (Kalista.Status_CheckBox(Kalista.MenuMisc, "E_Dmage") ? Kalista.Status_Slider(Kalista.MenuMisc, "E_Dmage_Value") : 0);
        }

        public static double Get_E_Damage_Double(this Obj_AI_Base target)
        {
            return Get_E_Damage(target, -1) + (Kalista.Status_CheckBox(Kalista.MenuMisc, "E_Dmage") ? Kalista.Status_Slider(Kalista.MenuMisc, "E_Dmage_Value") : 0);
        }

        public static double Get_E_Damage(this Obj_AI_Base target, int customStacks = -1, BuffInstance rendBuff = null)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, GetRawRendDamage(target, customStacks, rendBuff));
        }

        public static float GetRawRendDamage(Obj_AI_Base target, int customStacks = -1, BuffInstance rendBuff = null)
        {
            rendBuff = rendBuff ?? GetRendBuff(target);
            var stacks = (customStacks > -1 ? customStacks : rendBuff != null ? rendBuff.Count : 0) - 1;
            if (stacks > -1)
            {
                var index = SpellManager.E.Level - 1;
                return RawRendDamage[index] + stacks * RawRendDamagePerSpear[index] +
                       Player.Instance.TotalAttackDamage * (RawRendDamageMultiplier[index] + stacks * RawRendDamagePerSpearMultiplier[index]);
            }
            return 0;
        }
    }
}
