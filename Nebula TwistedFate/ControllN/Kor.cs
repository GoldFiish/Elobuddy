namespace NebulaTwistedFate.ControllN
{
    class Kor : Controller
    {
        public Kor()
        {
            Dictionary.Add(EnumContext.SelectLanguage,  "언어 선택 후 F5를 눌러주세요.");
            Dictionary.Add(EnumContext.Text_1,          "이 애드온이 마음에 드신다면, 추천 또는 페이팔 기부를 부탁드리며,");
            Dictionary.Add(EnumContext.Text_2,          "기부하신 금액은 애드온 제작에 사용됩니다. 감사합니다.");
           
            Dictionary.Add(EnumContext.Combo,           "- 콤보");
            Dictionary.Add(EnumContext.ComboQMode,      "[ Q ] 옵션");
            Dictionary.Add(EnumContext.ComboQMode0,     "아무때나");
            Dictionary.Add(EnumContext.ComboQMode1,     "스턴 상태");
            Dictionary.Add(EnumContext.EnemyNum0,       "빨강 카드 - 타켓 주위에 적이 ");
            Dictionary.Add(EnumContext.EnemyNum1,       " 명 이상");

            Dictionary.Add(EnumContext.Harass,          "- 견제");
            
            Dictionary.Add(EnumContext.Clear,           "- 라인 / 정글");
            Dictionary.Add(EnumContext.LaneClear,       "라인");
            Dictionary.Add(EnumContext.LaneMode,        "[ Q ] 옵션");
            Dictionary.Add(EnumContext.LaneMode0,       "자동");
            Dictionary.Add(EnumContext.LaneMode1,       "빠르게 밀기");
            Dictionary.Add(EnumContext.LaneMode2,       "미니언 킬");
            Dictionary.Add(EnumContext.MinionsNum,      "미니언 개수 - ");
            Dictionary.Add(EnumContext.LaneCardRed,     "빨강 카드 사용");
            Dictionary.Add(EnumContext.MinionsKNum,     "미니언 킬 개수 - ");
            Dictionary.Add(EnumContext.LaneCardBlue,    "파랑 카드 사용");
            
            Dictionary.Add(EnumContext.JungleClear,     "정글");

            Dictionary.Add(EnumContext.Item,            "- 아이템");
            Dictionary.Add(EnumContext.ItemExp,         "자동 - 빌지워터, 요우무, 마법공학 총검");
            Dictionary.Add(EnumContext.sBlade,          "몰락한 왕의 검");
            Dictionary.Add(EnumContext.sBKHp1,          "적 체력이 ");
            Dictionary.Add(EnumContext.sBKHp2,          " 이하일때 몰왕 사용");
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
            Dictionary.Add(EnumContext.sZhonya,         "존야의 모래시계");
            Dictionary.Add(EnumContext.sZhonyaBHp,      "사용자의 체력이 ");           
            Dictionary.Add(EnumContext.sZhonyaBDmg,     "순간 평타 데미지가 ");
            Dictionary.Add(EnumContext.sZhonyaSHp,      "사용자의 체력이 ");
            Dictionary.Add(EnumContext.sZhonyaSDmg,     "순간 스킬 데미지가 ");
            Dictionary.Add(EnumContext.sZhonyaR,        "위험한 궁극기");

            Dictionary.Add(EnumContext.Msic,            "- 기타");
            Dictionary.Add(EnumContext.AutoIgnite,      "자동 점화 사용");
            Dictionary.Add(EnumContext.KillSteal,       "킬 스틸 사용");
            Dictionary.Add(EnumContext.JungSteal,       "정글 스틸 사용");
            Dictionary.Add(EnumContext.AutoQ,           "자동 [ Q ] - 스턴, 속박, 제압, 매혹, 귀환");
            Dictionary.Add(EnumContext.AutoPickYel,     "궁극기 사용시 자동 픽 - 노랑 카드");
            Dictionary.Add(EnumContext.PickDelay,       "카드 픽 딜레이");
            Dictionary.Add(EnumContext.DisAA,           "카드 선택 시 자동 공격 금지");
            Dictionary.Add(EnumContext.InterruptLv,     "위험도");
            Dictionary.Add(EnumContext.InterruptLv1,    "보통");
            Dictionary.Add(EnumContext.InterruptLv2,    "높음");

            Dictionary.Add(EnumContext.Draw,            "- 시각효과");
            Dictionary.Add(EnumContext.DrawQ,           "Q 범위");
            Dictionary.Add(EnumContext.DrawW,           "W 범위");
            Dictionary.Add(EnumContext.DrawR,           "R 범위");
            Dictionary.Add(EnumContext.DrawText,        "킬각 및 데미지 %");
            Dictionary.Add(EnumContext.SpellQ,          "Q 사용");
            Dictionary.Add(EnumContext.SpellW,          "W 사용");
            Dictionary.Add(EnumContext.SpellR,          "R 사용");

            Dictionary.Add(EnumContext.QPrediction,     "정확도");
            Dictionary.Add(EnumContext.CardMode,        "[ W ] 옵션");
            Dictionary.Add(EnumContext.CardMode0,       "자동");
            Dictionary.Add(EnumContext.CardMode1,       "빨강 카드");
            Dictionary.Add(EnumContext.CardMode2,       "노랑 카드");

            Dictionary.Add(EnumContext.ManaStatus,      "보유한 마나가 ");
            Dictionary.Add(EnumContext.BlueStatus,      "이하일 때 파랑카드 사용 ");

            Dictionary.Add(EnumContext.sMore,           " 이상일 때");
            Dictionary.Add(EnumContext.sLow,            " 이하일 때");
        }
    }
}