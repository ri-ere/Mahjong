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
    List<List<string>> WhatIsSequence(List<string> tiles)
    {
        List<List<string>> result = new List<List<string>>();
        string possibilities = FindAllSequence(tiles, 0);
    
    
    
        return result;
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
            if (numbers[i][0] == numbers[i + 1][0] && numbers[i][0] == numbers[i + 2][0])
                if (numbers[i][1] == numbers[i + 1][1] - 1 && numbers[i][1] == numbers[i + 2][1] - 2)
                    sequences.Add(numbers[i] + numbers[i + 1] + numbers[i + 2]);
        return sequences;
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