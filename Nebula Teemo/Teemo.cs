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
        static SharpDX.Direct3D9.Font MainFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold));

        public static Menu Menu, MenuCombo, MenuHarass, MenuFlee, MenuLane, MenuJungle, MenuItem, MenuMisc, MenuDraw, MenuNVer;

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

            Menu = MainMenu.AddMenu("[ Nebula ] Teemo", "By.Natrium");
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

            MenuFlee = Menu.AddSubMenu("- Flee", "Sub3");
            MenuFlee.Add("Flee.Q.Use", new CheckBox(Res_Language.GetString("Flee_Q_Text")));
            MenuFlee.Add("Flee.Q.Range", new Slider(Res_Language.GetString("Flee_Q_Range"), 450, 0, 680));
            MenuFlee.Add("Flee.W.Use", new CheckBox(Res_Language.GetString("Flee_W_Text")));
            MenuFlee.Add("Flee.R.Use", new CheckBox(Res_Language.GetString("Flee_R_Text")));

            MenuLane = Menu.AddSubMenu("- Lane", "Sub4");
            MenuLane.Add("Lane.Minions.Big",    new CheckBox(Res_Language.GetString("Lane_Q_Big")));
            MenuLane.AddSeparator();
            MenuLane.Add("Lane.R.Use",          new CheckBox(Res_Language.GetString("Lane_R_Text")));
            MenuLane.Add("Lane.R.PCount",       new Slider(Res_Language.GetString("Lane_R_PoisonCount"), 3, 1, 5));
            MenuLane.Add("Lane.R.RCount",       new Slider(Res_Language.GetString("Lane_R_Count"), 2, 2, 3));
            MenuLane.Add("Lane.R.Mana",         new Slider(Res_Language.GetString("Lane_R_Mana"), 80, 0, 100));

            MenuJungle = Menu.AddSubMenu("- Jungle", "Sub5");
            MenuJungle.Add("Jungle.Q.Use",      new CheckBox(Res_Language.GetString("Jungle_Q_Text")));
            MenuJungle.Add("Jungle.Q.Mana",     new Slider(Res_Language.GetString("Jungle_Q_Mana"), 30, 0, 100));
            MenuJungle.AddSeparator();
            MenuJungle.Add("Jungle.R.Use",      new CheckBox(Res_Language.GetString("Jungle_R_Text"), false));
            MenuJungle.Add("Jungle.R.Count",    new Slider(Res_Language.GetString("Jungle_R_Count"), 2, 2, 3));
            MenuJungle.Add("Jungle.R.Mana",     new Slider(Res_Language.GetString("Jungle_R_Mana"), 50, 0, 100));

            MenuItem = Menu.AddSubMenu("- Item", "Sub6");
            MenuItem.AddLabel(Res_Language.GetString("Item_Exp_0"));
            MenuItem.AddLabel(Res_Language.GetString("Item_Item_Text"));
            MenuItem.Add("Item.BK.Hp",          new Slider(Res_Language.GetString("Item_A_BK_Hp"), 95, 0, 100));
            MenuItem.AddSeparator(10);
            MenuItem.Add("QSS",                 new CheckBox(Res_Language.GetString("Item_D_QSS")));
            MenuItem.Add("Scimitar",            new CheckBox(Res_Language.GetString("Item_D_Scimitar")));
            MenuItem.Add("CastDelay",           new Slider(Res_Language.GetString("Item_CastDelay"), 350, 0, 1200));
            MenuItem.AddSeparator(10);
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
            MenuItem.AddSeparator(10);
            MenuItem.AddLabel(Res_Language.GetString("Item_D_Zhonyas_Text") + " " + Res_Language.GetString("Item_Exp_1"));
            MenuItem.Add("Item.Zy",             new CheckBox(Res_Language.GetString("Item_D_Zhonyas_Text")));
            MenuItem.AddLabel(Res_Language.GetString("Item_D_Zhonyas_t1"));
            MenuItem.Add("Item.Zy.BHp",         new Slider(Res_Language.GetString("Item_D_Zhonyas_BHp"), 35, 0, 100));
            MenuItem.Add("Item.Zy.BDmg",        new Slider(Res_Language.GetString("Item_D_Zhonyas_BDmg"), 50, 0, 100));
            MenuItem.AddSeparator(10);
            MenuItem.AddLabel(Res_Language.GetString("Item_D_Zhonyas_t2"));
            MenuItem.Add("Item.Zy.SHp",         new Slider(Res_Language.GetString("Item_D_Zhonyas_SHp"), 35, 0, 100));
            MenuItem.Add("Item.Zy.SDmg",        new Slider(Res_Language.GetString("Item_D_Zhonyas_SDmg"), 50, 0, 100));
            
            MenuItem.AddSeparator(10);
            MenuItem.AddLabel(Res_Language.GetString("Item_D_Zhonyas_R"));
            foreach (var enemyR in EntityManager.Heroes.Enemies)
            {
                MenuItem.Add("R." + enemyR.ChampionName.ToLower(), new CheckBox(enemyR.ChampionName + " [ R ]"));
            }

            MenuMisc = Menu.AddSubMenu("- Misc", "SubMenu7");
            MenuMisc.AddLabel(Res_Language.GetString("Misc_JungleSteal"));
            MenuMisc.Add("Steal.J.0", new CheckBox(Res_Language.GetString("Misc_JungleSteal")));
            MenuMisc.AddSeparator();
            MenuMisc.AddLabel(Res_Language.GetString("Misc_KillSteal"));
            MenuMisc.Add("Steal.K.0", new CheckBox(Res_Language.GetString("Misc_KillSteal")));
            MenuMisc.AddSeparator();
            MenuMisc.AddLabel(Res_Language.GetString("Misc_AutoR_Text"));
            MenuMisc.Add("Auto.R",              new CheckBox(Res_Language.GetString("Misc_AutoR")));
            MenuMisc.AddLabel(Res_Language.GetString("Misc_AutoR_Exp"));
           
            MenuDraw = Menu.AddSubMenu("- Draw", "SubMenu8");
            MenuDraw.Add("Draw.Q.Range",        new CheckBox(Res_Language.GetString("Draw_Q")));
            MenuDraw.Add("Draw.Q.Big",          new CheckBox(Res_Language.GetString("Draw_LaneQ")));
            MenuDraw.Add("Draw.R.Range",        new CheckBox(Res_Language.GetString("Draw_R")));
            MenuDraw.Add("Draw.ComboCal",       new CheckBox(Res_Language.GetString("Draw_DmgPer")));
            MenuDraw.AddLabel(Res_Language.GetString("Draw_DmgPer_Text"));
            MenuDraw.AddSeparator(20);
            
            MenuDraw.Add("Draw.Virtual",        new CheckBox(Res_Language.GetString("Draw_Virtual"), false));
            MenuDraw.Add("Virtual.Range1",      new Slider(Res_Language.GetString("Draw_Virtual_Min"), 250, 0, 900));
            MenuDraw.Add("Virtual.Range2",      new Slider(Res_Language.GetString("Draw_Virtual_Max"), 900, 0, 900));
            MenuDraw.AddSeparator(20);

            MenuDraw.AddLabel(Res_Language.GetString("Draw_Enemy"));
            foreach (var enemyR in EntityManager.Heroes.Enemies)
            {
                MenuDraw.AddLabel(enemyR.ChampionName);
                MenuDraw.Add("Draw." + enemyR.ChampionName.ToLower() + ".Q", new CheckBox("[ Q ] - " + enemyR.Spellbook.GetSpell(SpellSlot.Q).Name, false));
                MenuDraw.Add("Draw." + enemyR.ChampionName.ToLower() + ".W", new CheckBox("[ W ] - " + enemyR.Spellbook.GetSpell(SpellSlot.W).Name, false));
                MenuDraw.Add("Draw." + enemyR.ChampionName.ToLower() + ".E", new CheckBox("[ E ] - " + enemyR.Spellbook.GetSpell(SpellSlot.E).Name, false));
                MenuDraw.Add("Draw." + enemyR.ChampionName.ToLower() + ".R", new CheckBox("[ R ] - " + enemyR.Spellbook.GetSpell(SpellSlot.R).Name, false));
                MenuDraw.AddSeparator(15);
            }

            CheckVersion.CheckUpdate();

            Menu["Language.Select"].Cast<ComboBox>().OnValueChange += (sender, vargs) =>
            {
                var index = vargs.NewValue;
                File.WriteAllText(Language_Path, Language_List[index], Encoding.Default);
            };

            Orbwalker.OnPostAttack += OnAfterAttack;
            Game.OnUpdate += Mode_Item.UltBuffUpdate;
            Obj_AI_Base.OnProcessSpellCast += Mode_Item.OnProcessSpellCast;
            Obj_AI_Base.OnBasicAttack += Mode_Item.OnBasicAttack;
            Game.OnUpdate += Game_OnUpdate;
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Gama_OnDraw;
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

            if (MenuDraw["Draw.ComboCal"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(2000) && x.IsHPBarRendered))
                {
                    var Damage_Per = (int)((Damage.DmgCalSteal(enemy) / enemy.TotalShieldHealth()) * 100);

                    if (Damage_Per > 0)
                    {
                        MainFont.DrawText(null, enemy.TotalShieldHealth() <= Damage_Per ? "Killable" : Damage_Per + "%", (int)enemy.HPBarPosition.X + 40, (int)enemy.HPBarPosition.Y - 45, Damage_Per > enemy.TotalShieldHealth() ? Color.Yellow : Color.White);
                    }
                }
            }


            foreach (var hero in EntityManager.Heroes.Enemies.Where(x => x.IsValid && !x.IsDead && Player.Instance.Distance(x) < 2000))
            {
                if (hero != null)
                {
                    if (MenuDraw["Draw." + hero.ChampionName.ToLower() + ".Q"].Cast<CheckBox>().CurrentValue)
                    {
                        Circle.Draw(!hero.Spellbook.GetSpell(SpellSlot.Q).IsReady ? Color.White : Color.Zero, hero.Spellbook.GetSpell(SpellSlot.Q).SData.CastRange, hero.Position);
                    }
                    if (MenuDraw["Draw." + hero.ChampionName.ToLower() + ".W"].Cast<CheckBox>().CurrentValue)
                    {
                        Circle.Draw(!hero.Spellbook.GetSpell(SpellSlot.W).IsReady ? Color.White : Color.Zero, hero.Spellbook.GetSpell(SpellSlot.W).SData.CastRange, hero.Position);
                    }
                    if (MenuDraw["Draw." + hero.ChampionName.ToLower() + ".E"].Cast<CheckBox>().CurrentValue)
                    {
                        Circle.Draw(!hero.Spellbook.GetSpell(SpellSlot.E).IsReady ? Color.White : Color.Zero, hero.Spellbook.GetSpell(SpellSlot.E).SData.CastRange, hero.Position);
                    }
                    if (MenuDraw["Draw." + hero.ChampionName.ToLower() + ".R"].Cast<CheckBox>().CurrentValue)
                    {
                        Circle.Draw(!hero.Spellbook.GetSpell(SpellSlot.R).IsReady ? Color.White : Color.Zero, hero.Spellbook.GetSpell(SpellSlot.R).SData.CastRange, hero.Position);
                    }
                }
            }

            if (MenuDraw["Draw.Virtual"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Color.White, MenuDraw["Virtual.Range1"].Cast<Slider>().CurrentValue, Player.Instance.Position);
                Circle.Draw(Color.White, MenuDraw["Virtual.Range2"].Cast<Slider>().CurrentValue, Player.Instance.Position);
            }
        }
        
        static void Game_OnUpdate(EventArgs args)
        {
            Mode_Steal.KillSteal();
            Mode_Steal.JungleSteal();
            Mode_Allways.AutoR();

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
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Mode_Flee.Flee();
            }
        }

        public static void OnAfterAttack(AttackableUnit target, EventArgs args)
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
    }   //End Class Teemo
}

