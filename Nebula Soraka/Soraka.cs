using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using NebulaSoraka.Modes;
using NebulaSoraka.ControllN;

namespace NebulaSoraka
{
    class Soraka
    {       
        public static Menu Menu, M_Main, M_Clear, M_Item, M_Auto, M_Misc, M_Draw, M_NVer;

        static SharpDX.Direct3D9.Font MainFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Bold));
        
        public static void Load()
        {
            Chat.Print("<font color = '#cfa9a'>Welcome to </font><font color = '#ffffff'>[ Nebula ] " + Player.Instance.ChampionName + "</font><font color = '#cfa9a'>. Addon is ready.</font>");            

            Menu = MainMenu.AddMenu("[ Nebula ] Soraka", "By.Natrium");           
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
            M_Main.Add("Combo_E",           new CheckBox(language.Dictionary[EnumContext.SpellE]));
            M_Main.AddSeparator(20);
            M_Main.AddGroupLabel(language.Dictionary[EnumContext.Harass]);
            M_Main.Add("Harass_Q",          new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Main.Add("Harass_Q_Mana",     new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 30));
            M_Main.AddSeparator(10);
            M_Main.Add("Harass_E",          new CheckBox(language.Dictionary[EnumContext.SpellE]));
            M_Main.Add("Harass_E_Mana",     new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 80));
            M_Main.AddSeparator(20);
            M_Main.AddGroupLabel(language.Dictionary[EnumContext.Flee]);
            M_Main.Add("Flee_Q",            new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Main.Add("Flee_E",            new CheckBox(language.Dictionary[EnumContext.SpellE]));
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//
            M_Clear = Menu.AddSubMenu(language.Dictionary[EnumContext.Clear]);
            M_Clear.AddGroupLabel(language.Dictionary[EnumContext.LaneClear]);
            M_Clear.Add("Lane_Q",           new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Clear.Add("Lane_Q_Hit",       new Slider(language.Dictionary[EnumContext.LaneQHit] + "{0} ]" , 2, 0, 5));
            M_Clear.Add("Lane_Q_Mana",      new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 45));
            M_Clear.AddSeparator(20);
            M_Clear.AddGroupLabel(language.Dictionary[EnumContext.JungleClear]);
            M_Clear.Add("Jungle_Q",         new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Clear.Add("Jungle_Q_Mana",    new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 25));
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//
            M_Item = Menu.AddSubMenu(language.Dictionary[EnumContext.Item]);
            M_Item.AddLabel(language.Dictionary[EnumContext.sModeCombo]);
            M_Item.Add("Item.BK",               new CheckBox(language.Dictionary[EnumContext.sBK]));
            M_Item.Add("Item.BK.Hp",            new Slider(language.Dictionary[EnumContext.sBKHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.sBKHp2], 95, 0, 100));
            M_Item.AddSeparator(10);           
            M_Item.Add("Item.Mikael",           new CheckBox(language.Dictionary[EnumContext.sMikael]));
            M_Item.Add("Item.Mikael_Op",        new ComboBox(language.Dictionary[EnumContext.sMikaelOp], 0, language.Dictionary[EnumContext.sMikaelOp1], language.Dictionary[EnumContext.sMikaelOp2]));
            M_Item.Add("Fear",                  new CheckBox(language.Dictionary[EnumContext.sFear]));
            M_Item.Add("Silence",               new CheckBox(language.Dictionary[EnumContext.sSilence]));
            M_Item.Add("Slow",                  new CheckBox(language.Dictionary[EnumContext.sSlow]));
            M_Item.Add("Snare",                 new CheckBox(language.Dictionary[EnumContext.sSnare]));
            M_Item.Add("Stun",                  new CheckBox(language.Dictionary[EnumContext.sStun]));
            M_Item.Add("Taunt",                 new CheckBox(language.Dictionary[EnumContext.sTaunt]));
            M_Item.AddLabel(language.Dictionary[EnumContext.sAlways]);
            M_Item.Add("Item.Solari",           new CheckBox(language.Dictionary[EnumContext.sSolari]));
            M_Item.Add("Item.Solari.MyHp",      new Slider(language.Dictionary[EnumContext.MyHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.MyHp2], 50));
            M_Item.Add("Item.Solari.AMyHp",     new Slider(language.Dictionary[EnumContext.sSolariAMyHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.sSolariAMyHp2], 25));
            M_Item.Add("Item.Solari.TeamHp",    new Slider(language.Dictionary[EnumContext.TeamHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.TeamHp2], 65));
            M_Item.AddSeparator(10);
            M_Item.Add("Item.Redemption",       new CheckBox(language.Dictionary[EnumContext.sRedemption]));
            M_Item.Add("Item.Redemption.MyHp",  new Slider(language.Dictionary[EnumContext.MyHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.MyHp2], 50, 0, 100));
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//
            M_Auto = Menu.AddSubMenu(language.Dictionary[EnumContext.Auto]);
            M_Auto.Add("Auto_Q",            new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Auto.Add("Auto_Q_Hit",        new Slider(language.Dictionary[EnumContext.AutoQHit], 65));
            M_Auto.Add("Auto_Q_Mana",       new Slider(language.Dictionary[EnumContext.ManaStatus1] + "[ {0}% ]" + language.Dictionary[EnumContext.ManaStatus2], 55));
            M_Auto.AddSeparator(10);
            M_Auto.Add("Auto_W",            new CheckBox(language.Dictionary[EnumContext.SpellW]));
            M_Auto.Add("Auto_W_Target",     new ComboBox(language.Dictionary[EnumContext.AutoWOp], 0, language.Dictionary[EnumContext.AutoWOp1], language.Dictionary[EnumContext.AutoWOp2], language.Dictionary[EnumContext.AutoWOp3]));
            M_Auto.Add("Auto_W_MyHp",       new Slider(language.Dictionary[EnumContext.AutoWMyHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.AutoWMyHp2], 50));
            M_Auto.Add("Auto_W_TeamHp",     new Slider(language.Dictionary[EnumContext.TeamHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.TeamHp2], 65));
            M_Auto.AddSeparator(10);
            M_Auto.Add("Auto_R",            new CheckBox(language.Dictionary[EnumContext.SpellR]));
            M_Auto.Add("Auto_R_MyHp",       new Slider(language.Dictionary[EnumContext.MyHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.MyHp2], 35));
            M_Auto.Add("Auto_R_TeamHp",     new Slider(language.Dictionary[EnumContext.TeamHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.TeamHp2], 35));
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//
            M_Misc = Menu.AddSubMenu(language.Dictionary[EnumContext.Msic]);
            M_Misc.Add("Misc_Ignite",   new CheckBox(language.Dictionary[EnumContext.AutoIgnite]));
            M_Misc.Add("Misc_KillSt",   new CheckBox(language.Dictionary[EnumContext.KillSteal]));
            M_Misc.AddSeparator(20);
            M_Misc.AddLabel("Gapcloser");
            M_Misc.Add("Misc_Gap_Q",    new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Misc.Add("Misc_Gap_E",    new CheckBox(language.Dictionary[EnumContext.SpellE], false));
            M_Misc.AddSeparator(20);
            M_Misc.AddLabel("Interrupt");
            M_Misc.Add("Misc_Int_Q",    new CheckBox(language.Dictionary[EnumContext.SpellQ], false));
            M_Misc.Add("Misc_Int_Q_Lv", new ComboBox(language.Dictionary[EnumContext.InterruptLv], 0, language.Dictionary[EnumContext.InterruptLv1], language.Dictionary[EnumContext.InterruptLv2]));
            M_Misc.Add("Misc_Int_E",    new CheckBox(language.Dictionary[EnumContext.SpellE]));
            M_Misc.Add("Misc_Int_E_Lv", new ComboBox(language.Dictionary[EnumContext.InterruptLv], 1, language.Dictionary[EnumContext.InterruptLv1], language.Dictionary[EnumContext.InterruptLv2]));
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

            CheckVersion.CheckUpdate();

            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
            Obj_AI_Base.OnProcessSpellCast += Mode_Item.OnProcessSpellCast;
            Obj_AI_Base.OnBasicAttack += Mode_Item.OnBasicAttack;
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

        public static int Status_ComboBox(Menu sub, string str)
        {
            return sub[str].Cast<ComboBox>().CurrentValue;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Status_CheckBox(M_Draw, "Draw_Q") && SpellManager.Q.IsLearned)
            {
                Circle.Draw(SpellManager.Q.IsReady() ? Color.Yellow : Color.IndianRed, SpellManager.Q.Range, Player.Instance.Position);
            }

            if (Status_CheckBox(M_Draw, "Draw_W") && SpellManager.W.IsLearned)
            {
                Circle.Draw(SpellManager.W.IsReady() ? Color.HotPink : Color.IndianRed, SpellManager.W.Range, Player.Instance.Position);
            }

            if (Status_CheckBox(M_Draw, "Draw_E") && SpellManager.E.IsLearned)
            {
                Circle.Draw(SpellManager.E.IsReady() ? Color.DeepSkyBlue : Color.IndianRed, SpellManager.E.Range, Player.Instance.Position);
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

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {      
            if (sender is AIHeroClient && sender.IsEnemy)
            {
                if(Status_CheckBox(M_Misc, "Misc_Gap_Q") && Player.Instance.Distance(args.Sender) <= SpellManager.Q.Range && SpellManager.Q.IsReady())
                {
                    var prediction = Mode_Combo.GetQPrediction(sender);

                    SpellManager.Q.Cast(prediction.CastPosition);
                }

                if (Status_CheckBox(M_Misc, "Misc_Gap_E") && Player.Instance.Distance(args.Sender) <= SpellManager.E.Range && SpellManager.E.IsReady())
                {
                    var prediction = SpellManager.E.GetPrediction(sender);

                    SpellManager.E.Cast(sender.Position);
                }
            }
        }

        static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (sender is AIHeroClient && sender.IsEnemy)
            {
                if(Status_CheckBox(M_Misc, "Misc_Int_Q") && Player.Instance.Distance(args.Sender) <= SpellManager.Q.Range && SpellManager.Q.IsReady())
                {
                     switch (Status_ComboBox(M_Misc, "Misc_Int_Q_Lv"))
                        {
                            case 0:
                                if (args.DangerLevel == DangerLevel.Medium)
                                {
                                    SpellManager.Q.Cast(sender.Position);
                                }
                                break;
                            case 1:
                                 if (args.DangerLevel == DangerLevel.High)
                                {
                                    SpellManager.Q.Cast(sender.Position);
                                }
                                break;
                        }
                }

                if(Status_CheckBox(M_Misc, "Misc_Int_E") && Player.Instance.Distance(args.Sender) <= SpellManager.E.Range && SpellManager.E.IsReady())
                {
                    switch (Status_ComboBox(M_Misc, "Misc_Int_E_Lv"))
                        {
                            case 0:
                                if (args.DangerLevel == DangerLevel.Medium)
                                {
                                    SpellManager.E.Cast(sender.Position);
                                }
                                break;
                            case 1:
                                 if (args.DangerLevel == DangerLevel.High)
                                {
                                   SpellManager.E.Cast(sender.Position);
                                }
                                break;
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
