using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
public class GamePlay : MonoBehaviour
{
    private HandChecker _handChecker = new HandChecker();
    private Tiles _tiles = new Tiles();
    private Yaku _yaku = new Yaku();
    private List<string> _dora = new List<string>();
    private TileDisplay _tileDisplay;
    private GameDirector _gameDirector;
    private PointCalculator _pointCalculator;
    private TextMeshProUGUI _nowTime;
    private ButtonController _buttonController;
    private TextMeshProUGUI _tileLeft;
    
    private bool _oyaWin = false;
    private bool _winGame = false;
    private bool _gameEnd = false;//gamestate로 옮기기
    private int _nowWind;//gamestate로 옮기기
    private bool[] _isRiichi;//gamestate로 옮기기
    private int _nowPlayer;
    
    private List<Player> _players = new List<Player>();
    private List<List<string>> _hands = new List<List<string>>();
    private readonly List<int> _discardNums = new List<int>();
    
    private bool _isMyTurn;
    private int _userTime;
    private const int WaitTime = 5;
    private bool _isTurnReady = false;
    private string _nowTsumoTile;
    private bool _isFirstTurn = true;
    
    private List<List<string>> _canChiList = new List<List<string>>();
    private List<List<string>> _canPongList = new List<List<string>>();
    private List<List<string>> _canKanList = new List<List<string>>();

    private void Start()
    {
        _tileDisplay = GameObject.Find("TileDisplay").GetComponent<TileDisplay>();
        _gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        _pointCalculator = GameObject.Find("PointCalculator").GetComponent<PointCalculator>();
        _nowTime = GameObject.Find("UserTime").GetComponent<TextMeshProUGUI>();
        _nowTime.text = "";
        _buttonController = GameObject.Find("ButtonController").GetComponent<ButtonController>();
        _tileLeft = GameObject.Find("TileLeft").GetComponent<TextMeshProUGUI>();
        _players = _gameDirector.GetPlayers();
        _nowWind = _gameDirector.GetNowWind();
        _nowPlayer = _gameDirector.GetOya();
        _isMyTurn = _nowPlayer == 0;
        
        _dora = _tiles.DrawDora();//도라 뽑기
        MakeFirstDoraDisplay();//도라 표시
        MakeFirstHand();//첫 손패 주기
        UpdateTileLeft();//남은 타일 갯수 표시
        //천화 지화 확인하기
        MakeCanHuroList();
        _buttonController.AllBtnDeactivate();
        
        StartCoroutine(MyUpdateCrt());//게임 시작
    }

    private IEnumerator MyUpdateCrt()
    {
        while (!_winGame)
        {
            yield return new WaitForSeconds(1f);
            if (_isFirstTurn) _isFirstTurn = false;
            else MyUpdate();
        }
        ReadyCalc();
    }
    private void MyUpdate()
    {
        // TODO: 리치했을때 츠모기리 하는걸로 만들기
        //손패를 받았을때 14개의 패로 승리 가능인지 확인?
        if(_isTurnReady)
        {
            if(_isMyTurn)
            {
                _nowTime.text = _userTime.ToString();
                
                //시간 끝나면 츠모한거 버리기
                if(_userTime < 1)
                {
                    Dahai(_nowTsumoTile, _nowPlayer);
                }
                else
                {
                    --_userTime;
                }
            }
            //내 턴 아닐때 실행
            else
            {
                _nowTime.text = "";
                StartCoroutine(NotMyTurnDahai());
            }
        }
        else
        {
            if (_tiles.GetTileLeft() <= 0)
            {
                _gameEnd = true;
            }
            else
            {
                _nowTsumoTile = _tiles.Tsumo();//타일에서 뽑기
                _hands[_nowPlayer].Add(_nowTsumoTile);//손패에 추가
                _hands[_nowPlayer] = HandArrange(_hands[_nowPlayer]);//손패 정리
                UpdateTileLeft();//남은 타일 갯수 표시
                TileDisplay.TsumoDisplay(_nowTsumoTile, _nowPlayer);//츠모한거 오브젝트 생성
            
            
            
            
                HandCheck(_hands[_nowPlayer]);//손패 확인
                Dictionary<int, List<string>> handState = _handChecker.NowHandState(_hands[_nowPlayer]);
            
                Debug.Log(_handChecker.CanRiichi(_hands[_nowPlayer], handState) ? "yes" : "no");
                if (_handChecker.IsKokushiMusouWait(_hands[_nowPlayer]))
                {
                    Debug.Log("IsKokushiMusouWait yes");
                    _buttonController.RiichiBtnActivate();
                }
                Debug.Log(_yaku.IsKokushiMusou(_hands[_nowPlayer]) ? "kkms yes" : "kkms no");
            
            
            
            
            
                _canChiList[_nowPlayer] = _handChecker.MakeCanChiList(_hands[_nowPlayer]);
                _canPongList[_nowPlayer] = _handChecker.MakeCanPongList(_hands[_nowPlayer]);
                _canKanList[_nowPlayer] = _handChecker.MakeCanKanList(_hands[_nowPlayer]);
                if(_canChiList[_nowPlayer].Contains(_nowTsumoTile)) _buttonController.ChiBtnActivate();
                if(_canPongList[_nowPlayer].Contains(_nowTsumoTile)) _buttonController.PongBtnActivate();
                if(_canKanList[_nowPlayer].Contains(_nowTsumoTile)) _buttonController.KanBtnActivate();


                _buttonController.RonBtnActivate();
            
            
                _userTime = WaitTime;
                _isTurnReady = true;
                _nowTime.text = _isMyTurn ? _userTime.ToString() : "";
                if(!_isMyTurn) _buttonController.AllBtnDeactivate();
            }
        }
    }

