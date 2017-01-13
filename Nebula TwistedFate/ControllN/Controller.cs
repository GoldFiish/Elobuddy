using System.Collections.Generic;

namespace NebulaTwistedFate.ControllN
{
    class Controller
    {
        public Dictionary<EnumContext, string> Dictionary = new Dictionary<EnumContext, string>();
    }

    enum EnumContext
    {
        Main,
        SelectLanguage,
        Text_1,
        Text_2,

        Combo,
        ComboQMode,
        ComboQMode0,
        ComboQMode1,
        EnemyNum0,
        EnemyNum1,

        Harass,

        Clear,
        LaneClear,
        LaneMode,
        LaneMode0,
        LaneMode1,
        LaneMode2,
        MinionsNum,
        MinionsKNum,
        LaneCardRed,
        LaneCardBlue,

        JungleClear,

        Item,
        ItemExp,
        sBlade,
        sBKHp1,
        sBKHp2,
        sQsilver,
        sScimitar,
        Delay,
        sBuffType,
        sPoisons,
        sSupression,
        sBlind,
        sCharm,
        sFear,
        sPolymorph,
        sSilence,
        sSlow,
        sSnare,
        sStun,
        sKnockup,
        sTaunt,
        sZhonya,
        sZhonyaBHp,
        sZhonyaBDmg,
        sZhonyaSHp,
        sZhonyaSDmg,
        sZhonyaR,      

        Msic,
        AutoIgnite,
        KillSteal,
        JungSteal,
        AutoQ,
        AutoPickYel,
        PickDelay,
        DisAA,
        GapcloseQ,
        InterruptQ,
        InterruptLv,
        InterruptLv1,
        InterruptLv2,

        Draw,
        DrawQ,
        DrawW,
        DrawR,
        DrawText,
        SpellQ,
        SpellW,
        SpellR,

        QPrediction,

        CardMode,
        CardMode0,
        CardMode1,
        CardMode2,
        CardRed,
        CardBlue,
        CardYellow,

        ManaStatus,
        BlueStatus,
        sMore,
        sLow,
    }
}
