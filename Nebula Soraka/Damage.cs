using EloBuddy;
using EloBuddy.SDK;
using NebulaSoraka.Modes;

namespace NebulaSoraka
{
    class Damage
    {
        public static float DmgIgnite(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.True, 50 + 20 * Player.Instance.Level - (target.HPRegenRate / 5 * 3));
        }
        public static float DmgRedemption(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.True, target.MaxHealth * 0.1f);
        }
 
        public static float DmgQ(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.Magical,
               new[] { 0, 70, 110, 150, 190, 230 }[SpellManager.Q.Level] + (Player.Instance.TotalMagicalDamage * 0.35f));
        }

        public static float DmgE(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 0, 70, 110, 150, 190, 230 }[SpellManager.E.Level] + (Player.Instance.TotalMagicalDamage * 0.4f));
        }

        public static float DmgCla(Obj_AI_Base target)
        {
            var damage = 0f;
            var Bdamage = 0f;

            if (Mode_Item.TriniForce.IsOwned() && Player.HasBuff("sheen"))
            {
                Bdamage = Player.Instance.BaseAttackDamage * 2;
            }
            else if ((Mode_Item.Sheen.IsOwned() && Player.HasBuff("sheen")) ||
                     (Mode_Item.IceGauntlet.IsOwned() && Player.HasBuff("itemfrozenfist")))
            {
                Bdamage = Player.Instance.BaseAttackDamage;
            }

            if (Player.Instance.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown && SpellManager.Ignite.IsReady())
            {
                damage += 50 + 20 * ObjectManager.Player.Level - (target.HPRegenRate / 5 * 3);
            }

            if (Mode_Item.Bilgewater.IsOwned() && Mode_Item.Bilgewater.IsReady())
            {
                damage += Player.Instance.GetItemDamage(target, ItemId.Bilgewater_Cutlass);
            }

            if (Mode_Item.BladeKing.IsOwned() && Mode_Item.BladeKing.IsReady())
            {
                damage += Player.Instance.GetItemDamage(target, ItemId.Blade_of_the_Ruined_King);
            }

            if (SpellManager.Q.IsReady())
            {
                damage += Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                            (new float[] { 0, 70, 110, 150, 190, 230 }[SpellManager.Q.Level] + Player.Instance.FlatPhysicalDamageMod) + Bdamage) + 
                            Player.Instance.GetAutoAttackDamage(target);
            }

            if (SpellManager.E.IsReady())
            {
                damage += DmgE(target);
            }

            if (target.BaseSkinName == "Moredkaiser") { damage -= target.Mana; }

            if (Player.Instance.HasBuff("SummonerExhaust")) { damage = damage * 0.6f; }

            if (target.HasBuff("GarenW")) { damage = damage * 0.7f; }

            if (target.HasBuff("ferocioushowl")) { damage = damage * 0.7f; }

            if (target.HasBuff("BlitzcrankManaBarrierCD") && target.HasBuff("ManaBarrier")) { damage -= target.Mana / 2f; }

            return Player.Instance.GetAutoAttackDamage(target) + damage;
        }
    }
}
