//======================================================================================//
//======================================================================================//
//                                       Kalista                                        //
//                                    By goldfinsh                                      //
//======================================================================================//
//======================================================================================//
using System;
using EloBuddy;
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
            if (Player.Instance.ChampionName != "Kalista") return;

            Kalista.Load();
        }        
    }
}
