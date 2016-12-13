using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace NebulaNasus
{
    public static class SpellManager
    {
        public static Spell.Targeted Ignite { get; private set; }        
        public static Spell.Active Q { get; private set; }
        public static Spell.Targeted W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Active R { get; private set; }
       
        static SpellManager()
        {
            if (Player.Instance.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown)
            {
                Ignite = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("summonerdot"), 600);
            }

            Q = new Spell.Active(SpellSlot.Q, 230);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Skillshot(SpellSlot.E, 650, SkillShotType.Circular, 500, 20, 380) { AllowedCollisionCount = int.MaxValue };
            R = new Spell.Active(SpellSlot.R, 500);           
        }
    }
}
