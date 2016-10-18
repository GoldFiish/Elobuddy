using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using EloBuddy;

namespace NebulaSmite
{
    class CheckVersion : smite
    {
        public static readonly string LocalVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string GitHubVersion;

        public static void CheckUpdate()
        {
            WebRequest Request_Ver = WebRequest.Create("https://github.com/GoldFiish/Elobuddy/blob/master/CheckVersion/SmiteVersion.txt");
            Request_Ver.Credentials = CredentialCache.DefaultCredentials;
            WebResponse Response_Ver = Request_Ver.GetResponse();
            Stream Stream_Ver = Response_Ver.GetResponseStream();
            StreamReader Reader_Ver = new StreamReader(Stream_Ver);
            GitHubVersion = Reader_Ver.ReadToEnd();
            Reader_Ver.Close();
            Response_Ver.Close();

            GitHubVersion = Regex.Split(Regex.Split(GitHubVersion, "type-text\">")[1], "</table>")[0];
            GitHubVersion = Regex.Replace(GitHubVersion, @"[<][a-z|A-Z|/](.|)*?[>]", "");

            GitHubVersion = GitHubVersion.Trim().Replace("\t", "").Replace("\r", "").Replace("\n", "");
            GitHubVersion = (new Regex(" +")).Replace(GitHubVersion, " ");

            string[] WordList = { "," };
            string[] NoticeList = GitHubVersion.Split(WordList, StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine("Local Version : " + LocalVersion + "  /  GitHub Version : " + NoticeList[0]);

            if (GitHubVersion != LocalVersion)
            {
                //Chat.Print("<font color = '#ffffff'>[ Notice ] </font><font color = '#94cdfc'>Nebula Skin has been Update </font><font color = '#ffffff'>" + NoticeList[0] + "</font>");

                MenuNVer = Menu.AddSubMenu("GitHub " + NoticeList[0], "Sub1");
                MenuNVer.AddGroupLabel("Update");
                for (int n = 1; n < NoticeList.Count(x => x.Contains("[")) + 1; n++)
                {
                    MenuNVer.AddLabel(NoticeList[n]);
                }
            }
        }
    }
}
