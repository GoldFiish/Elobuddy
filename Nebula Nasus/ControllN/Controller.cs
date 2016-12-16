using System.Collections.Generic;

namespace NebulaNasus.ControllN
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
        JungleClear,
        JungleHitNum1,
        JungleHitNum2,
        sModeCombo,
        sYoummu,
        sBK,
        sBKHp1,
        sBKHp2,
        sQSS,
        sScimiter,
        sDelay,
        sBlind,
        sCharm,
        sFear,
        sPloymorph,
        sPoisons,
        sSilence,
        sSlow,
        sStun,
        sSupression,
        sTaunt,
        sSnare,
        sAlways,
        sRedemption,
        AutoIgnite,
        KillSteal,
        KillOption,
        KillText1,
        KillText2,
        JungleSteal,
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
        ManaStatus2
    }
}
