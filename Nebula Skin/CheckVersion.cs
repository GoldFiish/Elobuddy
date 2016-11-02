using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using EloBuddy;

namespace NebulaSkin
{
    class CheckVersion : Skin
    {
        public static readonly string LocalVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string GitHubVersion;

        public static void CheckUpdate()
        {
            WebRequest Request_GitHubVer = WebRequest.Create("https://github.com/GoldFiish/Elobuddy/blob/master/CheckVersion/SkinVersion.txt");
            using (var Version_Response = (HttpWebResponse)Request_GitHubVer.GetResponse())
            {
                Stream Version_Stream = Version_Response.GetResponseStream();
                StreamReader Version_Reader = new StreamReader(Version_Stream);
                GitHubVersion = Version_Reader.ReadToEnd();
                Version_Response.Close();
                Version_Reader.Close();

                GitHubVersion = Regex.Split(Regex.Split(GitHubVersion, "type-text\">")[1], "</table>")[0];
                GitHubVersion = Regex.Replace(GitHubVersion, @"[<][a-z|A-Z|/](.|)*?[>]", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                GitHubVersion = (new Regex(" +")).Replace(GitHubVersion, " ").Trim().Replace(", ", ",");
                
                string[] WordList = { "," };
                string[] NoticeList = GitHubVersion.Split(WordList, StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine("Local Version : " + LocalVersion + "  /  GitHub Version : " + NoticeList[1]);

                if (LocalVersion != NoticeList[1])
                {
                    Chat.Print("<font color = '#ffffff'>[ Notice ] </font><font color = '#94cdfc'>Nebula Skin has been Update </font><font color = '#ffffff'>" + NoticeList[1] + "</font>");
                    
                    MenuNVer = Menu.AddSubMenu(NoticeList[0], "Sub1");
                    MenuNVer.AddGroupLabel(NoticeList[1]);
                    for (int n = 2; n < NoticeList.Count(x => x.Contains("[")) + 2; n++)
                    {
                        MenuNVer.AddLabel(NoticeList[n]);
                    }
                }
            }
        }
    }
}
