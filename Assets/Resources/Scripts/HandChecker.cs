using System.Collections.Generic;
using System.Linq;

public class HandChecker
{
    bool isTenpai(List<string> hand)
    {
        bool tenpai = false;

        Dictionary<string, int> handOrder = new Dictionary<string, int>();

        return tenpai;
    }

    //커츠들 이름 리스트로 넣어서 반환
    public List<string> whatIsTriplet(List<string> tiles)
    {
        List<string> singleTile = new List<string>();
        List<string> triplet = new List<string>();
        Dictionary<string, int> duplicates = new Dictionary<string, int>();
        foreach (string tile in tiles)
        {
            string tmp = tile;
            if(tmp.Length >= 3) tmp = tmp[..2]; //아카도라일수도 있으니 뒤에 r문자 자르기
            if (duplicates.ContainsKey(tmp))
                duplicates[tmp]++;
            else
            {
                duplicates[tmp] = 1;
                singleTile.Add(tmp);
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
    public List<string> whatIsPair(List<string> tiles)
    {
        List<string> singleTile = new List<string>();
        List<string> pair = new List<string>();
        Dictionary<string, int> duplicates = new Dictionary<string, int>();
        foreach (string tile in tiles)
        {
            string tmp = tile;
            if(tmp.Length >= 3) tmp = tmp[..2]; //아카도라일수도 있으니 뒤에 r문자 자르기
            if (duplicates.ContainsKey(tmp))
                duplicates[tmp]++;
            else
            {
                duplicates[tmp] = 1;
                singleTile.Add(tmp);
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
    //슌츠들 이름 리스트에 넣어서 반환
    public List<string> whatIsSequence(List<string> hand)
    {
        List<string> numbers = new List<string>();
        //문자열 길이가 2이상한 숫자패만 뽑기
        foreach (string tile in hand)
        {
            string tmp = tile;
            if (tmp.Length < 2) continue;
            else
            {
                tmp = tmp[..2]; //아카도라일수도 있으니 뒤에 r문자 자르기
                numbers.Add(tmp);
            }
        }
        string numberStr = numbers.Aggregate("", (current, tile) => current + (tile + ", "));
        
        List<string> sequence = new List<string>();
        bool isFind;
        int howManyTimes = 0;
        do
        {
            howManyTimes++;
            isFind = false;
            //중복제거
            List<string> tmp = numbers.Distinct().ToList();
            string tmpStr = tmp.Aggregate("", (current, tile) => current + (tile + ", "));
            //정렬되어 들어온다는 가정 하에 비교
            for (int i = 0; i < tmp.Count-2;++i)
            {
                string first = tmp[i];
                string second = tmp[i+1];
                string third = tmp[i+2];
                //첫글자 같은지 비교
                if (first[0] == second[0] && first[0] == third[0])
                {
                    int fi = first[1];
                    int si = second[1];
                    int ti = third[1];
                    string sText = first[0].ToString() + first[1] + second[1] + third[1];
                    //첫글자 같다면 두번째 글자가 i번과 i+1 번이 1차이로 같은지, i번과 i+2번이 2차이로 같은지 비교 
                    if (fi == si - 1 && fi == ti - 2)
                    {
                        sequence.Add(sText);
                        numbers.RemoveAt(numbers.IndexOf(first));
                        numbers.RemoveAt(numbers.IndexOf(second));
                        numbers.RemoveAt(numbers.IndexOf(third));
                        isFind = true;
                    }
                }
            }
        } while (isFind);
        return sequence;
    }
}
//커츠 모두 확인하고 머리 확인하고 슌츠 확인하기
//2종? 