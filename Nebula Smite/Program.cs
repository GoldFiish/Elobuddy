using System;
using EloBuddy.SDK.Events;

namespace NebulaSmite
{
    class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += LoadingComplete;         
        }
       
        static void LoadingComplete(EventArgs args)
        {
            smite.Load();           
        }        
    }
}
