using System.Drawing;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;
using Sprite = EloBuddy.SDK.Rendering.Sprite;

namespace NebulaTwistedFate.ControllN
{
    public static class PreviewHelpers
    {
        public static VsFrame AddVisualFrame(this Menu menu, VsFrame control)
        {
            menu.Add(control.SerializationId, control);
            return control;
        }
    }

    public class VsFrame : ValueBase<Color>
    {
        private static Sprite ImageSprite { get; set; }
        
        private readonly string _name;
        private Vector2 _offset;

        public override string VisibleName { get { return _name; } }
        public override Vector2 Offset { get { return _offset; } }

        public VsFrame(string uId, Color defaultValue) : base(uId, "", 265)
        {
            ImageSprite = new Sprite(
                Texture.FromMemory(
                    Drawing.Direct3DDevice, (byte[])new ImageConverter().ConvertTo(Properties.Resources.Img_TF, typeof(byte[])),
                    249, 241, 0, Usage.None, Format.A1, Pool.Managed, Filter.Default, Filter.Dither, 10));
        }

        public override bool Draw()
        {
            if (MainMenu.IsVisible && IsVisible)
            {
                try
                {
                    ImageSprite.Draw(new Vector2(TwistedFate.Menu["Language.Select"].Position.X - 14, TwistedFate.Menu["Language.Select"].Position.Y + 185));
                }
                catch
                {
                }
                return true;
            }
            return false;
        }
    }
}