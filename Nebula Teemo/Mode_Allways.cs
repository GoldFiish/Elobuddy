using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaTeemo
{
    internal class Mode_Allways : Teemo
    {
        public static void AutoR()
        {
            if (Player.Instance.IsDead) return;

            var Rtarget = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(1200) && SpellManager.R.IsInRange(x)).FirstOrDefault();

            if (Rtarget != null && SpellManager.R.IsReady())
            {
                if (MenuMisc["Auto.R"].Cast<CheckBox>().CurrentValue && Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo > 0)
                {
                    if (Rtarget.HasBuff("zhonyasringshield") || Rtarget.HasBuff("Recall") || Rtarget.HasBuff("teleport") || Rtarget.HasBuff("Pantheon_GrandSkyfall_Jump") || Rtarget.HasBuff("teleport_target") ||
                        Rtarget.HasBuff("BardRStasis") || Rtarget.HasBuffOfType(BuffType.Stun) || Rtarget.HasBuffOfType(BuffType.Snare) || Rtarget.HasBuffOfType(BuffType.Taunt) || Rtarget.HasBuffOfType(BuffType.Charm) ||
                        Rtarget.HasBuffOfType(BuffType.Suppression) || Rtarget.HasBuffOfType(BuffType.Knockup))
                    {
                        SpellManager.R.Cast(Rtarget.Position);
                    }                    
                }
            }
        }  //End AutoR
    } 
}
