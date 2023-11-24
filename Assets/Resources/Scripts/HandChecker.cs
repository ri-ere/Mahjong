using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class HandChecker
{
    public bool CanWin(List<string> hand, List<string> huroHand, string nowTile, bool isRiichi, int howLong, bool isMyTurn)//리치론이랑 따로 확인함
    {
        List<string> fakeHand = new List<string>();
        List<string> hand13 = new List<string>();
        fakeHand.AddRange(hand);
        if (fakeHand.Count is 2 or 5 or 8 or 11 or 14)
        {
            if (fakeHand.Contains(nowTile))
            {
                fakeHand.RemoveAt(fakeHand.IndexOf(nowTile));
            }
        }
        hand13.AddRange(fakeHand);
        fakeHand.Add(nowTile);
        // fakeHand = Yaku.HandArranger(fakeHand);
        Dictionary<int, List<string>> handState = NowHandState(fakeHand, huroHand);

        bool isMenzen = true;
        foreach (string dragon in huroHand)
        {
            if (dragon[0].Equals('a')) continue;
            else isMenzen = false;
        }
        //치또이츠 확인
        if (Yaku.Chiitoitsu(hand13, nowTile) && isMenzen) return true;
        //국사무쌍 확인
        // if (Yaku.IsKokushiMusou(hand) && isMenzen) return true;
        //다른 역 있는지 확인
        if (handState.TryGetValue(41, out List<string> tiles))
        {
            if (isRiichi) return true;
            foreach (string tile in tiles)
            {
                List<string> handDragons = new List<string>();
                handDragons.AddRange(tile.Split(","));
                if (Yaku.HasYaku(hand, handDragons, huroHand, nowTile,howLong, isMenzen, isMyTurn)) return true;
            }
        }
        return false;
    }

    public List<string> FindRiichiRon(List<string> hand)//13개 일때 확인하는거
    {
        List<string> result = new List<string>();
        Dictionary<int, List<string>> handState = NowHandState(hand, new List<string>());//리치라서 후로 핸드가 없음
        //치또이츠 확인
        List<string> chitoi = WhatIsPair(hand).Distinct().ToList();
        List<string> handDistinct = hand.Distinct().ToList();
        if(chitoi.Count == 6) result.AddRange(handDistinct.Except(chitoi));
        //국사무쌍 확인
        IsKokushiMusouWait(hand);
        //몸통 4개 머리 0개일때
        if (handState.TryGetValue(40, out List<string> hs40))
        {
            foreach (string dragon in hs40)
            {
                List<string> singleLeft = MakeLeftOverList(hand, dragon);
                result.AddRange(singleLeft);
            }
        }    
        if (handState.TryGetValue(32, out var hs32))
        {
            foreach (string dragon in hs32)
            {
                string[] bodies = dragon.Split(",");
                foreach (string body in bodies)
                {
                    if (body[0].Equals('h'))
                    {
                        if (body[2].Equals(body[3])) result.Add(body[2].ToString());
                        else result.Add(body[2..4]);
                    }
                }
            }
        }
        //총 11개 타일 남은 2개가 슌츠로 만들 수 있어야 가능
        if (handState.TryGetValue(31, out List<string> hs31))
        {
            foreach (string dragon in hs31)
            {
                List<string> canChi = Huro.MakeCanChiList(MakeLeftOverList(hand, dragon));
                if (canChi.Any())
                {
                    result.AddRange(canChi);
                }
            }
        }
        return result.Distinct().ToList();
    }
    public List<string> FindRiichiDiscard(List<string> hand)//14개 있어야함 리치니까! 근데 안깡은?
    {
        List<string> result = new List<string>();
        Dictionary<int, List<string>> handState = NowHandState(hand, new List<string>());//리치라서 후로 핸드가 없음
        //치또이츠 확인
        List<string> chitoi = WhatIsPair(hand).Distinct().ToList();
        List<string> handDistinct = hand.Distinct().ToList();
        if (chitoi.Count == 6 && handDistinct.Count > 6)
        {
            if (handDistinct.Count == 7)
            {
                List<string> tri = WhatIsTriplet(hand);
                result.Add(tri[0]);
            }
            else if (handDistinct.Count == 8)
                result.AddRange(handDistinct.Except(chitoi));
        }
        //국사확인
        //몸통 4개, 머리 0개
        if (handState.TryGetValue(40, out List<string> hs40))
        {
            foreach (string dragon in hs40)
            {
                List<string> singleLeft = MakeLeftOverList(hand, dragon);
                List<string> canChiList = new List<string>();
                if (singleLeft.Count == 2) canChiList = WhatCanWaitChi(singleLeft[0], singleLeft[1]);
                if (canChiList.Any())
                {
                    string[] bodies = dragon.Split(",");
                    foreach (string body in bodies)
                    {
                        if (body[0] == 't')
                        {
                            result.Add(body[2].Equals(body[3]) ? body[2].ToString() : body[2..4]);
                        }
                    }
                }
                result.AddRange(singleLeft);
            }
        }

        if (handState.TryGetValue(32, out var hs32))
        {
            foreach (string dragon in hs32)
            {
                List<string> singleLeft = MakeLeftOverList(hand, dragon);
                result.AddRange(singleLeft);
            }
        }
        //총 11개 타일, 남은 타일 3개 중에 몸통이 될 수 있는 경우가 있으면 true
        if (handState.TryGetValue(31, out List<string> hs31))
        {
            foreach (string dragon in hs31)
            {
                List<string> leftOver = MakeLeftOverList(hand, dragon);
                if (leftOver.Count > 2)
                {
                    List<string> ab = new List<string> { leftOver[0], leftOver[1] };
                    List<string> bc = new List<string> { leftOver[1], leftOver[2] };
                    List<string> canChiab = Huro.MakeCanChiList(ab);
                    List<string> canChibc = Huro.MakeCanChiList(bc);
                    if (canChibc.Any()) result.Add(leftOver[0]);
                    if (canChiab.Any()) result.Add(leftOver[2]);
                }
            }
        }
        return result.Distinct().ToList();
    }
    public static List<string> WhatCanWaitChi(string tile1, string tile2)
    {
        List<string> result = new List<string>();
        if (tile1.Length < 2 || tile2.Length < 2 || tile1[0] != tile2[0]) return result;
        const int zero = 48;
        int num1 = tile1[1] - zero;
        int num2 = tile2[1] - zero;
        if (num1 > num2) (num1, num2) = (num2, num1);
        string tileId = tile1[0].ToString();
        switch (num2 - num1)
        {
            case 0:
                break;
            case 1:
                if (num1 == 1) result.Add(tileId + "3");
                else if(num2 == 9) result.Add(tileId + "7");
                else
                {
                    result.Add(tileId + (num1 - 1).ToString());
                    result.Add(tileId + (num2 + 1).ToString());
                }
                break;
            case 2:
                result.Add(tileId + (num1 + 1).ToString());
                break;
        }
        return result;
    }
    public List<string> MakeLeftOverList(List<string> tiles, string deleteList)
    {
        List<string> fakeHand = new List<string>();
        fakeHand.AddRange(tiles);
        string[] bodies = deleteList.Split(",");
        foreach (string body in bodies)
        {
            switch (body[0])
            {
                case 's':
                    fakeHand.RemoveAt(fakeHand.IndexOf(body[2..4]));
                    fakeHand.RemoveAt(fakeHand.IndexOf(body[4..6]));
                    fakeHand.RemoveAt(fakeHand.IndexOf(body[6..8]));
                    break;
                case 't':
                    if (body[2].Equals(body[3]))
                    {
                        fakeHand.RemoveAt(fakeHand.IndexOf(body[2..3]));
                        fakeHand.RemoveAt(fakeHand.IndexOf(body[2..3]));
                        fakeHand.RemoveAt(fakeHand.IndexOf(body[2..3]));
                    }
                    else
                    {
                        fakeHand.RemoveAt(fakeHand.IndexOf(body[2..4]));
                        fakeHand.RemoveAt(fakeHand.IndexOf(body[2..4]));
                        fakeHand.RemoveAt(fakeHand.IndexOf(body[2..4]));
                    }
                    break;
                case 'h':
                    if (body[2].Equals(body[3]))
                    {
                        fakeHand.RemoveAt(fakeHand.IndexOf(body[2..3]));
                        fakeHand.RemoveAt(fakeHand.IndexOf(body[2..3]));
                    }
                    else
                    {
                        fakeHand.RemoveAt(fakeHand.IndexOf(body[2..4]));
                        fakeHand.RemoveAt(fakeHand.IndexOf(body[2..4]));
                    }
                    break;
            }
        }
        return fakeHand;
    }

    public bool IsKokushiMusouWait(List<string> hand)
    {
        // if (Yaku.IsKokushiMusou(hand)) return true;
        List<string> kokushiMusou = new List<string>
           { "m1", "m9", "p1", "p9", "s1", "s9", "e", "s", "w", "n", "p", "f", "c" };
        List<string> forRiichi = new List<string>();//여기에 들어가는 거에 kokushiMusou안에 없는게 버릴 타일
        List<string> forWinTile = new List<string>();//13면팅일수도? 여기에 아무것도 안들어가면 13면팅
        bool kokushiTenpai = true;
        bool kokushiDouble = true;
        int kokushiCnt = 0;
    
        for (int i = 0; i < hand.Count; i++)
        {
            if (kokushiCnt == 13)//마지막 중이 중복이면 13까지 가서 오류가 나기에 따로 체크
            {
                if (hand[i].Equals("c") && hand[i].Equals(hand[i - 1]) && kokushiDouble)
                {
                    kokushiDouble = !kokushiDouble;
                    forRiichi.Add(hand[i]);
                }
                else
                {
                    if (kokushiTenpai)
                    {
                        kokushiTenpai = !kokushiTenpai;
                        forRiichi.Add(hand[i]);
                    }
                    else return false;
                }
                continue;
            }
            if (hand[i].Equals(kokushiMusou[kokushiCnt])) ++kokushiCnt;
            else
            {
                if (kokushiMusou.Contains(hand[i]))
                {
                    if (hand[i].Equals(hand[i - 1]))
                    {
                        if (kokushiDouble)
                        {
                            kokushiDouble = !kokushiDouble;
                            forRiichi.Add(hand[i]);
                        }
                        else
                        {
                            if (kokushiTenpai)
                            {
                                kokushiTenpai = !kokushiTenpai;
                                forRiichi.Add(hand[i]);
                            }
                            else return false;
                        }
                    }
                    else
                    {
                        forWinTile.Add(kokushiMusou[kokushiCnt++]);
                        --i;
                    }
                }
                else
                {
                    if (kokushiTenpai)
                    {
                        kokushiTenpai = !kokushiTenpai;
                        forRiichi.Add(hand[i]);
                    }
                    else return false;
                }
            } 
        }
        return true;
    }
    //key = 31, value = s-m2m3m4,t-s5,t-ee,h-w 이런식으로 출력
    public Dictionary<int, List<string>> NowHandState(List<string> hand, List<string> huroHand)
    {
        Dictionary<int, List<string>> result = new Dictionary<int, List<string>>();
        List<string> tiles = hand.Select(tile => tile.Length >= 3 ? tile[..2] : tile).ToList(); //아카도라 제거

        foreach (string sequencePoss in WhatIsSequence(tiles))
        {
            string setList = "";
            int weight = 0;
            weight += huroHand.Count * 10;
            List<string> fakeHand = tiles;
            foreach (string seq in sequencePoss.Split(","))
            {
                setList += "s-" + seq + ",";
                weight += 10;
                fakeHand = SingleSeqRemover(fakeHand, seq);
            }

            foreach (string triplet in WhatIsTriplet(fakeHand))
            {
                if(triplet.Length > 1) setList += "t-" + triplet + ",";
                else setList += "t-" + triplet + triplet + ",";
                weight += 10;
                for (int i = 0; i < 3; i++) fakeHand.RemoveAt(fakeHand.IndexOf(triplet));
            }

            foreach (string pair in WhatIsPair(fakeHand))
            {
                if(pair.Length > 1) setList += "h-" + pair + ",";
                else setList += "h-" + pair + pair + ",";
                weight += 1;
                for (int i = 0; i < 2; i++) fakeHand.RemoveAt(fakeHand.IndexOf(pair));
            }

            if (setList.Equals("")) setList += ",";
            if (result.ContainsKey(weight)) result[weight].Add(setList[..^1]); //있으면 리스트에 추가
            else result[weight] = new List<string> { setList[..^1] }; //없을때 생성
        }

        //슌츠가 없는 경우로 한번 더 실행
        string setListForSingle = "";
        int weightForSingle = 0;
        weightForSingle += huroHand.Count * 10;
        foreach (string triplet in WhatIsTriplet(tiles))
        {
            if(triplet.Length > 1) setListForSingle += "t-" + triplet + ",";
            else setListForSingle += "t-" + triplet + triplet + ",";
            weightForSingle += 10;
            for (int i = 0; i < 3; i++) tiles.RemoveAt(tiles.IndexOf(triplet));
        }

        foreach (string pair in WhatIsPair(tiles))
        {
            if(pair.Length > 1) setListForSingle += "h-" + pair + ",";
            else setListForSingle += "h-" + pair + pair + ",";
            weightForSingle += 1;
            for (int i = 0; i < 2; i++) tiles.RemoveAt(tiles.IndexOf(pair));
        }

        if (weightForSingle != 0)
        {
            if (setListForSingle.Equals("")) setListForSingle += ",";
            if (result.ContainsKey(weightForSingle)) result[weightForSingle].Add(setListForSingle[..^1]); //있으면 리스트에 추가
            else result[weightForSingle] = new List<string> { setListForSingle[..^1] }; //없을때 생성
        }

        return result;
    }
    //커츠들 이름 리스트로 넣어서 반환
    protected static List<string> WhatIsTriplet(List<string> tiles)
    {
        List<string> singleTile = new List<string>();
        List<string> triplet = new List<string>();
        Dictionary<string, int> duplicates = new Dictionary<string, int>();
        foreach (string tile in tiles)
        {
            if (duplicates.ContainsKey(tile))
                duplicates[tile]++;
            else
            {
                duplicates[tile] = 1;
                singleTile.Add(tile);
            }
        }
        //커츠 찾는 부분
        foreach (string tile in singleTile)
        {
            if (duplicates[tile] >= 3) triplet.Add(tile);
        }
        return triplet;
    }

    //또이츠들 이름 리스트로 넣어서 반환
    protected static List<string> WhatIsPair(List<string> tiles)
    {
        List<string> singleTile = new List<string>();
        List<string> pair = new List<string>();
        Dictionary<string, int> duplicates = new Dictionary<string, int>();
        foreach (string tile in tiles)
        {
            if (duplicates.ContainsKey(tile))
                duplicates[tile]++;
            else
            {
                duplicates[tile] = 1;
                singleTile.Add(tile);
            }
        }
        foreach (string tile in singleTile)
        {
            if (duplicates[tile] >= 2)
            {
                pair.Add(tile);
                if (duplicates[tile] == 4)
                    pair.Add(tile);
            }
        }
        return pair;
    }

    public List<string> WhatIsSequence(List<string> tiles)
    {
        List<string> result = new List<string>();
        const int zero = 48;
        string[] stack = new string[4];
        string possibilities = FindAllSequence(tiles, 0);
        if (possibilities == "") return result; //가능한 슌츠가 없을때 반환
        string[] possibility = possibilities[..^1].Split(','); 
        stack[0] = possibility[0];
        for (int i = 1; i < possibility.Length; ++i)
        {
            if (possibility[i][0] <= possibility[i - 1][0])
            {
                // 잘라서 넣기
                for (int j = 0; j <= possibility[i - 1][0] - zero; j++)
                {
                    string input = "";
                    for (int k = 0; k <= j; k++) input += stack[k] + ",";
                    result.Add(MakeSequence(input[..^1], j));
                }
            }
            stack[possibility[i][0] - zero] = possibility[i];
        }
        for (int j = 0; j <= possibility[^1][0] - zero; j++)
        {
            string input = "";
            for (int k = 0; k <= j; k++) input += stack[k] + ",";
            result.Add(MakeSequence(input[..^1], j));
        }

        //중복 제거(.distinct)하고 return
        return result.Distinct().ToList();
    }
    string MakeSequence(string seq, int max)
    {
        Dictionary<string, int> mps = new Dictionary<string, int> { { "m", 0 }, { "p", 30 }, { "s", 60 } }; //손패 순서
        Dictionary<string, int> seqToNum = new Dictionary<string, int>();
        if (seq.Length < 10) return seq[2..];

        string[] sequences = seq.Split(",");
        for (int i = 0; i <= max; i++)
        {
            seqToNum[sequences[i] + i] =
                sequences[i][3] + sequences[i][5] + sequences[i][7] + mps[sequences[i][2].ToString()];
        }

        string result = seqToNum.OrderBy(x => x.Value)
            .Aggregate("", (current, pair) => current + (pair.Key[2..^1] + ","));
        return result[..^1];
    }

    private string FindAllSequence(List<string> tiles, int depth)
    {
        string result = "";
        List<string> numbers = (from tmp in tiles where tmp.Length >= 2 select tmp[..2]).Distinct().ToList();
        List<string> sequences = WhatCanBeSequence(numbers);

        if (sequences.Count < 1) return result;
        foreach (string sequence in sequences)
        {
            List<string> fakeHand = (from tmp in tiles where tmp.Length >= 2 select tmp[..2]).ToList();
            fakeHand = SingleSeqRemover(fakeHand, sequence);
            result += depth + "-" + sequence + "," + FindAllSequence(fakeHand, depth + 1);
        }

        return result;
    }

    //슌츠 가능 리스트들
    private List<string> WhatCanBeSequence(List<string> tiles)
    {
        List<string> sequences = new List<string>();
        List<string> numbers = (from tmp in tiles where tmp.Length >= 2 select tmp[..2]).Distinct().ToList();
        //첫글자 같다면 두번째 글자가 i번과 i+1 번이 1차이로 같은지, i번과 i+2번이 2차이로 같은지 비교, 같으면 추가
        for (int i = 0; i < numbers.Count - 2; ++i)
            if (IsSequence(numbers[i], numbers[i + 1], numbers[i + 2]))
                sequences.Add(numbers[i] + numbers[i + 1] + numbers[i + 2]);
        return sequences;
    }

    private bool IsSequence(string a, string b, string c)
    {
        if (a[0] == b[0] && a[0] == c[0])
            if (a[1] == b[1] - 1 && a[1] == c[1] - 2)
                return true;
        return false;
    }

    //아카도라 삭제오류 있어서 고쳐야함 - 일단 함수에 넣을때 정리해서 넣는걸로 해결
    List<string> SingleSeqRemover(List<string> tiles, string sequence)
    {
        List<string> tmp = new List<string>();
        foreach (string str in tiles)
        {
            tmp.Add(str.Length > 2 ? str[..2] : str);
        }
        tmp.RemoveAt(tmp.IndexOf(sequence[..2]));
        tmp.RemoveAt(tmp.IndexOf(sequence[2..4]));
        tmp.RemoveAt(tmp.IndexOf(sequence[4..6]));
        return tmp;
    }
}
//커츠 모두 확인하고 머리 확인하고 슌츠 확인하기
//2종? 

