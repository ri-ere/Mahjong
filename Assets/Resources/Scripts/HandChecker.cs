using System.Collections.Generic;
using System.Linq;

public class HandChecker
{
    private Yaku _yaku = new Yaku();
    public bool CanWin(List<string> hand, Dictionary<int, List<string>> handState, bool isMenzen)
    {
        //치또이츠 확인
        if (WhatIsPair(hand).Count == 7) return true;
        if (_yaku.IsKokushiMusou(hand)) return true;
        
        //이거 고쳐야함 저거랑
        if (handState.ContainsKey(41))
        {
            foreach (var tiles in handState[41])
            {
                
                if (_yaku.HasYaku(tiles, isMenzen)) return true;
            }
        }
        
        return false;
    }

    public List<string> MakeCanChiList(List<string> hand)
    {
        List<string> result = new List<string>();
        return result;
    }
    public List<string> MakeCanPongList(List<string> hand)
    {
        List<string> result = new List<string>();
        return result;
    }
    public List<string> MakeCanKanList(List<string> hand)
    {
        List<string> result = new List<string>();
        return result;
    }

    
    
    public bool CanRiichi(List<string> hand, Dictionary<int, List<string>> handState)
    {
        //치또이츠 확인
        if (WhatIsPair(hand).Count == 6) return true;
        if (IsKokushiMusouWait(hand)) return true;

        if (handState.ContainsKey(41)) return true;
        if (handState.ContainsKey(40)) return true;
        if (handState.ContainsKey(32)) return true;


        return false;
    }

    bool IsKokushiMusouWait(List<string> hand)
    {
        List<string> kokushiMusou = new List<string>
            { "m1", "m9", "p1", "p9", "s1", "s9", "e", "s", "w", "n", "p", "f", "c" };
        bool kokushiCheck = false;
        int kokushiCnt = 0;

        foreach (string tile in hand)
        {
            if (tile.Equals("c") && !kokushiCheck) return true;//국사 완성인데 마지막이 "중"이면 true 반환
            if (tile.Equals(kokushiMusou[kokushiCnt]))
            {
                ++kokushiCnt;
            }
            else
            {
                if (!kokushiCheck) kokushiCheck = true;
                else return false;
            }
        }
        return true;
    }

    public Dictionary<int, List<string>> NowHandState(List<string> hand)
    {
        Dictionary<int, List<string>> result = new Dictionary<int, List<string>>();
        List<string> tiles = hand.Select(tile => tile.Length >= 3 ? tile[..2] : tile).ToList(); //아카도라 제거

        foreach (string sequencePoss in WhatIsSequence(tiles))
        {
            string setList = "";
            int weight = 0;
            List<string> fakeHand = tiles;
            foreach (string seq in sequencePoss.Split(","))
            {
                setList += "b-" + seq + ",";
                weight += 10;
                fakeHand = SingleSeqRemover(fakeHand, seq);
            }

            foreach (string triplet in WhatIsTriplet(fakeHand))
            {
                setList += "b-" + triplet + ",";
                weight += 10;
                for (int i = 0; i < 3; i++) fakeHand.RemoveAt(fakeHand.IndexOf(triplet));
            }

            foreach (string pair in WhatIsPair(fakeHand))
            {
                setList += "h-" + pair + ",";
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
        foreach (string triplet in WhatIsTriplet(tiles))
        {
            setListForSingle += "b-" + triplet + ",";
            weightForSingle += 10;
            for (int i = 0; i < 3; i++) tiles.RemoveAt(tiles.IndexOf(triplet));
        }

        foreach (string pair in WhatIsPair(tiles))
        {
            setListForSingle += "h-" + pair + ",";
            weightForSingle += 5;
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

    public List<string> CanHuro()
    {
        List<string> result = new List<string>();




        return result;
    }

    List<string> TileWaitList(List<string> hand)
    {
        List<string> result = new List<string>();
        result.AddRange(CanChi(hand));
        result.AddRange(CanPong(hand));
        result.AddRange(CanKan(hand));
        return result;
    }
    List<string> CanChi(List<string> tiles)
    {
        List<string> result = new List<string>();

        return result;
    }

    List<string> CanPong(List<string> tiles)
    {
        List<string> result = new List<string>();

        return result;
    }

    List<string> CanKan(List<string> tiles)
    {
        List<string> result = new List<string>();

        return result;
    }

    List<string> CanRon(List<string> tiles)
    {
        List<string> result = new List<string>();

        return result;
    }

    //커츠들 이름 리스트로 넣어서 반환
    private static List<string> WhatIsTriplet(List<string> tiles)
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
    private List<string> WhatIsPair(List<string> tiles)
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

    public
        List<string> WhatIsSequence(List<string> tiles)
    {
        List<string> result = new List<string>();
        const int zero = 48; //아스키코드로 "0" == 48
        string[] stack = new string[4];
        string possibilities = FindAllSequence(tiles, 0);
        if (possibilities == "") return result; //가능한 슌츠가 없을때 반환

        string[] possibility = possibilities[..^1].Split(','); // "," 가 하나 더 있어서 마지막에 아무것도 없는 배열이 하나 더 있어서 마지막 "," 제거
        stack[0] = possibility[0];
        for (int i = 1; i < possibility.Length; ++i)
        {
            if (possibility[i][0] <= possibility[i - 1][0])
            {
                // 잘라서 넣기
                for (int j = 0; j <= possibility[i - 1][0] - zero; j++)
                {
                    string input = "";
                    for (int k = 0; k <= j; k++)
                    {
                        input += stack[k] + ",";
                    }

                    result.Add(MakeSequence(input[..^1], j));
                }
            }

            stack[possibility[i][0] - zero] = possibility[i];
        }

        // 잘라서 넣기
        for (int j = 0; j <= possibility[^1][0] - zero; j++)
        {
            string input = "";
            for (int k = 0; k <= j; k++)
            {
                input += stack[k] + ",";
            }

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