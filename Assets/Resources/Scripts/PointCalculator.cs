using System.Collections.Generic;
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
    public void DoCalc(List<string> hand, List<string> pattern, string waitType, bool didHuro, int someoneRiichi, int howLong)
    {
        
    }
    public void PlayerRiichiPointChanger(int user)
    {
        _userPoint[user] -= 1000;
        ChangeUserPoint();
    }
}
