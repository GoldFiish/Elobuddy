using System;
using System.Linq;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace NebulaKalista
{
    public class DamageIndicator
    {
        private static readonly int XOffset = 2;
        private static readonly int YOffset = 8;
        private static readonly int Width = 104;
        private static readonly int Height = 9;
               
        private static int width, height, xOffset, yOffset;

        private static DamageToUnitDelegate _damageToUnit;
        public delegate float DamageToUnitDelegate(Obj_AI_Base minion);
        
        public static DamageToUnitDelegate DamageToUnit
        {
            get
            {
                return _damageToUnit;
            }

            set
            {
                if (_damageToUnit == null)
                {
                    Drawing.OnEndScene += OnEndScene;
                }
                _damageToUnit = value;
            }
        }
      
        private static void OnEndScene(EventArgs args)
        {
            if (_damageToUnit == null) return;

            if (Kalista.MenuDraw["Draw.E.Damage.C"].Cast<CheckBox>().CurrentValue)
            {              

                foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget() && x.IsHPBarRendered && x.HasRendBuff()))
                {
                    width = Width;
                    height = Height;
                    xOffset = XOffset;
                    yOffset = YOffset;

                    DrawLine(enemy);
                }
            }

            if (Kalista.MenuDraw["Draw.E.Damage.M"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var monster in EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsValidTarget(1000) && x.IsHPBarRendered && x.HasRendBuff()))
                {
                    var M_Ofs = Ofs_List.FirstOrDefault(x => monster.Name.Contains(x.Name));

                    width = M_Ofs.Width;
                    height = M_Ofs.Height;
                    xOffset = M_Ofs.XOffset;
                    yOffset = M_Ofs.YOffset;

                    DrawLine(monster);
                }
            }
        }

        private static void DrawLine(Obj_AI_Base unit)
        {
            var damage = _damageToUnit(unit);
            if (damage <= 0) return;

            var barPos = unit.HPBarPosition;

            //Get remaining HP after damage applied in percent and the current percent of health
            var percentHealthAfterDamage = Math.Max(0, unit.GetTotalHealthWithShieldsApplied() - damage) / (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
            var currentHealthPercentage = unit.GetTotalHealthWithShieldsApplied() / (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);

            //Calculate start and end point of the bar indicator
            var startPoint = barPos.X + xOffset + (percentHealthAfterDamage * width);
            var endPoint = barPos.X + xOffset + (currentHealthPercentage * width);
            var yPos = barPos.Y + yOffset;

            //Draw the line
            if (Kalista.MenuDraw["Draw.E.Damage.C"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawLine((float)startPoint, yPos, (float)endPoint, yPos, height, System.Drawing.Color.Yellow);
            }
            if (Kalista.MenuDraw["Draw.E.Damage.M"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawLine((float)startPoint, yPos, (float)endPoint, yPos, height, System.Drawing.Color.White);
            }
        }

        internal static readonly List<MonsterOffset> Ofs_List = new List<MonsterOffset>
        {
            new MonsterOffset
            {
                Name = "Dragon", Width = 143, Height = 9, XOffset = -4, YOffset = 8
            },       
            new MonsterOffset
            {
                Name = "Baron", Width = 191, Height = 12, XOffset = -29, YOffset = 6
            },
            new MonsterOffset
            {
                Name = "Herald", Width = 142, Height = 9, XOffset = -4, YOffset = 7
            },
            // new MonsterOffset
            //{
            //    Name = "RedMini", Width = 49, Height = 4, XOffset = 42, YOffset = 7
            //},
            // new MonsterOffset
            //{
            //    Name = "BlueMini", Width = 49, Height = 4, XOffset = 42, YOffset = 7
            //},
            //   new MonsterOffset
            //{
            //    Name = "RazorbeakMini", Width = 49, Height = 4, XOffset = 42, YOffset = 7
            //},
            //  new MonsterOffset
            //{
            //    Name = "KrugMini", Width = 55, Height = 3, XOffset = 41, YOffset = 7
            //},
            
            //   new MonsterOffset
            //{
            //    Name = "MurkwolfMini", Width = 55, Height = 3, XOffset = 39, YOffset = 7
            //},
            //new MonsterOffset
            //{
            //    Name = "Red", Width = 141, Height = 9, XOffset = -3, YOffset = 12 //7
            //},           
            //new MonsterOffset
            //{
            //    Name = "Blue", Width = 141, Height = 9, XOffset = -3, YOffset = 7
            //},
            
            //new MonsterOffset
            //{
            //    Name = "Gromp", Width = 86, Height = 4, XOffset = 24, YOffset = 7
            //},
            new MonsterOffset
            { 
                Name = "Crab", Width = 61, Height = 4, XOffset = 36, YOffset = 21
            },
            //new MonsterOffset
            //{
            //    Name = "Krug", Width = 79, Height = 4, XOffset = 21, YOffset = 7
            //},
           
            //new MonsterOffset
            //{
            //    Name = "Razorbeak", Width = 74, Height = 4, XOffset = 30, YOffset = 9
            //},
            
            //new MonsterOffset
            //{
            //    Name = "Murkwolf", Width = 74, Height = 4, XOffset = 30, YOffset = 9
            //},
               
        };

        internal class MonsterOffset
        {
            internal string Name;
            internal int Height;
            internal int Width;
            internal int XOffset;
            internal int YOffset;
        }
    }
}
