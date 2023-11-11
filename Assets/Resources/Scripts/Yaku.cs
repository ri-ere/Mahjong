using System.Collections.Generic;

public class Yaku
{
    public bool HasYaku(string hand, bool menzen)
    {
        List<string> tiles = new List<string>();
        foreach (string tile in hand.Split(","))
        {
            
            //if (IsTanyao(tiles)) return true;
        }

        
        
        
        
        return false;
    }

    public int GetHan(string hand, bool menzen)
    {
        int han = 0;
        
        
        
        
        return han;
    }

    private bool IsTanyao(List<string> hand)
    {
        foreach (string tile in hand)
        {
            if (tile.Length > 1)
            {
                if (tile[1] > 49 && tile[1] < 57)
                {
                    return true;
                }
            }
            else return false;
        }
        return false;
    }
    bool IsTenpai(List<string> hand)
    {
        bool tenpai = false;

        Dictionary<string, int> handOrder = new Dictionary<string, int>();

        return tenpai;
    }
}