using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PointCalculator : MonoBehaviour
{
    //플레이어 4인 점수
    private readonly int[] _userPoint = new int[4];

    //점수용 변수
    private readonly TextMeshProUGUI[] _userPointText = new TextMeshProUGUI[4];

    void Start()
    {
        //점수 초기값 설정
        _userPoint[0] = 25000;
        _userPoint[1] = 25000;
        _userPoint[2] = 25000;
        _userPoint[3] = 25000;
        //점수 텍스트 변수 연결
        _userPointText[0] = GameObject.Find("User0Point").GetComponent<TextMeshProUGUI>();
        _userPointText[1] = GameObject.Find("User1Point").GetComponent<TextMeshProUGUI>();
        _userPointText[2] = GameObject.Find("User2Point").GetComponent<TextMeshProUGUI>();
        _userPointText[3] = GameObject.Find("User3Point").GetComponent<TextMeshProUGUI>();
        //점수 설정
        ChangeUserPoint();
    }
    private void ChangeUserPoint()
    {
        _userPointText[0].text = _userPoint[0].ToString();
        _userPointText[1].text = _userPoint[1].ToString();
        _userPointText[2].text = _userPoint[2].ToString();
        _userPointText[3].text = _userPoint[3].ToString();
    }

    public void DoCalc(List<string> hand, List<string> huroTiles, List<string> canHuroTiles, bool isMenzen, string winTile, int howLong, bool isMyTurn, bool isRiichi, int ippatsu)//string waitType, bool didHuro
    {
        
        HandChecker hc = new HandChecker();
        List<string> finishHand = new List<string>();
        finishHand.AddRange(hand);
        finishHand.Add(winTile);
        finishHand = Yaku.HandArranger(finishHand);
        Dictionary<int, List<string>> handState = hc.NowHandState(finishHand, huroTiles);
        int hanBusu = 0;
    
        List<int> point = new List<int>();
        if (handState.TryGetValue(41, out var hs41))
        {
            foreach (string dragons in hs41)
            {
                List<string> handDragons = new List<string>();
                handDragons.AddRange(dragons.Split(","));
                int han = Yaku.GetHan(hand, handDragons, huroTiles, canHuroTiles, isMenzen, winTile, howLong, isMyTurn, isRiichi,ippatsu);
                int busu = Yaku.GetBusu(hand, handDragons, huroTiles, canHuroTiles, isMyTurn, isMenzen);
                hanBusu = han * 1000 + busu;
                point.Add(PointChanger(han, busu));
            }
        }
        PointDisplay.DisplayPoint(point.Max(), hanBusu);
    }
    
    static int PointChanger(int han, int busu)
    {
        int result = 0;
        if (han == 1)
        {
            if (busu == 20) result = 1000;
            else if (busu == 30) result = 1500;
            else if (busu == 40) result = 2000;
            else if (busu == 50) result = 2400;
            else if (busu == 60) result = 2900;
            else if (busu == 70) result = 3400;
            else if (busu == 80) result = 3900;
            else if (busu == 90) result = 4400;
            else if (busu == 100) result = 4800;
            else if (busu == 110) result = 5300;
        }
        else if (han == 2)
        {
            if (busu == 20) result = 2000;
            else if (busu == 25) result = 2400;
            else if (busu == 30) result = 2900;
            else if (busu == 40) result = 3900;
            else if (busu == 50) result = 4800;
            else if (busu == 60) result = 5800;
            else if (busu == 70) result = 6800;
            else if (busu == 80) result = 7700;
            else if (busu == 90) result = 8700;
            else if (busu == 100) result = 9600;
            else if (busu == 110) result = 10600;
        }
        else if (han == 3)
        {
            if (busu == 20) result = 3900;
            else if (busu == 25) result = 4800;
            else if (busu == 30) result = 5800;
            else if (busu == 40) result = 7700;
            else if (busu == 50) result = 9600;
            else if (busu == 60) result = 11600;
            else if (busu >= 70) result = 12000;
        }
        else if (han == 4)
        {
            if (busu == 20) result = 7700;
            else if (busu == 25) result = 9600;
            else if (busu == 30) result = 11600;
            else if (busu >= 40) result = 12000;
        }
        else if (han == 5) result = 12000;
        else if (han == 6 || han == 7) result = 18000;
        else if (han == 8 || han == 9 || han == 10) result = 24000;
        else if (han == 11 || han == 12) result = 36000;
        else if (han >= 13) result = 48000;
    
        return result;
    }
    
    public void PlayerRiichiPointChanger(int user)
    {
        _userPoint[user] -= 1000;
        ChangeUserPoint();
    }
}
