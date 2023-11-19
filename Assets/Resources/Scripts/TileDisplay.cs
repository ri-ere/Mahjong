using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TileDisplay : MonoBehaviour
{
    //타일 오브젝트 생성하는 함수
    private static void TileSpawner(string tileName, int user,string tagName, Vector3 position, Vector3 scale, GameObject ownGameObject)
    {
        if (user != 0) tileName = "back";
        GameObject tile = Resources.Load<GameObject>("Prefabs/" + tileName);
        
        GameObject spawnedTile = Instantiate(tile, ownGameObject.transform);//타일 오브젝트 생성
        spawnedTile.transform.position += position;
        spawnedTile.transform.localScale = scale;//타일 크기 변경
        spawnedTile.tag = tagName;
    }
    public static void HandDisplay(List<string> hand, int user)
    {
        float xPos = -3.0f;
        string handName = "User" + user + "Hand";
        GameObject userHand = GameObject.Find(handName);
        //이전에 생성된 타일들 삭제 및 로테이션 초기화
        userHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        foreach (var child in userHand.GetComponentsInChildren<Transform>())
        {
            if(child != userHand.transform) Destroy(child.gameObject); // 하위 오브젝트 제거
        }
        //타일 생성하는 반복문
        foreach (string tile in hand)
        {
            TileSpawner(tile, user,user.ToString(), new Vector3(xPos, -4f, 0), new Vector3(0.75f, 0.75f, 0), userHand);
            xPos += 0.5f;
        }
        //유저 위치에 맞게 방향 돌리기
        userHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, user * 90));
    }

    public static void TsumoDisplay(string tile, int user, int huroCount)
    {
        //후로 1개 2.25, 2개 0.75, 3개면 -0.9
        List<float> huroCorrector = new List<float> { 0, -1.55f, -3.05f, -4.7f, -6.25f };
        string handName = "User" + user + "Hand";
        GameObject userHand = GameObject.Find(handName);
        userHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        TileSpawner(tile, user, user.ToString(), new Vector3(3.8f + huroCorrector[huroCount], -4.0f, 0), new Vector3(0.75f, 0.75f, 0), userHand);
        //유저 위치에 맞게 방향 돌리기
        userHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, user * 90));
    }
    public static void DahaiDisplay(string tile,int user, int howMany, bool isRiichi)
    {
        // TODO: 리치하면 그 줄 더 밀어서 밖으로 나오게 만들어야함
        string userName = "User" + user + "Discard";
        bool isRiichiLine = false;
        GameObject userDiscard = GameObject.Find(userName);
        userDiscard.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        float[] xPos = {-0.83f, -0.5f, -0.17f, 0.16f, 0.5f, 0.83f};
        float[] yPos = {-1.25f, -1.7f, -2.15f, -2.6f, -3.05f, -3.5f};
        float riichiPosCorrector = 0.06f;
        float[] xRiichiPos = {-1.25f, -1.7f, -2.15f, -2.6f, -3.05f, -3.5f};
        float[] yRiichiPos = {-1.19f, -1.64f, -2.09f, -2.54f, -2.99f, -3.44f};
        int x = howMany % 6;
        int y = howMany / 6;
        if (isRiichi)
        {
            //로테이션 z 값 90으로 변경
            TileSpawner(tile, 0, "Discard", new Vector3(xPos[x] + riichiPosCorrector, yPos[y] + riichiPosCorrector, 0), new Vector3(0.5f, 0.5f, 0), userDiscard);
        }
        //뒷면이 아니라 표시해야해서 user 0으로 설정
        else TileSpawner(tile, 0, "Discard", new Vector3(xPos[x], yPos[y], 0), new Vector3(0.5f, 0.5f, 0), userDiscard);
        //유저 위치에 맞게 방향 돌리기
        userDiscard.transform.rotation = Quaternion.Euler(new Vector3(0, 0, user * 90));
    }

    public static void RemoveStolenTile(int user)
    {
        Transform discardDeck = GameObject.Find("User" + user + "Discard").transform;
        Transform tile = discardDeck.GetChild(discardDeck.childCount - 1);
        Destroy(tile.gameObject);
    }
    public static void DoraDisplay(string tile, int howMany, bool isOpen)
    {
        //도라는 유저 5로 생성해서 타일 생성기에서 뒷면으로 만들기
        int isBack = isOpen ? 0 : 5;
        GameObject dora = GameObject.Find("Dora");
        dora.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //xPos는 전부 같아서 직접 입력
        float[] yPos = {5.33f, 5.99f, 6.65f, 7.31f, 7.97f};
        TileSpawner(tile, isBack, "Dora", new Vector3(yPos[howMany], 4.55f, 0), new Vector3(1f, 1f, 0), dora);
    }

    public static void HuroDisplay(List<string> huroTiles)
    {
        //츠모한 타일 위치도 바꿔야함
        // x 누워있는거랑 그냥 있는거 차이 : 0.6
        // x 그냥 있는거 2개 차이 : 0.5
        // x 그냥 있는거 첫번째꺼 4.25
        //안깡 4개 0번 4.1에서 0.5씩 4개 세우면 됨
        // y : 누워 있는거 -4.25, 똑바로 있는거 -4.16
        // 받아온건 로테이션 z = 90
        GameObject userHuro = GameObject.Find("User0Huro");
        //먼저 만들어놓은거 제거
        if (userHuro.transform.childCount > 0)
        {
            for (int i = 0; i < userHuro.transform.childCount; i++)
            {
                Transform child = userHuro.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
        userHuro.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        string first = "";
        string second = "";
        string third = "";
        string fourth = "";
        List<List<float>> tilePosition = new List<List<float>>();
        float yPos = -4.16f;
        tilePosition.Add(new List<float> {4.25f, 3.75f, 3.25f});
        tilePosition.Add(new List<float> {2.58f, 2.08f, 1.58f});
        tilePosition.Add(new List<float> {0.9f, 0.4f, -0.1f});
        tilePosition.Add(new List<float> {-0.78f, -1.28f, -1.78f});
        List<float> xCorrector = new List<float> { -0.09f, -0.18f, -0.18f };
        float yCorrector = -0.09f;
        //할일 포지션 다시 잡고 어떻게 위치 잡아서 후로 타일 할지 생각하기
        //후로 타일 t-ee-ee1 이런식으로 받을거
        //s-p1p2p3-p22 12문자
        //t-ee-ee1 8문자?
        //d-p2-p20 대명깡이라서 마지막이 0
        for (int i = 0; i < huroTiles.Count; i++)
        {
            if (huroTiles[i][0] == 'd' || huroTiles[i][0] == 'a')
            {
                
            }
            else
            {
                switch (huroTiles[i][0])
                {
                    case 's'://슌츠
                        Debug.Log("shunnz");
                        first = huroTiles[i][2..4];
                        second = huroTiles[i][4..6];
                        third = huroTiles[i][6..8];
                        string tmp;
                        if (huroTiles[i][^3..^1].Equals(second))
                        {
                            tmp = first;
                            first = second;
                            second = tmp;
                        }
                        else if (huroTiles[i][^3..^1].Equals(third))
                        {
                            tmp = first;
                            first = third;
                            third = second;
                            second = tmp;

                        }
                        break;
                    case 't'://커츠
                        Debug.Log("cuzz");
                        if (huroTiles[i][2].ToString().Equals(huroTiles[i][3].ToString()))
                            first = huroTiles[i][2].ToString();
                        else first = huroTiles[i][2..4];
                        second = first;
                        third = second;
                        break;
                }
                GameObject tile1 = Resources.Load<GameObject>("Prefabs/" + first);
                GameObject tile2 = Resources.Load<GameObject>("Prefabs/" + second);
                GameObject tile3 = Resources.Load<GameObject>("Prefabs/" + third);
                List<GameObject> spawnedTile = new List<GameObject>();
                spawnedTile.Add(Instantiate(tile1, userHuro.transform));
                spawnedTile.Add(Instantiate(tile2, userHuro.transform));
                spawnedTile.Add(Instantiate(tile3, userHuro.transform));
                for (int j = 0; j < 3; j++)
                {
                    spawnedTile[j].tag = "User0Huro";
                    spawnedTile[j].transform.position = new Vector3(tilePosition[i][j], yPos, 0);
                    spawnedTile[j].transform.localScale = new Vector3(0.75f, 0.75f, 0);
                }

                int cnt = 0;
                for (int j = huroTiles[i][^1] - 49; j <= 2; ++j)
                {
                    if (j == huroTiles[i][^1] - 49)
                    {
                        spawnedTile[j].transform.position = new Vector3(tilePosition[i][j] + xCorrector[cnt], yPos + yCorrector, 0);
                        spawnedTile[j].transform.localRotation = Quaternion.Euler(0, 0, 90f);
                    }
                    else
                    {
                        spawnedTile[j].transform.position = new Vector3(tilePosition[i][j] + xCorrector[cnt], yPos, 0);
                    }
                    ++cnt;
                }
            }
        }
    }

    public static void WhatToChiDisplay(List<string> chiPoss)
    {
        GameObject userChiPoss = GameObject.Find("User0ChiPoss");
        userChiPoss.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        List<float> xPos = new List<float> {5.5f, 6.09f, 6.59f };
        List<float> yPos = new List<float> {-4.16f, -3.46f, -2.76f };
        float yCorrector = -0.09f;
        for (int i = 0; i < chiPoss.Count; i++)
        {
            string first = chiPoss[i][..2];
            string second = chiPoss[i][2..4];
            string third = chiPoss[i][4..6];
            GameObject tile1 = Resources.Load<GameObject>("Prefabs/" + first);
            GameObject tile2 = Resources.Load<GameObject>("Prefabs/" + second);
            GameObject tile3 = Resources.Load<GameObject>("Prefabs/" + third);
            List<GameObject> spawnedTile = new List<GameObject>();
            spawnedTile.Add(Instantiate(tile1, userChiPoss.transform));
            spawnedTile.Add(Instantiate(tile2, userChiPoss.transform));
            spawnedTile.Add(Instantiate(tile3, userChiPoss.transform));
            for (int j = 0; j < 3; j++)
            {
                spawnedTile[j].tag = "Huro" + i.ToString();
                if (j == 0)
                {
                    spawnedTile[j].transform.position = new Vector3(xPos[j], yPos[i] + yCorrector, 0);
                    spawnedTile[j].transform.localRotation = Quaternion.Euler(0, 0, 90f);
                }
                else spawnedTile[j].transform.position = new Vector3(xPos[j], yPos[i], 0);
                spawnedTile[j].transform.localScale = new Vector3(0.75f, 0.75f, 0);
            }
        }
    }
}