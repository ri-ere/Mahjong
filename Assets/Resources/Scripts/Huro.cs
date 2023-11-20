using System.Collections.Generic;
using System.Linq;

public class Huro : HandChecker
{
    //CanRon()
    public List<string> MakeCanRonList(List<string> hand)
    {
        List<string> result = new List<string>();
        return result;
    }

    public static List<string> WhatToChi(List<string> hand, string huroTile)
    {
        List<string> result = new List<string>();
        List<string> numbers = (from tmp in hand where tmp.Length >= 2 select tmp[..2]).Distinct().ToList();
        const int zero = 48;
        int num = huroTile[1] - zero;
        char tileId = huroTile[0];
        string a;
        string b;
        switch (num)
        {
            case 1:
                a = tileId + (num + 1).ToString();
                b = tileId + (num + 2).ToString();
                if(numbers.Contains(a) && numbers.Contains(b)) result.Add(huroTile + a + b);
                break;
            case 2:
                a = tileId + (num - 1).ToString();
                b = tileId + (num + 1).ToString();
                if(numbers.Contains(a) && numbers.Contains(b)) result.Add(a + huroTile + b);
                a = tileId + (num + 1).ToString();
                b = tileId + (num + 2).ToString();
                if(numbers.Contains(a) && numbers.Contains(b)) result.Add(huroTile + a + b);
                break;
            case 8:
                a = tileId + (num - 2).ToString();
                b = tileId + (num - 1).ToString();
                if(numbers.Contains(a) && numbers.Contains(b)) result.Add(a + b + huroTile);
                a = tileId + (num - 1).ToString();
                b = tileId + (num + 1).ToString();
                if(numbers.Contains(a) && numbers.Contains(b)) result.Add(a + huroTile + b);
                break;
            case 9:
                a = tileId + (num - 2).ToString();
                b = tileId + (num - 1).ToString();
                if(numbers.Contains(a) && numbers.Contains(b)) result.Add(a + b + huroTile);
                break;
            default:
                a = tileId + (num - 2).ToString();
                b = tileId + (num - 1).ToString();
                if(numbers.Contains(a) && numbers.Contains(b)) result.Add(a + b + huroTile);
                a = tileId + (num - 1).ToString();
                b = tileId + (num + 1).ToString();
                if(numbers.Contains(a) && numbers.Contains(b)) result.Add(a + huroTile + b);
                a = tileId + (num + 1).ToString();
                b = tileId + (num + 2).ToString();
                if(numbers.Contains(a) && numbers.Contains(b)) result.Add(huroTile + a + b);
                break;
        }
        return result;
    }
    //후로 타일 변경해서 바꿔야함
    public List<string> DoKan(List<string> huroHand, string huroTile, bool isMyTurn)//huroHand 재구축
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
    public List<string> NotMyTurnCanHuroList(List<string> hand)
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
        foreach (string tile in MakeCanRonList(hand))
        {
            result.Add(tile);
        }

