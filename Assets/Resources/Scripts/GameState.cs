using System;

public class GameState
{
    private String nowWind;//현재 바람
    private int howLong;//연장 몇번 했는지 - 변수명 변경 필요
    //유저 점수
    private int user0point;
    private int user1point;
    private int user2point;
    private int user3point;
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
