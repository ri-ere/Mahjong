using System.Collections.Generic;
using System.Linq;

public class Huro : HandChecker
{
    List<string> DoPong(string huroTile)
    {
        List<string> result = new List<string>();
        return result;
    }
    List<string> DoKan(List<string> huroHand, string huroTile, bool isMyTurn)//huroHand 재구축
    {
        List<string> result = new List<string>();
        string tile = "";
        bool isFind = false;//찾으면 쇼밍깡 못찾으면 안깡
        if (huroTile.Length > 1) tile += huroTile;
        else tile += huroTile + huroTile;

        foreach (string huro in huroHand) //쇼밍깡인지 확인하는 코드
        {
            if (huro[..2].Equals("t-") && huro[2..4].Equals(tile))
            {
                if(isMyTurn)//쇼밍깡
                {
                    result.Add("m-" + tile);
                    isFind = true;
                }
                else//내턴 아닐때니까 대명깡
                {
                    result.Add("d-" + tile);
                    isFind = true;
                }
            }
            else
            {
                result.Add(huro);
            }
        }
        if (!isFind)//위에서 못찾았으면 안깡
        {
            result.Add("a-" + tile);
        }
        return result;
    }
    List<string> NotMyTurnCanHuroList(List<string> hand)
    {
        List<string> result = new List<string>();
        foreach (string tile in MakeCanPongList(hand))
        {
            result.Add(tile);
        }
        foreach (string tile in MakeCanDaiminKanList(hand))
        {
            result.Add(tile);
        }
        foreach (string tile in MakeCanChiList(hand))
        {
            result.Add(tile);
        }

        return result.Distinct().ToList();
    }
    List<string> MyTurnCanHuroList(List<string> hand, List<string> huroList)
    {
        List<string> result = new List<string>();
        //쇼밍캉 - 퐁한거 있는거에 추가
        foreach (string tile in MakeCanShouminKanList(huroList))
        {
            if (tile[1] > 48 && tile[1] < 58) result.Add(tile);
            else result.Add(tile[0].ToString());
        }
        //안깡
        foreach (string tile in MakeCanAnKanList(hand))
        {
            result.Add(tile);
        }
        return result;
    }
    List<string> MakeCanChiList(List<string> hand)
    {
        List<string> result = WhatIsPair(hand).Distinct().ToList();
        return result;
    }
    List<string> MakeCanPongList(List<string> hand)
    {
        List<string> result = WhatIsPair(hand).Distinct().ToList();
        return result;
    }
    List<string> MakeCanDaiminKanList(List<string> hand)//대명깡, so 커츠 있는거에 추가, 남꺼 가져와서 하는거라 그거 체크 하는거 넣어야할듯
    {
        List<string> result = WhatIsTriplet(hand).Distinct().ToList();
        return result;
    }
    List<string> MakeCanShouminKanList(List<string> pongedList)//쇼밍깡, 퐁한거에 추가
    {
        List<string> result = new List<string>();
        foreach (string pong in pongedList)
        {
            if (pong.Length < 5 && pong[..2].Equals("t-"))
            {
                result.Add(pong[2..4]);
            }
        }
        return result;
    }
    List<string> MakeCanAnKanList(List<string> hand)//안깡, 패 4개
    {
        List<string> result = new List<string>();
        List<string> pairs = WhatIsPair(hand);
        for (int i = 0; i < pairs.Count - 1; i++)
        {
            if (pairs[i].Equals(pairs[i + 1]))
            {
                result.Add(pairs[i]);
                ++i;
            }
        }
        return result;
    }
}