        return result.Distinct().ToList();
    }
    public List<string> MyTurnCanHuroList(List<string> hand, List<string> huroList)
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
    public static List<string> MakeCanChiList(List<string> hand)
    {
        List<string> result = new List<string>();
        List<string> numbers = hand.Select(tile => tile.Length > 2 ? tile[..2] : tile).ToList();
        result.AddRange(MChiMaker(numbers));
        result.AddRange(PChiMaker(numbers));
        result.AddRange(SChiMaker(numbers));

        return result.Distinct().ToList();
    }
    public List<string> MakeCanPongList(List<string> hand)
    {
        List<string> result = WhatIsPair(hand).Distinct().ToList();
        return result;
    }
    public List<string> MakeCanDaiminKanList(List<string> hand)//대명깡, so 커츠 있는거에 추가
    {
        List<string> result = WhatIsTriplet(hand).Distinct().ToList();
        return result;
    }
    public List<string> MakeCanShouminKanList(List<string> pongedList)//쇼밍깡, 퐁한거에 추가
    {
        List<string> result = new List<string>();
        if (pongedList.Count == 0) return result;
        foreach (string pong in pongedList)
        {
            if (pong.Length is < 10 and > 1)//길이 늘어날때마다 이거 변경함
            {
                if(pong[..2].Equals("t-"))
                    result.Add(pong[2..4]);
            }
        }
        return result;
    }
    public List<string> MakeCanAnKanList(List<string> hand)//안깡, 패 4개
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

    static List<string> MChiMaker(List<string> numbers)
    {
        List<string> result = new List<string>();
        //변짱 대기
        if(numbers.Contains("m1") && numbers.Contains("m2")) result.Add("m3");
        if(numbers.Contains("m8") && numbers.Contains("m9")) result.Add("m7");
        //간짱 대기
        if(numbers.Contains("m1") && numbers.Contains("m3")) result.Add("m2");
        if(numbers.Contains("m2") && numbers.Contains("m4")) result.Add("m3");
        if(numbers.Contains("m3") && numbers.Contains("m5")) result.Add("m4");
        if(numbers.Contains("m4") && numbers.Contains("m6")) result.Add("m5");
        if(numbers.Contains("m5") && numbers.Contains("m7")) result.Add("m6");
        if(numbers.Contains("m6") && numbers.Contains("m8")) result.Add("m7");
        if(numbers.Contains("m7") && numbers.Contains("m9")) result.Add("m8");
        //양면 대기
        if(numbers.Contains("m2") && numbers.Contains("m3"))
        {
            result.Add("m1");
            result.Add("m4");
        }
        if(numbers.Contains("m3") && numbers.Contains("m4"))
        {
            result.Add("m2");
            result.Add("m5");
        }
        if(numbers.Contains("m4") && numbers.Contains("m5"))
        {
            result.Add("m3");
            result.Add("m6");
        }
        if(numbers.Contains("m5") && numbers.Contains("m6"))
        {
            result.Add("m4");
            result.Add("m7");
        }
        if(numbers.Contains("m6") && numbers.Contains("m7"))
        {
            result.Add("m5");
            result.Add("m8");
        }
        if(numbers.Contains("m7") && numbers.Contains("m8"))
        {
            result.Add("m6");
            result.Add("m9");
        }
        return result;
    }

    static List<string> PChiMaker(List<string> numbers)
    {
        List<string> result = new List<string>();
        //변짱 대기
        if(numbers.Contains("p1") && numbers.Contains("p2")) result.Add("p3");
        if(numbers.Contains("p8") && numbers.Contains("p9")) result.Add("p7");
        //간짱 대기
        if(numbers.Contains("p1") && numbers.Contains("p3")) result.Add("p2");
        if(numbers.Contains("p2") && numbers.Contains("p4")) result.Add("p3");
        if(numbers.Contains("p3") && numbers.Contains("p5")) result.Add("p4");
        if(numbers.Contains("p4") && numbers.Contains("p6")) result.Add("p5");
        if(numbers.Contains("p5") && numbers.Contains("p7")) result.Add("p6");
        if(numbers.Contains("p6") && numbers.Contains("p8")) result.Add("p7");
        if(numbers.Contains("p7") && numbers.Contains("p9")) result.Add("p8");
        //양면 대기
        if(numbers.Contains("p2") && numbers.Contains("p3"))
        {
            result.Add("p1");
            result.Add("p4");
        }
        if(numbers.Contains("p3") && numbers.Contains("p4"))
        {
            result.Add("p2");
            result.Add("p5");
        }
        if(numbers.Contains("p4") && numbers.Contains("p5"))
        {
            result.Add("p3");
            result.Add("p6");
        }
        if(numbers.Contains("p5") && numbers.Contains("p6"))
        {
            result.Add("p4");
            result.Add("p7");
        }
        if(numbers.Contains("p6") && numbers.Contains("p7"))
        {
            result.Add("p5");
            result.Add("p8");
        }
        if(numbers.Contains("p7") && numbers.Contains("p8"))
        {
            result.Add("p6");
            result.Add("p9");
        }
        return result;
    }

    static List<string> SChiMaker(List<string> numbers)
    {
        List<string> result = new List<string>();
        //변짱 대기
        if(numbers.Contains("s1") && numbers.Contains("s2")) result.Add("s3");
        if(numbers.Contains("s8") && numbers.Contains("s9")) result.Add("s7");
        //간짱 대기
        if(numbers.Contains("s1") && numbers.Contains("s3")) result.Add("s2");
        if(numbers.Contains("s2") && numbers.Contains("s4")) result.Add("s3");
        if(numbers.Contains("s3") && numbers.Contains("s5")) result.Add("s4");
        if(numbers.Contains("s4") && numbers.Contains("s6")) result.Add("s5");
        if(numbers.Contains("s5") && numbers.Contains("s7")) result.Add("s6");
        if(numbers.Contains("s6") && numbers.Contains("s8")) result.Add("s7");
        if(numbers.Contains("s7") && numbers.Contains("s9")) result.Add("s8");
        //양면 대기
        if(numbers.Contains("s2") && numbers.Contains("s3"))
        {
            result.Add("s1");
            result.Add("s4");
        }
        if(numbers.Contains("s3") && numbers.Contains("s4"))
        {
            result.Add("s2");
            result.Add("s5");
        }
        if(numbers.Contains("s4") && numbers.Contains("s5"))
        {
            result.Add("s3");
            result.Add("s6");
        }
        if(numbers.Contains("s5") && numbers.Contains("s6"))
        {
            result.Add("s4");
            result.Add("s7");
        }
        if(numbers.Contains("s6") && numbers.Contains("s7"))
        {
            result.Add("s5");
            result.Add("s8");
        }
        if(numbers.Contains("s7") && numbers.Contains("s8"))
        {
            result.Add("s6");
            result.Add("s9");
        }
        return result;
    }
}
