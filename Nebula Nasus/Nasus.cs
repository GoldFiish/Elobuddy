using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using NebulaNasus.Modes;
using NebulaNasus.ControllN;

namespace NebulaNasus
{
    class Nasus
    {       
        public static Menu Menu, M_Main, M_Clear, M_Item, M_Misc, M_Draw, M_NVer;

        static SharpDX.Direct3D9.Font MainFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Bold));
        
        public static void Load()
        {
            Chat.Print("<font color = '#20b2aa'>Welcome to </font><font color = '#ffffff'>[ Nebula ] " + Player.Instance.ChampionName + "</font><font color = '#20b2aa'>. Addon is ready.</font>");
            CheckVersion.CheckUpdate();

            Menu = MainMenu.AddMenu("[ Nebula ] Nasus", "By.Natrium");           
            Menu.Add("Language.Select", new ComboBox("Language / 언어", 0, "English", "한국어"));
            Menu.AddVisualFrame(new VsFrame("Ward.Preview", System.Drawing.Color.Purple));

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

            Menu.AddLabel(language.Dictionary[EnumContext.SelectLanguage]);
            Menu.AddSeparator(10);
            Menu.AddLabel(language.Dictionary[EnumContext.Text_1]);
            Menu.AddLabel(language.Dictionary[EnumContext.Text_2]);
            
            M_Main = Menu.AddSubMenu(language.Dictionary[EnumContext.Main]);
            M_Main.AddGroupLabel(language.Dictionary[EnumContext.Combo]);
            M_Main.Add("Combo_Q",           new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Main.Add("Combo_W",           new CheckBox(language.Dictionary[EnumContext.SpellW]));
            M_Main.Add("Combo_W_Dis",       new Slider(language.Dictionary[EnumContext.ComboWDis1] + "[ {0} ]" + language.Dictionary[EnumContext.ComboWDis2], 350, 0, 600));
            M_Main.Add("Combo_E",           new CheckBox(language.Dictionary[EnumContext.SpellE]));
            M_Main.Add("Combo_R",           new CheckBox(language.Dictionary[EnumContext.SpellR]));
            M_Main.Add("Combo_R_Hp",        new Slider(language.Dictionary[EnumContext.MyHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.MyHp2], 55));
            M_Main.AddSeparator(20);
            M_Main.AddGroupLabel(language.Dictionary[EnumContext.Harass]);
            M_Main.Add("Harass_Q",          new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Main.Add("Harass_Q_Mana",     new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 30));
            M_Main.AddSeparator(10);
            M_Main.Add("Harass_E",          new CheckBox(language.Dictionary[EnumContext.SpellE]));
            M_Main.Add("Harass_E_Mana",     new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 80));
            M_Main.AddSeparator(10);
            M_Main.Add("Harass_R",          new CheckBox(language.Dictionary[EnumContext.SpellR]));
            M_Main.Add("Harass_R_Hp",       new Slider(language.Dictionary[EnumContext.MyHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.MyHp2], 55));
            M_Main.Add("Harass_R_Mana",     new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 25));
            M_Main.AddSeparator(20);
            M_Main.AddGroupLabel(language.Dictionary[EnumContext.Flee]);
            M_Main.Add("Flee_W",            new CheckBox(language.Dictionary[EnumContext.SpellW]));
            M_Main.Add("Flee_E",            new CheckBox(language.Dictionary[EnumContext.SpellE]));
            M_Main.Add("Flee_R",            new CheckBox(language.Dictionary[EnumContext.SpellR]));
            M_Main.Add("Flee_R_Hp",         new Slider(language.Dictionary[EnumContext.MyHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.MyHp2], 35));
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//
            M_Clear = Menu.AddSubMenu(language.Dictionary[EnumContext.Clear]);
            M_Clear.AddGroupLabel(language.Dictionary[EnumContext.LaneClear]);
            M_Clear.Add("Lane_Q",           new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Clear.Add("Lane_Q_Mana",      new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 45));
            M_Clear.AddSeparator(10);
            M_Clear.Add("Lane_E",           new CheckBox(language.Dictionary[EnumContext.SpellE]));
            M_Clear.Add("Lane_E_Mana",      new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 85));
            M_Clear.AddSeparator(20);
            M_Clear.AddGroupLabel(language.Dictionary[EnumContext.JungleClear]);
            M_Clear.Add("Jungle_Q",         new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Clear.Add("Jungle_Q_Mana",    new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 25));
            M_Clear.Add("Jungle_E",         new CheckBox(language.Dictionary[EnumContext.SpellE]));
            M_Clear.Add("Jungle_E_Hit",     new Slider(language.Dictionary[EnumContext.JungleHitNum1] + "[ {0} ]" + language.Dictionary[EnumContext.JungleHitNum2], 2, 0, 5));
            M_Clear.Add("Jungle_E_Mana",    new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 75));
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//
            M_Item = Menu.AddSubMenu(language.Dictionary[EnumContext.Item]);
            M_Item.AddLabel(language.Dictionary[EnumContext.sModeCombo]);
            M_Item.Add("Item.Youmuu",       new CheckBox(language.Dictionary[EnumContext.sYoummu]));
            M_Item.Add("Item.BK",           new CheckBox(language.Dictionary[EnumContext.sBK]));
            M_Item.Add("Item.BK.Hp",        new Slider(language.Dictionary[EnumContext.sBKHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.sBKHp2], 95, 0, 100));
            M_Item.AddSeparator(10);
            M_Item.Add("QSS",               new CheckBox(language.Dictionary[EnumContext.sQSS]));
            M_Item.Add("Scimitar",          new CheckBox(language.Dictionary[EnumContext.sScimiter]));
            M_Item.Add("CastDelay",         new Slider("{0}ms" + language.Dictionary[EnumContext.sDelay], 350, 0, 1200));
            M_Item.AddSeparator(10);            
            M_Item.Add("Blind",             new CheckBox(language.Dictionary[EnumContext.sBlind]));
            M_Item.Add("Charm",             new CheckBox(language.Dictionary[EnumContext.sCharm]));
            M_Item.Add("Fear",              new CheckBox(language.Dictionary[EnumContext.sFear]));
            M_Item.Add("Ploymorph",         new CheckBox(language.Dictionary[EnumContext.sPloymorph]));
            M_Item.Add("Poisons",           new CheckBox(language.Dictionary[EnumContext.sPoisons]));
            M_Item.Add("Silence",           new CheckBox(language.Dictionary[EnumContext.sSilence]));
            M_Item.Add("Slow",              new CheckBox(language.Dictionary[EnumContext.sSlow]));
            M_Item.Add("Stun",              new CheckBox(language.Dictionary[EnumContext.sStun]));
            M_Item.Add("Supression",        new CheckBox(language.Dictionary[EnumContext.sSupression]));
            M_Item.Add("Taunt",             new CheckBox(language.Dictionary[EnumContext.sTaunt]));
            M_Item.Add("Snare",             new CheckBox(language.Dictionary[EnumContext.sSnare]));
            M_Item.AddSeparator(10);
            M_Item.AddLabel(language.Dictionary[EnumContext.sAlways]);
            M_Item.Add("Item.Redemption", new CheckBox(language.Dictionary[EnumContext.sRedemption]));
            M_Item.Add("Item.Redemption.MyHp", new Slider(language.Dictionary[EnumContext.MyHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.MyHp2], 50, 0, 100));
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//
            M_Misc = Menu.AddSubMenu(language.Dictionary[EnumContext.Msic]);
            M_Misc.Add("Misc_Ignite",   new CheckBox(language.Dictionary[EnumContext.AutoIgnite]));
            M_Misc.AddSeparator(10);
            M_Misc.Add("Misc_KillSt",   new CheckBox(language.Dictionary[EnumContext.KillSteal]));
            M_Misc.Add("Misc_KillStE",  new ComboBox(language.Dictionary[EnumContext.KillOption], 0, language.Dictionary[EnumContext.KillText1], language.Dictionary[EnumContext.KillText2]));
            M_Misc.AddSeparator(10);
            M_Misc.Add("Misc_JungleSt", new CheckBox(language.Dictionary[EnumContext.JungleSteal]));
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//
            M_Draw = Menu.AddSubMenu(language.Dictionary[EnumContext.Draw]);
            M_Draw.Add("Draw_Killable", new CheckBox(language.Dictionary[EnumContext.DrawText]));
            M_Draw.AddSeparator(10);
            M_Draw.Add("Draw_Q",        new CheckBox(language.Dictionary[EnumContext.DrawQ]));
            M_Draw.AddSeparator(10);
            M_Draw.Add("Draw_W",        new CheckBox(language.Dictionary[EnumContext.DrawW]));
            M_Draw.AddSeparator(10);
            M_Draw.Add("Draw_E",        new CheckBox(language.Dictionary[EnumContext.DrawE]));

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        public static bool Status_CheckBox(Menu sub, string str)
        {
            return sub[str].Cast<CheckBox>().CurrentValue;
        }

        public static int Status_Slider(Menu sub, string str)
        {
            return sub[str].Cast<Slider>().CurrentValue;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Status_CheckBox(M_Draw, "Draw_Q") && SpellManager.Q.IsLearned)
            {
                Circle.Draw(SpellManager.Q.IsReady() ? Color.Lime : Color.IndianRed, SpellManager.Q.Range, Player.Instance.Position);
            }

            if (Status_CheckBox(M_Draw, "Draw_W") && SpellManager.W.IsLearned)
            {
                Circle.Draw(SpellManager.W.IsReady() ? Color.DeepSkyBlue : Color.IndianRed, SpellManager.W.Range, Player.Instance.Position);
            }

            if (Status_CheckBox(M_Draw, "Draw_E") && SpellManager.E.IsLearned)
            {
                Circle.Draw(SpellManager.E.IsReady() ? Color.Yellow : Color.IndianRed, SpellManager.E.Range, Player.Instance.Position);
            }

            if (Status_CheckBox(M_Draw, "Draw_Killable"))
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(2000) && x.IsHPBarRendered))
                {
                    if (enemy != null)
                    {
                        var Damage_Per = (int)((Damage.DmgCla(enemy) / enemy.TotalShieldHealth()) * 100);
                        
                        if (Damage_Per > 0)
                        {
                            MainFont.DrawText(null, enemy.TotalShieldHealth() <= Damage_Per ? "Killable" : Damage_Per + "%", (int)enemy.HPBarPosition.X + 40, (int)enemy.HPBarPosition.Y - 45, Damage_Per > enemy.TotalShieldHealth() ? Color.Yellow : Color.White);
                        }
                    }
                }
            }
        }

        static void Game_OnUpdate(EventArgs args)
        {
            Mode_Actives.Active();
            Mode_Item.Active_Redemption();

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
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Mode_Flee.Flee();
            }
        }
    }
}
