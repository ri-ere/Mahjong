
public class GameState
{
    private string _nowWind;//현재 바람
    private int _howLong;//연장 몇번 했는지 - 변수명 변경 필요
    // //유저 점수
    // private int user0point = 25000;
    // private int user1point = 25000;
    // private int user2point = 25000;
    // private int user3point = 25000;
    //유저 바람
    private int _user0Wind;
    private int _user1Wind;
    private int _user2Wind;
    private int _user3Wind;
    
    private readonly bool _gameEnd = false;
    public bool IsGameEnd()
    {
        return _gameEnd;
    }
}
