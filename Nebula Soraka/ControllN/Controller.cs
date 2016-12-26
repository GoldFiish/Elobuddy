﻿using System.Collections.Generic;

namespace NebulaSoraka.ControllN
{
    class Controller
    {
        public Dictionary<EnumContext, string> Dictionary = new Dictionary<EnumContext, string>();
    }

    enum EnumContext
    {
        Main,
        Clear,
        Item,
        Auto,
        Msic,
        Draw,
        SelectLanguage,
        Text_1,
        Text_2,
        Combo,
        ComboWDis1,
        ComboWDis2,
        Harass,
        Flee,
        LaneClear,
        LaneQHit,
        JungleClear,
        JungleHitNum1,
        JungleHitNum2,
        sModeCombo,
        sYoummu,
        sBK,
        sBKHp1,
        sBKHp2,
        sMikael,
        sMikaelOp,
        sMikaelOp1,
        sMikaelOp2,
        sFear,
        sSilence,
        sSlow,
        sSnare,
        sStun,
        sTaunt,       
        sAlways,
        sSolari,
        sSolariAMyHp1,
        sSolariAMyHp2,
        sRedemption,
        AutoQMode,
        AutoQModeOp1,
        AutoQModeOp2,
        AutoQHit,
        AutoWMyHp1,
        AutoWMyHp2,
        AutoWOp,
        AutoWOp1,
        AutoWOp2,
        AutoWOp3,
        AutoIgnite,
        KillSteal,
        InterruptLv,
        InterruptLv1,
        InterruptLv2,
        DrawQ,
        DrawW,
        DrawE,
        DrawR,
        DrawText,
        SpellQ,
        SpellW,
        SpellE,
        SpellR,
        MyHp1,
        MyHp2,
        ManaStatus1,
        ManaStatus2,
        TeamHp1,
        TeamHp2
    }
}