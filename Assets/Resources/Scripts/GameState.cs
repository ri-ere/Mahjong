using System;

public class GameState
{
    private String nowWind;//현재 바람
    private int howLong;//연장 몇번 했는지 - 변수명 변경 필요
    // //유저 점수
    // private int user0point = 25000;
    // private int user1point = 25000;
    // private int user2point = 25000;
    // private int user3point = 25000;
    //유저 바람
    private int user0Wind;
    private int user1Wind;
    private int user2Wind;
    private int user3Wind;
    
    private bool gameEnd = false;
    public bool isGameEnd()
    {
        return gameEnd;
    }
}
