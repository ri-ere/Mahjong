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
    private Huro _huro = new Huro();
    private List<string> _dora = new List<string>();
    // private TileDisplay _tileDisplay;
    private GameDirector _gameDirector;
    private PointCalculator _pointCalculator;
    private TextMeshProUGUI _nowTime;
    private ButtonController _buttonController;
    private TextMeshProUGUI _tileLeft;
    
    private bool _oyaWin = false;
    private bool _winGame = false;
    private bool _gameEnd = false;//gamestate로 옮기기
    private int _nowWind;//gamestate로 옮기기
    private List<bool> _isRiichi = new List<bool>();//gamestate로 옮기기
    private int _nowPlayer;
    private int _playerNum = 0;
    
    private List<Player> _players = new List<Player>();
    private List<List<string>> _hands = new List<List<string>>();
    private List<List<string>> _canHuroTiles = new List<List<string>>();
    private List<List<string>> _huroTiles = new List<List<string>>();
    private List<int> _discardNums = new List<int>();
    private List<List<string>> _discardTiles = new List<List<string>>();
    private bool _canHuro = false;
    private bool _huroBtnOn = false;
    private bool _ronBtnOn = false;
    private List<string> chiList;
    private List<string> _riichiList;
    private bool _doneHuro = true;
    private bool _haveToDoChi = false;
    private bool _haveToDoKan = false;
    private bool _nowRiichi = false;
    
    private bool _isMyTurn;
    private int _userTime;
    private const int WaitTime = 300;
    private const int HuroTime = 5;
    private int _huroTime = HuroTime;
    private bool _isTurnReady = false;
    private string _nowTsumoTile;
    private bool _isFirstTurn = true;
    private string _nowDahaiTile = "";
    
    private void Start()
    {
        // _tileDisplay = GameObject.Find("TileDisplay").GetComponent<TileDisplay>();
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
        MakeFirstCanHuroList();//후로 가능 타일 체크
        _buttonController.AllBtnDeactivate();
        
        //후로 디스플레이 테스트용
        //TileDisplay.HuroDisplay(new List<string>(){"t-ee-ee1"});
        
        
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
        if(!_canHuro)
        {
            if(_isTurnReady)
            {
                if(_isMyTurn)
                {
                    _nowTime.text = _userTime.ToString();
                    
                    //시간 끝나면 츠모한거 버리기
                    if(_userTime < 1)
                    {
                        if (!_doneHuro)
                        {
                            FinishHuro();
                        }
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
                    _userTime = 0;
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
                    HandCheck(_hands[_nowPlayer], "hand");
                    UpdateTileLeft();//남은 타일 갯수 표시
                    TileDisplay.TsumoDisplay(_nowTsumoTile, _nowPlayer, _huroTiles[_nowPlayer].Count);//츠모한거 오브젝트 생성
                    _userTime = WaitTime;
                    _isTurnReady = true;
                    _buttonController.AllBtnDeactivate();
                    
                    if (_isRiichi[_nowPlayer]) _canHuroTiles[_nowPlayer] = _handChecker.FindRiichiRon(_hands[_nowPlayer]);
                    else _canHuroTiles[_nowPlayer] = _huro.MyTurnCanHuroList(_hands[_nowPlayer], _canHuroTiles[_nowPlayer]);
                    if (_isMyTurn)//내턴일때 후로 가능 타일 확인 츠모승리랑 리치도 확인
                    {
                        if (_isRiichi[0])//리치 했을때
                        {
                            RiichiTsumoTagChanger();//리치하고 다른거 버리는거 방지용 츠모한거 말고는 태그 변경
                        }
                        else
                        {
                            _nowTime.text = _userTime.ToString();
                        }
                        if (_handChecker.CanWin(_hands[0], _huroTiles[0], _isRiichi[0]))//승리 가능이면
                        {
                            _buttonController.RonBtnActivate();
                        }

                        if (_handChecker.FindRiichiDiscard(_hands[0]).Any() && !_huroTiles[0].Any() && !_isRiichi[0])
                        {
                            _buttonController.RiichiBtnActivate();
                        }
                        if (_huro.MakeCanShouminKanList(_canHuroTiles[_nowPlayer]).Count != 0)
                        {
                            _buttonController.KanBtnActivate();
                        }
                        if (_huro.MakeCanAnKanList(_hands[_nowPlayer]).Count != 0)
                        {
                            _buttonController.KanBtnActivate();
                        }
                    }
                    else//내턴 아닐때
                    {
                        _nowTime.text = "";
                    }
                }
            }
        }
        else//내 턴 아닐때 후로 가능한 경우에 실행
        {
            if (_nowPlayer == 1)//내가 버린 타일이면 끝내기, 리치여도 끝내기
            {
                _canHuro = false;
            }
            else if (_isRiichi[0] && !_ronBtnOn)
            {
                bool isOn = false;
                if (_canHuroTiles[0].Contains(_nowDahaiTile))
                {
                    _buttonController.RonBtnActivate();
                    isOn = true;
                }
                _ronBtnOn = true;
                _huroBtnOn = true;
                if (isOn)//후로 가능
                {
                    _huroTime = HuroTime;
                    _huroBtnOn = true;
                    _nowTime.text = _huroTime.ToString();
                }
                else
                {
                    _ronBtnOn = false;
                    _huroBtnOn = false;
                    _canHuro = false;
                }
            }
            else
            {
                //퐁캉이면 true
                if (!_huroBtnOn)
                {
                    Debug.Log("button check");
                    bool isOn = false;
                    if (Huro.MakeCanChiList(_hands[0]).Contains(_nowDahaiTile) && _isMyTurn)
                    {
                        _buttonController.ChiBtnActivate();
                        isOn = true;
                    }

                    if (_huro.MakeCanPongList(_hands[0]).Contains(_nowDahaiTile))
                    {
                        _buttonController.PongBtnActivate();
                        isOn = true;
                    }

                    if (_huro.MakeCanDaiminKanList(_hands[0]).Contains(_nowDahaiTile))
                    {
                        _buttonController.KanBtnActivate();
                        isOn = true;
                    }

                    if (_huro.MakeCanRonList(_hands[0]).Contains(_nowDahaiTile))
                    {
                        _buttonController.RonBtnActivate();
                        isOn = true;
                    }
                    _huroBtnOn = true;
                    if (isOn)//후로 가능
                    {
                        _huroTime = HuroTime;
                        _huroBtnOn = true;
                        _nowTime.text = _huroTime.ToString();
                    }
                    else
                    {
                        _huroBtnOn = false;
                        _canHuro = false;
                    }
                }
                else
                {
                    _nowTime.text = _huroTime.ToString();
                    if (_huroTime < 1)
                    {
                        _canHuro = false;
                        _huroBtnOn = false;
                        _ronBtnOn = false;
                        _nowTime.text = "";
                    }
                    else
                    {
                        --_huroTime;
                    }
                }
            }
        }
    }

    private void MakeFirstCanHuroList()
    {
        for (int i = 0; i < 4; i++)
        {
            _huroTiles.Add(new List<string>());
            if (_nowPlayer == i)
                _canHuroTiles.Add(_huro.MyTurnCanHuroList(_hands[i], new List<string>()));
            else _canHuroTiles.Add(_huro.NotMyTurnCanHuroList(_hands[i]));
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void FinishHuro()
    {
        if (_haveToDoChi)
        {
            OnClickedWantToChi(0);
            _doneHuro = true;
            _haveToDoChi = false;
        }

        if (_haveToDoKan)
        {
            
        }
    }

    public void RiichiTsumoTagChanger()
    {
        GameObject userHand = GameObject.Find("User0Hand");
        foreach (var child in userHand.GetComponentsInChildren<Transform>())
        {
            string[] tiles = child.name.Split("(");
            if (!tiles[0].Equals(_nowTsumoTile)) child.tag = "NotRiichi";
        }
    }
    public void OnClickedRiichiButton()
    {
        _buttonController.AllBtnDeactivate();
        _pointCalculator.PlayerRiichiPointChanger(0);
        _isRiichi[0] = true;
        _nowRiichi = true;
        GameObject userHand = GameObject.Find("User0Hand");
        _riichiList = _handChecker.FindRiichiDiscard(_hands[0]);
        //확인용 코드
        Debug.Log(_riichiList.Aggregate("riichi list : ", (current, tile) => current + ("\"" + tile + "\", ")));
        
        foreach (var child in userHand.GetComponentsInChildren<Transform>())
        {
            string[] tiles = child.name.Split("(");
            if (_riichiList.Contains(tiles[0]))
            {
                Vector3 currentPosition = child.transform.position;
                currentPosition.y += 0.09f;
                child.transform.position = currentPosition;
            }
            else
            {
                child.tag = "NotRiichi";
            }
        }
    }
    public void OnClickedRonButton()
    {
        _buttonController.AllBtnDeactivate();


        _winGame = true;//코루틴 돌아가는거 멈추는 용도
        _isFirstTurn = true;//대기하고 있는 코루틴 업데이트 못하게 하는 용도
        _nowTime.text = "";

        if (_userTime > 1)
        {
            _pointCalculator.DoCalc(_hands[0], _huroTiles[0], _nowTsumoTile, _tiles.GetTileLeft(), true);
        }
        else
        {
            _pointCalculator.DoCalc(_hands[0], _huroTiles[0], _nowDahaiTile, _tiles.GetTileLeft(), false);
        }
        
    }
    public void OnClickedChiButton()
    {
        _buttonController.AllBtnDeactivate();
        int tileTakenPlayer = 3;//타일 뺏긴 유저
        --_discardNums[tileTakenPlayer];
        TileDisplay.RemoveStolenTile(tileTakenPlayer);
        //가져왔을때 내 손 타일 재생성
        chiList = Huro.WhatToChi(_hands[0], _nowDahaiTile);
        if (chiList.Count == 1)
        {
            OnClickedWantToChi(0);
        }
        else
        {
            _doneHuro = false;
            _haveToDoChi = true;
            TileDisplay.WhatToChiDisplay(chiList);
        }
        
        _huroBtnOn = false;
        _canHuro = false;
        _isMyTurn = true;
        _isTurnReady = true;
        _nowPlayer = 0;
    }

    public void OnClickedWantToChi(int chiNum)
    {
        GameObject userChiPoss = GameObject.Find("User0ChiPoss");
        // 치 고르는거용 만들어놓은거 제거
        if (userChiPoss.transform.childCount > 0)
        {
            for (int i = 0; i < userChiPoss.transform.childCount; i++)
            {
                Transform child = userChiPoss.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        string huroTiles = "s-" + chiList[chiNum] + "-" + _nowDahaiTile + "3";
        List<string> listTiles = new List<string> { chiList[chiNum][..2], chiList[chiNum][2..4], chiList[chiNum][4..6]};
        if (listTiles[0].Equals(_nowDahaiTile))
        {
            _hands[0].RemoveAt(_hands[0].IndexOf(listTiles[1]));
            _hands[0].RemoveAt(_hands[0].IndexOf(listTiles[2]));
        }
        else if (listTiles[1].Equals(_nowDahaiTile))
        {
            _hands[0].RemoveAt(_hands[0].IndexOf(listTiles[0]));
            _hands[0].RemoveAt(_hands[0].IndexOf(listTiles[2]));
        }
        else if (listTiles[2].Equals(_nowDahaiTile))
        {
            _hands[0].RemoveAt(_hands[0].IndexOf(listTiles[0]));
            _hands[0].RemoveAt(_hands[0].IndexOf(listTiles[1]));
        }
        _hands[0] = HandArrange(_hands[0]);//손패 정리
        TileDisplay.HandDisplay(_hands[0], 0);
        _huroTiles[0].Add(huroTiles);
        if(_huroTiles[0].Count != 0) TileDisplay.HuroDisplay(_huroTiles[0]);

        _doneHuro = true;
    }
    public void OnClickedPongButton()
    {
        _buttonController.AllBtnDeactivate();
        //타일 없어질 유저 타일 제거
        int tileTakenPlayer;
        if (_nowPlayer == 0) tileTakenPlayer = 3;
        else tileTakenPlayer = _nowPlayer - 1;
        --_discardNums[tileTakenPlayer];
        TileDisplay.RemoveStolenTile(tileTakenPlayer);
        
        //가져왔을때 내 손 타일 재생성
        string huroTiles;
        if(_nowDahaiTile.Length > 1) huroTiles = "t-" + _nowDahaiTile + "-" + _nowDahaiTile + tileTakenPlayer;
        else huroTiles = "t-" + _nowDahaiTile + _nowDahaiTile + "-" + _nowDahaiTile + _nowDahaiTile + tileTakenPlayer;
        
        //퐁한 타일 2개 손패에서 제거
        _hands[0].RemoveAt(_hands[0].IndexOf(_nowDahaiTile));
        _hands[0].RemoveAt(_hands[0].IndexOf(_nowDahaiTile));
        _hands[0] = HandArrange(_hands[0]);//손패 정리
        TileDisplay.HandDisplay(_hands[0], 0);
        _huroTiles[0].Add(huroTiles);
        if(_huroTiles[0].Count != 0) TileDisplay.HuroDisplay(_huroTiles[0]);


        _huroBtnOn = false;
        _canHuro = false;
        _isMyTurn = true;
        _isTurnReady = true;
        _nowPlayer = 0;
    }
    public void OnClickedKanButton()
    {
        
    }

    public void OnClickedPassButton()
    {
        _canHuro = false;
        _huroBtnOn = false;
        _ronBtnOn = false;
        _nowTime.text = "";
    }
    private void ReadyCalc()//게임 끝났을때 사용
    {
        // huroHand.Count 했을때 0이 아니면 didHuro true 하면 될듯
        //DoCalc(List<string> hand, List<string> pattern, string waitType, bool didHuro, int someoneRiichi, int howLong)
        //_pointCalculator.DoCalc(_hands[_winUser]);
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
    
    public void Dahai(string tileName, int user)
    {
        //뒷면엔 클릭할때 이루어지는 상호작용이 없어서 안버려짐
        _nowDahaiTile = tileName;//후로용 변수
        Debug.Log("dahai : " + _nowDahaiTile);
        _hands[user].RemoveAt(_hands[user].IndexOf(tileName));//손패에서 타일 제거
        _discardTiles[_nowPlayer].Add(tileName);//버린 타일에 추가
        TileDisplay.DahaiDisplay(tileName, user, ++_discardNums[user], _nowRiichi);
        if (_nowRiichi) _nowRiichi = false;//리치한 패 돌리는 용도
        _hands[user] = HandArrange(_hands[user]);//손패 정리
        TileDisplay.HandDisplay(_hands[user], user);

        if (_isRiichi[_nowPlayer]) _canHuroTiles[_nowPlayer] = _handChecker.FindRiichiRon(_hands[_nowPlayer]);
        else _canHuroTiles[_nowPlayer] = _huro.NotMyTurnCanHuroList(_hands[_nowPlayer]);
        Debug.Log(_canHuroTiles[_nowPlayer].Aggregate(_nowPlayer + "can huro List : ", (current, tile) => current + ("\"" + tile + "\", ")));
        
        ++_nowPlayer;
        _nowPlayer %= 4;
        _isMyTurn = _nowPlayer == 0;
        _nowTime.text = "";
        _isTurnReady = false;
        
        //버린 타일로 플레이어가 후로 가능한지 확인하는 코드
        if (_canHuroTiles[_playerNum].Contains(_nowDahaiTile))
        {
            _canHuro = true;
        }
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
    private void MakeFirstHand()
    {
        //손패 만들기
        for (int i = 0; i < 4; ++i)
        {
            _hands.Add(_tiles.GetFirstHand());
            _hands[0] = new List<string>
            {
                "p1", "p1", "p1",
                "p3", "p4", "p5",
                "s3", "s3",
                "s4", "s4",
                "s5", "s5", "s5",
            };
            _discardTiles.Add(new List<string>());
            _hands[i] = HandArrange(_hands[i]);//손패 정리
            _discardNums.Add(-1);
            _isRiichi.Add(false);
            TileDisplay.HandDisplay(_hands[i], i);
        }

    }
    private IEnumerator NotMyTurnDahai()
    {
        yield return new WaitForSeconds(0.01f);
        Dahai(_nowTsumoTile, _nowPlayer);
    }
    //전체 핸드 로그로 확인하는 함수
    private void HandCheck(List<string> hand, string outStr)
    {
        Debug.Log(hand.Aggregate(outStr + " : ", (current, tile) => current + ("\"" + tile + "\", ")));
    }

    public bool GetIsTurnReady()
    {
        return _isTurnReady;
    }
    public bool GetDoneHuro()
    {
        return _doneHuro;
    }
    private void UpdateTileLeft()
    {
        _tileLeft.text = "남은 패 : " + _tiles.GetTileLeft();
    }
    public void UserWin()
    {
        _winGame = true;
    }
    public bool GetIsMyTurn()
    {
        return _isMyTurn;
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
}
//위에 핸드 추가하는 함수에 for문 1부터 시작으로 바꿔야함
// private void MakeTestHand()
// {
//     List<string> testHand = new List<string>
//     {
//         "m5", "m5", "m5", "m1", "m2",
//         "m3", "m2", "s1", "s2", "s4",
//         "p3", "m7", "m4"
//     };
//     _hands.Add(testHand);
//     _hands[0] = HandArrange(_hands[0]);//손패 정리
//     _discardNums.Add(-1);
//     TileDisplay.HandDisplay(_hands[0], 0);
// }

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