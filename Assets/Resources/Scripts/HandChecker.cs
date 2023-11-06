using System.Collections.Generic;
using System.Linq;

public class HandChecker
{
    //여기서 나오는 것들은 다 울지 않은 멘젠 상태의 패들만 나옴
    public Dictionary<string, List<string>> NowHandState(List<string> hand)
    {
        List<string> tmpHand = new List<string>();
        foreach (string tile in hand)
        {
            string tmp = tile;
            if (tmp.Length >= 3) tmp = tmp[..2]; //아카도라일수도 있으니 뒤에 r문자 자르기
            tmpHand.Add(tmp);
        }

        Dictionary<string, List<string>> handState = new Dictionary<string, List<string>>();
        handState.Add("triplet", WhatIsTriplet(tmpHand));
        // handState.Add("sequence", WhatIsSequence(tmpHand));
        handState.Add("pair", WhatIsPair(tmpHand));

        return handState;
    }
    public List<string> CanHuro()
    {
        List<string> result = new List<string>();
    
    
    
    
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
                if(duplicates[tile] == 4)
                    pair.Add(tile);
            }
        }
        return pair;
    }
    public List<string> WhatIsSequence(List<string> tiles)
    {
        List<string> result = new List<string>();
        const int zero = 48;//아스키코드로 "0" == 48
        string[] stack = new string[4];
        string possibilities = FindAllSequence(tiles, 0);
        if (possibilities == "") return result;//가능한 슌츠가 없을때 반환
    
        string[] possibility = possibilities[..^1].Split(',');// "," 가 하나 더 있어서 마지막에 아무것도 없는 배열이 하나 더 있어서 마지막 "," 제거
        stack[0] = possibility[0];
        for (int i = 1; i < possibility.Length; ++i)
        {
            if (possibility[i][0] <= possibility[i - 1][0])
            {
                // 01 있으면 정렬
                // 잘라서 넣기
                string sequences = MakeSequence(stack, possibility[i-1][0] - zero);
                string[] seq = sequences.Split(",");
                string strToAdd = "";
                foreach (string s in seq)
                {
                    strToAdd += s + ",";
                    result.Add(strToAdd[..^1]);
                }
            }
            stack[possibility[i][0] - zero] = possibility[i];
        }
        //정렬하고 넣기
        string a = MakeSequence(stack, possibility[^1][0] - zero);
        string[] b = a.Split(",");
        string str = "";
        foreach (string s in b)
        {
            str += s + ",";
            result.Add(str[..^1]);
        }
        return result.Distinct().ToList();
    }
    string MakeSequence(string[] sequences, int max)
    {
        Dictionary<string, int> mps = new Dictionary<string, int> { {"m", 0}, {"p", 30}, {"s", 60} };//손패 순서
        Dictionary<string, int> seqToNum = new Dictionary<string, int>();
        for (int i = 0; i <= max; i++)
        {
            seqToNum[sequences[i] + i] = sequences[i][3] + sequences[i][5] + sequences[i][7] + mps[sequences[i][2].ToString()];
        }
        string result = seqToNum.OrderBy(x => x.Value).Aggregate("", (current, pair) => current + (pair.Key[2..^1] + ","));
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
            if(IsSequence(numbers[i], numbers[i + 1], numbers[i + 2]))
                sequences.Add(numbers[i] + numbers[i + 1] + numbers[i + 2]);
        return sequences;
    }

    private bool IsSequence(string a, string b, string c)
    {
        if(a[0] == b[0] && a[0] == c[0])
            if (a[1] == b[1] - 1 && a[1] == c[1] - 2)
                return true;
        return false;
    }
    //아카도라 삭제오류 있어서 고쳐야함 - 일단 함수에 넣을때 정리해서 넣는걸로 해결
    List<string> SingleSeqRemover(List<string> tiles, string sequence)
    {
        List<string> tmp = tiles;
        tmp.RemoveAt(tmp.IndexOf(sequence[..2]));
        tmp.RemoveAt(tmp.IndexOf(sequence[2..4]));
        tmp.RemoveAt(tmp.IndexOf(sequence[4..6]));
        return tmp;
    }
}
//커츠 모두 확인하고 머리 확인하고 슌츠 확인하기
//2종? 