using EloBuddy;
using EloBuddy.SDK;
using NebulaTwistedFate.Modes;

namespace NebulaTwistedFate
{
    class Damage
    {
        public static float DmgIgnite(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.True, 50 + 20 * Player.Instance.Level - (target.HPRegenRate / 5 * 3));
        }     
 
        public static float DmgQ(Obj_AI_Base target)
        {
            return target.CalculateDamageOnUnit(target, DamageType.Magical,
               new[] { 0, 60, 105, 150, 195, 240 }[SpellManager.Q.Level] + (Player.Instance.TotalMagicalDamage * 0.65f)); //FlatMagicDamageMod?
        }

        public static float PredictWDamage(Obj_AI_Base target, Cards card)
        {
            if (target != null)
            {
                return DmgW(target, card);
            }
            return 0f;
        }

        public static float DmgW(Obj_AI_Base target, Cards card)
        {
            if (Player.Instance.HasBuff("bluecardpreattack") || Cards.Blue.Equals(card))
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Mixed,
                    new[] { 0, 40, 60, 80, 100, 120 }[SpellManager.W.Level]) + (Player.Instance.TotalMagicalDamage * 0.5f) + (Player.Instance.TotalAttackDamage);
            }
            if (Player.Instance.HasBuff("redcardpreattack") || Cards.Red.Equals(card))
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Mixed,
                    new[] { 0, 30, 45, 60, 75, 90 }[SpellManager.W.Level] + (Player.Instance.TotalMagicalDamage * 0.5f) + Player.Instance.TotalAttackDamage);
            }
            if (Player.Instance.HasBuff("goldcardpreattack") || Cards.Yellow.Equals(card))
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Mixed,
                    new[] { 0, 15, 22.5f, 30, 37.5f, 45 }[SpellManager.W.Level] + (Player.Instance.TotalMagicalDamage * 0.5f) + Player.Instance.TotalAttackDamage);
            }
            return 0;
        }

        public static float DmgE(Obj_AI_Base target)
        {
            if (Player.Instance.HasBuff("cardmasterstackparticle"))
            {
                return target.CalculateDamageOnUnit(target, DamageType.Magical,
                    new[] { 0, 55, 80, 105, 130, 155 }[SpellManager.E.Level] + (Player.Instance.TotalMagicalDamage * 0.5f));
            }
            return 0;
        }

        public static float DmgECla(Obj_AI_Base target)
        {
            var damage = 0f;
            var Bdamage = 0f;
            var Mdamage = 0f;

            if (Mode_Item.TriniForce.IsOwned() && Player.HasBuff("sheen"))
            {
                Bdamage = Player.Instance.BaseAttackDamage * 2;
            }           
            else if ((Mode_Item.Sheen.IsOwned() && Player.HasBuff("sheen")) || (Mode_Item.IceGauntlet.IsOwned() && Player.HasBuff("itemfrozenfist")))
            {
                Bdamage = Player.Instance.BaseAttackDamage;
            }

            if ((Mode_Item.LichBane.IsOwned() && Player.HasBuff("sheen")))
            {
                Mdamage = (Player.Instance.BaseAttackDamage * 0.75f) + (Player.Instance.TotalMagicalDamage * 0.5f);
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

            return Player.Instance.GetAutoAttackDamage(target) + damage + Mdamage;
        }

        public static float DmgCla(Obj_AI_Base target)
        {
            var damage = 0f;
            var Bdamage = 0f;
            var Mdamage = 0f;

            if (Mode_Item.TriniForce.IsOwned() && Player.HasBuff("sheen"))
            {
                Bdamage = Player.Instance.BaseAttackDamage * 2;
            }
            else if ((Mode_Item.Sheen.IsOwned() && Player.HasBuff("sheen")) ||
                     (Mode_Item.IceGauntlet.IsOwned() && Player.HasBuff("itemfrozenfist")))
            {
                Bdamage = Player.Instance.BaseAttackDamage;
            }

            if ((Mode_Item.LichBane.IsOwned() && Player.HasBuff("sheen")))
            {
                Mdamage = (Player.Instance.BaseAttackDamage * 0.75f) + (Player.Instance.TotalMagicalDamage * 0.5f);
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
                damage += DmgQ(target);
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

            return Player.Instance.GetAutoAttackDamage(target) + damage + Mdamage + Bdamage;
        }
    }
}
