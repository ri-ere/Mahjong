using System.Collections.Generic;
using System.Linq;
public class Yaku : HandChecker
{
    public static bool NagashiMangan(List<string> discardTiles)
    {
        return false;
    }
    public static bool HasYaku(List<string> hand, List<string> handDragons, List<string> huroTiles, string nowTile, int howLong, bool isMenzen, bool isMyTurn)
    {
        //hand : 13개 있는거 주르륵 나열되어 있는거
        //huroHand : 제일 긴거
        //handStateStr : 짧은거
        //1판 멘젠만
        if (isMenzen)
        {
            if (MenzenTsumo(isMenzen, isMyTurn)) return true;
            if (Lipeikou(handDragons, huroTiles)) return true;
        }
        //1판
        if (HaiteiRaouyue(isMyTurn, howLong == 0)) return true;
        if(HouteiRaoyui(isMyTurn, howLong == 0)) return true;
        if (Tanyao(hand, nowTile)) return true;
        if(Yakuhai(handDragons, huroTiles) != 0) return true;
        //2판
        if(Chantaiyao(handDragons, huroTiles)) return true;
        if (SanshokuDoujun(handDragons, huroTiles)) return true;
        if (SanshokuDoukou(handDragons, huroTiles)) return true;
        if(Ittsu(handDragons, huroTiles)) if(isMenzen) return true;
        if (Toitoi(handDragons, huroTiles)) return true;
        if(Sanankou(handDragons, huroTiles)) return true;
        if (Sankantsu(huroTiles)) return true;
        if (Honroutou(hand, nowTile)) return true;
        if (Shousangen(handDragons, huroTiles)) return true;
        //3판
        if(Honitsu(hand, nowTile)) if(isMenzen) return true;
        if (JunchanTaiyao(handDragons, huroTiles)) return true;
        if (Ryanpeikou(handDragons, huroTiles) && isMenzen) return true;
    
        //6판
        if(Chinitsu(hand, nowTile)) if(isMenzen) return true;


        
        
        
        
        return false;
    }

    
    
    
    
    
    
    
    
    


public static int GetHan(List<string> hand,List<string> handDragons, List<string> huroTiles, List<string> canHuroTiles, bool isMenzen, string winTile, int howLong, bool isMyTurn, bool isRiichi, int ippatsu)
{
    int han = 0;
    //1판 멘젠만
    if (isMenzen)
    {
        if (MenzenTsumo(isMenzen, isMyTurn)) ++han;
        if (Ippatsu(isMyTurn, ippatsu)) ++han;
        if (Pinfu(handDragons, canHuroTiles, isMenzen)) ++han;
        if (Lipeikou(handDragons, huroTiles)) ++han;
    }
    //1판
    if (isRiichi) ++han;
    if (HaiteiRaouyue(isMyTurn, howLong == 0)) ++han;
    if(HouteiRaoyui(isMyTurn, howLong == 0)) ++han;
    if (Tanyao(hand, winTile)) ++han;
    han += Yakuhai(handDragons, huroTiles);
    //2판
    if(Chantaiyao(handDragons, huroTiles)) if(isMenzen) han += 2; else han += 1;
    if (SanshokuDoujun(handDragons, huroTiles)) if(isMenzen) han += 2; else han += 1;
    if (SanshokuDoukou(handDragons, huroTiles)) han += 2;
    if(Ittsu(handDragons, huroTiles)) if(isMenzen) han += 2; else han += 1;
    if (Toitoi(handDragons, huroTiles)) han += 2;
    if(Sanankou(handDragons, huroTiles)) if(isMenzen) han += 2; else han += 1;
    if (Sankantsu(huroTiles)) han += 2;
    if (Honroutou(hand, winTile)) han += 2;
    if (Shousangen(handDragons, huroTiles)) han += 2;
    //치또이?
    // if (Chiitoitsu(hand, winTile) && isMenzen) han += 2;
    //3판
    if(Honitsu(hand, winTile)) if(isMenzen) han += 2; else han += 1;
    if (JunchanTaiyao(handDragons, huroTiles)) han += 3;
    if (Ryanpeikou(handDragons, huroTiles) && isMenzen) han += 3;
    
    //6판
    if(Chinitsu(hand, winTile)) if(isMenzen) han += 2; else han += 1;
    
    //역만
        
    
    
    
    return han;
}
//1판 멘젠만
static bool MenzenTsumo(bool isMenzen, bool isMyTurn)//멘젠츠모
{
    if(isMyTurn && isMenzen) return true;
    return false;
}
static bool Ippatsu(bool isMyTurn, int ippatsu)//일발
{
    if (ippatsu == 0) return true;
    if (ippatsu == 1 && isMyTurn) return true;
    return false;
}
static bool Pinfu(List<string> handDragons,  List<string> canHuroTiles, bool isMenzen)//핑후
{
    if (canHuroTiles.Count > 1)
    {
        foreach (string dragon in handDragons)
        {
            if (dragon[0].Equals('t')) return false;
        }
    }
    return true;
}
static bool Lipeikou(List<string> handDragons, List<string> huroTiles)//이페코
{
    List<string> merge = new List<string>();
    merge.AddRange(handDragons);
    merge.AddRange(huroTiles);
    for (int i = 0; i < merge.Count - 1; i++)
    {
        for (int j = i + 1; j < merge.Count; j++)
        {
            if (merge[i].Equals(merge[j]) && merge[i][0].Equals('s'))
            {
                return true;
            }
        }
    }
    return false;
}
//1판
static bool HaiteiRaouyue(bool isMyTurn, bool howLong)//해저로월
{
    if (isMyTurn && howLong) return true;
    return false;
}

static bool HouteiRaoyui(bool isMyTurn, bool howLong) //하저로어
{
    if (isMyTurn && howLong) return true;
    return false;
}

static bool RinshanKaihou()//영상개화 - 내가 깡한게 오름패면 1판 인데 깡이 구현 X
{
    
    return false;
}
static bool Chankan()//창깡 - 남이 깡하는게 없어서 구현 안해도 될듯
{
    return false;
}    

static bool Tanyao(List<string> hand, string winTile)//탕야오
{
    List<string> fakeHand = new List<string>();
    fakeHand.AddRange(hand);
    fakeHand.Add(winTile);
    foreach (string tile in fakeHand)
    {
        if (tile.Length > 1)
        {
            if (tile[1] > '1' && tile[1] < '9')
            {
                continue;
            }
            else return false;
        }
        else return false;
    }
    return true;
}

static int Yakuhai(List<string> handDragons, List<string> huroTiles)//삼원패, 자장풍패
{
    int result = 0;
    List<string> fakeDragons = new List<string>();
    fakeDragons.AddRange(handDragons);
    fakeDragons.AddRange(huroTiles);
    foreach (string dragon in fakeDragons)
    {
        if (dragon[0].Equals('t') || dragon[0].Equals('d') || dragon[0].Equals('a') || dragon[0].Equals('m'))
        {
            if (dragon[2].Equals(dragon[3]))
            { 
                if(dragon[2].Equals('e')) result += 2;//게임 진행되면 이것도 바꿔야함
                if (dragon[2].Equals('p')) ++result;
                if (dragon[2].Equals('f')) ++result;
                if (dragon[2].Equals('c')) ++result;
            }
        }
    }
    return result;
}
//2판
static bool DoubleRiichi(int howLong)
{
    return false;//구현 어케하지
}

static bool Chantaiyao(List<string> handDragons, List<string> huroTiles)//챤타 모든 패에 귀족패 들어있기
{
    List<string> honorTiles = new List<string> { "m1", "m9", "p1", "p9", "s1", "s9", "e", "s", "p", "n", "p", "f", "c" };
    List<string> fakeDragon = new List<string>();
    fakeDragon.AddRange(handDragons);
    fakeDragon.AddRange(huroTiles);
    foreach (string dragon in fakeDragon)
    {
        if (dragon[0].Equals('s'))
        {
            if (honorTiles.Contains(dragon[2..4]) || honorTiles.Contains(dragon[6..8])) continue;
            else return false;
        }
        else
        {
            if (dragon[2].Equals(dragon[3]))
            {
                if (!honorTiles.Contains(dragon[3].ToString())) return false;
            }
            else
            {
                if (!honorTiles.Contains(dragon[2..4])) return false;
            }
        }
    }
    return true;
}
static bool SanshokuDoujun(List<string> handDragons, List<string> huroTiles)//삼색동순
{
    List<string> fakeDragon = new List<string>();
    List<string> sequences = new List<string>();
    fakeDragon.AddRange(handDragons);
    fakeDragon.AddRange(huroTiles);
    foreach (string dragon in fakeDragon)
    {
        if (dragon[0].Equals('s'))
        {
            sequences.Add(dragon);
        }
    }
    if (sequences.Count == 4)
    {
        if (SanshokuChk(sequences[0], sequences[1], sequences[2])) return true;
        if (SanshokuChk(sequences[0], sequences[1], sequences[3])) return true;
        if (SanshokuChk(sequences[0], sequences[2], sequences[3])) return true;
        if (SanshokuChk(sequences[1], sequences[2], sequences[3])) return true;
    }
    else if (sequences.Count == 3)
    {
        if (SanshokuChk(sequences[0], sequences[1], sequences[2])) return true;
    }
    else return false;
    return false;
}
static bool SanshokuDoukou(List<string> handDragons, List<string> huroTiles)//삼색동각
{
    List<string> fakeDragon = new List<string>();
    List<string> triplets = new List<string>();
    fakeDragon.AddRange(handDragons);
    fakeDragon.AddRange(huroTiles);
    foreach (string dragon in fakeDragon)
    {
        if (dragon[0].Equals('t') || dragon[0].Equals('d') || dragon[0].Equals('m') || dragon[0].Equals('a'))
        {
            triplets.Add(dragon);
        }
    }
    if (triplets.Count == 4)
    {
        if (SanshokuChk(triplets[0], triplets[1], triplets[2])) return true;
        if (SanshokuChk(triplets[0], triplets[1], triplets[3])) return true;
        if (SanshokuChk(triplets[0], triplets[2], triplets[3])) return true;
        if (SanshokuChk(triplets[1], triplets[2], triplets[3])) return true;
    }
    else if (triplets.Count == 3)
    {
        if (SanshokuChk(triplets[0], triplets[1], triplets[2])) return true;
    }
    else return false;
    return false;
}
static bool SanshokuChk(string a, string b, string c)
{
    if (!(a[2].Equals(b[2])) && !(a[2].Equals(c[2])) && !(b[2].Equals(c[2])))
    {
        if (a[3].Equals(b[3]) && a[3].Equals(c[3]) && b[3].Equals(c[3])) return true;
    }
    return false;
}
static bool Ittsu(List<string> handDragons, List<string> huroTiles)//일기통관
{
    List<string> fakeDragon = new List<string>();
    List<string> sequences = new List<string>();
    fakeDragon.AddRange(handDragons);
    fakeDragon.AddRange(huroTiles);
    foreach (string dragon in fakeDragon)
    {
        if (dragon[0].Equals('s'))
        {
            sequences.Add(dragon);
        }
    }
    if (sequences.Count == 4)
    {
        if (IttsuChk(sequences[0], sequences[1], sequences[2])) return true;
        if (IttsuChk(sequences[0], sequences[1], sequences[3])) return true;
        if (IttsuChk(sequences[0], sequences[2], sequences[3])) return true;
        if (IttsuChk(sequences[1], sequences[2], sequences[3])) return true;
    }
    else if (sequences.Count == 3)
    {
        if (SanshokuChk(sequences[0], sequences[1], sequences[2])) return true;
    }
    return false;
}
static bool IttsuChk(string a, string b, string c)
{
    List<char> ittsu = new List<char> {'1', '4', '7'};
    if (a[2].Equals(b[2]) && a[2].Equals(c[2]) && b[2].Equals(c[2]))
    {
        if (ittsu.Contains(a[3]) && ittsu.Contains(b[3]) && ittsu.Contains(c[3]))
        {
            if (!a[3].Equals(b[3]) && a[3] + b[3] + c[3] == 156) return true;
        }
    }
    return false;
}
static bool Toitoi(List<string> handDragons, List<string> huroTiles)//또이또이
{
    List<string> fakeDragon = new List<string>();
    fakeDragon.AddRange(handDragons);
    fakeDragon.AddRange(huroTiles);
    int cnt = 0;
    foreach (string dragon in fakeDragon)
    {
        if (dragon[0].Equals('t') || dragon[0].Equals('d') || dragon[0].Equals('m') || dragon[0].Equals('a'))
        {
            ++cnt;
        }
    }

    if (cnt == 4) return true;
    return false;
}
static bool Sanankou(List<string> handDragons, List<string> huroTiles)//산안커
{
    int cnt = 0;
    foreach (string dragon in handDragons)
    {
        if (dragon[0].Equals('t')) ++cnt;
    }

    foreach (string dragon in huroTiles)
    {
        if (dragon[0].Equals('a')) ++cnt;
    }

    if (cnt >= 3) return true;
    return false;
}
static bool Sankantsu(List<string> huroTiles)//산깡즈
{
    int cnt = 0;
    foreach (string dragon in huroTiles)
    {
        if (dragon[0].Equals('a')) ++cnt;
        else if (dragon[0].Equals('d')) ++cnt;
        else if (dragon[0].Equals('m')) ++cnt;
    }
    if (cnt >= 3) return true;
    return false;
}
static bool Honroutou(List<string> hand, string winTile)//혼노두 - 모든패가 귀족
{
    List<string> honorTiles = new List<string> { "m1", "m9", "p1", "p9", "s1", "s9", "e", "s", "p", "n", "p", "f", "c" };
    List<string> fakeHand = new List<string>();
    fakeHand.AddRange(hand);
    fakeHand.Add(winTile);
    foreach (string tile in fakeHand)
    {
        if (!honorTiles.Contains(tile)) return false;
    }
    return true;
}

public static bool Chiitoitsu(List<string> hand, string winTile)//치또이츠
{
    List<string> fakeHand = new List<string>();
    fakeHand.AddRange(hand);
    fakeHand.Add(winTile);
    fakeHand = HandArranger(fakeHand);
    if (WhatIsPair(fakeHand).Distinct().ToList().Count == 7) return true;
    return false;
}
static bool Shousangen(List<string> handDragons, List<string> huroTiles)//소삼원
{
    List<string> fakeDragon = new List<string>();
    List<string> shortDragon = new List<string>();
    fakeDragon.AddRange(handDragons);
    fakeDragon.AddRange(huroTiles);
    foreach (string dragon in fakeDragon)
    {
        switch (dragon[0])
        {
            case 'h':
                shortDragon.Add(dragon[..4]);
                break;
            case 's':
                break;
            default:
                shortDragon.Add("t" + dragon[1..4]);
                break;
        }
    }
    if (shortDragon.Contains("t-pp") && shortDragon.Contains("t-ff") && shortDragon.Contains("h-cc")) return true;
    if (shortDragon.Contains("t-cc") && shortDragon.Contains("t-pp") && shortDragon.Contains("h-ff")) return true;
    if (shortDragon.Contains("t-ff") && shortDragon.Contains("t-cc") && shortDragon.Contains("h-pp")) return true;
    return false;
}
//3판
static bool Honitsu(List<string> hand, string winTile)//혼일색
{
    List<string> fakeHand = new List<string>();
    fakeHand.AddRange(hand);
    fakeHand.Add(winTile);
    string single = "";
    foreach (string tile in fakeHand)
    {
        if (tile.Length > 1)
        {
            if (single == "")
            {
                single = tile[0].ToString();
            }
            else
            {
                if (!tile[0].ToString().Equals(single)) return false;
            }
        }
    }
    return true;
}
static bool JunchanTaiyao(List<string> handDragons, List<string> huroTiles)//준챤타 - 머리 몸통 모두 노두(1, 9)
{
    List<string> terminalTiles = new List<string> { "m1", "m9", "p1", "p9", "s1", "s9" };
    List<string> fakeDragon = new List<string>();
    fakeDragon.AddRange(handDragons);
    fakeDragon.AddRange(huroTiles);
    foreach (string dragon in fakeDragon)
    {
        if (dragon[0].Equals('s'))
        {
            if (terminalTiles.Contains(dragon[2..4]) || terminalTiles.Contains(dragon[6..8])) continue;
            else return false;
        }
        else
        {
            if (dragon[2].Equals(dragon[3])) return false;
            else
            {
                if (!terminalTiles.Contains(dragon[2..4])) return false;
            }
        }
    }
    return true;
}
static bool Ryanpeikou(List<string> handDragons, List<string> huroTiles)//량페코
{
    List<string> fakeDragon = new List<string>();
    List<string> sequences = new List<string>();
    fakeDragon.AddRange(handDragons);
    fakeDragon.AddRange(huroTiles);
    foreach (string dragon in fakeDragon)
    {
        if(dragon[0].Equals('s')) sequences.Add(dragon);
    }
    if (sequences.Count != 4)
    {
        return false;
    }
    else
    {
        if (Lipeikou(new List<string> { sequences[0] }, new List<string> { sequences[1] }))
            if (Lipeikou(new List<string> { sequences[2] }, new List<string> { sequences[3] }))
                return true;
    }
    return false;
}
//6판
static bool Chinitsu(List<string> hand, string winTile)//청일색
{
    if (winTile.Length < 2) return false;
    foreach (string tile in hand)
    {
        if (tile.Length < 2 || !tile[0].Equals(winTile[0])) return false;
    }
    return true;
}

//역만


public static int GetBusu(List<string> hand, List<string> handDragons, List<string> huroTiles, List<string> canHuroTiles, bool isMyTurn, bool isMenzen)
{
    if (Pinfu(handDragons, canHuroTiles, isMenzen)) return 20;
    int busu = 20;
    if (isMenzen) busu += 10;
    if (isMyTurn) busu += 2;
    if (canHuroTiles.Any())
    {
        if (canHuroTiles.Count == 1) busu += 4;
    }
    foreach (string dragon in handDragons)
    {
        if (dragon[0].Equals('t'))
        {
            if (dragon[2].Equals(dragon[3])) busu += 8;
            else if(dragon[3].Equals('1') || dragon[3].Equals('9')) busu += 8;
            else busu += 4;
        }
        if (dragon[0].Equals('h'))
        {
            if (dragon[2].Equals(dragon[3]))
            {
                if (dragon[3].Equals('e')) busu += 4;
                else if(dragon[3].Equals('p') || dragon[3].Equals('f') || dragon[3].Equals('c')) busu += 2;
            }
        }
    }
    foreach (string dragon in huroTiles)
    {
        switch (dragon[0])
        {
            case 't':
                if (dragon[2].Equals(dragon[3])) busu += 4;
                else if(dragon[3].Equals('1') || dragon[3].Equals('9')) busu += 4;
                else busu += 2;
                break;
            case 'd':
                if (dragon[2].Equals(dragon[3])) busu += 8;
                else if(dragon[3].Equals('1') || dragon[3].Equals('9')) busu += 8;
                else busu += 4;
                break;
            case 'm':
                if (dragon[2].Equals(dragon[3])) busu += 8;
                else if(dragon[3].Equals('1') || dragon[3].Equals('9')) busu += 8;
                else busu += 4;
                break;
            case 'a':
                if (dragon[2].Equals(dragon[3])) busu += 16;
                else if(dragon[3].Equals('1') || dragon[3].Equals('9')) busu += 16;
                else busu += 8;
                break;
        }
    }
    if (busu % 10 != 0)
    {
        busu /= 10;
        busu = busu * 10 + 10;
    }
    return busu;
}

public static List<string> HandArranger(List<string> hand)
{
    List<string> sortSequence = new List<string>
    {
        "m1", "m2", "m3", "m4", "m5", "m5r", "m6", "m7", "m8", "m9",
        "p1", "p2", "p3", "p4", "p5", "p5r", "p6", "p7", "p8", "p9",
        "s1", "s2", "s3", "s4", "s5", "s5r", "s6", "s7", "s8", "s9",
        "e", "s", "w", "n", "p", "f", "c"
    };
    List<string> tmp = new List<string>();
    foreach (string searchValue in sortSequence)
    {
        List<string> matchingValues = hand.FindAll(item => item == searchValue);
        tmp.AddRange(matchingValues);
    }
    return tmp;
}
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public static bool IsKokushiMusou(List<string> hand)
    {
        const string kokushiMusou = "m1,m9,p1,p9,s1,s9,e,s,w,n,p,f,c,";
        string tiles = hand.Distinct().ToList().Aggregate("", (current, tile) => current + tile + ",");
        return tiles.Equals(kokushiMusou) && hand.Count == 14;
    }
}