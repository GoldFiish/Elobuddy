using System;
using EloBuddy.SDK.Events;

namespace NebulaKalista
{
    class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += LoadingComplete;         
        }
       
        static void LoadingComplete(EventArgs args)
        {
            Kalista.Load();
        }        
    }
}
