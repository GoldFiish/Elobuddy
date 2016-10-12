using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;

namespace NebulaSkin
{
    class Model : Skin
    {
        public static void OnCreate(GameObject sender, EventArgs args)
        {
            var Unit = sender as Obj_AI_Minion;

            if (Unit != null)
            {
                if (Unit.Name.Contains("Ward") && !Unit.BaseSkinName.Contains("WardCorpse") && Unit.Buffs.Where(x =>x.IsValid &&  x.Caster.IsMe) != null )
                {
                    Core.DelayAction(() =>
                    {
                        Unit.SetSkinId(Menu["Ward.Skin"].Cast<Slider>().CurrentValue);
                    }, 0);
                }
            }
        }
    }
}
