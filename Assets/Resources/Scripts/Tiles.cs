using System.Collections.Generic;
using System.Linq;
using System;
//타일 인스턴스 생성 후 게임 디렉터에서 하나씩 빼서 사용하는 방식으로 구현

public class Tiles
{
    int tileLeft = 70;//남은 츠모용 변수
    bool Ryuukyoku = false;
    
    //전체 패 선언, 총 139개
    List<string> allTiles = new List<string>()
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
        "p", "p", "p", "p", "f", "f", "f", "f", "c", "c", "c", "c", "m5r", "p5r", "s5r"
    };
    //모든 타일 뽑기
    void drawAllTiles() {
        while(!isTileEmpty()) {
            tileDraw();
        }
    }
    //게임 끝났는지 확인
    public bool isGameEnd()
    {
        if(Ryuukyoku) {
            return true;
        }
        else {
            return false;
        }
    }
    //남은 타일 있는지 확인
    bool isTileEmpty() {
        if(!allTiles.Any()) {
            return true;
        }
        else {
            return false;
        }
    }
    //타일 1장 뽑기
    private string tileDraw() {
        string drawnTile;
        Random random = new Random();
        int randNum = random.Next(0, allTiles.Count);
        drawnTile = allTiles[randNum];
        allTiles.RemoveAt(randNum);
        
        return drawnTile;
    }
    //한번에 여러개 타일 드로우하는 함수, 츠모갯수 줄지 않음
    private List<string> getManyTiles(int amount)
    {
        List<string> basket = new List<String>();
        for (int i = 0; i < amount; i++)
        {
            basket.Add(tileDraw());
        }
        return basket;
    }
    //도라표시패 8개 드로우
    public List<string> drawDora()
    {
        List<string> doras = new List<String>();
        doras = getManyTiles(10);
        return doras;
    }
    //타일 분배
    public List<string> getFirstHand() {
        return getManyTiles(13);
    }
    //츠모
    public string tsumo() {
        --tileLeft;
		if(tileLeft == 0) Ryuukyoku = true;
        return tileDraw();
    }
    public int getTileLeft()
    {
        return tileLeft;
    }

    public int howManyTileleft()
    {
        return allTiles.Count;
    }
}
