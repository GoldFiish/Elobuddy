using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace NebulaKalista
{
    class Program
    {
        private static readonly TextureLoader TextureLoader = new TextureLoader();
        private static Sprite TopSprite { get; set; }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += LoadingComplete;         
        }
       
        static void LoadingComplete(EventArgs args)
        {
            Kalista.Load();

            TextureLoader.Load("top", Properties.Resources.KalistaLogo);
            TopSprite = new Sprite(() => TextureLoader["top"]);

            Drawing.OnEndScene += Drawing_OnEndScene;
        }           

        public static void Drawing_OnEndScene(EventArgs args)
        {
            TopSprite.Draw(new Vector2((Drawing.Width - 600) / 2, 30));

            Core.DelayAction(() => TextureLoader.Unload("top"), 12000);
        }
    }
}
