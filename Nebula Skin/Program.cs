using System;
using EloBuddy.SDK.Events;

namespace NebulaSkin
{
    class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += LoadingComplete;         
        }
       
        static void LoadingComplete(EventArgs args)
        {
            Skin.Load();           
        }        
    }
}
