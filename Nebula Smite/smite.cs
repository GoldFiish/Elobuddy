using System;
using System.IO;
using System.Linq;
using System.Text;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.Sandbox;
using SharpDX;
using SharpDX.Direct3D9;

namespace NebulaSmite
{ 
    internal class smite
    {
        static Font DrawFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 15, System.Drawing.FontStyle.Bold));

        public static Menu Menu, MenuVer, MenuNVer;
        static System.Resources.ResourceManager Res_Language;

        static string[] Monster_List = new string[] {
            "SRU_Dragon_Water", "SRU_Dragon_Fire", "SRU_Dragon_Earth", "SRU_Dragon_Air", "SRU_Dragon_Elder",
            "SRU_RiftHerald", "SRU_Baron", "Sru_Crab",
            "SRU_Krug", "SRU_Red", "SRU_Razorbeak", "SRU_Murkwolf", "SRU_Blue",  "SRU_Gromp" };

        static string[] Language_List = new string[] { "en_US", "ko_KR"};
        static string Language_Path = SandboxConfig.DataDirectory + "\\MenuSaveData\\Nebula Smite_Culture_Set.txt";               

        public static void Load()
        {
            if (SpellManager.Smite == null ) { return; }

            Language_Set();

            Chat.Print("<font color = '#94cdfc'>Welcome to </font><font color = '#ffffff'>[ Nebula ] Smite</font><font color = '#94cdfc'>. Addon is ready.</font>");

            Menu = MainMenu.AddMenu("[ Nebula ] Smite", "By.Natrium");
            Menu.AddLabel(Res_Language.GetString("Main_Text_1"));
            Menu.AddLabel(Res_Language.GetString("Main_Text_2"));
            Menu.AddSeparator(20);          
            Menu.Add("Language.Select", new ComboBox(Res_Language.GetString("Main_Language_Select"), 0, "English", "한국어"));
            Menu.AddSeparator(20);
            Menu.AddLabel(Res_Language.GetString("Label_Smite"));
            Menu.Add("Semi.Work_J", new KeyBind(Res_Language.GetString("Label_Smite_J"), false, KeyBind.BindTypes.HoldActive, 'G'));
            Menu.Add("Semi.Work_C", new KeyBind(Res_Language.GetString("Label_Smite_C"), false, KeyBind.BindTypes.HoldActive, 32));
            Menu.Add("Hp.Champ", new Slider(Res_Language.GetString("Label_CheckHP"), 50, 0, 100));
            Menu.Add("Draw.CheckRange", new Slider(Res_Language.GetString("Label_CheckEnemy"), 1000, 0, 1700));
            Menu.AddSeparator(20);
            Menu.Add("Check.Champ", new CheckBox(Res_Language.GetString("Label_Smite_C")));
            Menu.Add("Check.Monster", new CheckBox(Res_Language.GetString("Label_Smite_J")));
            Menu.Add("Draw.Range", new CheckBox(Res_Language.GetString("Label_Draw_Range")));
            Menu.Add("Draw.DmgP", new CheckBox(Res_Language.GetString("Label_Draw_DmgP")));
            Menu.AddSeparator(20);
            Menu.AddLabel(Res_Language.GetString("Label_Auto"));
            Menu.AddLabel(Res_Language.GetString("Label_Auto_1"));

            MenuVer = Menu.AddSubMenu("Local " + CheckVersion.LocalVersion, "Sub0");
            MenuVer.AddGroupLabel(Res_Language.GetString("Label_By"));

            CheckVersion.CheckUpdate();

            Menu["Language.Select"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
            {
                var index = vargs.NewValue;
                File.WriteAllText(Language_Path, Language_List[index], Encoding.Default);
            };

            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
        }
       
        public static void Game_OnDraw(EventArgs args)
        {
            if (MainMenu.IsVisible)
            {
                var target_monster = EntityManager.MinionsAndMonsters.Monsters.Where(x => Player.Instance.Distance(x) < 1000 && !x.BaseSkinName.ToLower().Contains("mini")).LastOrDefault();

                if (target_monster != null)
                {
                    Circle.Draw(Color.White, Menu["Draw.CheckRange"].Cast<Slider>().CurrentValue, target_monster.Position);
                    Circle.Draw(Color.White, Menu["Draw.CheckRange"].Cast<Slider>().CurrentValue, Player.Instance.Position);
                }
            }

            if (Menu["Draw.Range"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(Player.Instance.Position, SpellManager.Smite.Range, System.Drawing.Color.White);
            }

            if (Menu["Draw.DmgP"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var champ in EntityManager.Heroes.Enemies.Where(x => x.IsValid() && Player.Instance.Distance(x) < 1500 && !x.IsDead))
                {
                    var Smite_Damage = ((int)((Player.Instance.GetSummonerSpellDamage(champ, DamageLibrary.SummonerSpells.Smite) / champ.Health) * 100));

                    DrawFont.DrawText(null, Smite_Damage >= champ.Health ? "Killable" : Smite_Damage + "%",
                        (int)champ.HPBarPosition.X + 40, (int)champ.HPBarPosition.Y - 22, Smite_Damage >= champ.Health ? Color.Yellow : Color.White);
                }

                foreach (var target in EntityManager.MinionsAndMonsters.Monsters.Where(x => Player.Instance.Distance(x) < 1000 && !x.BaseSkinName.ToLower().Contains("mini")))
                {
                    var Smite_Damage = ((int)((Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Smite) / target.Health) * 100));

                    if(target.BaseSkinName.Contains("Dragon") || target.BaseSkinName.Contains("RiftHerald") || target.BaseSkinName.Contains("Baron") || target.BaseSkinName.Contains("Blue") || target.BaseSkinName.Contains("Red"))
                    {
                        DrawFont.DrawText(null, Smite_Damage >= target.Health ? "Killable" : Smite_Damage + "%",
                        (int)target.HPBarPosition.X + 50, (int)target.HPBarPosition.Y - 45, Smite_Damage >= target.Health ? Color.Yellow : Color.White);
                    }
                    else
                    {
                        DrawFont.DrawText(null, Smite_Damage >= target.Health ? "Killable" : Smite_Damage + "%",
                        (int)target.HPBarPosition.X + 50, (int)target.HPBarPosition.Y - 22, Smite_Damage >= target.Health ? Color.Yellow : Color.White);
                    }
                }
            }
        }

        static void Game_OnUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead) { return; }

            var target_champion = TargetSelector.GetTarget(SpellManager.Smite.Range, DamageType.Physical);
            var target_monster = EntityManager.MinionsAndMonsters.Monsters.Where(x => Player.Instance.Distance(x) < 1000 && !x.BaseSkinName.ToLower().Contains("mini")).LastOrDefault();
          
            if (SpellManager.Smite.IsReady())
            {
                if (target_champion != null && 
                    SpellManager.Smite.IsInRange(target_champion) && 
                    target_champion.Health <= Player.Instance.GetSummonerSpellDamage(target_champion, DamageLibrary.SummonerSpells.Smite))
                {
                    SpellManager.Smite.Cast(target_champion);
                }

                if (target_monster != null &&
                    SpellManager.Smite.IsInRange(target_monster) &&
                    (target_monster.BaseSkinName.Contains("Dragon") || target_monster.BaseSkinName.Contains("Herald") || target_monster.BaseSkinName.Contains("Baron")) &&
                    target_monster.Health <= Player.Instance.GetSummonerSpellDamage(target_champion, DamageLibrary.SummonerSpells.Smite))
                {
                    SpellManager.Smite.Cast(target_monster);
                }

                if (target_champion != null && target_monster != null &&
                    SpellManager.Smite.IsInRange(target_monster) &&
                    target_monster.Distance(target_champion) <= Menu["Draw.CheckRange"].Cast<Slider>().CurrentValue &&
                    Player.Instance.Distance(target_champion) <= Menu["Draw.CheckRange"].Cast<Slider>().CurrentValue &&
                    target_monster.Health <= Player.Instance.GetSummonerSpellDamage(target_champion, DamageLibrary.SummonerSpells.Smite))
                {
                    SpellManager.Smite.Cast(target_monster);
                }
            }

            if (Menu["Semi.Work_C"].Cast<KeyBind>().CurrentValue)
            {
                if (SpellManager.Smite.IsReady())
                {
                    if (target_champion != null && Menu["Check.Champ"].Cast<CheckBox>().CurrentValue && SpellManager.Smite.IsInRange(target_champion))
                    {
                       if(target_champion.HealthPercent <= Menu["Hp.Champ"].Cast<Slider>().CurrentValue)
                        {
                            SpellManager.Smite.Cast(target_champion);
                        }
                    }
                }
            }

            if (Menu["Semi.Work_J"].Cast<KeyBind>().CurrentValue)
            {
               if(SpellManager.Smite.IsReady())
                {
                    if (target_monster != null && Menu["Check.Monster"].Cast<CheckBox>().CurrentValue && SpellManager.Smite.IsInRange(target_monster))
                    {
                        SpellManager.Smite.Cast(target_monster);
                    }
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
                    Res_Language = new System.Resources.ResourceManager("NebulaSmite.Resources.en_US", typeof(Program).Assembly);
                    Console.WriteLine("Language Setting : en_US");
                }
                else
                {
                    Res_Language = new System.Resources.ResourceManager("NebulaSmite.Resources." + File.ReadLines(Language_Path).First(), typeof(Program).Assembly);
                    Console.WriteLine("Select Language : " + File.ReadLines(Language_Path).First());
                }
            }
            catch
            {
                Res_Language = new System.Resources.ResourceManager("NebulaSmite.Resources.en_US", typeof(Program).Assembly);
                Console.WriteLine("Default Language : en_US");
            }
        }
    }   //End Class
}