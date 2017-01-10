using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.Sandbox;
using SharpDX;

using NebulaKalista.ControllN;
using NebulaKalista.Modes;

namespace NebulaKalista
{
    internal class Kalista
    {
        static SharpDX.Direct3D9.Font MainFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold));
        static SharpDX.Direct3D9.Font SideFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 10));
        
        public static Menu Menu, MenuCombo, MenuHarass, MenuLane, MenuMisc, MenuItem, MenuDraw, MenuVesion;
        
        static String[] Language_List = new String[] { "Lang_En", "Lang_Kor" };
        static string Language_Path = SandboxConfig.DataDirectory + "\\MenuSaveData\\Nebula Kalista_Culture_Set.txt";
        
        public static void Load()
        {            
            Chat.Print("<font color = '#ebfd00'>Welcome to </font><font color = '#ffffff'>[ Nebula ] " + Player.Instance.ChampionName + "</font><font color = '#ebfd00'>. Addon is ready.</font>");

            Menu = MainMenu.AddMenu("[ Nebula ] " + Player.Instance.ChampionName, "By.Natrium");           
            Menu.Add("Language.Select", new ComboBox("Language / 언어", 0, "English", "한국어"));
            Menu.AddVisualFrame(new VsFrame("Img_Kailsta", System.Drawing.Color.Purple));

            Controller language;

            switch (Menu["Language.Select"].Cast<ComboBox>().CurrentValue)
            {
                case 0:
                    language = new Eng();
                    break;
                case 1:
                    language = new Kor();
                    break;
                default:
                    language = new Eng();
                    break;
            }

            Menu.AddLabel(language.Dictionary[EnumContext.Main0]);
            Menu.AddLabel(language.Dictionary[EnumContext.Main1]);

            MenuCombo = Menu.AddSubMenu(language.Dictionary[EnumContext.Combo]);
            MenuCombo.Add("Combo_Q",            new CheckBox(language.Dictionary[EnumContext.UseQ]));
            MenuCombo.Add("Combo_Q_Style",      new ComboBox(language.Dictionary[EnumContext.Combo_Q_Mode], 1,
                                                    language.Dictionary[EnumContext.Combo_Q_Mode0], language.Dictionary[EnumContext.Combo_Q_Mode1],
                                                    language.Dictionary[EnumContext.Combo_Q_Mode2], language.Dictionary[EnumContext.Combo_Q_Mode3]));
            MenuCombo.Add("Combo_Q_Mana",       new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 15, 0, 100));
            MenuCombo.AddSeparator();
            MenuCombo.Add("Combo_W",            new CheckBox(language.Dictionary[EnumContext.UseW]));
            MenuCombo.Add("Combo_E",            new CheckBox(language.Dictionary[EnumContext.UseE]));

            MenuHarass = Menu.AddSubMenu(language.Dictionary[EnumContext.Harass]);
            MenuHarass.Add("Harass_Q",          new CheckBox(language.Dictionary[EnumContext.UseQ]));
            MenuHarass.Add("Harass_Q_MCount",   new Slider(language.Dictionary[EnumContext.MinionNum0] + "[ {0} ]" + language.Dictionary[EnumContext.MinionNum1], 1, 1, 5));
            MenuHarass.Add("Harass_Q_Mana",     new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 80, 0, 100));
            MenuHarass.AddSeparator();
            MenuHarass.Add("Harass_E",          new CheckBox(language.Dictionary[EnumContext.UseE]));            
            MenuHarass.Add("Harass_E_MCount",   new Slider(language.Dictionary[EnumContext.MinionNum0] + "[ {0} ]" + language.Dictionary[EnumContext.MinionNum1], 1, 0, 5));
            MenuHarass.Add("Harass_E_CStack",   new Slider(language.Dictionary[EnumContext.StackNum0] + "[ {0} ]" + language.Dictionary[EnumContext.StackNum1], 3, 1, 5));
            MenuHarass.Add("Harass_E_Mana",     new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 70, 0, 100));

            MenuLane = Menu.AddSubMenu(language.Dictionary[EnumContext.Clear]);
            MenuLane.AddLabel(language.Dictionary[EnumContext.Lane]);
            MenuLane.Add("Lane_Q",              new CheckBox(language.Dictionary[EnumContext.UseQ]));
            MenuLane.Add("Lane_Q_Num",          new Slider(language.Dictionary[EnumContext.MinionNum0] + "[ {0} ]" + language.Dictionary[EnumContext.MinionNum1], 2, 1, 5));
            MenuLane.Add("Lane_Q_Mana",         new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 80, 0, 100));
            MenuLane.AddSeparator();
            MenuLane.Add("Lane_E_All",          new CheckBox(language.Dictionary[EnumContext.UseE]));
            MenuLane.Add("Lane_E_Num",          new Slider(language.Dictionary[EnumContext.MinionNum0] + "[ {0} ]" + language.Dictionary[EnumContext.MinionNum1], 2, 1, 5));
            MenuLane.Add("Lane_E_Mana",         new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 70, 0, 100));
            MenuLane.AddSeparator();
            MenuLane.AddLabel(language.Dictionary[EnumContext.Jungle]);
            MenuLane.Add("Jungle_Q_Mode",       new ComboBox(language.Dictionary[EnumContext.JungleModeQ], 2,
                                                    language.Dictionary[EnumContext.JungleMode0], language.Dictionary[EnumContext.JungleMode1],
                                                    language.Dictionary[EnumContext.JungleMode2], language.Dictionary[EnumContext.JungleMode3]));
            MenuLane.Add("Jungle_Q_Mana",       new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 55, 0, 100));
            MenuLane.Add("Jungle_E_Mode",       new ComboBox(language.Dictionary[EnumContext.JungleModeE], 2,
                                                    language.Dictionary[EnumContext.JungleMode0], language.Dictionary[EnumContext.JungleMode1],
                                                    language.Dictionary[EnumContext.JungleMode2], language.Dictionary[EnumContext.JungleMode3]));
            MenuLane.Add("Jungle_E_Mana",       new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 45, 0, 100));

            MenuMisc = Menu.AddSubMenu(language.Dictionary[EnumContext.Misc]);
            //MenuMisc.Add("WallJump",        new CheckBox("벽 점프 - Flee 모드")); //To do
            //MenuMisc.AddSeparator(15);
            MenuMisc.AddLabel(language.Dictionary[EnumContext.Esetting]);
            MenuMisc.Add("E_KillSteal",         new CheckBox(language.Dictionary[EnumContext.EKillSteal]));
            MenuMisc.Add("E_MonsterSteal",      new CheckBox(language.Dictionary[EnumContext.EJungSteal]));
            MenuMisc.Add("E_Dmage",             new CheckBox(language.Dictionary[EnumContext.ECustom]));
            MenuMisc.Add("E_Dmage_Value",       new Slider(language.Dictionary[EnumContext.ECustomDmg0] + "[ {0} ]" + language.Dictionary[EnumContext.ECustomDmg1], 0, -100, 0));
            MenuMisc.AddSeparator(15);
            MenuMisc.Add("E_Death",             new CheckBox(language.Dictionary[EnumContext.EDeath]));
            MenuMisc.Add("E_Death_Hp",          new Slider(language.Dictionary[EnumContext.EDeathHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.EDeathHp2], 10, 0, 30));
            MenuMisc.AddSeparator(15);
            MenuMisc.AddLabel(language.Dictionary[EnumContext.Rsetting]);
            MenuMisc.Add("R_Save",              new CheckBox(language.Dictionary[EnumContext.SaveR]));
            MenuMisc.Add("R_Save_Hp",           new Slider(language.Dictionary[EnumContext.SaveRHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.SaveRHp2], 30, 0, 30));
            MenuMisc.AddSeparator(15);
            MenuMisc.Add("R_LongGrap",          new CheckBox(language.Dictionary[EnumContext.Bali]));
            MenuMisc.Add("R_LongGrap_Dis",      new Slider(language.Dictionary[EnumContext.BaliDis], 700, 450, 1100));
            foreach (var enemyR in EntityManager.Heroes.Enemies)
            {
                MenuMisc.Add("R_" + enemyR.ChampionName, new CheckBox(enemyR.ChampionName));
            }

            MenuItem = Menu.AddSubMenu(language.Dictionary[EnumContext.Item]);
            MenuItem.Add("Hextech",         new CheckBox(language.Dictionary[EnumContext.sHextechG]));
            MenuItem.Add("Bilge",           new CheckBox(language.Dictionary[EnumContext.sBilge]));
            MenuItem.Add("BladeKing",       new CheckBox(language.Dictionary[EnumContext.sBlade]));
            MenuItem.Add("Youmuu",          new CheckBox(language.Dictionary[EnumContext.sYoumuu]));
            MenuItem.Add("BladeKing.Use",   new Slider(language.Dictionary[EnumContext.sBKHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.sBKHp2], 95, 0, 100));
            MenuItem.AddSeparator();
            MenuItem.Add("Quicksilver",     new CheckBox(language.Dictionary[EnumContext.sQsilver]));
            MenuItem.Add("Scimitar",        new CheckBox(language.Dictionary[EnumContext.sScimitar]));
            MenuItem.Add("Cast.Delay",      new Slider(language.Dictionary[EnumContext.Delay], 350, 0, 2000));// /ms
            MenuItem.AddSeparator();
            MenuItem.AddLabel(language.Dictionary[EnumContext.sBuffType]);
            MenuItem.Add("Poisons",         new CheckBox(language.Dictionary[EnumContext.sPoisons]));
            MenuItem.Add("Supression",      new CheckBox(language.Dictionary[EnumContext.sSupression]));
            MenuItem.Add("Blind",           new CheckBox(language.Dictionary[EnumContext.sBlind]));
            MenuItem.Add("Charm",           new CheckBox(language.Dictionary[EnumContext.sCharm]));
            MenuItem.Add("Fear",            new CheckBox(language.Dictionary[EnumContext.sFear]));
            MenuItem.Add("Polymorph",       new CheckBox(language.Dictionary[EnumContext.sPolymorph]));
            MenuItem.Add("Silence",         new CheckBox(language.Dictionary[EnumContext.sSilence]));
            MenuItem.Add("Slow",            new CheckBox(language.Dictionary[EnumContext.sSlow]));
            MenuItem.Add("Snare",           new CheckBox(language.Dictionary[EnumContext.sSnare]));
            MenuItem.Add("Stun",            new CheckBox(language.Dictionary[EnumContext.sStun]));
            MenuItem.Add("Knockup",         new CheckBox(language.Dictionary[EnumContext.sKnockup]));
            MenuItem.Add("Taunt",           new CheckBox(language.Dictionary[EnumContext.sTaunt]));

            MenuDraw = Menu.AddSubMenu(language.Dictionary[EnumContext.Draw]);
            MenuDraw.Add("Draw_Q",          new CheckBox(language.Dictionary[EnumContext.DrawQ]));
            MenuDraw.Add("Draw_E",          new CheckBox(language.Dictionary[EnumContext.DrawE]));
            MenuDraw.Add("Draw_R",          new CheckBox(language.Dictionary[EnumContext.DrawR], false));
            MenuDraw.AddSeparator();
            MenuDraw.Add("Draw_E_CDamage",  new CheckBox(language.Dictionary[EnumContext.DrawECDamage]));
            MenuDraw.Add("Draw_E_MDamage",  new CheckBox(language.Dictionary[EnumContext.DrawEJDamage]));
            MenuDraw.AddSeparator();
            MenuDraw.Add("Draw_E_Position", new ComboBox(language.Dictionary[EnumContext.DrawPosition], 1,
                                                language.Dictionary[EnumContext.Position0], language.Dictionary[EnumContext.Position1],
                                                language.Dictionary[EnumContext.Position2], language.Dictionary[EnumContext.Position3]));
            MenuDraw.Add("Draw_Position_X", new Slider(language.Dictionary[EnumContext.DrawX], 160, 160, Drawing.Width - 10));
            MenuDraw.Add("Draw_Position_Y", new Slider(language.Dictionary[EnumContext.DrawY], 60, 0, 900));

            CheckVersion.CheckUpdate();

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Draw_Range;
        }

        public static bool Status_CheckBox(Menu sub, string str)
        {
            return sub[str].Cast<CheckBox>().CurrentValue;
        }

        public static int Status_Slider(Menu sub, string str)
        {
            return sub[str].Cast<Slider>().CurrentValue;
        }

        public static int Status_ComboBox(Menu sub, string str)
        {
            return sub[str].Cast<ComboBox>().CurrentValue;
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
          
            if (Status_CheckBox(MenuDraw, "Draw_Q"))
                Circle.Draw(SpellManager.Q.IsReady() ? Color.YellowGreen : Color.IndianRed, SpellManager.Q.Range, Player.Instance.Position);

            if (Status_CheckBox(MenuDraw, "Draw_E"))
                Circle.Draw(SpellManager.E.IsReady() ? Color.Orange : Color.IndianRed, SpellManager.E.Range, Player.Instance.Position);

            if (Status_CheckBox(MenuDraw, "Draw_R"))
                Circle.Draw(SpellManager.R.IsReady() ? Color.YellowGreen : Color.IndianRed, SpellManager.R.Range, Player.Instance.Position);

            if (Status_CheckBox(MenuDraw, "Draw_E_CDamage"))
            {
                int i = 0;

                foreach (var hero in EntityManager.Heroes.Enemies.Where(hero => hero.IsEnemy && hero.IsValidTarget(1600)))
                {
                    if (hero != null)
                    {
                        var E_Damage = ((int)((Extensions.Get_E_Damage_Float(hero) / hero.TotalShieldHealth()) * 100));
                        var Q_Damage = ((int)((Extensions.Get_Q_Damage_Float(hero) / hero.TotalShieldHealth()) * 100));

                        if (E_Damage > 0 || Q_Damage > 0)
                        {
                            switch (Status_ComboBox(MenuDraw, "Draw_E_Position"))
                            {
                                case 0:     //Next to Name
                                    MainFont.DrawText(null, Extensions.IsRendKillable(hero) ? "E : Killable" : "E : " + E_Damage + " %", (int)hero.HPBarPosition.X + 160, (int)hero.HPBarPosition.Y, E_Damage > hero.TotalShieldHealth() ? Color.Yellow : Color.White);
                                    MainFont.DrawText(null, Extensions.IsRendKillable(hero) ? "Q : Killable" : "Q : " + Q_Damage + " %", (int)hero.HPBarPosition.X + 160, (int)hero.HPBarPosition.Y - 27, Q_Damage > hero.TotalShieldHealth() ? Color.Yellow : Color.White);
                                    break;
                                case 1:     //On the Hpbar
                                    MainFont.DrawText(null, Extensions.IsRendKillable(hero) ? "E : Killable" : "E : " + E_Damage + " %", (int)hero.HPBarPosition.X, (int)hero.HPBarPosition.Y - 22, E_Damage > hero.TotalShieldHealth() ? Color.Yellow : Color.White);
                                    MainFont.DrawText(null, Extensions.IsRendKillable(hero) ? "Q : Killable" : "Q : " + Q_Damage + " %", (int)hero.HPBarPosition.X + 120, (int)hero.HPBarPosition.Y - 22, Q_Damage > hero.TotalShieldHealth() ? Color.Yellow : Color.White);

                                    break;
                                case 2:    //On the Name
                                    MainFont.DrawText(null, Extensions.IsRendKillable(hero) ? "E : Killable" : "E : " + E_Damage + " %", (int)hero.HPBarPosition.X, (int)hero.HPBarPosition.Y - 45, E_Damage > hero.TotalShieldHealth() ? Color.Yellow : Color.White);
                                    MainFont.DrawText(null, Extensions.IsRendKillable(hero) ? "Q : Killable" : "Q : " + Q_Damage + " %", (int)hero.HPBarPosition.X + 120, (int)hero.HPBarPosition.Y - 45, Q_Damage > hero.TotalShieldHealth() ? Color.Yellow : Color.White);
                                    break;
                                case 3:     //Sidebar
                                    var SideBar_X = Status_Slider(MenuDraw, "Draw_Position_X");
                                    var SideBar_Y = Status_Slider(MenuDraw, "Draw_Position_Y");
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
                                    break;
                            }
                        }
                    }
                }
            }

            if (Status_CheckBox(MenuDraw, "Draw_E_MDamage"))
            {
                foreach (var jungle in EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(1200) && !x.Name.Contains("Mini")))
                {
                    if (jungle != null)
                    {
                        var E_Damage = ((int)((Extensions.Get_E_Damage_Float(jungle) / jungle.Health) * 100));
                        var Pos_X = 0;
                        var Pos_Y = 0;

                        if (jungle.BaseSkinName.Contains("Gromp"))
                        {
                            Pos_X = 125;
                            Pos_Y = 5;
                        }

                        if (jungle.BaseSkinName.Contains("Blue") || jungle.BaseSkinName.Contains("Red") || jungle.BaseSkinName.Contains("Herald"))
                        {
                            Pos_X = 155;
                            Pos_Y = 5;
                        }

                        if (jungle.BaseSkinName.Contains("Dragon"))
                        {
                            Pos_X = 155;
                            Pos_Y = 0;
                        }

                        if (jungle.BaseSkinName.Contains("Razorbeak") || jungle.BaseSkinName.Contains("wolf") || jungle.BaseSkinName.Contains("Krug"))
                        {
                            Pos_X = 120;
                            Pos_Y = 5;
                        }

                        if (jungle.BaseSkinName.Contains("Crab"))
                        {
                            Pos_X = 40;
                            Pos_Y = 5;
                        }

                        if (jungle.BaseSkinName.Contains("Baron"))
                        {
                            Pos_X = 190;
                            Pos_Y = 5;
                        }

                        if (E_Damage > 0)
                        {
                            MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %", (int)jungle.HPBarPosition.X + Pos_X, (int)jungle.HPBarPosition.Y - Pos_Y, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                        }
                    }
                }
            } 
        }   //End Draw_Range
    }
}

