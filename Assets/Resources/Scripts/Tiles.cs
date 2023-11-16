using System.Collections.Generic;
using System.Linq;
using System;
//타일 인스턴스 생성 후 게임 디렉터에서 하나씩 빼서 사용하는 방식으로 구현

public class Tiles
{
    private int _tileLeft = 70;//남은 츠모용 변수
    private bool _ryuukyoku = false;
    
    //전체 패 선언, 총 136개 + 아카도라 3개
    private readonly List<string> _allTiles = new List<string>()
    {
        "m1", "m1", "m1", "m1", "m2", "m2", "m2", "m2", "m3", "m3", "m3", "m3",
        "m4", "m4", "m4", "m4", "m5", "m5", "m5", "m5", "m6", "m6", "m6", "m6",
        "m7", "m7", "m7", "m7", "m8", "m8", "m8", "m8", "m9", "m9", "m9", "m9",
        
        "p1", "p1", "p1", "p1", "p2", "p2", "p2", "p2", "p3", "p3", "p3", "p3",
        "p4", "p4", "p4", "p4", "p5", "p5", "p5", "p5", "p6", "p6", "p6", "p6",
        "p7", "p7", "p7", "p7", "p8", "p8", "p8", "p8", "p9", "p9", "p9", "p9",
        
        "s1", "s1", "s1", "s1", "s2", "s2", "s2", "s2", "s3", "s3", "s3", "s3",
        "s4", "s4", "s4", "s4", "s5", "s5", "s5", "s5", "s6", "s6", "s6", "s6",
        "s7", "s7", "s7", "s7", "s8", "s8", "s8", "s8", "s9", "s9", "s9", "s9",
        
        "e", "e", "e", "e", "s", "s", "s", "s", "w", "w", "w", "w", "n", "n", "n", "n",
        "p", "p", "p", "p", "f", "f", "f", "f", "c", "c", "c", "c"//, "m5r", "p5r", "s5r"
    };
    //게임 끝났는지 확인
    public bool IsGameEnd()
    {
        return _ryuukyoku;
    }
    //남은 타일 있는지 확인
    private bool IsTileEmpty()
    {
        return !_allTiles.Any();
    }
    //타일 1장 뽑기
    private string TileDraw() {
        Random random = new Random();
        int randNum = random.Next(0, _allTiles.Count);
        string drawnTile = _allTiles[randNum];
        _allTiles.RemoveAt(randNum);
        
        return drawnTile;
    }
    //한번에 여러개 타일 드로우하는 함수, 츠모갯수 줄지 않음
    private List<string> GetManyTiles(int amount)
    {
        List<string> basket = new List<String>();
        for (int i = 0; i < amount; i++)
        {
            basket.Add(TileDraw());
        }
        return basket;
    }
    //도라표시패 8개 드로우
    public List<string> DrawDora()
    {
        return GetManyTiles(10);
    }
    //타일 분배
    public List<string> GetFirstHand() {
        return GetManyTiles(13);
    }
    //츠모
    public string Tsumo() {
        --_tileLeft;
		if(_tileLeft == 0) _ryuukyoku = true;
        return TileDraw();
    }
    public int GetTileLeft()
    {
        return _tileLeft;
    }

    public int HowManyTileLeft()
    {
        return _allTiles.Count;
    }
    //모든 타일 뽑기
    private void DrawAllTiles() {
        while(!IsTileEmpty()) {
            TileDraw();
        }
    }
}
