namespace NebulaKalista.ControllN
{
    class Kor : Controller
    {
        public Kor()
        {
            Dictionary.Add(EnumContext.Main0,           "이 애드온이 마음에 드신다면, 추천 또는 페이팔 기부를 부탁드리며,");
            Dictionary.Add(EnumContext.Main1,           "기부하신 금액은 애드온 제작에 사용됩니다. 감사합니다.");            
            Dictionary.Add(EnumContext.Combo,           "- 콤보");
            Dictionary.Add(EnumContext.Combo_Q_Mode,    "[ Q ] 옵션");
            Dictionary.Add(EnumContext.Combo_Q_Mode0,   "활성화일 때");
            Dictionary.Add(EnumContext.Combo_Q_Mode1,   "[ Q + E ] 킬각일 때");
            Dictionary.Add(EnumContext.Combo_Q_Mode2,   "[ E ] 스택이 5 이상");
            Dictionary.Add(EnumContext.Combo_Q_Mode3,   "[ E ] 실패 시");
            Dictionary.Add(EnumContext.Harass,          "- 견제");
            Dictionary.Add(EnumContext.StackNum0,       "챔피언의 스택 개수가 ");
            Dictionary.Add(EnumContext.StackNum1,       " 이상일 때");
            Dictionary.Add(EnumContext.Clear,           "- 라인 / 정글");;
            Dictionary.Add(EnumContext.Lane,            "라인 정리");
            Dictionary.Add(EnumContext.Jungle,          "정글 정리");
            Dictionary.Add(EnumContext.JungleModeQ,     "[ Q ] 옵션");
            Dictionary.Add(EnumContext.JungleModeE,     "[ E ] 옵션");
            Dictionary.Add(EnumContext.JungleMode0,     "사용안함");
            Dictionary.Add(EnumContext.JungleMode1,     "대형 + 소형");
            Dictionary.Add(EnumContext.JungleMode2,     "대형");
            Dictionary.Add(EnumContext.JungleMode3,     "소형");
            Dictionary.Add(EnumContext.Misc,            "- 기타");
            Dictionary.Add(EnumContext.Esetting,        "[ E ] 설정");
            Dictionary.Add(EnumContext.EKillSteal,      "킬 스틸");
            Dictionary.Add(EnumContext.EJungSteal,      "정글 스틸");
            Dictionary.Add(EnumContext.ECustom,         "[ E ] 공격력 재설정");
            Dictionary.Add(EnumContext.ECustomDmg0,     "[ E ] 공격력을 ");
            Dictionary.Add(EnumContext.ECustomDmg1,     " 줄여서 계산");
            Dictionary.Add(EnumContext.EDeath,          "[ E ] 죽기전에 사용 ");
            Dictionary.Add(EnumContext.EDeathHp1,       "사용자의 체력이 ");
            Dictionary.Add(EnumContext.EDeathHp2,       " 이하일 때");
            Dictionary.Add(EnumContext.Rsetting,        "[ R ] 설정");
            Dictionary.Add(EnumContext.SaveR,           "계약가 보호");
            Dictionary.Add(EnumContext.SaveRHp1,        "계약자의 체력이 ");
            Dictionary.Add(EnumContext.SaveRHp2,        " 이하일 때");
            Dictionary.Add(EnumContext.Bali,            "발리스타 사용");
            Dictionary.Add(EnumContext.BaliDis,         "계약자와의 최소거리");
            Dictionary.Add(EnumContext.Item,            "- 아이템");
            Dictionary.Add(EnumContext.sHextechG,       "마법 공학 총검");           
            Dictionary.Add(EnumContext.sBilge,          "빌지워터의 해적검");
            Dictionary.Add(EnumContext.sBlade,          "몰락한 왕의 검");
            Dictionary.Add(EnumContext.sBKHp1,          "적 체력이 ");
            Dictionary.Add(EnumContext.sBKHp2,          " 이하일때 몰왕 사용");
            Dictionary.Add(EnumContext.sYoumuu,         "요우무의 유령검");
            Dictionary.Add(EnumContext.sQsilver,        "수은 장식띠");
            Dictionary.Add(EnumContext.sScimitar,       "헤르매스의 시미터");
            Dictionary.Add(EnumContext.Delay,           "캐스팅 속도");
            Dictionary.Add(EnumContext.sBuffType,       "버프 타입");
            Dictionary.Add(EnumContext.sPoisons,        "독");
            Dictionary.Add(EnumContext.sSupression,     "제압");
            Dictionary.Add(EnumContext.sBlind,          "실명");
            Dictionary.Add(EnumContext.sCharm,          "매혹");
            Dictionary.Add(EnumContext.sFear,           "공포");
            Dictionary.Add(EnumContext.sPolymorph,      "변형");
            Dictionary.Add(EnumContext.sSilence,        "침묵");
            Dictionary.Add(EnumContext.sSlow,           "둔화");
            Dictionary.Add(EnumContext.sSnare,          "덫");
            Dictionary.Add(EnumContext.sStun,           "기절");
            Dictionary.Add(EnumContext.sKnockup,        "에어본");
            Dictionary.Add(EnumContext.sTaunt,          "도발");
            Dictionary.Add(EnumContext.Draw,            "- 시각 효과");
            Dictionary.Add(EnumContext.DrawQ,           "[ Q ] 범위");
            Dictionary.Add(EnumContext.DrawE,           "[ E ] 범위");
            Dictionary.Add(EnumContext.DrawR,           "[ R ] 범위");
            Dictionary.Add(EnumContext.DrawECDamage,    "[ E / Q ] 공격력 % - 챔피언");
            Dictionary.Add(EnumContext.DrawEJDamage,    "[ E ] 공격력 % - 몬스터");
            Dictionary.Add(EnumContext.DrawPosition,    "% 표시 위치");
            Dictionary.Add(EnumContext.Position0,       "이름 옆에");
            Dictionary.Add(EnumContext.Position1,       "체력바 위에");
            Dictionary.Add(EnumContext.Position2,       "이름 위에");
            Dictionary.Add(EnumContext.Position3,       "화면 가장자리");
            Dictionary.Add(EnumContext.DrawX,           "화면 가장자리 - X");
            Dictionary.Add(EnumContext.DrawY,           "화면 가장자리 - Y");
            Dictionary.Add(EnumContext.UseQ,            "Q 사용");
            Dictionary.Add(EnumContext.UseW,            "W 사용");
            Dictionary.Add(EnumContext.UseE,            "E 사용");
            Dictionary.Add(EnumContext.UseR,            "R 사용");
            Dictionary.Add(EnumContext.ManaStatus1,     "보유한 마나가 ");
            Dictionary.Add(EnumContext.ManaStatus2,     " 이상일 때");
            Dictionary.Add(EnumContext.MinionNum0,      "미니언 킬 개수가 ");
            Dictionary.Add(EnumContext.MinionNum1,      "개 이상일 때");
        }
    }
}