// public List<string> CanRiichi(List<string> hand, List<string> huroHand)
// {
//     List<string> result = new List<string>();
//     Dictionary<int, List<string>> handState = NowHandState(hand, huroHand);
//     //치또이츠 확인
//     //고쳐야함 같은거 3개 있을수도 있어서
//     if (WhatIsPair(hand).Distinct().ToList().Count == 6 && hand.Distinct().ToList().Count > 6)
//     {
//         
//     }
//     //국사 확인
//     if (IsKokushiMusouWait(hand))
//     {
//         
//     }
//     //if (handState.ContainsKey(41)) return true; canwin에서 확인
//     if (handState.TryGetValue(40, out List<string> hs40))
//     {
//         foreach (string dragon in hs40)
//         {
//             List<string> singleLeft = MakeLeftOverList(hand, dragon);
//             result.AddRange(singleLeft);
//         }
//     }
//
//     if (handState.TryGetValue(32, out var hs32))
//     {
//         foreach (string dragon in hs32)
//         {
//             string[] d = dragon.Split(",");
//             foreach (string tile in d)
//             {
//                 if (tile[0].Equals('h'))
//                 {
//                     result.Add(tile[2].Equals(tile[3]) ? tile[2].ToString() : tile[2..4]);
//                 }
//             }
//         }
//     }
//     //총 11개 타일, 남은 타일 3개 중에 몸통이 될 수 있는 경우가 있으면 true
//     if (handState.TryGetValue(31, out List<string> hs31))
//     {
//         foreach (string dragon in hs31)
//         {
//             List<string> canChi = Huro.MakeCanChiList(MakeLeftOverList(hand, dragon));
//             if (canChi.Any())
//             {
//                 result.AddRange(canChi);
//             }
//         }
//     }
//     return result;
// }