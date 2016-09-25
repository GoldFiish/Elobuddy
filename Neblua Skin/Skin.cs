using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Resources;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.Sandbox;

using System.Text.RegularExpressions;

namespace NebulaSkin
{
    internal class Skin
    {
        public static Menu Menu;

        static ResourceManager Res_Language;
        static String[] Language_List = new String[] { "en_US", "ko_KR", "ja_JP", "es_ES", "fr_FR", "de_DE", "it_IT", "pl_PL", "el_GR", "ro_RO",
                                                       "pt_BR", "tr_TR", "th_TH", "vn_VN", "id_ID", "ru_RU", "zh_CN", "zh_TW"};

        static string Language_Path = SandboxConfig.DataDirectory + "\\MenuSaveData\\Nebula Skin_Culture_Set.txt";
        
        public static void Load()
        {
            Language_Set();

            Chat.Print("<font color = '#94cdfc'>Welcome to </font><font color = '#ffffff'>[ Nebula ] Skin</font><font color = '#94cdfc'>. Addon is ready.</font>");

            Menu = MainMenu.AddMenu("[ Neblua ] Skin", "By.Natrium");
            Menu.AddLabel(Res_Language.GetString("Main_Text_0"));
            Menu.AddLabel(Res_Language.GetString("Main_Text_1"));
            Menu.AddLabel(Res_Language.GetString("Main_Text_2"));
            Menu.AddSeparator();
            Menu.AddLabel(Res_Language.GetString("Main_Language_Exp"));
            Menu.Add("Language.Select", new ComboBox(Res_Language.GetString("Main_Language_Select"), 0, 
                "English", "한국어", "Japanese", "Spanish", "French", "German", "Italian", "Polish", "Greek", "Romanian", "Portuguese (Brazil)",
                "Turkish", "Thai", "Vietnamese", "Russian", "Chinese (China)", "Chinese (Taiwan)"));
            Menu.AddSeparator();
            Menu.AddLabel("Champion : " + Player.Instance.ChampionName);
            Get_SkinInfo();

            Player.SetSkinId(Menu["Skin"].Cast<ComboBox>().CurrentValue);
            Menu["Skin"].Cast<ComboBox>().OnValueChange += (sender, vargs) => { Player.SetSkinId(vargs.NewValue); };

            Menu["Language.Select"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
            {
                var index = vargs.NewValue;
                File.WriteAllText(Language_Path, Language_List[index], Encoding.Default);
            };
        }

        private static void Get_SkinInfo()
        {
            //Get Version
            WebRequest Request_Ver = WebRequest.Create("http://ddragon.leagueoflegends.com/realms/na.json");
            Request_Ver.Credentials = CredentialCache.DefaultCredentials;
            WebResponse Response_Ver = Request_Ver.GetResponse();
            Console.WriteLine("Response Version : " + ((HttpWebResponse)Response_Ver).StatusDescription);
            Stream Stream_Ver = Response_Ver.GetResponseStream();
            StreamReader Reader_Ver = new StreamReader(Stream_Ver);
            string Server_StrVer = Reader_Ver.ReadToEnd();
            Reader_Ver.Close();
            Response_Ver.Close();

            Server_StrVer = Regex.Split(Regex.Split(Server_StrVer, "v\":")[1], ",\"l\":\"en_US")[0].Replace("\"", "");
            Console.WriteLine("Version : " + Server_StrVer);

            //Get Skin List
            WebRequest Request_List = WebRequest.Create("http://ddragon.leagueoflegends.com/cdn/" + Server_StrVer + "/data/" + File.ReadLines(Language_Path).First() + "/champion/" + Player.Instance.ChampionName + ".json");
            Request_List.Credentials = CredentialCache.DefaultCredentials;
            WebResponse Response_List = Request_List.GetResponse();
            Console.WriteLine("Response SkinList : " + ((HttpWebResponse)Response_List).StatusDescription);
            Stream Stream_List = Response_List.GetResponseStream();
            StreamReader Reader_List = new StreamReader(Stream_List);
            string Server_String = Reader_List.ReadToEnd();
            Reader_List.Close();
            Response_List.Close();
            
            Server_String = Regex.Split(Regex.Split(Server_String, "skins")[1], "}],\"lore")[0];
            Server_String = Regex.Replace(Server_String, @"\d", "");
                        
            if (Player.Instance.ChampionName == "JarvanIV")
            {
                string[] WordList = { "[", ",", "{", "}", ":", "\"", "id", "num", "name", "chromas", "true", "false", "4세" };
                string[] SkinList = Server_String.Split(WordList, StringSplitOptions.RemoveEmptyEntries);
                Menu.Add("Skin", new ComboBox(Res_Language.GetString("Main_Skin"), 0, SkinList));
            }
            else
            {
                string[] WordList = { "[", ",", "{", "}", ":", "\"", "<br>", "id", "num", "name", "chromas", "true", "false" };
                string[] SkinList = Server_String.Split(WordList, StringSplitOptions.RemoveEmptyEntries);
                Menu.Add("Skin", new ComboBox(Res_Language.GetString("Main_Skin"), 0, SkinList));
            }
        }

        private static void Language_Set()
        {
            try
            {
                FileInfo File_Check = new FileInfo(Language_Path);

                if (!File_Check.Exists)
                {
                    File.AppendAllText(Language_Path, "Lang_En", Encoding.Default);
                    Res_Language = new ResourceManager("NebulaSkin.Resources.Lang_En", typeof(Program).Assembly);
                    Console.WriteLine("Language Setting : Lang_En");
                }
                else
                {
                    Res_Language = new ResourceManager("NebulaSkin.Resources." + File.ReadLines(Language_Path).First(), typeof(Program).Assembly);
                    Console.WriteLine("Select Language : " + File.ReadLines(Language_Path).First());
                }
            }
            catch
            {
                Res_Language = new ResourceManager("NebulaSkin.Resources.Lang_En", typeof(Program).Assembly);
                Console.WriteLine("Default Language : Lang_En");
            }
        }
    }   //End Class
}

