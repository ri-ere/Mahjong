
public class Player
{
    private int _id;
    private int _score;
    public Player(int ID)
    {
        _id = ID;
        _score = 25000;
    }

    public void ScoreChange()
    {
        _score += 1000;
    }
}
