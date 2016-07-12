using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace NebulaKalista
{
    public class Mode_Jungle
    {       
        public static void JungleClear()
        {
            if (!Orbwalker.CanMove) { return; }
         
            if (Kalista.MenuFarm["Jungle.E"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.ManaPercent > Kalista.MenuFarm["Jungle.Mana"].Cast<Slider>().CurrentValue)
			{
                if (EntityManager.MinionsAndMonsters.Monsters.Any(x => x.Health <= x.Get_E_Damage_Double()))
                {
                    SpellManager.E.Cast();
                }
            }

            if (Kalista.MenuFarm["Jungle.E.Big"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.ManaPercent > Kalista.MenuFarm["Jungle.Mana"].Cast<Slider>().CurrentValue)
            {
                if (EntityManager.MinionsAndMonsters.Monsters.Any(x => Extensions.IsRendKillable(x) &&  x.BaseSkinName.ToLower().Contains("Dragon") || x.BaseSkinName.ToLower().Contains("Herald") 
                || x.BaseSkinName.ToLower().Contains("Baron")))
                {
                    SpellManager.E.Cast();                    
                }

                if (EntityManager.MinionsAndMonsters.Monsters.Any(x => x.Health <= x.Get_E_Damage_Double() && !x.Name.Contains("Mini")))
                {
                    SpellManager.E.Cast();
                }
            }

            if (Kalista.MenuFarm["Jungle.Q"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.ManaPercent > Kalista.MenuFarm["Jungle.Mana"].Cast<Slider>().CurrentValue)
            {
                var Jtarget = EntityManager.MinionsAndMonsters.GetJungleMonsters(ObjectManager.Player.ServerPosition, SpellManager.Q.Range);
              
                foreach (Obj_AI_Base minion in Jtarget)
                {
                    if (Jtarget.Any(x => x.Health <= Player.Instance.GetSpellDamage(x, SpellSlot.Q) && SpellManager.Q.GetPrediction(minion).HitChance >= HitChance.High))
                    {
                        SpellManager.Q.Cast(minion);
                    }
                }
            }

            if (Kalista.MenuFarm["Jungle.Q.Big"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.ManaPercent > Kalista.MenuFarm["Jungle.Mana"].Cast<Slider>().CurrentValue)
            {
                var Jtarget = EntityManager.MinionsAndMonsters.GetJungleMonsters(ObjectManager.Player.ServerPosition, SpellManager.Q.Range);
                
                foreach (Obj_AI_Base minion in Jtarget)
                {
                    if (Jtarget.Any(x => x.Health <= Player.Instance.GetSpellDamage(x, SpellSlot.Q) && !x.Name.Contains("Mini")  && SpellManager.Q.GetPrediction(minion).HitChance >= HitChance.High))
                    {
                        SpellManager.Q.Cast(minion);
                    }
                }
            }
        }
    }
}
