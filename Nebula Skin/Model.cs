using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaSkin
{
    class Model : Skin
    {
        static string[] SubModel =
        {
            "AnnieTibbers",
            "BardFollower", "BardHealthShrine", "BardPickup", "BardPickupNoIcon",
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
            //need more test :(
            "AniviaEgg", "AniviaIceblock",
            "AzirSoldier", "AzirSunDisc", "AzirTowerClicker", "AzirUltSoldier",
            "CaitlynTrap",
            "Cassiopeia_Death",
            "CorkiBomb", "CorkiBombAlly",
            "FizzBait", "FizzShark",
            "MonkeyKingFlying",
            "NasusUlt",
            "NidaleeCougar", "NidaleeSpear",
            "SyndraOrbs", "SyndraSphere",
            "TaliyahWallChunk",
            "YorickBigGhoul", "YorickGhoulMelee", "YorickWGhoul",
            "ZyraGraspingPlant", "ZyraPassive", "ZyraSeed", "ZyraThornPlant",
            "IvernMinion", "IvernTotem",
            "JarvanIVStandard", "JarvanIVWall",
            "MalzaharVoidling",
            "OlafAxe",
            "OriannaBall", "OriannaNoBall",
            "RammusDBC", "RammusPB",
            "ShenSpirit",
            "SkarnerPassiveCrystal",
            "TaricGem",
            "TrundleWall",
            "UdyrPhoenix", "UdyrPhoenixUlt", "UdyrTiger", "UdyrTigerUlt", "UdyrTurtle", "UdyrTurtleUlt", "UdyrUlt",
            "LuluBlob", "LuluCupcake", "LuluDragon", "LuluFaerie", "LuluKitty", "LuluLadybug", "LuluPig", "LuluSeal", "LuluSnowman", "LuluSquill"
        };

        public static void OnCreate(GameObject sender, EventArgs args)
        {
            var Unit = sender as Obj_AI_Minion;
          
            if (Unit != null)
            {
                if (Unit.Name.Contains("Ward") && !Unit.BaseSkinName.Contains("WardCorpse") && Unit.Buffs.Where(x =>x.IsValid &&  x.Caster.IsMe) != null )
                {
                    if (Menu["Ward.Skin"].Cast<Slider>().CurrentValue != Menu["Ward.Skin"].Cast<Slider>().MaxValue)
                    {
                        Core.DelayAction(() =>
                        {
                            Unit.SetSkinId(Menu["Ward.Skin"].Cast<Slider>().CurrentValue);
                        }, 0);
                    }
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
                if (ChromaTrueIndex == -1)
                {
                    if (Player.Instance.SkinId == Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue)
                    {
                        if (model.SkinId != Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue)
                        {
                            model.SetSkinId(Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue);
                        }
                    }
                }

                if (ChromaTrueIndex != -1)               
                {
                    if (Player.Instance.SkinId == ChromaTrueIndex ||
                        Player.Instance.SkinId == Menu["Skin.Chroma"].Cast<ComboBox>().CurrentValue + ChromaStartNum ||
                        Player.Instance.SkinId == Menu["Skin.Chroma"].Cast<ComboBox>().CurrentValue + GetSkinInfo.Last().num + 1)
                    {
                        if (model.SkinId != ChromaTrueIndex)
                        {
                            model.SetSkinId(ChromaTrueIndex);
                        }
                    }
                    else if (Player.Instance.SkinId < ChromaStartNum)
                    {
                        if (Player.Instance.SkinId == ChromaTrueIndex) return;

                        if (model.SkinId != Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue)
                        {
                            model.SetSkinId(Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue);
                        }
                    }
                    else if (Player.Instance.SkinId >= ChromaStartNum)
                    {
                        if (Player.Instance.SkinId == Menu["Skin.Chroma"].Cast<ComboBox>().CurrentValue + ChromaStartNum ||
                            Player.Instance.SkinId == Menu["Skin.Chroma"].Cast<ComboBox>().CurrentValue + GetSkinInfo.Last().num + 1) return;

                        if (model.SkinId != Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue + ChromaCount)
                        {
                            model.SetSkinId(Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue + ChromaCount);
                        }
                    }
                }
            }
        }
    }
}
