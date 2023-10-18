using System.Collections.Generic;
using UnityEngine;
public class GamePlay : MonoBehaviour
{
    private HandChecker _handChecker = new HandChecker();
    private Tiles tiles = new Tiles();
    private List<string> dora = new List<string>();
    private TileDisplay _tileDisplay;
    private GameDirector _gameDirector;
    private bool oyaWin = false;
    private bool gameEnd = false;
    private int oya;//gamestate로 옮기기
    private int nowWind;//gamestate로 옮기기
    private int nowPlayer;
    private List<Player> players = new List<Player>();
    private List<List<string>> hands = new List<List<string>>();
    private List<int> discardNums = new List<int>();

    void Start()
    {
        _tileDisplay = GameObject.Find("TileDisplay").GetComponent<TileDisplay>();
        _gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        players = _gameDirector.getPlayers();
        
        oya = _gameDirector.getOya();
        nowWind = _gameDirector.getNowWind();
        nowPlayer = oya;
        dora = tiles.drawDora();//도라
        makeFirstDora();
        
        //손패
        //makeTestHand();
        for (int i = 0; i < 4; ++i)//0으로 바꿔야함
        {
            hands.Add(tiles.getFirstHand());
            hands[i] = HandArrange(hands[i]);//손패 정리
            discardNums.Add(-1);
            _tileDisplay.handDisplay(hands[i], i);
        }
    }
    
    void Update()
    {
        //손패를 받았을때 14개의 패로 승리 가능인지 확인?
        //
        //if(_gameState.isGameEnd()) Debug.Log("end");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string tile = tiles.tsumo();//타일에서 뽑기
            hands[nowPlayer].Add(tile);//손패에 추가
            hands[nowPlayer] = HandArrange(hands[nowPlayer]);//손패 정리
            _tileDisplay.tsumoDisplay(tile, nowPlayer);//츠모한거 오브젝트 생성

            
            //플레이어 0번 아니면 바로 타패
            if (nowPlayer != 0)
            {
                dahai(tile, nowPlayer);
            }
            
            //커츠인거 확인하는 테스트 코드
            string tripletStr = "";
            List<string> triplet = _handChecker.WhatIsTriplet(hands[nowPlayer]);
            foreach (var t in triplet)
            {
                tripletStr += t + ", ";
            }
            Debug.Log("triplets : " + tripletStr);
            //또이츠인거 확인하는 테스트 코드
            string pairStr = "";
            List<string> pair = _handChecker.WhatIsPair(hands[nowPlayer]);
            foreach (var p in pair)
            {
                pairStr += p + ", ";
            }
            Debug.Log("pairs : " + pairStr);
            //슌츠인거 확인하는 테스트 코드
            string sequenceStr = "";
            List<string> sequence = _handChecker.WhatIsSequence(hands[nowPlayer]);
            foreach (var s in sequence)
            {
                sequenceStr += s + ", ";
            }
            Debug.Log("sequence : " + sequenceStr);
            
            ++nowPlayer;
            nowPlayer %= 4;
        }
    }
    private void makeFirstDora()
    {
        _tileDisplay.doraDisplay(dora[0], 0, true);
        _tileDisplay.doraDisplay(dora[1], 1, false);
        _tileDisplay.doraDisplay(dora[2], 2, false);
        _tileDisplay.doraDisplay(dora[3], 3, false);
        _tileDisplay.doraDisplay(dora[4], 4, false);
        
    }
    private void makeKanDora(int num)
    {
        _tileDisplay.doraDisplay(dora[num], num, true);
        
    }
    //위에 핸드 추가하는 함수에 for문 1부터 시작으로 바꿔야함
    private void makeTestHand()
    {
        List<string> testhand = new List<string>();
        testhand.Add("m5r");
        testhand.Add("m5");
        testhand.Add("m5");
        testhand.Add("m1");
        testhand.Add("m2");
        testhand.Add("m3");
        testhand.Add("m2");
        testhand.Add("s1");
        testhand.Add("s2");
        testhand.Add("s4");
        testhand.Add("p3");
        testhand.Add("m7");
        testhand.Add("m4");
        hands.Add(testhand);
        hands[0] = HandArrange(hands[0]);//손패 정리
        discardNums.Add(-1);
        _tileDisplay.handDisplay(hands[0], 0);
    }
    public void dahai(string _tileName, int _user)
    {
        //뒷면엔 클릭할때 이루어지는 상호작용이 없어서 안버려짐
        hands[_user].RemoveAt(hands[_user].IndexOf(_tileName));
        _tileDisplay.dahaiDisplay(_tileName, _user, ++discardNums[_user]);
        hands[_user] = HandArrange(hands[_user]);//손패 정리
        _tileDisplay.handDisplay(hands[_user], _user);
    }
    
    
    //게임 끝났을때 점수 계산
    public void GameEnd()
    {
        gameEnd = true;
    }

    public bool isOyaWin()
    {
        return oyaWin;
    }
    public bool isGameEnd()
    {
        return gameEnd;
    }
    //핸드 위치 변경하는 함수
    private static List<string> HandArrange(List<string> hand)
    {
        List<string> sortSequence = new List<string>()
        {
            "m1", "m2", "m3", "m4", "m5", "m5r", "m6", "m7", "m8", "m9",
            "p1", "p2", "p3", "p4", "p5", "p5r", "p6", "p7", "p8", "p9",
            "s1", "s2", "s3", "s4", "s5", "s5r", "s6", "s7", "s8", "s9",
            "e", "s", "w", "n", "p", "f", "c"
        };
        List<string> tmp = new List<string>();
        foreach (string searchValue in sortSequence)
        {
            List<string> matchingValues = hand.FindAll(item => item == searchValue);
            foreach (string tile in matchingValues)
            {
                tmp.Add(tile);
            }
        }
        return tmp;
    }
    public void DoChi()
    {
        
    }
    public void DoPong()
    {
        
    }
    //전체 핸드 로그로 확인하는 함수
    void handCheck(List<string> _hand)
    {
        string tileToPrint = "";
        foreach (string tile in _hand)
        {
            tileToPrint += tile + ", ";
        }
        Debug.Log(tileToPrint);
    }
}
