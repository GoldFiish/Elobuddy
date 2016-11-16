using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;
using Rectangle = SharpDX.Rectangle;
using Sprite = EloBuddy.SDK.Rendering.Sprite;

namespace NebulaSkin
{
    public static class PreviewHelpers
    {
        public static WardPreview AddVisualFrame(this Menu menu, WardPreview control)
        {
            menu.Add(control.SerializationId, control);
            return control;
        }
    }

    public class WardPreview : ValueBase<Color>
    {
        public static String[] Ward_Name = new String[]
        {
            "Classic", "BatOLantern ", "Haunting", "Widow", "Deadfall", "TombAngel", "Snowman", "Gingerbread", "LanternoftheSerpent", "BanneroftheSerpent",
            "Starcall", "Draven", "Luminosity", "Season3Victorious", "Season3Championship", "Candycane", "BanneroftheHorse", "Gong", "Bouquet", "Riot",
            "Wardskin20", "Dragonslayer", "All-Stars", "GoldenGoal", "Mecha", "Rammus", "Amumu", "Ascension", "2014Championship", "S4Conquering",
            "S4Triumphant", "S4Victorious", "Battlecast", "NomalPoro", "AstroPoro", "GentlemanPoro", "BattlecastPoro", "DragonSlayerPoro ", "UnderworldPoro", "LunarDragon",
            "Heartseeker", "UrfTriumphant", "MotherSerpent", "SlaughterFleet", "OpticEnhancer", "Championship2015", "2015Conquering", "2015Triumphant", "2015Victorious", "Riggle",
            "TeamFire", "TeamIce", "PenguinSkier", "RisingDawn", "Harpseeker", "Hextech", "DefinitelyNotAWard", "MechaZero", "ElPoroWard", "DarkStar",
            "HisRoyalCrabness", "PROJECT", "GAMEON", "StarGuardian", "2016Championship", "MechsvsMinions", "2016Conquering", "2016Triumphant", "2016Victorious", "Off"
        };

        private static WardAtlass WardAtlas { get; set; }
        private static Sprite ImageSprite { get; set; }
        
        private readonly string _name;
        private Vector2 _offset;

        public override string VisibleName { get { return _name; } }
        public override Vector2 Offset { get { return _offset; } }

        public WardPreview(string uId, Color defaultValue) : base(uId, "", 265)
        {
            ImageSprite = new Sprite(
                Texture.FromMemory(
                    Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Properties.Resources.WardList, typeof(byte[])),
                    710, 574, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Default, 10));

            WardAtlas = JsonConvert.DeserializeObject<WardAtlass>(Properties.Resources.WardPosition);
        }

        public override bool Draw()
        {
            if (MainMenu.IsVisible && IsVisible)
            {
                try
                {
                    if (WardAtlas.WardSprite == null)
                    {
                        throw new NullReferenceException("WardAtlas is null");                        
                    }

                    var Preview = WardAtlas[Ward_Name[Skin.Menu["Ward.Skin"].Cast<Slider>().CurrentValue]];

                    ImageSprite.Draw(new Vector2(MainMenu.Position.X + 615, Skin.Menu["Ward.Skin"].Position.Y - 50), Preview.Rectangle);
                }
                catch
                {
                }
                return true;
            }
            return false;
        }
    }

    [DataContract]
    public class WardAtlass
    {
        [DataMember]
        public SpriteAtlas WardSprite { get; private set; }

        public WardOffset this[string WardName]
        {
            get
            {
                var lowerName = WardName.ToLower();
                return WardSprite.SkinName.Where(entry => lowerName.Contains(entry.Key.ToLower()) ||
                                                            (entry.Value.AlternativeNames != null
                                                             && entry.Value.AlternativeNames.Any(o => lowerName.Contains(o.ToLower())))).Select(entry => entry.Value).FirstOrDefault();
            }
        }
    }

    [DataContract]
    public class SpriteAtlas
    {
        [DataMember(IsRequired = true)]
        public int Width { get; private set; }
        [DataMember(IsRequired = true)]
        public int Height { get; private set; }
        [DataMember(IsRequired = true)]
        public Dictionary<string, WardOffset> SkinName { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var summoner in SkinName.Values)
            {
                summoner.Rectangle = new Rectangle(summoner.X, summoner.Y, Width, Height);
                summoner.LeftHalf = new Rectangle(summoner.X, summoner.Y, Width / 2, Height);
                summoner.RightHalf = new Rectangle(summoner.X + Width / 2, summoner.Y, Width / 2, Height);
            }

            SkinName = SkinName.OrderByDescending(o => o.Key).ToDictionary(o => o.Key, o => o.Value);
        }
    }

    [DataContract]
    public class WardOffset
    {
        [DataMember(IsRequired = true)]
        public int X { get; private set; }
        [DataMember(IsRequired = true)]
        public int Y { get; private set; }

        [DataMember]
        public List<string> AlternativeNames { get; private set; }
        public Rectangle Rectangle { get; set; }
        public Rectangle LeftHalf { get; set; }
        public Rectangle RightHalf { get; set; }
    }
}
