using System;
using EloBuddy.SDK.Events;

namespace NebulaTeemo
{
    class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += LoadingComplete;         
        }
       
        static void LoadingComplete(EventArgs args)
        {
            Teemo.Load();
        }        
    }
}
