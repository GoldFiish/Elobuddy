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
using System.Collections.Generic;

namespace NebulaTeemo
{
    internal class Teemo
    {
        public static Menu Menu, MenuCombo, MenuHarass, MenuLane, MenuJungle, MenuItem, MenuMisc, MenuDraw;

        static ResourceManager Res_Language;
        static String[] Language_List = new String[] { "Lang_En", "Lang_Kor" };
        static string Language_Path = SandboxConfig.DataDirectory + "\\MenuSaveData\\Nebula Teemo_Culture_Set.txt";

        public static String[] myArray = new String[5];

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
          
            MenuCombo = Menu.AddSubMenu("- Combo", "Sub0");
            MenuCombo.AddLabel(Res_Language.GetString("Attack_Range_Exp"));
            MenuCombo.Add("Combo.Ignite",   new CheckBox(Res_Language.GetString("Combo_Ignite")));
            MenuCombo.AddSeparator();
            MenuCombo.Add("Combo.Q.Use",    new CheckBox(Res_Language.GetString("Combo_Q_Text")));
            MenuCombo.Add("Combo.Q.Mana",   new Slider(Res_Language.GetString("Combo_Q_Mana"), 25, 0, 100));
            MenuCombo.AddSeparator();
            MenuCombo.Add("Combo.W.Use",    new CheckBox(Res_Language.GetString("Combo_W_Text")));
            MenuCombo.Add("Combo.W.Range",  new Slider(Res_Language.GetString("Combo_W_Range"), 500, 0, 800));
            MenuCombo.Add("Combo.W.Mana",   new Slider(Res_Language.GetString("Combo_W_Mana"), 25, 0, 100));
            MenuCombo.AddSeparator();
            MenuCombo.Add("Combo.R.Use",    new CheckBox(Res_Language.GetString("Combo_R_Text")));
            MenuCombo.Add("Combo.R.Count",  new Slider(Res_Language.GetString("Combo_R_Count"), 1, 1, 3));

            MenuHarass = Menu.AddSubMenu("- Harass", "Sub1");
            MenuHarass.AddLabel(Res_Language.GetString("Attack_Range_Exp"));
            MenuHarass.Add("Harass.Support",    new CheckBox(Res_Language.GetString("Harass_SuppotMode"), false));
            MenuHarass.AddSeparator();
            MenuHarass.Add("Harass.Q.Use",      new CheckBox(Res_Language.GetString("Harass_Q_Text")));
            MenuHarass.Add("Harass.Q.Mana",     new Slider(Res_Language.GetString("Harass_Q_Mana"), 65, 0, 100));
            MenuHarass.AddSeparator();
            MenuHarass.Add("Harass.W.Use",      new CheckBox(Res_Language.GetString("Harass_W_Text")));
            MenuHarass.Add("Harass.W.Range",    new Slider(Res_Language.GetString("Harass_W_Range"), 450, 0, 800));
            MenuHarass.Add("Harass.W.Mana",     new Slider(Res_Language.GetString("Harass_W_Mana"), 70, 0, 100));
            MenuHarass.AddSeparator();
            MenuHarass.Add("Harass.R.Use",      new CheckBox(Res_Language.GetString("Harass_R_Text")));
            MenuHarass.Add("Harass.R.Count",    new Slider(Res_Language.GetString("Harass_R_Count"), 2, 2, 3));
            MenuHarass.Add("Harass.R.Mana",     new Slider(Res_Language.GetString("Harass_R_Mana"), 45, 0, 100));

            MenuLane = Menu.AddSubMenu("- Lane", "Sub2");
            MenuLane.Add("Lane.Minions.Big",    new CheckBox(Res_Language.GetString("Lane_Q_Big")));
            MenuLane.AddSeparator();
            MenuLane.Add("Lane.R.Use",          new CheckBox(Res_Language.GetString("Lane_R_Text")));
            MenuLane.Add("Lane.R.PCount",       new Slider(Res_Language.GetString("Lane_R_PoisonCount"), 3, 1, 5));
            MenuLane.Add("Lane.R.RCount",       new Slider(Res_Language.GetString("Lane_R_Count"), 2, 2, 3));
            MenuLane.Add("Lane.R.Mana",         new Slider(Res_Language.GetString("Lane_R_Mana"), 80, 0, 100));

