using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Resources;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.Sandbox;
using SharpDX;

namespace NebulaKalista
{
    internal class Kalista
    {
        static SharpDX.Direct3D9.Font MainFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold));
        static SharpDX.Direct3D9.Font SideFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 10));

        public static int Combo_QNum;
        static int Num;
        static int SideBar_X;
        static int SideBar_Y;

        public static Menu Menu, MenuCombo, MenuHarass, MenuLane, MenuJungle, MenuMisc, MenuItem, MenuDraw;
        
        static ResourceManager Res_Language;
        static String[] Language_List = new String[] { "Lang_En", "Lang_Kor" };
        static string Language_Path = SandboxConfig.DataDirectory + "\\MenuSaveData\\Nebula Kalista_Culture_Set.txt";
        
        public static void Load()
        {
            if (Player.Instance.ChampionName != "Kalista") { return; }

            Language_Set();
            
            Chat.Print("<font color = '#ebfd00'>Welcome to </font><font color = '#ffffff'>[ Nebula ] " + Player.Instance.ChampionName +
                "</font><font color = '#ebfd00'>. Addon is ready.</font>");
            
            Menu = MainMenu.AddMenu("[ Nebula ] " + Player.Instance.ChampionName, " By.Natrium");
            Menu.AddLabel(Res_Language.GetString("Intro_Language_Str"));
            Menu.Add("Language.Select",     new ComboBox(Res_Language.GetString("Intro_Language_Select"), 0, "English", "한국어"));
            Menu.AddSeparator();
            Menu.AddLabel(Res_Language.GetString("Intro_Str_0"));
            Menu.AddLabel(Res_Language.GetString("Intro_Str_1"));
            Menu.AddLabel(Res_Language.GetString("Intro_Str_2"));

            MenuCombo = Menu.AddSubMenu("- Combo", "SubMenu0");
            MenuCombo.Add("Combo.Q",        new CheckBox(Res_Language.GetString("Combo_Q")));
            MenuCombo.Add("Combo.Q.Style",  new ComboBox(Res_Language.GetString("Como_QStyle"), 1, Res_Language.GetString("Como_QStyle_0"), Res_Language.GetString("Como_QStyle_1"), 
                                                                                                   Res_Language.GetString("Como_QStyle_2"), Res_Language.GetString("Como_QStyle_3")));
            MenuCombo.Add("Combo.Q.Mana",   new Slider(Res_Language.GetString("Combo_Q_Mana"), 15, 0, 100));
            MenuCombo.AddSeparator();
            MenuCombo.Add("Combo.W",        new CheckBox(Res_Language.GetString("Combo_W")));
            MenuCombo.Add("Combo.E",        new CheckBox(Res_Language.GetString("Combo_E")));

            MenuHarass = Menu.AddSubMenu("- Harass", "SubMenu1");
            MenuHarass.Add("Harass.Q",          new CheckBox(Res_Language.GetString("Harass_Q")));
            MenuHarass.Add("Harass.Q.MCount",   new Slider(Res_Language.GetString("Harass_Q_Minion"), 1, 1, 5));
            MenuHarass.Add("Harass.Q.Mana",     new Slider(Res_Language.GetString("Harass_Q_Mana"), 80, 0, 100));
            MenuHarass.AddSeparator();
            MenuHarass.Add("Harass.E",          new CheckBox(Res_Language.GetString("Harass_E")));            
            MenuHarass.Add("Harass.E.MCount",   new Slider(Res_Language.GetString("Harass_E_Minion"), 1, 0, 5)); //0개로 했을떄 어떻게 견제하는지 실험해볼것
            MenuHarass.Add("Harass.E.CStack",   new Slider(Res_Language.GetString("Harass_E_Stack"), 3, 1, 5));
            MenuHarass.Add("Harass.E.Mana",     new Slider(Res_Language.GetString("Harass_E_Mana"), 70, 0, 100));

            MenuLane = Menu.AddSubMenu("- Lane", "SubMenu2");
            MenuLane.Add("Lane.Q",      new CheckBox(Res_Language.GetString("Lane_Q")));
            MenuLane.Add("Lane.Q.Num",  new Slider(Res_Language.GetString("Lane_Q_Num"), 2, 1, 5));
            MenuLane.Add("Lane.Q.Mana", new Slider(Res_Language.GetString("Lane_Q_Mana"), 80, 0, 100));
            MenuLane.AddSeparator();
            MenuLane.Add("Lane.E.All",  new CheckBox(Res_Language.GetString("Lane_E_All")));
            MenuLane.Add("Lane.E.Num",  new Slider(Res_Language.GetString("Lane_E_Num"), 2, 1, 5));
            MenuLane.Add("Lane.E.Mana", new Slider(Res_Language.GetString("Lane_E_Mana"), 70, 0, 100));

            MenuJungle = Menu.AddSubMenu("- Jungle", "SubMenu3");
            MenuJungle.Add("Jungle.Q.All",  new CheckBox(Res_Language.GetString("Jungle_Q_All"), false));
            MenuJungle.Add("Jungle.Q.Big",  new CheckBox(Res_Language.GetString("Jungle_Q_Big")));
            MenuJungle.Add("Jungle.E.All",  new CheckBox(Res_Language.GetString("Jungle_E_All"), false));
            MenuJungle.Add("Jungle.E.Big",  new CheckBox(Res_Language.GetString("Jungle_E_Big")));
            MenuJungle.Add("Jungle.Mana",   new Slider(Res_Language.GetString("Jungle_Mana"), 60, 0, 100));

            MenuMisc = Menu.AddSubMenu("- Misc", "SubMenu4");
            //MenuMisc.Add("WallJump",        new CheckBox("벽 점프 - Flee 모드")); //To do
            //MenuMisc.AddSeparator(15);
            MenuMisc.AddLabel(Res_Language.GetString("Misc_E_Str"));
            MenuMisc.Add("E.KillSteal",     new CheckBox(Res_Language.GetString("Misc_E_Steal_H")));
            MenuMisc.Add("E.MonsterSteal",  new CheckBox(Res_Language.GetString("Misc_E_Steal_J")));
            MenuMisc.Add("E.Dmage",         new CheckBox(Res_Language.GetString("Misc_E_Dmg")));
            MenuMisc.Add("E.Dmage.Value",   new Slider(Res_Language.GetString("Misc_E_Dmg_Value"), 0, -100, 0));
            MenuMisc.AddSeparator(15);
            MenuMisc.Add("E.Death",         new CheckBox(Res_Language.GetString("Misc_E_Death")));
            MenuMisc.Add("E.Death.Hp",      new Slider(Res_Language.GetString("Misc_E_Death_Hp"), 10, 0, 30));
            MenuMisc.AddSeparator(15);
            MenuMisc.AddLabel(Res_Language.GetString("Misc_R_Str"));
            MenuMisc.Add("R.Save",          new CheckBox(Res_Language.GetString("Misc_R_Save")));
            MenuMisc.Add("R.Save.Hp",       new Slider(Res_Language.GetString("Misc_R_Save_Hp"), 30, 0, 30));
            MenuMisc.AddSeparator(15);
            MenuMisc.Add("R.LongGrap",      new CheckBox(Res_Language.GetString("Misc_R_Balista")));
            MenuMisc.Add("R.LongGrap.Dis",  new Slider(Res_Language.GetString("Misc_R_Balista_Dis"), 700, 450, 1100));            
            foreach (var enemyR in EntityManager.Heroes.Enemies)
            {
                MenuMisc.Add("R." + enemyR.ChampionName, new CheckBox(enemyR.ChampionName));
            }

            MenuItem = Menu.AddSubMenu("- Item", "SubMenu5");
            MenuItem.AddLabel(Res_Language.GetString("Item_Srt"));
            MenuItem.Add("Hextech",         new CheckBox(Res_Language.GetString("Item_A_Hextech")));
            MenuItem.Add("Bilge",           new CheckBox(Res_Language.GetString("Item_A_Blige")));
            MenuItem.Add("BladeKing",       new CheckBox(Res_Language.GetString("Item_A_Blade")));
            MenuItem.Add("Youmuu",          new CheckBox(Res_Language.GetString("Item_A_Youmu")));
            MenuItem.Add("BladeKing.Use",   new Slider(Res_Language.GetString("Item_A_Blade_Hp"), 95, 0, 100));
            MenuItem.AddSeparator();
            MenuItem.Add("Quicksilver",     new CheckBox(Res_Language.GetString("Item_D_QuickSilver")));
            MenuItem.Add("Scimitar",        new CheckBox(Res_Language.GetString("Item_D_Mercurial")));
            MenuItem.Add("Cast.Delay",      new Slider(Res_Language.GetString("Item_D_CastDelay"), 350, 0, 2000));       // /ms
            MenuItem.AddSeparator();
            MenuItem.AddLabel(Res_Language.GetString("Item_D_Str"));
            MenuItem.Add("Poisons",         new CheckBox(Res_Language.GetString("Item_Buff_Poisons")));
            MenuItem.Add("Supression",      new CheckBox(Res_Language.GetString("Item_Buff_Supression")));
            MenuItem.Add("Blind",           new CheckBox(Res_Language.GetString("Item_Buff_Blind")));
            MenuItem.Add("Charm",           new CheckBox(Res_Language.GetString("Item_Buff_Charm")));
            MenuItem.Add("Fear",            new CheckBox(Res_Language.GetString("Item_Buff_Fear")));
            MenuItem.Add("Polymorph",       new CheckBox(Res_Language.GetString("Item_Buff_Ploymorph")));
            MenuItem.Add("Silence",         new CheckBox(Res_Language.GetString("Item_Buff_Silence")));
            MenuItem.Add("Slow",            new CheckBox(Res_Language.GetString("Item_Buff_Slow")));
            MenuItem.Add("Stun",            new CheckBox(Res_Language.GetString("Item_Buff_Stun")));
            MenuItem.Add("Knockup",         new CheckBox(Res_Language.GetString("Item_Buff_Knockup")));
            MenuItem.Add("Taunt",           new CheckBox(Res_Language.GetString("Item_Buff_Taunt")));

            MenuDraw = Menu.AddSubMenu("- Draw", "SubMenu6");
            MenuDraw.Add("Draw.Q",          new CheckBox(Res_Language.GetString("Draw_Range_Q")));
            MenuDraw.Add("Draw.E",          new CheckBox(Res_Language.GetString("Draw_Range_E")));
            MenuDraw.Add("Draw.R",          new CheckBox(Res_Language.GetString("Draw_Range_R"), false));
            MenuDraw.AddSeparator();
            MenuDraw.Add("Draw.E.Damage.C", new CheckBox(Res_Language.GetString("Draw_E_Dmg_Champ")));
            MenuDraw.Add("Draw.E.Damage.M", new CheckBox(Res_Language.GetString("Draw_E_Dmg_Monster")));
            MenuDraw.AddSeparator();
            MenuDraw.Add("Draw.E.Percent", new ComboBox(Res_Language.GetString("Draw_E_Percent_Str"), 1, Res_Language.GetString("Draw_E_Percent0"),Res_Language.GetString("Draw_E_Percent1"),
                                                                                                         Res_Language.GetString("Draw_E_Percent2"),Res_Language.GetString("Draw_E_Percent3"),
                                                                                                         Res_Language.GetString("Draw_E_Percent4")));
            MenuDraw.Add("Draw.Percent.X",  new Slider(Res_Language.GetString("Draw_E_Sidebar_X"), 160, 160, Drawing.Width - 10));
            MenuDraw.Add("Draw.Percent.Y",  new Slider(Res_Language.GetString("Draw_E_Sidebar_Y"), 60, 0, 900));
                    
            Menu["Language.Select"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
            {
                var index = vargs.NewValue;             
                File.WriteAllText(Language_Path, Language_List[index], Encoding.Default);
            };

            Combo_QNum = MenuCombo["Combo.Q.Style"].Cast<ComboBox>().CurrentValue;
            MenuCombo["Combo.Q.Style"].Cast<ComboBox>().OnValueChange += (sender, vargs) => { Num = vargs.NewValue; };

            Num = MenuDraw["Draw.E.Percent"].Cast<ComboBox>().CurrentValue;
            MenuDraw["Draw.E.Percent"].Cast<ComboBox>().OnValueChange += (sender, vargs) => { Num = vargs.NewValue; };

            SideBar_X = MenuDraw["Draw.Percent.X"].Cast<Slider>().CurrentValue;
            MenuDraw["Draw.Percent.X"].Cast<Slider>().OnValueChange += (sender, vargs) => { SideBar_X = vargs.NewValue; };

            SideBar_Y = MenuDraw["Draw.Percent.Y"].Cast<Slider>().CurrentValue;
            MenuDraw["Draw.Percent.Y"].Cast<Slider>().OnValueChange += (sender, vargs) => { SideBar_Y = vargs.NewValue; };

            DamageIndicator.DamageToUnit = Extensions.Get_E_Damage_Float;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Draw_Range;
        }
        
        private static void Language_Set()
        {
            try
            {
                FileInfo File_Check = new FileInfo(Language_Path);

                if(!File_Check.Exists)
                {
                    File.AppendAllText(Language_Path, "Lang_En", Encoding.Default);
                    Res_Language = new ResourceManager("NebulaKalista.Resources.Lang_En", typeof(Program).Assembly);
                    Console.WriteLine("Language Setting : Lang_En");
                }
                else
                {
                    Res_Language = new ResourceManager("NebulaKalista.Resources." + File.ReadLines(Language_Path).First(), typeof(Program).Assembly);
                    Console.WriteLine("Select Language : " + File.ReadLines(Language_Path).First());
                }                
            }           
            catch
            {
                Res_Language = new ResourceManager("NebulaKalista.Resources.Lang_En", typeof(Program).Assembly);
                Console.WriteLine("Default Language : Lang_En");
            }
        }    

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead) { return; }

            Mode_Always.Always();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Mode_Item.Items_Use();
                Mode_Combo.Combo();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Mode_Harass.Harass();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Mode_Lane.LaneClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Mode_Jungle.JungleClear();
            }
        }

        private static void Draw_Range(EventArgs args)
        {
            if (Player.Instance.IsDead) { return; }
          
            if (MenuDraw["Draw.Q"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(SpellManager.Q.IsReady() ? Color.YellowGreen : Color.IndianRed, SpellManager.Q.Range, Player.Instance.Position);

            if (MenuDraw["Draw.E"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(SpellManager.E.IsReady() ? Color.Orange : Color.IndianRed, SpellManager.E.Range, Player.Instance.Position);

            if (MenuDraw["Draw.R"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(SpellManager.R.IsReady() ? Color.YellowGreen : Color.IndianRed, SpellManager.R.Range, Player.Instance.Position);

            if (Num != 0 )
            {
                int i = 0;

                //Draw champion % damage
                foreach (var hero in EntityManager.Heroes.Enemies.Where(hero => hero.IsEnemy && hero.IsValidTarget(1200)))
                {
                    if (hero != null)
                    {
                        var E_Damage = ((int)((Extensions.Get_E_Damage_Float(hero) / hero.TotalShieldHealth()) * 100));

                        if (E_Damage > 0)
                        {
                            //Next to Name
                            if (Num == 1)
                            {
                                MainFont.DrawText(null, Extensions.IsRendKillable(hero) ? "Killable" : E_Damage + "%", (int)hero.HPBarPosition.X + 160, (int)hero.HPBarPosition.Y, E_Damage > hero.TotalShieldHealth() ? Color.Yellow : Color.White);
                            }

                            //On the Hpbar
                            if (Num == 2)
                            {
                                MainFont.DrawText(null, Extensions.IsRendKillable(hero) ? "Killable" : E_Damage + "%", (int)hero.HPBarPosition.X + 40, (int)hero.HPBarPosition.Y - 22, E_Damage > hero.TotalShieldHealth() ? Color.Yellow : Color.White);
                            }

                            //On the Name
                            if (Num == 3)
                            {
                                MainFont.DrawText(null, Extensions.IsRendKillable(hero) ? "Killable" : E_Damage + "%", (int)hero.HPBarPosition.X + 40, (int)hero.HPBarPosition.Y - 45, E_Damage > hero.TotalShieldHealth() ? Color.Yellow : Color.White);
                            }

                            //Sidebar
                            if (Num == 4)
                            {
                                var ReName = hero.ChampionName;

                                if (ReName.Length > 10)
                                {
                                    ReName = ReName.Remove(10);
                                }

                                SideFont.DrawText(null, hero.ChampionName, Drawing.Width - SideBar_X, SideBar_Y + i, E_Damage > hero.TotalShieldHealth() ? Color.Yellow : Color.White); // X -160, Y +60

                                if (!hero.IsDead)
                                {
                                    SideFont.DrawText(null, Extensions.IsRendKillable(hero) ? " ⇒   Killable" : " ⇒   " + E_Damage + " %", Drawing.Width - (SideBar_X - 80), SideBar_Y + i, E_Damage > hero.TotalShieldHealth() ? Color.Yellow : Color.White);
                                }
                                else
                                {
                                    SideFont.DrawText(null, " ⇒   " + "Death", Drawing.Width - (SideBar_X - 80), SideBar_Y + i, Color.White);
                                }
                                i += 22;
                            }
                        }
                    }
                }

                //Draw Jungle % damage
                foreach (var jungle in EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(1200) && !x.Name.Contains("Mini")))
                {
                    if (jungle != null)
                    {
                        var E_Damage = ((int)((Extensions.Get_E_Damage_Float(jungle) / jungle.Health) * 100));

                        if (E_Damage > 0)
                        {
                            if (jungle.BaseSkinName.Contains("Gromp"))
                            {
                                MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %", (int)jungle.HPBarPosition.X + 125, (int)jungle.HPBarPosition.Y - 5, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                            }

                            if (jungle.BaseSkinName.Contains("Blue") || jungle.BaseSkinName.Contains("Red") || jungle.BaseSkinName.Contains("Herald"))
                            {
                                MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %", (int)jungle.HPBarPosition.X + 155, (int)jungle.HPBarPosition.Y - 5, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                            }

                            if (jungle.BaseSkinName.Contains("Dragon"))
                            {
                                MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %", (int)jungle.HPBarPosition.X + 155, (int)jungle.HPBarPosition.Y, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                            }

                            if (jungle.BaseSkinName.Contains("Razorbeak") || jungle.BaseSkinName.Contains("wolf") || jungle.BaseSkinName.Contains("Krug"))
                            {
                                MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %", (int)jungle.HPBarPosition.X + 120, (int)jungle.HPBarPosition.Y - 5, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                            }

                            if (jungle.BaseSkinName.Contains("Crab"))
                            {
                                MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %", (int)jungle.HPBarPosition.X + 40, (int)jungle.HPBarPosition.Y - 5, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                            }

                            if (jungle.BaseSkinName.Contains("Baron"))
                            {
                                MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %", (int)jungle.HPBarPosition.X + 190, (int)jungle.HPBarPosition.Y - 5, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                            }
                        }
                    }
                }                
            }
        }   //End Draw_Range
    }
}

