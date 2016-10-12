using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Resources;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.Sandbox;


namespace NebulaSkin
{ 
    internal class Skin
    {  
        static string Server_StrVer;
        public static string Server_String;
        static string Server_C_Name;
        static string Cpoy_Server_String;

        public static Menu Menu, MenuVer, MenuNVer;

        static ResourceManager Res_Language;

        static String[] Language_List = new String[] { "en_US", "ko_KR", "ja_JP", "es_ES", "fr_FR", "de_DE", "it_IT", "pl_PL", "el_GR", "hu_HU", "cs_CZ", "ro_RO", "pt_BR", "id_ID", "ru_RU", "tr_TR"};
        static string Language_Path = SandboxConfig.DataDirectory + "\\MenuSaveData\\Nebula Skin_Culture_Set.txt";

        static List<int> SkinNumList = new List<int>();
        static List<int> ChromaNumlist = new List<int>();
                
        public static void Load()
        {
            var Map = EntityManager.Turrets.Allies.FirstOrDefault();

            Language_Set();

            Chat.Print("<font color = '#94cdfc'>Welcome to </font><font color = '#ffffff'>[ Nebula ] Skin</font><font color = '#94cdfc'>. Addon is ready.</font>");

            Menu = MainMenu.AddMenu("[ Neblua ] Skin", "By.Natrium");
            Menu.AddLabel(Res_Language.GetString("Main_Text_1"));
            Menu.AddLabel(Res_Language.GetString("Main_Text_2"));
            Menu.AddSeparator();
          
            Menu.Add("Language.Select", new ComboBox(Res_Language.GetString("Main_Language_Select"), 0, 
                "English", "한국어", "Japanese", "Spanish", "French", "German", "Italian", "Polish", "Greek", "Hungarian", "Czech", "Romanian", "Portuguese (Brazil)", "Indonesia", "Russian", "Turkish"));
            Menu.AddSeparator();

            Get_SkinInfo();
            
            Menu.AddSeparator();
            if (Map.BaseSkinName.Contains("SRUAP"))
            {
                Menu.AddLabel(Res_Language.GetString("Label_Minions") + Res_Language.GetString("Label_NotYet"));

                Menu.Add("Minions.Team", new ComboBox(Res_Language.GetString("Label_TeamColor"), 0, Res_Language.GetString("Label_NotYet")));
                Menu.Add("Minions.Skin", new ComboBox(Res_Language.GetString("Label_Skin"), 0, Res_Language.GetString("Label_NotYet")));
            }
            else
            {
                if (Map.Name.Contains("HA"))
                {
                    Menu.AddLabel(Res_Language.GetString("Label_Minions") + Res_Language.GetString("Label_Map_HA"));
                }
                else
                {
                    Menu.AddLabel(Res_Language.GetString("Label_Minions") + Res_Language.GetString("Label_Map_TT"));
                }
                Menu.Add("Minions.Skin", new ComboBox(Res_Language.GetString("Label_Skin"), 0, Res_Language.GetString("Label_NotYet")));
            }

            Menu.AddSeparator();
            Menu.AddLabel(Res_Language.GetString("Label_Ward"));            
            Menu.AddVisualFrame(new WardPreview("Ward.Preview", System.Drawing.Color.Purple));
            Menu.Add("Ward.Skin", new Slider(Res_Language.GetString("Label_Skin"), 0, 0, 65));

            MenuVer = Menu.AddSubMenu("Local " + CheckVersion.LocalVersion, "Sub0");
            MenuVer.AddGroupLabel(Res_Language.GetString("Label_By"));
            MenuVer.AddLabel(Res_Language.GetString("Label_NoticeThx"));
                       
            CheckVersion.CheckUpdate();

            Player.SetSkinId(Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue);
            Menu["Skin.Nomal"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
            {
                if (ChromaNumlist.Count < 1)
                {
                    Player.SetSkinId(vargs.NewValue);
                }
                else
                {
                    if (Menu["Skin.Nomal"].Cast<ComboBox>().CurrentValue < ChromaNumlist.FirstOrDefault())
                    {
                        Player.SetSkinId(vargs.NewValue);
                    }
                    else
                    {
                        Player.SetSkinId(vargs.NewValue + ChromaNumlist.Count);
                    }
                }
            };
            
            if (Server_String.Contains("true"))
            {
                Menu["Skin.Chroma"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
                {
                    if (ChromaNumlist.Count < 1) 
                    {
                          Player.SetSkinId(vargs.NewValue + SkinNumList.LastOrDefault() + 1);
                    }
                    else
                    {   
                          Player.SetSkinId(vargs.NewValue + ChromaNumlist.FirstOrDefault());
                    }
                };
            }

            Menu["Ward.Skin"].Cast<Slider>().OnValueChange += (sender, vargs) =>
            {
                foreach (var DyWard in EntityManager.MinionsAndMonsters.OtherAllyMinions.Where(x => x.Name.Contains("Ward") && !x.BaseSkinName.Contains("WardCorpse") && x.Buffs.FirstOrDefault(b => b.IsValid && b.Caster.IsMe) != null))
                {
                    if (DyWard.SkinId != Menu["Ward.Skin"].Cast<Slider>().CurrentValue)
                    {
                        DyWard.SetSkinId(Menu["Ward.Skin"].Cast<Slider>().CurrentValue);
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
            WebRequest Request_Ver = WebRequest.Create("http://ddragon.leagueoflegends.com/realms/na.json");
            Request_Ver.Credentials = CredentialCache.DefaultCredentials;
            WebResponse Response_Ver = Request_Ver.GetResponse();
            Console.WriteLine("Response Version : " + ((HttpWebResponse)Response_Ver).StatusDescription);
            Stream Stream_Ver = Response_Ver.GetResponseStream();
            StreamReader Reader_Ver = new StreamReader(Stream_Ver);
            Server_StrVer = Reader_Ver.ReadToEnd();
            Reader_Ver.Close();
            Response_Ver.Close();

            Server_StrVer = Regex.Split(Regex.Split(Server_StrVer, "v\":")[1], ",\"l\":\"en_US")[0].Replace("\"", "");
            Console.WriteLine("LoL Version : " + Server_StrVer);

            WebRequest Request_List = WebRequest.Create("http://ddragon.leagueoflegends.com/cdn/" + Server_StrVer + "/data/" + File.ReadLines(Language_Path).First() + "/champion/" + Player.Instance.ChampionName + ".json");
            Request_List.Credentials = CredentialCache.DefaultCredentials;
            WebResponse Response_List = Request_List.GetResponse();
            Console.WriteLine("Response SkinList : " + ((HttpWebResponse)Response_List).StatusDescription);
            Stream Stream_List = Response_List.GetResponseStream();
            StreamReader Reader_List = new StreamReader(Stream_List);
            Server_String = Reader_List.ReadToEnd();
            Reader_List.Close();
            Response_List.Close();

            Server_C_Name = Regex.Split(Regex.Split(Regex.Split(Server_String, "data\":{\"")[1], "\"name\":\"")[1], "\",\"title\"")[0];          
            Server_String = Regex.Split(Regex.Split(Server_String, "skins")[1], "}],\"lore")[0];
            Cpoy_Server_String = Server_String;
       
            Cpoy_Server_String = Regex.Replace(Cpoy_Server_String, @"\d", "").Replace("\"id\"", "").Replace("\"num\"", "").Replace(":", "").Replace(",", "").Replace("<br>", " ");

            string[] WordList = { "[", "{", "}", "\"", "name", "chromas", "true", "false" };
            string[] SkinList = Cpoy_Server_String.Split(WordList, StringSplitOptions.RemoveEmptyEntries);

            Menu.AddLabel(Res_Language.GetString("Label_Cham_Name") + Server_C_Name);
            Menu.Add("Skin.Nomal", new ComboBox(Res_Language.GetString("Label_Skin"), 0, SkinList));

            MatchCollection matches = Regex.Matches(Server_String, @"\d{1,10}");

            foreach (Match match in matches)
            {
                if (match.Length < 3)
                {
                    SkinNumList.Add(Convert.ToInt32(match.Value));
                }
            }

            if (Server_String.Contains("true"))
            {
                Console.WriteLine("Chromas : OK");

                int j = 0;
               
                for (j = 0; j < SkinNumList.LastOrDefault() + 1; j++)
                {
                    var aa = SkinNumList.FirstOrDefault(x => x.Equals(j));

                    if (j > 0 && aa == 0)
                    {
                        ChromaNumlist.Add(j); 
                    }
                }
                            
                if (ChromaNumlist.Count < 1)
                {
                    if (Player.Instance.ChampionName == "Cassiopeia" || Player.Instance.ChampionName == "Fizz" || Player.Instance.ChampionName == "Karthus" || Player.Instance.ChampionName == "Nasus" || Player.Instance.ChampionName == "Zac")
                    {
                        Menu.Add("Skin.Chroma", new ComboBox(Res_Language.GetString("Label_Chor_Name"), 0, "1", "2", "3"));
                    }

                    if (Player.Instance.ChampionName == "MissFortune" || Player.Instance.ChampionName == "Teemo" || Player.Instance.ChampionName == "Yasuo")
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
 
                    if (Player.Instance.ChampionName == "Ezreal" || Player.Instance.ChampionName == "Malphite" || Player.Instance.ChampionName == "Riven")
                    {
                        Menu.Add("Skin.Chroma", new ComboBox(Res_Language.GetString("Label_Chor_Name"), 0, "1", "2", "3", "4", "5", "6", "7", "8"));
                    }
                }
                else
                {
                    string[] LIST_C = new string[ChromaNumlist.Count];

                    int k = 0;
                    int OP = 0;
                    for (k = 0; k < ChromaNumlist.Count; k++)
                    {
                        LIST_C[OP] = Convert.ToString (OP + 1);
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