            MenuJungle = Menu.AddSubMenu("- Jungle", "Sub3");
            MenuJungle.Add("Jungle.Q.Use",      new CheckBox(Res_Language.GetString("Jungle_Q_Text")));
            MenuJungle.Add("Jungle.Q.Mana",     new Slider(Res_Language.GetString("Jungle_Q_Mana"), 30, 0, 100));
            MenuJungle.AddSeparator();
            MenuJungle.Add("Jungle.R.Use",      new CheckBox(Res_Language.GetString("Jungle_R_Text")));
            MenuJungle.Add("Jungle.R.Count",    new Slider(Res_Language.GetString("Jungle_R_Count"), 2, 2, 3));
            MenuJungle.Add("Jungle.R.Mana",     new Slider(Res_Language.GetString("Jungle_R_Mana"), 50, 0, 100));

            MenuItem = Menu.AddSubMenu("- Item", "Sub4");
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
            MenuItem.Add("Item.Zy",             new CheckBox(Res_Language.GetString("Item_D_Zhonyas_Text")));
            MenuItem.Add("Item.Zy.Hp",          new Slider(Res_Language.GetString("Item_D_Zhonyas_Hp"), 35, 0, 100));
            MenuItem.Add("Item.Zy.Dmg",         new Slider(Res_Language.GetString("Item_D_Zhonyas_Dmg"), 50, 0, 100));
            MenuItem.AddSeparator();
            MenuItem.AddLabel(Res_Language.GetString("Item_D_Zhonyas_R"));
            foreach (var enemyR in EntityManager.Heroes.Enemies)
            {
                MenuItem.Add("R." + enemyR.ChampionName.ToLower(), new CheckBox(enemyR.ChampionName + " [ R ]"));
            }

            MenuMisc = Menu.AddSubMenu("- Misc", "SubMenu5");
            MenuMisc.AddLabel(Res_Language.GetString("Misc_JungleSteal"));
            MenuMisc.Add("Steal.J.0",           new CheckBox(Res_Language.GetString("Misc_JungleSteal_0")));
            MenuMisc.Add("Steal.J.1",           new CheckBox(Res_Language.GetString("Misc_JungleSteal_1")));
            MenuMisc.AddSeparator();
            MenuMisc.AddLabel(Res_Language.GetString("Misc_KillSteal"));
            MenuMisc.Add("Steal.K.0",           new CheckBox(Res_Language.GetString("Misc_KillSteal_0")));
            MenuMisc.Add("Steal.K.1",           new CheckBox(Res_Language.GetString("Misc_KillSteal_1")));
            MenuMisc.Add("Steal.K.2",           new CheckBox(Res_Language.GetString("Misc_KillSteal_2")));
            MenuMisc.Add("Steal.K.3",           new CheckBox(Res_Language.GetString("Misc_KillSteal_3")));
            MenuMisc.Add("Steal.K.4",           new CheckBox(Res_Language.GetString("Misc_KillSteal_4")));
            MenuMisc.Add("Steal.K.5",           new CheckBox(Res_Language.GetString("Misc_KillSteal_5")));
            MenuMisc.Add("Steal.K.6",           new CheckBox(Res_Language.GetString("Misc_KillSteal_6")));
            MenuMisc.Add("Steal.K.7",           new CheckBox(Res_Language.GetString("Misc_KillSteal_7")));
            MenuMisc.AddSeparator();
            MenuMisc.AddLabel(Res_Language.GetString("Misc_AutoR_Text"));
            MenuMisc.Add("Auto.R",              new CheckBox(Res_Language.GetString("Misc_AutoR")));
            MenuMisc.AddLabel(Res_Language.GetString("Misc_AutoR_Exp"));
            MenuMisc.AddSeparator();
            MenuMisc.AddLabel(Res_Language.GetString("Misc_Skin_Text"));
            MenuMisc.Add("Skin.Id", new ComboBox(Res_Language.GetString("Misc_Skin_Select"), 8, Res_Language.GetString("Misc_Skin_Id_0"), Res_Language.GetString("Misc_Skin_Id_1"),
                                                                                                Res_Language.GetString("Misc_Skin_Id_2"), Res_Language.GetString("Misc_Skin_Id_3"), 
                                                                                                Res_Language.GetString("Misc_Skin_Id_4"), Res_Language.GetString("Misc_Skin_Id_5"),
                                                                                                Res_Language.GetString("Misc_Skin_Id_6"), Res_Language.GetString("Misc_Skin_Id_7"), 
                                                                                                Res_Language.GetString("Misc_Skin_Id_8")));
            MenuDraw = Menu.AddSubMenu("- Draw", "SubMenu6");
            MenuDraw.Add("Draw.Q.Range",        new CheckBox(Res_Language.GetString("Draw_Q")));
            MenuDraw.Add("Draw.Q.Big",          new CheckBox(Res_Language.GetString("Draw_LaneQ")));
            MenuDraw.Add("Draw.R.Range",        new CheckBox(Res_Language.GetString("Draw_R")));
            MenuDraw.AddSeparator();
            MenuDraw.Add("Draw.Virtual",        new CheckBox(Res_Language.GetString("Draw_Virtual"), false));
            MenuDraw.Add("Virtual.Range1",      new Slider(Res_Language.GetString("Draw_Virtual_Min"), 250, 0, 900));
            MenuDraw.Add("Virtual.Range2",      new Slider(Res_Language.GetString("Draw_Virtual_Max"), 900, 0, 900)); 

