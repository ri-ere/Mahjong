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
        handState.Add("sequence", WhatIsSequence(tmpHand));
        handState.Add("pair", WhatIsPair(tmpHand));

        return handState;
    }
    string SequenceCheckTest(List<string> tiles, string stack, string before, int depth)
     {
         string result = "";
         
         List<string> numbers = (from tmp in tiles where tmp.Length >= 2 select tmp[..2]).Distinct().ToList();
         List<string> sequences = WhatCanBeSequence(numbers);
         
         if (sequences.Count >= 1)
         {
             // foreach (string sequence in sequences)
             // {
             //     result += "[" + depth + " " + before + "->" + sequence + "]";
             // }
             foreach (string sequence in sequences)
             {
                 List<string> fakeHand = (from tmp in tiles where tmp.Length >= 2 select tmp[..2]).ToList();
                 fakeHand = SingleSeqRemover(fakeHand, sequence);
                 // result += "["  + depth + " " + sequence + MyTest(fakeHand, stack, sequence, ++depth) + "]";
                 
                 stack += "[" + depth + "->" + sequence + "]";
                 result += SequenceCheckTest(fakeHand, stack, sequence, ++depth);
             }
         }
         else result += "[" + depth + " " + stack + "]";
         return result;
     }
    //아카도라 삭제오류 있어서 고쳐야함
    List<string> SingleSeqRemover(List<string> tiles, string sequence)
    {
        List<string> tmp = tiles;
        tmp.RemoveAt(tmp.IndexOf(sequence[..2]));
        tmp.RemoveAt(tmp.IndexOf(sequence[2..4]));
        tmp.RemoveAt(tmp.IndexOf(sequence[4..6]));
        return tmp;
    }
    //커츠들 이름 리스트로 넣어서 반환
    public static List<string> WhatIsTriplet(List<string> tiles)
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
    public List<string> WhatIsPair(List<string> tiles)
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
    //슌츠 가능 리스트들
    public List<string> WhatCanBeSequence(List<string> tiles)
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
    //슌츠를 위한 뭔가 함수
    List<List<string>> PossibleSequence(List<string> tiles)
    {
        List<string> numbers = (from tmp in tiles where tmp.Length >= 2 select tmp[..2]).ToList();//아카도라 삭제 및 숫자만
        List<string> possibility = WhatCanBeSequence(numbers);//가능한 슌츠 모두
        List<List<string>> sequence = new List<List<string>>();//반환용 변수
        for (int i = 0; i < possibility.Count; ++i)
        {
            List<string> tmp = numbers;
            string seq = possibility[0];
            sequence[i].Add(seq);
            while (true)
            {
                tmp.RemoveAt(tmp.IndexOf(seq[..2]));
                tmp.RemoveAt(tmp.IndexOf(seq[2..4]));
                tmp.RemoveAt(tmp.IndexOf(seq[4..6]));
                tmp = WhatCanBeSequence(tmp);
                if (tmp.Count != 0)
                {
                    sequence.Add(tmp);
                    continue;
                }
                break;
            }
        }
        return sequence;
    }
    //현재 손패에서 가능한 모든 슌츠들
    public List<string> EverySequence(List<string> tiles)
    {
        List<string> possibility = WhatCanBeSequence(tiles);
        List<string> sequence = new List<string>();
        int i = possibility.Count;
        while (i != 0)
        {
            tiles.RemoveAt(tiles.IndexOf(possibility[0][..2]));
            tiles.RemoveAt(tiles.IndexOf(possibility[0][2..4]));
            tiles.RemoveAt(tiles.IndexOf(possibility[0][4..6]));
            sequence.Add(possibility[0]);
            possibility = WhatCanBeSequence(tiles);
            i = possibility.Count;
        }
        return sequence;
    }
    //슌츠들 이름 리스트에 넣어서 반환
    public List<string> WhatIsSequence(List<string> tiles)
    {
        List<string> sequence = new List<string>();
        List<string> numbers = (from tmp in tiles where tmp.Length >= 2 select tmp[..2]).ToList();
        bool isFind;
        do
        {
            isFind = false;
            List<string> tmp = numbers.Distinct().ToList(); //중복제거
            //정렬되어 들어온다는 가정 하에 비교
            //첫글자 같다면 두번째 글자가 i번과 i+1 번이 1차이로 같은지, i번과 i+2번이 2차이로 같은지 비교
            for (int i = 0; i < tmp.Count-2;++i)
                if (tmp[i][0] == tmp[i + 1][0] && tmp[i][0] == tmp[i + 2][0])
                    if (tmp[i][1] == tmp[i + 1][1] - 1 && (tmp[i][1] == (tmp[i + 2][1] - 2)))
                    {
                        //전부 같다면 슌츠 리스트에 넣고 numbers에서 제거해서 다른 슌츠 찾기
                        sequence.Add(tmp[i] + tmp[i + 1] + tmp[i + 2]);
                        numbers.RemoveAt(numbers.IndexOf(tmp[i]));
                        numbers.RemoveAt(numbers.IndexOf(tmp[i+1]));
                        numbers.RemoveAt(numbers.IndexOf(tmp[i+2]));
                        isFind = true;
                        break;
                    }
        } while (isFind);
        return sequence;
    }
}
//커츠 모두 확인하고 머리 확인하고 슌츠 확인하기
//2종? 