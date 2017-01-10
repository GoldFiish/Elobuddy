using System.Collections.Generic;

namespace NebulaKalista.ControllN
{
    class Controller
    {
        public Dictionary<EnumContext, string> Dictionary = new Dictionary<EnumContext, string>();
    }

    enum EnumContext
    {
        Main0,
        Main1,

        Combo,
        Combo_Q_Mode,
        Combo_Q_Mode0,
        Combo_Q_Mode1,
        Combo_Q_Mode2,
        Combo_Q_Mode3,

        Harass,
        StackNum0,
        StackNum1,

        Clear,
        Lane,
        Jungle,
        JungleModeQ,
        JungleModeE,
        JungleMode0,
        JungleMode1,
        JungleMode2,
        JungleMode3,

        Misc,
        Esetting,
        EKillSteal,
        EJungSteal,
        ECustom,
        ECustomDmg0,
        ECustomDmg1,
        EDeath,
        EDeathHp1,
        EDeathHp2,
        Rsetting,
        SaveR,
        SaveRHp1,
        SaveRHp2,
        Bali,
        BaliDis,

        Item,
        sHextechG,
        sBilge,
        sBlade,       
        sBKHp1,
        sBKHp2,
        sYoumuu,
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

        Draw,
        DrawQ,
        DrawE,
        DrawR,
        DrawECDamage,
        DrawEJDamage,
        DrawPosition,
        Position0,
        Position1,
        Position2,
        Position3,
        DrawX,
        DrawY,

        //스펠 사용
        UseQ,
        UseW,
        UseE,
        UseR,

        //마나 상태
        ManaStatus1,
        ManaStatus2,

        //미니언 카운트
        MinionNum0,
        MinionNum1        
    }
}