            Player.SetSkinId(MenuMisc["Skin.Id"].Cast<ComboBox>().CurrentValue);
            MenuMisc["Skin.Id"].Cast<ComboBox>().OnValueChange += (sender, vargs) => { Player.SetSkinId(vargs.NewValue); };
            
            Menu["Language.Select"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
            {
                var index = vargs.NewValue;
                File.WriteAllText(Language_Path, Language_List[index], Encoding.Default);
            };

            Drawing.OnDraw += Gama_OnDraw;
            Game.OnTick += Game_OnTick;
            Orbwalker.OnPreAttack += OnBeforeAttack;
            Obj_AI_Base.OnProcessSpellCast += Mode_Item.OnProcessSpellCast;
            Obj_AI_Base.OnBasicAttack += Mode_Item.OnBasicAttack;
            Game.OnUpdate += Game_OnUpdate;
            Orbwalker.OnPostAttack += OnAfterAttack;
            
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

            if (MenuDraw["Draw.Q.Range"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Q.IsReady() ? Color.Orange : Color.IndianRed, SpellManager.Q.Range, Player.Instance.Position);
            }

            if (MenuDraw["Draw.Q.Big"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.Q.IsReady() ? Color.Orange : Color.IndianRed, 450, Player.Instance.Position);                
            }

            if (MenuDraw["Draw.R.Range"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SpellManager.R.IsReady() ? Color.DeepSkyBlue : Color.IndianRed, SpellManager.R.Range, Player.Instance.Position);
            }

            if (MenuDraw["Draw.Virtual"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.White, MenuDraw["Virtual.Range1"].Cast<Slider>().CurrentValue, Player.Instance.Position);
                Circle.Draw(Color.White, MenuDraw["Virtual.Range2"].Cast<Slider>().CurrentValue, Player.Instance.Position);                               
            }
        }
        
        private static void OnBeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (MenuHarass["Harass.Support"].Cast<CheckBox>().CurrentValue)
                {
                    if (args.Target.Type == GameObjectType.obj_AI_Minion)
                    {
                        var alliesinrange = EntityManager.Heroes.Allies.Count(x => !x.IsMe && x.Distance(Player.Instance) <= 999);
                        if (alliesinrange > 0)
                        {
                            args.Process = false;
                        }
                    }
                }
            }
        }

        static void Game_OnUpdate(EventArgs args)
        {
            try
            {
                Mode_Steal.KillSteal();
                Mode_Steal.JungleSteal();

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

                Mode_Allways.AutoR();
            }
            catch (Exception)
            {
            }
        }
    
        public static void OnAfterAttack(AttackableUnit target, EventArgs args)
        {
            try
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    Mode_AfterAA.AA_Combo();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {                   
                    Mode_AfterAA.AA_Harass();
                }

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                { 
                    Mode_AfterAA.AA_Jungle();
                }
            }
            catch (Exception)
            {
            }
        }
    }   //End Class Teemo
}

