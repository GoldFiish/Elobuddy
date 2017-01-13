//======================================================================================//
//======================================================================================//
//                                    TwistedFate                                       //
//                                    By goldfinsh                                      //
//======================================================================================//
//======================================================================================//
using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace NebulaTwistedFate
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }
        
        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "TwistedFate") return;

            TwistedFate.Load();
        }
    }
}