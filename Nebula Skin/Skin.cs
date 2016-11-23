using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Resources;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.Sandbox;
using Newtonsoft.Json;

namespace NebulaSkin
{ 
    internal class Skin
    {  
        static string DragonVersion;
        static string DragonSkin;
        public static List<DragonJson.Skin> GetSkinInfo;
        public static int ChromaIndex;
        public static int ChromaCount;
        public static int ChromaStartNum;

        public static Menu Menu, MenuVer, MenuNVer;
        public static string Map;
        static ResourceManager Res_Language;

        static String[] Language_List = new String[] { "en_US", "ko_KR", "ja_JP", "es_ES", "fr_FR", "de_DE", "it_IT", "pl_PL", "el_GR", "hu_HU", "cs_CZ", "ro_RO", "pt_BR", "id_ID", "ru_RU", "tr_TR"};
        static string Language_Path = SandboxConfig.DataDirectory + "\\MenuSaveData\\Nebula Skin_Culture_Set.txt";

        public static void Load()
        {
            Map = EntityManager.Turrets.Allies.FirstOrDefault().BaseSkinName;

            Console.WriteLine("Nebula skin state");

            Language_Set();

            Chat.Print("<font color = '#94cdfc'>Welcome to </font><font color = '#ffffff'>[ Nebula ] Skin</font><font color = '#94cdfc'>. Addon is ready.</font>");

            Menu = MainMenu.AddMenu("[ Nebula ] Skin", "By.Natrium");
            Menu.AddLabel(Res_Language.GetString("Main_Text_1"));
            Menu.AddLabel(Res_Language.GetString("Main_Text_2"));
            Menu.AddSeparator();
          
            Menu.Add("Language.Select", new ComboBox(Res_Language.GetString("Main_Language_Select"), 0, 
                "English", "한국어", "Japanese", "Spanish", "French", "German", "Italian", "Polish", "Greek", "Hungarian", "Czech", "Romanian", "Portuguese (Brazil)", "Indonesia", "Russian", "Turkish"));
            Menu.AddSeparator();

            Get_SkinInfo();
            
            Menu.AddSeparator();
            if (Map.Contains("SRUAP"))
            {
                Menu.AddLabel(Res_Language.GetString("Label_Minions") + Res_Language.GetString("Label_Map_SRU"));

                Menu.Add("Minions.Team", new ComboBox(Res_Language.GetString("Label_TeamColor"), 0,
                    Res_Language.GetString("Label_TeamColor_0"), Res_Language.GetString("Label_TeamColor_1")));
                Menu.Add("Minions.Skin", new ComboBox(Res_Language.GetString("Label_Skin"), 0, 
                    Res_Language.GetString("Label_Minions_0"), Res_Language.GetString("Label_Minions_1"), Res_Language.GetString("Label_Minions_2"),
                    Res_Language.GetString("Label_Minions_3"), Res_Language.GetString("Label_Minions_4"), Res_Language.GetString("Label_Minions_5")));                
            }
            else
            {
                if (Map.Contains("HA"))
                {
                    Menu.AddLabel(Res_Language.GetString("Label_Minions") + Res_Language.GetString("Label_Map_HA"));
                }
                else
                {
                    Menu.AddLabel(Res_Language.GetString("Label_Minions") + Res_Language.GetString("Label_Map_TT"));
                }
                Menu.Add("Minions.Skin", new ComboBox(Res_Language.GetString("Label_Skin"), 0,
                    Res_Language.GetString("Label_Minions_0"), Res_Language.GetString("Label_Minions_3"), Res_Language.GetString("Label_Minions_4"),
                    Res_Language.GetString("Label_Minions_5")));
            }

            Menu.AddSeparator();
            Menu.AddLabel(Res_Language.GetString("Label_Ward"));            
            Menu.AddVisualFrame(new WardPreview("Ward.Preview", System.Drawing.Color.Purple));
            Menu.Add("Ward.Skin", new Slider(Res_Language.GetString("Label_Skin"), 0, 0, WardPreview.Ward_Name.Count() - 1));
            
            MenuVer = Menu.AddSubMenu("Local " + CheckVersion.LocalVersion, "Sub0");
            MenuVer.AddGroupLabel(Res_Language.GetString("Label_By"));
            MenuVer.AddLabel(Res_Language.GetString("Label_NoticeThx"));

            CheckVersion.CheckUpdate();

            Player.SetSkinId(Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue);
            Menu["Skin.Nomal"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
            {
                if (ChromaIndex == -1)
                {
                    Player.SetSkinId(vargs.NewValue);
                }
                else
                {
                    if (ChromaCount == 0)
                    {
                        Player.SetSkinId(vargs.NewValue);
                    }
                    else
                    {
                        if(Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue == ChromaStartNum)
                        {
                            Player.SetSkinId(vargs.NewValue + ChromaIndex);
                        }
                        else
                        {
                            Player.SetSkinId(vargs.NewValue);
                        }
                    }
                }
            };

            if (ChromaIndex != -1 && Menu["Skin.Chroma"].Cast<ComboBox>().IsVisible)
            {
                Menu["Skin.Chroma"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
                {
                    if (ChromaCount == 0)
                    {
                        Player.SetSkinId(vargs.NewValue + GetSkinInfo.Last().num + 1);
                    }
                    else
                    {
                        Player.SetSkinId(vargs.NewValue + ChromaStartNum);
                    }
                };
            }

            Menu["Ward.Skin"].Cast<Slider>().OnValueChange += (sender, vargs) =>
            {
                foreach (var DyWard in EntityManager.MinionsAndMonsters.OtherAllyMinions.Where(x => x.Name.Contains("Ward") && !x.BaseSkinName.Contains("WardCorpse") && x.Buffs.FirstOrDefault(b => b.IsValid && b.Caster.IsMe) != null))
                {
                    if (vargs.NewValue != Menu["Ward.Skin"].Cast<Slider>().MaxValue)
                    {
                        if (DyWard.SkinId != Menu["Ward.Skin"].Cast<Slider>().CurrentValue)
                        {
                            DyWard.SetSkinId(Menu["Ward.Skin"].Cast<Slider>().CurrentValue);
                        }
                    }
                }
            };

            if (Map.Contains("SRUAP"))
            {
                Menu["Minions.Team"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
                {
                    if (vargs.NewValue == 0)
                    {
                        foreach (var DyMinions in EntityManager.MinionsAndMonsters.Minions.Where(x => x.Name.Contains("Minion")))
                        {
                            if (DyMinions.SkinId != (Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2))
                            {
                                DyMinions.SetSkinId(DyMinions.SkinId);
                                DyMinions.SetSkinId((Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2));
                            }
                        }
                    }
                    else
                    {
                        foreach (var DyMinions in EntityManager.MinionsAndMonsters.Minions.Where(x => x.Name.Contains("Minion")))
                        {
                            if (DyMinions.SkinId != (Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2) + 1)
                            {
                                DyMinions.SetSkinId(DyMinions.SkinId);
                                DyMinions.SetSkinId((Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2) + 1);
                            }
                        }
                    }
                };
            }

            Menu["Minions.Skin"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
            {
                foreach (var DyMinions in EntityManager.MinionsAndMonsters.Minions.Where(x => x.Name.Contains("Minion")))
                {
                    if (Map.Contains("SRUAP"))
                    {
                        if(Menu["Minions.Team"].Cast<ComboBox>().CurrentValue == 0)
                        {
                            if (DyMinions.SkinId != (Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2))
                            {
                                DyMinions.SetSkinId(DyMinions.SkinId);
                                DyMinions.SetSkinId((Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2));
                            }
                        }
                        else
                        {
                            if (DyMinions.SkinId != (Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2) + 1)
                            {
                                DyMinions.SetSkinId(DyMinions.SkinId);
                                DyMinions.SetSkinId((Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue * 2) + 1);
                            }
                        }
                    }
                    else
                    {
                        if (DyMinions.SkinId != Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue)
                        {
                            DyMinions.SetSkinId(DyMinions.SkinId);
                            DyMinions.SetSkinId(Menu["Minions.Skin"].Cast<ComboBox>().CurrentValue);
                        }
                    }
                }
            };

            Menu["Language.Select"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
            {
                var index = vargs.NewValue;
                File.WriteAllText(Language_Path, Language_List[index], Encoding.Default);
            };

            GameObject.OnCreate += Model.OnCreate;
        }

        private static void Get_SkinInfo()
        {
            WebRequest Request_Ver = WebRequest.Create("https://ddragon.leagueoflegends.com/api/versions.json");
            using (var Version_Response = (HttpWebResponse)Request_Ver.GetResponse())
            {
                Stream Version_Stream = Version_Response.GetResponseStream();
                StreamReader Version_Reader = new StreamReader(Version_Stream);
                DragonVersion = Version_Reader.ReadToEnd();
                Version_Response.Close();
                Version_Reader.Close();

                DragonVersion = JsonConvert.DeserializeObject<string[]>(DragonVersion)[0];
                Console.WriteLine("DDragon Version : " + DragonVersion);
            }

            WebRequest DragonSkinList = (HttpWebRequest)WebRequest.Create("http://ddragon.leagueoflegends.com/cdn/" + DragonVersion + "/data/" + File.ReadLines(Language_Path).First() + "/champion/" + Player.Instance.ChampionName + ".json");
            using (var Skin_Response = (HttpWebResponse)DragonSkinList.GetResponse())
            {
                Stream Skin_Stream = Skin_Response.GetResponseStream();
                StreamReader Skin_Reader = new StreamReader(Skin_Stream);
                DragonSkin = Skin_Reader.ReadToEnd();
                Skin_Response.Close();
                Skin_Reader.Close();

                Console.WriteLine("DDragon Skin Response : " + (Skin_Response).StatusDescription);
            }

            var DataConvert = JsonConvert.DeserializeObject<DragonJson>(DragonSkin);
            var GetBaseName = DataConvert.data[DataConvert.data.Keys.First().ToString()].name;
            GetSkinInfo = DataConvert.data[DataConvert.data.Keys.First().ToString()].skins;
            ChromaIndex = GetSkinInfo.FindIndex(x => x.chromas == true); //fail value -1
          
            string[] _SkinNomal = new string[GetSkinInfo.Count];
            for (int n = 0; n < GetSkinInfo.Count; n++)
            {
                _SkinNomal[n] = GetSkinInfo[n].name;
                ChromaStartNum = GetSkinInfo[n].num.CompareTo(n);

                if(ChromaStartNum == 1)
                {
                    ChromaStartNum = n;
                }
            }

            Menu.AddLabel(Res_Language.GetString("Label_Cham_Name") + GetBaseName);
            Menu.Add("Skin.Nomal", new ComboBox(Res_Language.GetString("Label_Skin"), 0, _SkinNomal));

            if(ChromaIndex > -1)
            {
                Console.WriteLine(GetBaseName + " have chroma skin");

                ChromaCount = (GetSkinInfo.Last().num + 1) - GetSkinInfo.Count;

                if (ChromaCount < 1)
                {
                    if (Player.Instance.ChampionName == "Cassiopeia" || Player.Instance.ChampionName == "Fizz" || Player.Instance.ChampionName == "Karthus" || Player.Instance.ChampionName == "Nasus" || Player.Instance.ChampionName == "Zac")
                    {
                        Menu.Add("Skin.Chroma", new ComboBox(Res_Language.GetString("Label_Chor_Name"), 0, "1", "2", "3"));
                    }

                    if (Player.Instance.ChampionName == "MissFortune" || Player.Instance.ChampionName == "Yasuo" || Player.Instance.ChampionName == "Braum" || Player.Instance.ChampionName == "Poppy")
                    {
                        Menu.Add("Skin.Chroma", new ComboBox(Res_Language.GetString("Label_Chor_Name"), 0, "1", "2", "3", "4", "5"));
                    }

                    if (Player.Instance.ChampionName == "XinZhao")
                    {
                        Menu.Add("Skin.Chroma", new ComboBox(Res_Language.GetString("Label_Chor_Name"), 0, "1", "2", "3", "4", "5", "6"));
                    }

                    if (Player.Instance.ChampionName == "Diana")
                    {
                        Menu.Add("Skin.Chroma", new ComboBox(Res_Language.GetString("Label_Chor_Name"), 0, "1", "2", "3", "4", "5", "6", "7"));
                    }

                    if (Player.Instance.ChampionName == "Ezreal" || Player.Instance.ChampionName == "Malphite" || Player.Instance.ChampionName == "Riven" || Player.Instance.ChampionName == "Fiora")
                    {
                        Menu.Add("Skin.Chroma", new ComboBox(Res_Language.GetString("Label_Chor_Name"), 0, "1", "2", "3", "4", "5", "6", "7", "8"));
                    }
                }
                else
                {
                    string[] LIST_C = new string[ChromaCount];

                    int k = 0;
                    int OP = 0;
                    for (k = 0; k < ChromaCount; k++)
                    {
                        LIST_C[OP] = Convert.ToString(OP + 1);
                        OP++;
                    }

                    Menu.Add("Skin.Chroma", new ComboBox(Res_Language.GetString("Label_Chor_Name"), 0, LIST_C));
                }
            }
        }

        private static void Language_Set()
        {
            try
            {
                FileInfo File_Check = new FileInfo(Language_Path);

                if (!File_Check.Exists)
                {
                    File.AppendAllText(Language_Path, "en_US", Encoding.Default);
                    Res_Language = new ResourceManager("NebulaSkin.Resources.en_US", typeof(Program).Assembly);
                    Console.WriteLine("Language Setting : en_US");
                }
                else
                {
                    Res_Language = new ResourceManager("NebulaSkin.Resources." + File.ReadLines(Language_Path).First(), typeof(Program).Assembly);
                    Console.WriteLine("Select Language : " + File.ReadLines(Language_Path).First());
                }
            }
            catch
            {
                Res_Language = new ResourceManager("NebulaSkin.Resources.en_US", typeof(Program).Assembly);
                Console.WriteLine("Default Language : en_US");
            }
        }
    }   //End Class
}