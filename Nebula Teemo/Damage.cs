using EloBuddy;
using EloBuddy.SDK;
namespace NebulaTeemo
{
    internal class Damage
    {
        public static float DmgIgnite = 50 + (20 * Player.Instance.Level);

        public static float Bilgewater(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.Magical, 100);
        }

        public static float DmgHextech(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.Magical, 250 + (Player.Instance.TotalMagicalDamage * 0.3f));
        }
        public static float DmgQ(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 0, 80, 125, 170, 215, 260 }[SpellManager.Q.Level] + (Player.Instance.TotalMagicalDamage * 0.8f));
        }

        public static float DmgE(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new[] { 0, 10, 20, 30, 40, 50 }[SpellManager.E.Level] + (Player.Instance.TotalMagicalDamage * 0.3f) +
                     new[] { 0, 6, 12, 18, 24, 30 }[SpellManager.E.Level] + (Player.Instance.TotalMagicalDamage * 0.1f)));
        }

        public static float DmgR(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new[] { 0, 200, 325, 450 }[SpellManager.R.Level] + (Player.Instance.TotalMagicalDamage * 0.5f)));
        }

        public static double DmgCal(AIHeroClient target)
        {
            if (target.HasUndyingBuff() || target.IsInvulnerable) { return 0; }

            var damage = 0d;

            damage += ObjectManager.Player.GetAutoAttackDamage(target) +
                ((Player.Instance.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown && Mode_Item.Ignite.IsReady())
                ? 50 + 20 * ObjectManager.Player.Level - (target.HPRegenRate / 5 * 3) : 0d) +
                (SpellManager.Q.IsReady() ? DmgQ(target) : 0d) +
                (SpellManager.E.IsReady() ? DmgE(target) : 0d) +
                (SpellManager.R.IsReady() ? DmgR(target) : 0d);

            if (target.ChampionName == "Moredkaiser") { damage -= target.Mana; }

            if (ObjectManager.Player.HasBuff("SummonerExhaust")) { damage = damage * 0.6f; }

            if (target.HasBuff("GarenW")) { damage = damage * 0.7f; }

            if (target.HasBuff("ferocioushowl")) { damage = damage * 0.7f; }

            if (target.HasBuff("BlitzcrankManaBarrierCD") && target.HasBuff("ManaBarrier")) { damage -= target.Mana / 2f; }

            return damage;
        }

        public static double DmgCalSteal(AIHeroClient target)
        {
            if (target.HasUndyingBuff() || target.IsInvulnerable) { return 0; }

            var damage = 0d;

            damage += ObjectManager.Player.GetAutoAttackDamage(target) +
                ((Player.Instance.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown && Mode_Item.Ignite.IsReady())
                ? 50 + 20 * ObjectManager.Player.Level - (target.HPRegenRate / 5 * 3) : 0d) +
                (Mode_Item.Bilgewater.IsOwned() && Mode_Item.Bilgewater.IsReady() ? Player.Instance.GetItemDamage(target, ItemId.Bilgewater_Cutlass) : 0d) +
                (Mode_Item.BladeKing.IsOwned() && Mode_Item.BladeKing.IsReady() ? Player.Instance.GetItemDamage(target, ItemId.Blade_of_the_Ruined_King) : 0d) +
                (Mode_Item.Hextech.IsOwned() && Mode_Item.Hextech.IsReady() ? Player.Instance.GetItemDamage(target, ItemId.Hextech_Gunblade) : 0d) +
                (SpellManager.Q.IsReady() ? DmgQ(target) : 0d) +
                (SpellManager.E.IsReady() ? DmgE(target) : 0d);

            if (target.ChampionName == "Moredkaiser") { damage -= target.Mana; }

            if (ObjectManager.Player.HasBuff("SummonerExhaust")) { damage = damage * 0.6f; }

            if (target.HasBuff("GarenW")) { damage = damage * 0.7f; }

            if (target.HasBuff("ferocioushowl")) { damage = damage * 0.7f; }

            if (target.HasBuff("BlitzcrankManaBarrierCD") && target.HasBuff("ManaBarrier")) { damage -= target.Mana / 2f; }

            return damage;
        }
    }
}
