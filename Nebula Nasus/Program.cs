//======================================================================================//
//======================================================================================//
//                                        Nasus                                         //
//                                    By goldfinsh                                      //
//======================================================================================//
//======================================================================================//
using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace NebulaNasus
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }
        
        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Nasus") return;

            Nasus.Load();
        }
    }
}