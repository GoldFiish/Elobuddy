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

namespace NebulaTeemo
{
    internal class Teemo
    {
        public static Menu Menu, MenuCombo, MenuHarass, MenuLane, MenuJungle, MenuItem, MenuMisc;

        static ResourceManager Res_Language;
        static String[] Language_List = new String[] { "Lang_En", "Lang_Kor" };
        static string Language_Path = SandboxConfig.DataDirectory + "\\MenuSaveData\\Nebula Teemo_Culture_Set.txt";
        
        public static void Load()
        {
            if (Player.Instance.ChampionName != "Teemo") { return; }

            Language_Set();            

            Chat.Print("<font color = '#ebfd00'>Welcome to </font><font color = '#ffffff'>[ Nebula ] " + Player.Instance.ChampionName +
                "</font><font color = '#ebfd00'>. Addon is ready.</font>");
            Chat.Print("<font color = '#ebfd00'>Use Stealth Passive.</font>");

            Menu = MainMenu.AddMenu("[ Neblua ] Teemo", "By.Natrium");
            Menu.AddLabel(Res_Language.GetString("Main_Text_0"));
            Menu.AddLabel(Res_Language.GetString("Main_Text_1"));
            Menu.AddLabel(Res_Language.GetString("Main_Text_2"));
            Menu.AddSeparator();
            Menu.AddLabel(Res_Language.GetString("Main_Language_Exp"));
            Menu.Add("Language.Select",     new ComboBox(Res_Language.GetString("Main_Language_Select"), 0, "English", "Korean"));
            Menu.AddSeparator();
            Menu.Add("Attack.Stlye",        new Slider(Res_Language.GetString("Main_Attack_Style"), 1, 0, 1));
            Menu.AddLabel(Res_Language.GetString("Main_Attack_Style_Exp_0"));
            Menu.AddLabel(Res_Language.GetString("Main_Attack_Style_Exp_1"));

            MenuCombo = Menu.AddSubMenu("- Combo", "SubMenu0");
            MenuCombo.AddLabel(Res_Language.GetString("Attack_Range_Exp"));
            MenuCombo.Add("Combo.Ignite",   new CheckBox(Res_Language.GetString("Combo_Ignite")));
            MenuCombo.AddSeparator();
            MenuCombo.Add("Combo.Q.Use",    new CheckBox(Res_Language.GetString("Combo_Q_Text")));
            MenuCombo.Add("Combo.Q.Mana",   new Slider(Res_Language.GetString("Combo_Q_Mana"), 25, 0, 100));
            MenuCombo.AddSeparator();
            MenuCombo.Add("Combo.W.Use",    new CheckBox(Res_Language.GetString("Combo_W_Text")));
            MenuCombo.Add("Combo.W.Range",  new Slider(Res_Language.GetString("Combo_W_Range"), 350, 0, 720));
            MenuCombo.Add("Combo.W.Mana",   new Slider(Res_Language.GetString("Combo_W_Mana"), 25, 0, 100));
            MenuCombo.AddSeparator();
            MenuCombo.Add("Combo.R.Use",    new CheckBox(Res_Language.GetString("Combo_R_Text")));
            MenuCombo.Add("Combo.R.Count",  new Slider(Res_Language.GetString("Combo_R_Count"), 1, 1, 3));

            MenuHarass = Menu.AddSubMenu("- Harass", "SubMenu1");
            MenuHarass.AddLabel(Res_Language.GetString("Attack_Range_Exp"));
            MenuHarass.Add("Harass.Q.Use",      new CheckBox(Res_Language.GetString("Harass_Q_Text")));
            MenuHarass.Add("Harass.Q.Range",    new Slider(Res_Language.GetString("Harass_Q_Range"), 400, 0, 680));
            MenuHarass.Add("Harass.Q.Mana",     new Slider(Res_Language.GetString("Harass_Q_Mana"), 50, 0, 100));
            MenuHarass.AddSeparator();
            MenuHarass.Add("Harass.W.Use",      new CheckBox(Res_Language.GetString("Harass_W_Text")));
            MenuHarass.Add("Harass.W.Range", new Slider(Res_Language.GetString("Harass_W_Range"), 450, 0, 720));
            MenuHarass.Add("Harass.W.Mana",     new Slider(Res_Language.GetString("Harass_W_Mana"), 70, 0, 100));
            MenuHarass.AddSeparator();
            MenuHarass.Add("Harass.R.Use",      new CheckBox(Res_Language.GetString("Harass_R_Text")));
            MenuHarass.Add("Harass.R.Range",    new Slider(Res_Language.GetString("Harass_R_Range"), 450, 0, 900));
            MenuHarass.Add("Harass.R.Count",    new Slider(Res_Language.GetString("Harass_R_Count"), 2, 2, 3));
            MenuHarass.Add("Harass.R.Mana",     new Slider(Res_Language.GetString("Harass_R_Mana"), 80, 0, 100));

            MenuLane = Menu.AddSubMenu("- Lane", "SubMenu2");
            MenuLane.Add("Lane.Minions.Big",    new CheckBox(Res_Language.GetString("Lane_Q_Big")));
            MenuLane.AddSeparator();
            MenuLane.Add("Lane.R.Use",          new CheckBox(Res_Language.GetString("Lane_R_Text")));
            MenuLane.Add("Lane.R.PCount",       new Slider(Res_Language.GetString("Lane_R_PoisonCount"), 3, 1, 5));
            //MenuLane.Add("Lane.R.KCount",       new Slider(Res_Language.GetString("Lane_R_KillableCount"), 2, 1, 5));
            MenuLane.Add("Lane.R.RCount",       new Slider(Res_Language.GetString("Lane_R_Count"), 2, 2, 3));
            MenuLane.Add("Lane.R.Mana",         new Slider(Res_Language.GetString("Lane_R_Mana"), 80, 0, 100));

            MenuJungle = Menu.AddSubMenu("- Jungle", "SubMenu3");
            MenuJungle.Add("Jungle.AA",         new Slider(Res_Language.GetString("Jungle_Style"), 1, 0, 1));
            MenuJungle.AddLabel(Res_Language.GetString("Jungle_Style_Exp_1"));
            MenuJungle.AddLabel(Res_Language.GetString("Jungle_Style_Exp_2"));
            MenuJungle.AddSeparator();
            MenuJungle.Add("Jungle.Q.Use",      new CheckBox(Res_Language.GetString("Jungle_Q_Text")));
            MenuJungle.Add("Jungle.Q.Mana",     new Slider(Res_Language.GetString("Jungle_Q_Mana"), 30, 0, 100));
            MenuJungle.AddSeparator();
            MenuJungle.Add("Jungle.R.Use",      new CheckBox(Res_Language.GetString("Jungle_R_Text")));
            MenuJungle.Add("Jungle.R.Count",    new Slider(Res_Language.GetString("Jungle_R_Count"), 2, 2, 3));
            MenuJungle.Add("Jungle.R.Mana",     new Slider(Res_Language.GetString("Jungle_R_Mana"), 50, 0, 100));

            MenuItem = Menu.AddSubMenu("- Item", "SubMenu4");
            MenuItem.AddLabel(Res_Language.GetString("Item_Exp_0"));
            MenuItem.AddLabel(Res_Language.GetString("Item_Item_Text"));
            MenuItem.Add("Item.BK.Hp",          new Slider(Res_Language.GetString("Item_A_BK_Hp"), 95, 0, 100));
            MenuItem.AddSeparator();
            MenuItem.Add("QSS",                 new CheckBox(Res_Language.GetString("Item_D_QSS")));
            MenuItem.Add("Scimitar",            new CheckBox(Res_Language.GetString("Item_D_Scimitar")));
            MenuItem.Add("CastDelay",           new Slider(Res_Language.GetString("Item_CastDelay"), 350, 0, 1200));
            MenuItem.AddSeparator();
            MenuItem.AddLabel(Res_Language.GetString("Item_Debuff_Text"));
            MenuItem.Add("Blind",               new CheckBox(Res_Language.GetString("Item_Buff_Blind")));
            MenuItem.Add("Charm",               new CheckBox(Res_Language.GetString("Item_Buff_Charm")));
            MenuItem.Add("Fear",                new CheckBox(Res_Language.GetString("Item_Buff_Fear")));
            MenuItem.Add("Ploymorph",           new CheckBox(Res_Language.GetString("Item_Buff_Ploymorph")));
            MenuItem.Add("Poisons",             new CheckBox(Res_Language.GetString("Item_Buff_Poisons")));
            MenuItem.Add("Silence",             new CheckBox(Res_Language.GetString("Item_Buff_Silence")));
            MenuItem.Add("Slow",                new CheckBox(Res_Language.GetString("Item_Buff_Slow")));
            MenuItem.Add("Stun",                new CheckBox(Res_Language.GetString("Item_Buff_Stun")));
            MenuItem.Add("Supression",          new CheckBox(Res_Language.GetString("Item_Buff_Supression")));
            MenuItem.Add("Taunt",               new CheckBox(Res_Language.GetString("Item_Buff_Taunt")));
            MenuItem.Add("Snare",               new CheckBox(Res_Language.GetString("Item_Buff_Snare")));
            MenuItem.AddSeparator();
            MenuItem.AddLabel(Res_Language.GetString("Item_Exp_1"));
            MenuItem.AddLabel(Res_Language.GetString("Item_D_Zhonyas_Text"));
            MenuItem.Add("Item.Zy.Hp",          new Slider(Res_Language.GetString("Item_D_Zhonyas_Hp"), 35, 0, 100));
            //MenuItem.Add("Item.Zy.Dmg", new Slider(Res_Language.GetString("Item_D_Zhonyas_Dmg"), 50, 0, 100));


            MenuMisc = Menu.AddSubMenu("- Misc", "SubMenu5");
            MenuMisc.AddLabel(Res_Language.GetString("Misc_KillSteal_Text"));
            MenuMisc.Add("Steal.Ignite",        new CheckBox(Res_Language.GetString("Misc_Ignite")));
            MenuMisc.Add("Steal.Skill",         new CheckBox(Res_Language.GetString("Misc_KillSteal")));
            MenuMisc.Add("Steal.Monster",       new CheckBox(Res_Language.GetString("Misc_JungleSteal")));
            MenuMisc.AddSeparator();
            MenuMisc.Add("Auto.R",              new CheckBox(Res_Language.GetString("Misc_AutoR")));
            MenuMisc.AddLabel(Res_Language.GetString("Misc_AutoR_Exp"));
            MenuMisc.AddSeparator();
            MenuMisc.AddLabel(Res_Language.GetString("Misc_Skin_Text"));
            MenuMisc.Add("Skin.Id", new ComboBox(Res_Language.GetString("Misc_Skin_Select"), 8, Res_Language.GetString("Misc_Skin_Id_0"), Res_Language.GetString("Misc_Skin_Id_1"),
                Res_Language.GetString("Misc_Skin_Id_2"), Res_Language.GetString("Misc_Skin_Id_3"), Res_Language.GetString("Misc_Skin_Id_4"), Res_Language.GetString("Misc_Skin_Id_5"),
                Res_Language.GetString("Misc_Skin_Id_6"), Res_Language.GetString("Misc_Skin_Id_7"), Res_Language.GetString("Misc_Skin_Id_8")));
            MenuMisc.AddSeparator();
            MenuMisc.AddLabel(Res_Language.GetString("Misc_Draw_Text"));
            MenuMisc.Add("Draw.Q.Range",        new CheckBox(Res_Language.GetString("Misc_Draw_Q"))); 
            MenuMisc.Add("Draw.Q.Big",          new CheckBox(Res_Language.GetString("Misc_Draw_ComboQ")));
            MenuMisc.Add("Draw.R.Range",        new CheckBox(Res_Language.GetString("Misc_Draw_R")));
            MenuMisc.AddSeparator();
            MenuMisc.Add("Draw.Virtual",        new CheckBox(Res_Language.GetString("Misc_Draw_Virtual"), false));
            MenuMisc.Add("Virtual.Range1",      new Slider(Res_Language.GetString("Misc_Draw_Virtual_Min"), 250, 0, 900));
            MenuMisc.Add("Virtual.Range2",      new Slider(Res_Language.GetString("Misc_Draw_Virtual_Max"), 900, 0, 900)); 

            Player.SetSkinId(MenuMisc["Skin.Id"].Cast<ComboBox>().CurrentValue);
            MenuMisc["Skin.Id"].Cast<ComboBox>().OnValueChange += (sender, vargs) => { Player.SetSkinId(vargs.NewValue); };

            Menu["Language.Select"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
            {
                var index = vargs.NewValue;
                File.WriteAllText(Language_Path, Language_List[index], Encoding.Default);
            };           

            if (Menu["Attack.Stlye"].Cast<Slider>().CurrentValue == 1)
            {
                Game.OnUpdate -= Game_OnUpdate;
                Orbwalker.OnPostAttack += Orbwalker_JungleClear_OnPostAttack;
            }
                
            Menu["Attack.Stlye"].Cast<Slider>().OnValueChange += (sender, vargs) =>
            {
                if (vargs.NewValue == 1)
                {
                    Game.OnUpdate -= Game_OnUpdate;
                    Orbwalker.OnPostAttack += Orbwalker_JungleClear_OnPostAttack;
                }
                else
                {
                    Game.OnUpdate += Game_OnUpdate;
                    Orbwalker.OnPostAttack -= Orbwalker_JungleClear_OnPostAttack;
                }
            };

            Drawing.OnDraw += Gama_OnDraw;
            Game.OnTick += Game_OnTick;
        }

        private static void Language_Set()
        {
            try
            {
                FileInfo File_Check = new FileInfo(Language_Path);

                if (!File_Check.Exists)
                {
                    File.AppendAllText(Language_Path, "Lang_En", Encoding.Default);
                    Res_Language = new ResourceManager("NebulaTeemo.Resources.Lang_En", typeof(Program).Assembly);
                    Console.WriteLine("Language Setting : Lang_En");
                }
                else
                {
                    Res_Language = new ResourceManager("NebulaTeemo.Resources." + File.ReadLines(Language_Path).First(), typeof(Program).Assembly);
                    Console.WriteLine("Select Language : " + File.ReadLines(Language_Path).First());
                }
            }
            catch
            {
                Res_Language = new ResourceManager("NebulaTeemo.Resources.Lang_En", typeof(Program).Assembly);
                Console.WriteLine("Default Language : Lang_En");
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            SpellManager.R.Range = new uint[] { 0, 400, 650, 900 }[SpellManager.R.Level];
            
        }

        private static void Gama_OnDraw(EventArgs args)
        {
            if (Player.Instance.IsDead) return;

            if (MenuMisc["Draw.Q.Range"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Q.IsReady() ? Color.YellowGreen : Color.IndianRed, SpellManager.Q.Range, Player.Instance.Position);
            }

            if (MenuMisc["Draw.Q.Big"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Q.IsReady() ? Color.Orange : Color.IndianRed, 680, Player.Instance.Position);
                Circle.Draw(SpellManager.Q.IsReady() ? Color.Orange : Color.IndianRed, 450, Player.Instance.Position);
                
            }

            if (MenuMisc["Draw.R.Range"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.R.IsReady() ? Color.DeepSkyBlue : Color.IndianRed, SpellManager.R.Range, Player.Instance.Position);
            }

            if (MenuMisc["Draw.Virtual"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.White, MenuMisc["Virtual.Range1"].Cast<Slider>().CurrentValue, Player.Instance.Position);
                Circle.Draw(Color.White, MenuMisc["Virtual.Range2"].Cast<Slider>().CurrentValue, Player.Instance.Position);                               
            }
        }

        static void Game_OnUpdate(EventArgs args)
        {
            try
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    Mode_Combo.Combo();
                    Mode_Item.Items_Use();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    Mode_Harass.Harass();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {
                    Mode_Lane.Lane();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    Mode_Jungle.Jungle();
                }

                Mode_Allways.Steal();
                Mode_Allways.Zy_Shield();
                Mode_Allways.AutoR();
            }
            catch (Exception)
            {
            }
        }
    
        private static void Orbwalker_JungleClear_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            try
            {
                if (Menu["Attack.Stlye"].Cast<Slider>().CurrentValue == 1)
                {
                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                    {
                        Mode_Combo.Combo();
                        Mode_Item.Items_Use();
                    }
                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                    {
                        Mode_Harass.Harass();
                    }

                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                    {
                       Mode_Lane.Lane();
                    }

                    if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                    {
                        Mode_Jungle.Jungle();                       
                    }

                    Mode_Allways.Steal();
                    Mode_Allways.Zy_Shield();
                    Mode_Allways.AutoR();
                }
            }
            catch (Exception)
            {
            }            
        }
    }   //End Class Teemo
}

