using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
namespace NebulaTeemo
{
    internal class Damage
    {
        public static float DmgIgnite = 50 + 20 * Player.Instance.Level;

        public static float DmgQ(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 0, 80, 125, 170, 215, 260 }[SpellManager.Q.Level] + (Player.Instance.TotalMagicalDamage * 0.8f));
        }

        public static float DmgE(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new[] { 10, 20, 30, 40, 50 }[SpellManager.R.Level] + (Player.Instance.TotalMagicalDamage * 0.3f)));
        }

        public static float DmgR(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new[] { 0, 200, 325, 450 }[SpellManager.R.Level] + (Player.Instance.TotalMagicalDamage * 0.5f)));
        }

        public static float DmgCal(Obj_AI_Base target)
        {
            if (target == null) return 0;

            float damage = 0;

            //damage = damage + DmgE(target);

            if (SpellManager.Q.IsReady())
            {
                damage = damage + DmgQ(target);
            }

            //if (SpellManager.E.IsReady())
            //{
            //    damage = damage + DmgE(target);
            //}

            if (SpellManager.R.IsReady())
            {
                damage = damage + DmgR(target);
            }

            return damage;
        }
    }
}
