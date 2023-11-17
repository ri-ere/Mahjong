using System.Collections.Generic;
using UnityEngine;
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

    public static void TsumoDisplay(string tile, int user)
    {
        string handName = "User" + user + "Hand";
        GameObject userHand = GameObject.Find(handName);
        userHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        TileSpawner(tile, user, user.ToString(), new Vector3(3.8f, -4.0f, 0), new Vector3(0.75f, 0.75f, 0), userHand);
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
        float xRiichiPosCorrector = 0.06f;
        float yRiichiPosCorrector = 0.06f;
        float[] xRiichiPos = {-1.25f, -1.7f, -2.15f, -2.6f, -3.05f, -3.5f};
        float[] yRiichiPos = {-1.19f, -1.64f, -2.09f, -2.54f, -2.99f, -3.44f};
        int x = howMany % 6;
        int y = howMany / 6;
        if (isRiichi)
        {
            
        }
        //뒷면이 아니라 표시해야해서 user 0으로 설정
        TileSpawner(tile, 0, "Discard", new Vector3(xPos[x], yPos[y], 0), new Vector3(0.5f, 0.5f, 0), userDiscard);
        //유저 위치에 맞게 방향 돌리기
        userDiscard.transform.rotation = Quaternion.Euler(new Vector3(0, 0, user * 90));
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
}
//ㄱ 장르 ab커플링