using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;

namespace NebulaSkin
{
    class Model : Skin
    {      
        public static List<Obj_AI_Minion> ChangedSkin = new List<Obj_AI_Minion>();
        public static Obj_AI_Minion WardGet;
      
        public static void OnTick(EventArgs args)
        {
            WardGet = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.Name.Contains("Ward") && !ChangedSkin.Contains(x) && x.Buffs.FirstOrDefault(b => b.IsValid && b.Caster.IsMe) != null).LastOrDefault();
            if (WardGet != null)
            {
                var WardCount = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.Name.Contains("Ward") && x.Buffs.FirstOrDefault(b => b.IsValid && b.Caster.IsMe) != null).Count();
                                
                if (WardCount > 6)
                {
                    ChangedSkin = new List<Obj_AI_Minion>();
                    //return;                   
                }
                else
                {
                    ChangedSkin.Add(WardGet);
                    WardGet.SetSkin("SightWard", Menu["Ward.Skin"].Cast<Slider>().CurrentValue);
                }
            }
        }
    }
}
