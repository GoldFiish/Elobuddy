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
using NebulaTwistedFate.Modes;
using NebulaTwistedFate.ControllN;

namespace NebulaTwistedFate
{
    class TwistedFate
    {       
        public static Menu Menu, M_Combo, M_Harras, M_Clear, M_Item, M_Auto, M_Misc, M_Draw, M_NVer;

        static SharpDX.Direct3D9.Font MainFont = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 11, System.Drawing.FontStyle.Bold));
        
        public static void Load()
        {
            Chat.Print("<font color = '#cfa9a'>Welcome to </font><font color = '#ffffff'>[ Nebula ] " + Player.Instance.ChampionName + "</font><font color = '#cfa9a'>. Addon is ready.</font>");            

            Menu = MainMenu.AddMenu("[ Nebula ] TwistedFate", "By.Natrium");           
            Menu.Add("Language.Select", new ComboBox("Language / 언어", 0, "English", "한국어"));
            //Menu.AddVisualFrame(new VsFrame("Load_Img", System.Drawing.Color.Purple));

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

            M_Combo = Menu.AddSubMenu(language.Dictionary[EnumContext.Combo]);
            M_Combo.Add("Combo_Q",          new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Combo.Add("Combo_Q_Stun",     new ComboBox(language.Dictionary[EnumContext.ComboQMode], 0, language.Dictionary[EnumContext.ComboQMode0], language.Dictionary[EnumContext.ComboQMode1]));
            M_Combo.Add("Combo_Q_Pre",      new Slider(language.Dictionary[EnumContext.QPrediction], 75));
            M_Combo.Add("Combo_Q_Mana",     new Slider(language.Dictionary[EnumContext.ManaStatus] + "[ {0}% ]" + language.Dictionary[EnumContext.sMore], 25));
            M_Combo.AddSeparator(10);
            M_Combo.Add("Combo_W",          new CheckBox(language.Dictionary[EnumContext.SpellW]));
            M_Combo.Add("Combo_W_Pick",     new ComboBox(language.Dictionary[EnumContext.CardMode], 0,
                                                language.Dictionary[EnumContext.CardMode0], language.Dictionary[EnumContext.CardMode1], language.Dictionary[EnumContext.CardMode2]));
            M_Combo.Add("Combo_W_Red",      new Slider(language.Dictionary[EnumContext.EnemyNum0] + "[ {0} }" + language.Dictionary[EnumContext.EnemyNum1], 4, 1, 5));
            M_Combo.Add("Combo_W_Blue",     new Slider(language.Dictionary[EnumContext.ManaStatus] + "[ {0}% ]" + language.Dictionary[EnumContext.BlueStatus], 25));

            M_Harras = Menu.AddSubMenu(language.Dictionary[EnumContext.Harass]);            
            M_Harras.Add("Harass_Q",        new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Harras.Add("Harass_Q_Pre",    new Slider(language.Dictionary[EnumContext.QPrediction], 75));
            M_Harras.Add("Harass_Q_Mana",   new Slider(language.Dictionary[EnumContext.ManaStatus] + "[ {0}% ]" + language.Dictionary[EnumContext.sMore], 30));
            M_Harras.AddSeparator(10);
            M_Harras.Add("Harass_W",        new CheckBox(language.Dictionary[EnumContext.SpellW]));
            M_Harras.Add("Harras_W_Pick",   new ComboBox(language.Dictionary[EnumContext.CardMode], 0, 
                                                language.Dictionary[EnumContext.CardMode0], language.Dictionary[EnumContext.CardMode1], language.Dictionary[EnumContext.CardMode2])); //스마트 모드 챔치언 주위에 적이 3명이거나 라이너 + 미니언2이상일때 레드카드
            M_Harras.Add("Harras_W_Blue",   new Slider(language.Dictionary[EnumContext.ManaStatus] + "[ {0}% ]" + language.Dictionary[EnumContext.BlueStatus], 25));           
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//
            M_Clear = Menu.AddSubMenu(language.Dictionary[EnumContext.Clear]);
            M_Clear.AddGroupLabel(language.Dictionary[EnumContext.LaneClear]);
            M_Clear.Add("Lane_Q",               new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Clear.Add("Lane_Q_Mode",          new ComboBox(language.Dictionary[EnumContext.LaneMode], 0,
                                                    language.Dictionary[EnumContext.LaneMode0], language.Dictionary[EnumContext.LaneMode1], language.Dictionary[EnumContext.LaneMode2]));
            M_Clear.Add("Lane_Q_Hit",           new Slider(language.Dictionary[EnumContext.MinionsNum] + "[ {0} ]" , 2, 0, 5));
            M_Clear.Add("Lane_Q_Mana",          new Slider(language.Dictionary[EnumContext.ManaStatus] + "[ {0}% ]" + language.Dictionary[EnumContext.sMore], 45));
            M_Clear.AddSeparator(10);
            M_Clear.Add("Lane_W_Red",           new CheckBox("W 레드 사용"));
            M_Clear.Add("Lane_W_RedHit",        new Slider(language.Dictionary[EnumContext.MinionsNum] + "[ {0} ]", 3, 0, 5));
            M_Clear.Add("Lane_W_RedKill",       new Slider(language.Dictionary[EnumContext.MinionsKNum] + "[ {0} ]", 2, 0, 5));
            M_Clear.Add("Lane_W_Blue",          new CheckBox("W 블루 사용"));
            M_Clear.Add("Lane_W_TotalMana",     new Slider(language.Dictionary[EnumContext.ManaStatus] + "[ {0}% ]" + language.Dictionary[EnumContext.sLow], 50));           
            M_Clear.AddSeparator(20);
            M_Clear.AddGroupLabel(language.Dictionary[EnumContext.JungleClear]);
            M_Clear.Add("Jungle_Q",             new CheckBox(language.Dictionary[EnumContext.SpellQ]));            
            M_Clear.Add("Jungle_Q_Mana",        new Slider(language.Dictionary[EnumContext.ManaStatus] + "[ {0}% ]" + language.Dictionary[EnumContext.sMore], 25));
            M_Clear.AddSeparator(10);
            M_Clear.Add("Jungle_W",             new CheckBox(language.Dictionary[EnumContext.SpellW]));
            M_Clear.Add("Jungle_W_Pick",        new ComboBox(language.Dictionary[EnumContext.CardMode], 0,
                                                    language.Dictionary[EnumContext.CardMode0], language.Dictionary[EnumContext.CardMode1], language.Dictionary[EnumContext.CardMode2]));
            M_Clear.Add("Jungle_W_TotalMana",   new Slider(language.Dictionary[EnumContext.ManaStatus] + "[ {0}% ]" + language.Dictionary[EnumContext.BlueStatus], 35));
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//           
            M_Item.AddLabel(language.Dictionary[EnumContext.ItemExp]);
            M_Item.Add("Item_BK_Hp",            new Slider(language.Dictionary[EnumContext.sBKHp1] + "[ {0}% ]" + language.Dictionary[EnumContext.sBKHp2], 95, 0, 100));
            M_Item.AddSeparator(10);
            M_Item.Add("QSS",                   new CheckBox(language.Dictionary[EnumContext.sQsilver]));
            M_Item.Add("Scimitar",              new CheckBox(language.Dictionary[EnumContext.sScimitar]));
            M_Item.Add("CastDelay",             new Slider(language.Dictionary[EnumContext.Delay], 350, 0, 1200));
            M_Item.AddSeparator(10);
            M_Item.AddLabel(language.Dictionary[EnumContext.SpellQ]);
            M_Item.Add("Blind",                 new CheckBox(language.Dictionary[EnumContext.sBlind]));
            M_Item.Add("Charm",                 new CheckBox(language.Dictionary[EnumContext.sCharm]));
            M_Item.Add("Fear",                  new CheckBox(language.Dictionary[EnumContext.sFear]));
            M_Item.Add("Ploymorph",             new CheckBox(language.Dictionary[EnumContext.sPolymorph]));
            M_Item.Add("Poisons",               new CheckBox(language.Dictionary[EnumContext.sPoisons]));
            M_Item.Add("Silence",               new CheckBox(language.Dictionary[EnumContext.sSilence]));
            M_Item.Add("Slow",                  new CheckBox(language.Dictionary[EnumContext.sSlow]));
            M_Item.Add("Stun",                  new CheckBox(language.Dictionary[EnumContext.sStun]));
            M_Item.Add("Supression",            new CheckBox(language.Dictionary[EnumContext.sSupression]));
            M_Item.Add("Taunt",                 new CheckBox(language.Dictionary[EnumContext.sTaunt]));
            M_Item.Add("Snare",                 new CheckBox(language.Dictionary[EnumContext.sSnare]));
            M_Item.AddSeparator(10);
            M_Item.AddLabel(language.Dictionary[EnumContext.sZhonya]);
            M_Item.Add("Item_Zy",               new CheckBox(language.Dictionary[EnumContext.sZhonya]));
            M_Item.Add("Item_Zy_BHp",           new Slider(language.Dictionary[EnumContext.sZhonyaBHp] + "[ {0}% ]" + language.Dictionary[EnumContext.sLow], 35, 0, 100));
            M_Item.Add("Item_Zy_BDmg",          new Slider(language.Dictionary[EnumContext.sZhonyaBDmg] + "[ {0}% ]" + language.Dictionary[EnumContext.sMore], 50, 0, 100));
            M_Item.AddSeparator(10);
            M_Item.Add("Item_Zy_SHp",           new Slider(language.Dictionary[EnumContext.sZhonyaSHp] + "[ {0}% ]" + language.Dictionary[EnumContext.sLow], 35, 0, 100));
            M_Item.Add("Item_Zy_SDmg",          new Slider(language.Dictionary[EnumContext.sZhonyaSDmg] + "[ {0}% ]" + language.Dictionary[EnumContext.sMore], 50, 0, 100));
            M_Item.AddSeparator(10);
            M_Item.AddLabel(language.Dictionary[EnumContext.sZhonyaR]);
            foreach (var enemyR in EntityManager.Heroes.Enemies)
            {
                M_Item.Add("R_" + enemyR.ChampionName.ToLower(), new CheckBox(enemyR.ChampionName + " [ R ]"));
            }
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//
            M_Misc = Menu.AddSubMenu(language.Dictionary[EnumContext.Msic]);
            M_Misc.Add("Misc_Ignite",           new CheckBox(language.Dictionary[EnumContext.AutoIgnite]));
            M_Misc.Add("Misc_KillSt",           new CheckBox(language.Dictionary[EnumContext.KillSteal]));
            M_Misc.Add("Misc_JungSt",           new CheckBox(language.Dictionary[EnumContext.JungSteal]));
            M_Misc.Add("Misc_Auto_Q",           new CheckBox(language.Dictionary[EnumContext.AutoQ]));
            M_Misc.Add("Misc_Auto_Yel",         new CheckBox(language.Dictionary[EnumContext.AutoPickYel]));
            M_Misc.Add("Misc_Delay",            new CheckBox(language.Dictionary[EnumContext.PickDelay], false));
            M_Misc.Add("Misc_DisableAA",        new CheckBox(language.Dictionary[EnumContext.DisAA]));
            M_Misc.AddSeparator(20);
            M_Misc.AddLabel("Gapcloser");
            M_Misc.Add("Misc_Gap_Q",            new CheckBox(language.Dictionary[EnumContext.SpellQ]));
            M_Misc.AddSeparator(20);
            M_Misc.AddLabel("Interrupt");
            M_Misc.Add("Misc_Int_Q",            new CheckBox(language.Dictionary[EnumContext.SpellW]));
            M_Misc.Add("Misc_Int_Q_Lv",         new ComboBox(language.Dictionary[EnumContext.InterruptLv], 0, language.Dictionary[EnumContext.InterruptLv1], language.Dictionary[EnumContext.InterruptLv2]));
            //===================================================================================================================================================================//
            //===================================================================================================================================================================//
            M_Draw = Menu.AddSubMenu(language.Dictionary[EnumContext.Draw]);
            M_Draw.Add("Draw_Killable",         new CheckBox(language.Dictionary[EnumContext.DrawText]));
            M_Draw.AddSeparator(10);
            M_Draw.Add("Draw_Q",                new CheckBox(language.Dictionary[EnumContext.DrawQ]));
            M_Draw.AddSeparator(10);
            M_Draw.Add("Draw_W",                new CheckBox(language.Dictionary[EnumContext.DrawW]));
            M_Draw.AddSeparator(10);
            M_Draw.Add("Draw_R",                new CheckBox(language.Dictionary[EnumContext.DrawR]));
            
            CheckVersion.CheckUpdate();

            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
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

            if (Status_CheckBox(M_Draw, "Draw_R") && SpellManager.E.IsLearned)
            {
                Circle.Draw(SpellManager.R.IsReady() ? Color.DeepSkyBlue : Color.IndianRed, SpellManager.R.Range, Player.Instance.Position);
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

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Status_CheckBox(M_Misc, "Misc_DisableAA") && CardSelect.Status == SelectStatus.Selecting)
            {
                args.Process = false;
                return;
            }
            args.Process = true;
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {      
            if (sender is AIHeroClient && sender.IsEnemy)
            {
                if(Status_CheckBox(M_Misc, "Misc_Gap_Q") && Player.Instance.Distance(args.Sender) <= SpellManager.Q.Range && SpellManager.Q.IsReady())
                {
                    var prediction = SpellManager.Q.GetPrediction(sender);

                    SpellManager.Q.Cast(prediction.CastPosition);
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
            }
        }

        static void Game_OnUpdate(EventArgs args)
        {
            Mode_Actives.Active();

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
        }
    }
}
