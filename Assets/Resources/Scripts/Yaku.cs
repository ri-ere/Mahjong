using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class Yaku : HandChecker
{
    public static bool HasYaku(string hand, bool menzen)
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

    public bool IsChiitoitsu(List<string> hand)
    {
        return WhatIsPair(hand).Distinct().ToList().Count == 7;
    }
    public static bool IsKokushiMusou(List<string> hand)
    {
        const string kokushiMusou = "m1,m9,p1,p9,s1,s9,e,s,w,n,p,f,c,";
        string tiles = hand.Distinct().ToList().Aggregate("", (current, tile) => current + tile + ",");
        return tiles.Equals(kokushiMusou) && hand.Count == 14;
    }
    bool IsTenpai(List<string> hand)
    {
        bool tenpai = false;

        Dictionary<string, int> handOrder = new Dictionary<string, int>();

        return tenpai;
    }
}