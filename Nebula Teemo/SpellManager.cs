using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace NebulaTeemo
{
    internal class SpellManager
    {
        public static Spell.Targeted Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Skillshot R { get;private set; }

        static SpellManager()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 680);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, (uint)Player.Instance.AttackRange);
            R = new Spell.Skillshot(SpellSlot.R, 0, SkillShotType.Circular, 1000, 1000, 135);
        }
    }   //End SpellManager
}
