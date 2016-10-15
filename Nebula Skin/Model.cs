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
        static string[] SubModel =
        {
            "AnnieTibbers",
            "BardFollower",
            "EliseSpider", "EliseSpiderling",
            "GangplankBarrel",
            "GnarBig",
            "HeimerTBlue", "HeimerTYellow",
            "IllaoiMinion",
            "JhinTrap",
            "JinxMine",
            "KalistaAltar", "KalistaSpawn",
            "KindredWolf",
            "KledMount", "KledRider",
            "KogMawDead",
            "MaokaiSproutling",
            "QuinnValor",
            "RekSaiTunnel",
            "ShacoBox",
            "ShyvanaDragon",
            "SwainBeam", "SwainNoBird", "SwainRaven",
            "TeemoMushroom",
            "ThreshLantern",
            "ZacRebirthBloblet",
            "ZedShadow",
        };

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

                if (Unit.Name.Contains("Minion") && !Unit.IsDead)
                {
                    if (Map.Contains("SRUAP"))
                    {
                        if (Menu["Minions.Team"].Cast<ComboBox>().CurrentValue == 0)
                        {
                            if (Unit.SkinId != (Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2))
                            {
                                Core.DelayAction(() =>
                                {
                                    Unit.SetSkinId((Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2));
                                }, 0);
                            }
                        }
                        else
                        {
                            if (Unit.SkinId != (Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2) + 1)
                            {
                                Core.DelayAction(() =>
                                {
                                    Unit.SetSkinId((Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2) + 1);
                                }, 0);
                            }
                        }
                    }
                    else
                    {
                        if (Unit.SkinId != Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue)
                        {
                            Core.DelayAction(() =>
                            {
                                Unit.SetSkinId(Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue);
                            }, 0);
                        }
                    }
                }
            }          

            var model = EntityManager.MinionsAndMonsters.OtherAllyMinions.Where(x => x.Buffs.FirstOrDefault(b => b.IsValid && b.Caster.IsMe) != null).LastOrDefault();

            if (model != null && SubModel.Contains(model.BaseSkinName))
            {
                if(Player.Instance.SkinId == Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue)
                {
                    if (model.SkinId != Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue)
                    {
                        model.SetSkinId(Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue);
                    }
                }

                if (Server_String.Contains("true"))
                {
                    if (model.SkinId != ChromaNum - 1)
                    {
                        if (Player.Instance.SkinId == Menu["Skin.Chroma"].Cast<ComboBox>().CurrentValue + SkinNumList.LastOrDefault() + 1 ||
                        Player.Instance.SkinId == Menu["Skin.Chroma"].Cast<ComboBox>().CurrentValue + ChromaNumlist.FirstOrDefault() || Player.Instance.SkinId == ChromaNum - 1)
                        {
                            model.SetSkinId(ChromaNum - 1);
                        }
                    }
                }
            }
        }
    }
}
