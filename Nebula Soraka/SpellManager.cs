using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace NebulaSoraka
{
    public static class SpellManager
    {
        public static Spell.Targeted Ignite { get; private set; }        
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Targeted W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Active R { get; private set; }
       
        static SpellManager()
        {
            if (Player.Instance.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown)
            {
                Ignite = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("summonerdot"), 600);
            }

            Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Circular, 283, 1100, 210) { AllowedCollisionCount = int.MaxValue };
            W = new Spell.Targeted(SpellSlot.W, 550);
            E = new Spell.Skillshot(SpellSlot.E, 925, SkillShotType.Circular, 500, 1750, 70) { AllowedCollisionCount = int.MaxValue };
            R = new Spell.Active(SpellSlot.R, 30000);           
        }
    }
}
