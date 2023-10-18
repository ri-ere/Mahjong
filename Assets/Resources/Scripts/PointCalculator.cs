using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointCalculator : MonoBehaviour
{
    //플레이어 4인 점수
    private int[] userPoint = new Int[];

    private TextMeshProUGUI[] userPointText;
    //점수용 변수
    public TextMeshProUGUI user0PointText;
    public TextMeshProUGUI user1PointText;
    public TextMeshProUGUI user2PointText;
    public TextMeshProUGUI user3PointText;

    void Start()
    {
        //점수 초기값 설정
        user0Point = 25000;
        user1Point = 25000;
        user2Point = 25000;
        user3Point = 25000;
        //점수 텍스트 변수 연결
        user0PointText = GameObject.Find("User0Point").GetComponent<TextMeshProUGUI>();
        user1PointText = GameObject.Find("User1Point").GetComponent<TextMeshProUGUI>();
        user2PointText = GameObject.Find("User2Point").GetComponent<TextMeshProUGUI>();
        user3PointText = GameObject.Find("User3Point").GetComponent<TextMeshProUGUI>();
        //점수 설정
        user0PointText.text = user0Point.ToString();
        user1PointText.text = user1Point.ToString();
        user2PointText.text = user2Point.ToString();
        user3PointText.text = user3Point.ToString();
    }
    private void ChangeUserPoint()
    {
        user0PointText.text = user0Point.ToString();
        user1PointText.text = user1Point.ToString();
        user2PointText.text = user2Point.ToString();
        user3PointText.text = user3Point.ToString();
    }

    public void UserWin(int user)
    {
        string userName = "user" + user + "name";
        
    }
}
