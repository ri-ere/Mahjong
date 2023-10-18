using System.Collections;
using System;
using System.Collections.Generic;

public class Player
{
    private int ID;
    private int score;
    public Player(int ID)
    {
        this.ID = ID;
        score = 25000;
    }

    public void scoreChange()
    {
        score += 1000;
    }
}
