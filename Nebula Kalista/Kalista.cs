using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace NebulaKalista
{  
    class Kalista
    {
        static SharpDX.Direct3D9.Font MainFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold));
        static SharpDX.Direct3D9.Font SideFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 10));

        public static Menu Menu, MenuMain, MenuFarm, MenuMisc, MenuItem, MenuDraw;

         public static void Load()
        {
            if (Player.Instance.ChampionName != "Kalista") { return; }

            Chat.Print("<font color = '#ebfd00'>Welcome to </font><font color = '#ffffff'>[ Nebula ] " + Player.Instance.ChampionName +
                "</font><font color = '#ebfd00'>. Addon is ready.</font>");
            
            Menu = MainMenu.AddMenu("[ Neblua ] " + Player.Instance.ChampionName, " By.Natrium");
            Menu.AddSeparator();         
            Menu.AddLabel(Language.Intro_Str_0);
            Menu.AddLabel(Language.Intro_Str_1);
            Menu.AddSeparator();
            //Menu.Add("Language", new Slider("[ 0 = Korean ] [ 1 = English ]", 0, 0, 1)); //Not work

            MenuMain = Menu.AddSubMenu("- Main", "SubMenu0");
            MenuMain.AddLabel(Language.Main_Combo_Str);
            MenuMain.Add("Combo.Q",         new CheckBox(Language.Main_Combo_Q));
            MenuMain.Add("Combo.E",         new CheckBox(Language.Main_Combo_E));
            MenuMain.AddSeparator();
            MenuMain.AddLabel(Language.Main_Harass_Str);
            MenuMain.Add("Harass.Q",        new CheckBox(Language.Main_Harass_Q));
            MenuMain.Add("Harass.E",        new CheckBox(Language.Main_Harass_E));
            MenuMain.Add("Harass.Mana",     new Slider(Language.Main_Harass_Mana, 70, 0, 100));
            //MenuMain.AddLabel("견제 조건 1 : 미니언은 한마리라도 킬 가능 + 적 챔피언 E스택이 한개 이상 + 거리가 900 이상일때");
            //MenuMain.AddLabel("견제 조건 2 : 적 챔피언 E스택이 세개 이상 + 거리가 900 이상일때");

            MenuFarm = Menu.AddSubMenu("- Farm", "SubMenu1");
            MenuFarm.AddLabel(Language.Farm_Lane_Str);
            MenuFarm.Add("Lane.Q",          new CheckBox(Language.Farm_Lane_Q));
            MenuFarm.Add("Lane.Q.Num",      new Slider(Language.Farm_Lane_Q_Num, 2, 1, 5));
            MenuFarm.Add("Lane.Q.Mana",     new Slider(Language.Farm_Lane_Q_Mana, 80, 0, 100));
            MenuFarm.AddSeparator();
            MenuFarm.Add("Lane.E",          new CheckBox(Language.Farm_Lane_E));
            MenuFarm.Add("Lane.E.Big",      new CheckBox(Language.Farm_Lane_E_Big));
            MenuFarm.Add("Lane.E.Num",      new Slider(Language.Farm_Lane_E_Num, 2, 1, 5));
            MenuFarm.Add("Lane.E.Mana",     new Slider(Language.Farm_Lane_E_Mana, 70, 0, 100));
            MenuFarm.AddSeparator();
            MenuFarm.AddLabel(Language.Farm_Jungle_Str);
            MenuFarm.Add("Jungle.Q.Big",    new CheckBox(Language.Farm_Jungle_Q ));
            MenuFarm.Add("Jungle.Q",        new CheckBox(Language.Farm_Jungle_Q_Big));
            MenuFarm.Add("Jungle.E.Big",    new CheckBox(Language.Farm_Jungle_E));
            MenuFarm.Add("Jungle.E",        new CheckBox(Language.Farm_Jungle_E_Big));
            MenuFarm.Add("Jungle.Mana",     new Slider(Language.Farm_Jungle_Mana, 60, 0, 100));
            MenuFarm.AddSeparator();

            MenuMisc = Menu.AddSubMenu("- Misc", "SubMenu2");
            MenuMisc.AddLabel(Language.Misc_E_Str);
            MenuMisc.Add("E.KillSteal",     new CheckBox(Language.Misc_E_Steal_H));
            MenuMisc.Add("E.MonsterSteal",  new CheckBox(Language.Misc_E_Steal_J));
            MenuMisc.Add("E.Dmage",         new CheckBox(Language.Misc_E_Dmg));
            MenuMisc.Add("E.Dmage.Value",   new Slider(Language.Misc_E_Dmg_Value, -2, -100, 0));
            MenuMisc.Add("E.Death",         new CheckBox(Language.Misc_E_Death));
            MenuMisc.Add("E.Death.Hp",      new Slider(Language.Misc_E_Death_Hp, 10, 0, 30));
            MenuMisc.AddSeparator();
            MenuMisc.AddLabel(Language.Misc_R_Str);
            MenuMisc.Add("R.Save",          new CheckBox(Language.Misc_R_Save));
            MenuMisc.Add("R.Save.Hp",       new Slider(Language.Misc_R_Save_Hp, 10, 0, 30));
            MenuMisc.AddSeparator();
            MenuMisc.AddLabel(Language.Misc_Skin);
            MenuMisc.Add("Skin.Changer",    new CheckBox(Language.Misc_Skin_State, false));
            MenuMisc.Add("Skin.ID",         new Slider(Language.Misc_Skin_Id, 0, 0, 2));

            MenuItem = Menu.AddSubMenu("- Item", "SubMenu3");
            MenuItem.Add("Bilge",           new CheckBox(Language.Item_A_Blige));
            MenuItem.Add("BladeKing",       new CheckBox(Language.Item_A_Blade));
            MenuItem.Add("Youmuu",          new CheckBox(Language.Item_A_Youmu));
            MenuItem.Add("BladeKing.Use",   new Slider(Language.Item_A_Blade_Hp, 95, 0, 100));
            MenuItem.AddSeparator();
            MenuItem.Add("Quicksilver",     new CheckBox(Language.Item_D_QuickSilver));
            MenuItem.Add("Scimitar",        new CheckBox(Language.Item_D_Mercurial));
            MenuItem.Add("Cast.Delay",      new Slider(Language.Item_D_CastDelay, 270, 0, 2000));       // /ms

            MenuItem.AddSeparator();
            MenuItem.AddLabel(Language.Item_D_Str);
            MenuItem.Add("Poisons",         new CheckBox(Language.Item_Buff_Poisons));
            MenuItem.Add("Supression",      new CheckBox(Language.Item_Buff_Supression));
            MenuItem.Add("Blind",           new CheckBox(Language.Item_Buff_Blind));
            MenuItem.Add("Charm",           new CheckBox(Language.Item_Buff_Charm));
            MenuItem.Add("Fear",            new CheckBox(Language.Item_Buff_Fear));
            MenuItem.Add("Polymorph",       new CheckBox(Language.Item_Buff_Ploymorph));
            MenuItem.Add("Silence",         new CheckBox(Language.Item_Buff_Silence));
            MenuItem.Add("Slow",            new CheckBox(Language.Item_Buff_Slow));
            MenuItem.Add("Stun",            new CheckBox(Language.Item_Buff_Stun));
            MenuItem.Add("Knockup",         new CheckBox(Language.Item_Buff_Knockup));
            MenuItem.Add("Taunt",           new CheckBox(Language.Item_Buff_Taunt));

            MenuDraw = Menu.AddSubMenu("- Draw", "SubMenu4");
            MenuDraw.Add("Draw.Q",          new CheckBox(Language.Draw_Range_Q));
            MenuDraw.Add("Draw.E",          new CheckBox(Language.Draw_Range_E));
            MenuDraw.Add("Draw.R",          new CheckBox(Language.Draw_Range_R, false));
            MenuDraw.Add("Draw.E.Damage",   new CheckBox(Language.Draw_E_Dmg));  //DamageIndicator by Monstertje. Thank u
            MenuDraw.AddSeparator();
            MenuDraw.AddLabel(Language.Draw_E_Percent_Str);
            MenuDraw.Add("Draw.E.Percent",  new Slider(Language.Draw_E_Percent, 1, 0, 4));
            MenuDraw.Add("Draw.Percent.X",  new Slider(Language.Draw_E_Sidebar_X, 160, 160, Drawing.Width - 10)); //40
            MenuDraw.Add("Draw.Percent.Y",  new Slider(Language.Draw_E_Sidebar_Y, 60, 0, 900)); //40

            DamageIndicator.DamageToUnit = Extensions.IsRendKillable_f;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Draw_Range;
            Game.OnTick += On_Tick;            
        }
     
        static void On_Tick(EventArgs args)
        {            
            if (MenuMisc["Skin.Changer"].Cast<CheckBox>().CurrentValue == true)
            {
                Player.Instance.SetSkinId(MenuMisc["Skin.ID"].Cast<Slider>().CurrentValue);
            }
            else
            {
                Player.Instance.SetSkinId(0);
            }                    
        }
        
        static void Game_OnUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead) { return; }
           
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Mode_Item.Items_Use();
                Mode_Main.Combo();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Mode_Main.Harass();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Mode_Lane.LaneClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Mode_Jungle.JungleClear();
            }

            if (MenuMisc["E.KillSteal"].Cast<CheckBox>().CurrentValue)
            {
                if (EntityManager.Heroes.Enemies.Any(x => Extensions.IsRendKillable(x)))
                {
                    SpellManager.E.Cast();
                }              

                foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.Health <= x.Get_Q_Damage_Float() && !x.IsDead))
                {
                    SpellManager.Q.Cast(target);
                }
            }

            if (MenuMisc["E.MonsterSteal"].Cast<CheckBox>().CurrentValue)
            {
                if (ObjectManager.Get<Obj_AI_Minion>().Any(m => (m.CharData.BaseSkinName.Contains("Baron") ||
                    m.CharData.BaseSkinName.Contains("Dragon") || m.CharData.BaseSkinName.Contains("Herald")) && Extensions.IsRendKillable(m)))
                {
                    SpellManager.E.Cast();
                }

                if (EntityManager.MinionsAndMonsters.Monsters.Any(x => x.Health <= x.Get_E_Damage_Double() && !x.Name.Contains("Mini")))
                {
                    SpellManager.E.Cast();
                }
            }

            if (MenuMisc["E.Death"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HealthPercent <= MenuMisc["E.Death.Hp"].Cast<Slider>().CurrentValue)
            {
                if(EntityManager.Heroes.Enemies.Any(x => x.IsValidTarget(SpellManager.E.Range)))
                {
                    SpellManager.E.Cast();
                }                
            }

            if (MenuMisc["R.Save"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.HealthPercent <= MenuMisc["R.Save.Hp"].Cast<Slider>().CurrentValue)
            {
                var Partner = EntityManager.Heroes.Allies.FirstOrDefault(x => x.HasBuff("kalistacoopstrikeally"));

                if (Partner.HealthPercent < MenuMisc["R.Save.Hp"].Cast<Slider>().CurrentValue && Partner.IsValidTarget(SpellManager.R.Range) && 
                    EntityManager.Heroes.Enemies.Any(x => x.Distance(ObjectManager.Player.ServerPosition) > 1300)) 
                {
                    //if (SpellManager.R.IsInRange(Partner) && Partner.HealthPercent < MenuMisc["R.Save.Hp"].Cast<Slider>().CurrentValue)
                    //if (Partner.IsValidTarget(SpellManager.R.Range) && Partner.HealthPercent < MenuMisc["R.Save.Hp"].Cast<Slider>().CurrentValue)
                    SpellManager.R.Cast();                   
                }                
            }
        }       
       
        static void Draw_Range(EventArgs args)
        {
            if (Player.Instance.IsDead) { return; }
          
            if (MenuDraw["Draw.Q"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(SpellManager.Q.IsReady() ? Color.YellowGreen : Color.IndianRed, SpellManager.Q.Range, Player.Instance.Position);

            if (MenuDraw["Draw.E"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(SpellManager.E.IsReady() ? Color.Orange : Color.IndianRed, SpellManager.E.Range, Player.Instance.Position);

            if (MenuDraw["Draw.R"].Cast<CheckBox>().CurrentValue)
                Circle.Draw(SpellManager.R.IsReady() ? Color.YellowGreen : Color.IndianRed, SpellManager.R.Range, Player.Instance.Position);

            if (MenuDraw["Draw.E.Percent"].Cast<Slider>().CurrentValue != 0 )
            {
                var Num = MenuDraw["Draw.E.Percent"].Cast<Slider>().CurrentValue;
                int i = 0;

                foreach (var hero in EntityManager.Heroes.Enemies.Where(hero => hero.IsEnemy))
                {
                    var E_Damage = ((int)((Extensions.IsRendKillable_f(hero) / hero.Health) * 100));

                    if (Num == 1 || Num == 2 || Num == 3 )
                    {
                        if (E_Damage > 0 && hero.IsValidTarget(1100))
                        {
                            if (Num == 1)
                            {
                                MainFont.DrawText(null, E_Damage > hero.Health ? "Killable" : E_Damage + "%",
                                    (int)hero.HPBarPosition.X + 160, (int)hero.HPBarPosition.Y, E_Damage > hero.Health ? Color.Yellow : Color.White);
                            }

                            if (Num == 2)
                            {
                                MainFont.DrawText(null, E_Damage > hero.Health ? "Killable" : E_Damage + "%",
                                    (int)hero.HPBarPosition.X + 40, (int)hero.HPBarPosition.Y - 22, E_Damage > hero.Health ? Color.Yellow : Color.White);
                            }

                            if (Num == 3)
                            {
                                MainFont.DrawText(null, E_Damage > hero.Health ? "Killable" : E_Damage + "%",
                                    (int)hero.HPBarPosition.X + 40, (int)hero.HPBarPosition.Y - 45, E_Damage > hero.Health ? Color.Yellow : Color.White);
                            }
                        }
                    }

                    if (Num == 4)
                    {
                        var X = MenuDraw["Draw.Percent.X"].Cast<Slider>().CurrentValue;
                        var Y = MenuDraw["Draw.Percent.Y"].Cast<Slider>().CurrentValue;
                        var ReName = hero.ChampionName;

                        if (ReName.Length > 10)
                        {
                            ReName = ReName.Remove(10);                            
                        }

                        SideFont.DrawText(null, hero.ChampionName, Drawing.Width - X, Y + i, E_Damage > hero.Health ? Color.Yellow : Color.White); // X -160, Y +60

                        if (!hero.IsDead)
                        {
                            SideFont.DrawText(null, E_Damage > hero.Health ? " ⇒   Killable" : " ⇒   " + E_Damage + " %",
                                Drawing.Width - (X - 80), Y + i, E_Damage > hero.Health ? Color.Yellow : Color.White);
                        }
                        else
                        {
                            SideFont.DrawText(null, " ⇒   " + "Death", Drawing.Width - (X - 80), Y + i, Color.White);
                        }
                       
                        i += 22;                     
                    }                  
                }

                foreach (var jungle in EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsMonster))
                {
                    var E_Damage = ((int)((Extensions.IsRendKillable_f(jungle) / jungle.Health) * 100));

                    if(E_Damage > 0 && !jungle.Name.Contains("Mini") && jungle.IsValidTarget(1100))
                    {
                        if(jungle.BaseSkinName.Contains("Gromp"))
                        {
                            MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %",
                                (int)jungle.HPBarPosition.X + 125, (int)jungle.HPBarPosition.Y - 5, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                        }

                        if (jungle.BaseSkinName.Contains("Blue") || jungle.BaseSkinName.Contains("Red") || jungle.BaseSkinName.Contains("Herald"))
                        {
                            MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %",
                                (int)jungle.HPBarPosition.X + 155, (int)jungle.HPBarPosition.Y - 5, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                        }

                        if (jungle.BaseSkinName.Contains("Dragon"))
                        {
                            MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %",
                                (int)jungle.HPBarPosition.X + 155, (int)jungle.HPBarPosition.Y, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                        }

                        if (jungle.BaseSkinName.Contains("Razorbeak") || jungle.BaseSkinName.Contains("wolf") || jungle.BaseSkinName.Contains("Krug"))
                        {
                            MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %",
                                (int)jungle.HPBarPosition.X + 120, (int)jungle.HPBarPosition.Y - 5, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                        }

                        if (jungle.BaseSkinName.Contains("Crab"))
                        {
                            MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %",
                                (int)jungle.HPBarPosition.X + 40, (int)jungle.HPBarPosition.Y - 5, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                        }

                        if (jungle.BaseSkinName.Contains("Baron"))
                        {
                            MainFont.DrawText(null, E_Damage > jungle.Health ? "Killable" : E_Damage + " %",
                                (int)jungle.HPBarPosition.X + 190, (int)jungle.HPBarPosition.Y - 5, E_Damage > jungle.Health ? Color.Yellow : Color.White);
                        }
                    }
                }
            }
        }
    }
}