    public void DoChi()
    {
        
    }
    public void DoPong()
    {
        
    }
    private void MakeCanHuroList()
    {
        for (int i = 0; i < 4; i++)
        {
            _canChiList.Add(_handChecker.MakeCanChiList(_hands[i]));
            _canPongList.Add(_handChecker.MakeCanPongList(_hands[i]));
            _canKanList.Add(_handChecker.MakeCanKanList(_hands[i]));
        }
        
    }

    private void DoRiichi(string tile, int user)
    {
        
    }
    private void MakeFirstDoraDisplay()
    {
        TileDisplay.DoraDisplay(_dora[0], 0, true);
        TileDisplay.DoraDisplay(_dora[1], 1, false);
        TileDisplay.DoraDisplay(_dora[2], 2, false);
        TileDisplay.DoraDisplay(_dora[3], 3, false);
        TileDisplay.DoraDisplay(_dora[4], 4, false);
    }
    private void MakeKanDoraDisplay(int num)
    {
        TileDisplay.DoraDisplay(_dora[num], num, true);
    }

    private void ReadyCalc()
    {
        
        //DoCalc(List<string> hand, List<string> pattern, string waitType, bool didHuro, int someoneRiichi, int howLong)
        //_pointCalculator.DoCalc(_hands[_winUser]);
    }
    public void UserWin()
    {
        _winGame = true;
    }
    
    //게임 끝났을때 점수 계산
    public void GameEnd()
    {
        _gameEnd = true;
    }


    public bool IsOyaWin()
    {
        return _oyaWin;
    }
    public bool IsGameEnd()
    {
        return _gameEnd;
    }

    private void UpdateTileLeft()
    {
        _tileLeft.text = "남은 패 : " + _tiles.GetTileLeft();
    }

    public bool GetIsMyTurn()
    {
        return _isMyTurn;
    }
    public void Dahai(string tileName, int user)
    {
        //뒷면엔 클릭할때 이루어지는 상호작용이 없어서 안버려짐
        _hands[user].RemoveAt(_hands[user].IndexOf(tileName));
        TileDisplay.DahaiDisplay(tileName, user, ++_discardNums[user], false);
        _hands[user] = HandArrange(_hands[user]);//손패 정리
        TileDisplay.HandDisplay(_hands[user], user);

        ++_nowPlayer;
        _nowPlayer %= 4;
        _isMyTurn = _nowPlayer == 0;
        _nowTime.text = "";
        _isTurnReady = false;
    }
    //핸드 위치 변경하는 함수
    private static List<string> HandArrange(List<string> hand)
    {
        List<string> sortSequence = new List<string>
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
            tmp.AddRange(matchingValues);
        }
        return tmp;
    }
    private IEnumerator NotMyTurnDahai()
    {
        yield return new WaitForSeconds(0.01f);
        Dahai(_nowTsumoTile, _nowPlayer);
    }
    private void MakeFirstHand()
    {
        //손패 만들기
        for (int i = 0; i < 4; ++i)
        {
            _hands.Add(_tiles.GetFirstHand());
            _hands[i] = HandArrange(_hands[i]);//손패 정리
            _discardNums.Add(-1);
            TileDisplay.HandDisplay(_hands[i], i);
        }

        _hands[0] = new List<string>
        {
            "m1", "m9", "p1", "p9", "s1", "s9", "e", "s", "w", "n", "p", "f", "c"
        };
    }
    //위에 핸드 추가하는 함수에 for문 1부터 시작으로 바꿔야함
    private void MakeTestHand()
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
        _hands.Add(testhand);
        _hands[0] = HandArrange(_hands[0]);//손패 정리
        _discardNums.Add(-1);
        TileDisplay.HandDisplay(_hands[0], 0);
    }
    //전체 핸드 로그로 확인하는 함수
    private void HandCheck(List<string> hand)
    {
        Debug.Log(hand.Aggregate("", (current, tile) => current + ("\"" + tile + "\", ")));
    }
}

// //커츠인거 확인하는 테스트 코드
// string tripletStr = "";
// List<string> triplet = _handChecker.WhatIsTriplet(hands[nowPlayer]);
// foreach (var t in triplet)
// {
//     tripletStr += t + ", ";
// }
// Debug.Log("triplets : " + tripletStr);
// //또이츠인거 확인하는 테스트 코드
// string pairStr = "";
// List<string> pair = _handChecker.WhatIsPair(hands[nowPlayer]);
// foreach (var p in pair)
// {
//     pairStr += p + ", ";
// }
// Debug.Log("pairs : " + pairStr);
// //슌츠인거 확인하는 테스트 코드
// string sequenceStr = "";
// List<string> sequence = _handChecker.WhatIsSequence(hands[nowPlayer]);
// foreach (var s in sequence)
// {
//     sequenceStr += s + ", ";
// }
// Debug.Log("sequence : " + sequenceStr);