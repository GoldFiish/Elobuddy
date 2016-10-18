using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace NebulaSmite
{
    public class SpellManager
    {
        public static Spell.Targeted Smite { get; private set; }        

        static SpellManager()
        {
            Smite = new Spell.Targeted(Player.Instance.Spellbook.Spells.FirstOrDefault(s => s.SData.Name.ToLower().Contains("smite")).Slot, 570);
        }       
    }
}