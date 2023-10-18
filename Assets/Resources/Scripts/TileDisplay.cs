using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TileDisplay : MonoBehaviour
{
    //타일 오브젝트 생성하는 함수
    private void tileSpawner(string tileName, int _user, Vector3 _spawn, Vector3 _scale, GameObject _userHand)
    {
        if (_user != 0) tileName = "back";
        GameObject tile = Resources.Load<GameObject>("Prefabs/" + tileName);
        
        GameObject spawnedTile = Instantiate(tile, _userHand.transform);//타일 오브젝트 생성
        spawnedTile.transform.position += _spawn;
        spawnedTile.transform.localScale = _scale;//타일 크기 변경
        spawnedTile.tag = _user.ToString();
    }
    public void handDisplay(List<string> _hand, int _user)
    {
        float xPos = -3.0f;
        string handName = "User" + _user + "Hand";
        GameObject userHand = GameObject.Find(handName);
        //이전에 생성된 타일들 삭제 및 로테이션 초기화
        userHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        foreach (var child in userHand.GetComponentsInChildren<Transform>())
        {
            if(child != userHand.transform) Destroy(child.gameObject); // 하위 오브젝트 제거
        }
        //타일 생성하는 반복문
        foreach (string tile in _hand)
        {
            tileSpawner(tile, _user, new Vector3(xPos, -4f, 0), new Vector3(0.75f, 0.75f, 0), userHand);
            xPos += 0.5f;
        }
        //유저 위치에 맞게 방향 돌리기
        userHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _user * 90));
    }

    public void tsumoDisplay(string _tile, int _user)
    {
        string handName = "User" + _user + "Hand";
        GameObject userHand = GameObject.Find(handName);
        userHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        float[] xPos = {4.0f, 4.0f, -4.0f, -4.0f};
        float[] yPos = {-4.0f, 4.0f, 4.0f, -4.0f};
        tileSpawner(_tile, _user, new Vector3(xPos[_user], yPos[_user], 0), new Vector3(0.75f, 0.75f, 0), userHand);
        //유저 위치에 맞게 방향 돌리기 
        userHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _user * 90));
    }
    public void dahaiDisplay(string _tile,int _user, int howMany)
    {
        string userName = "User" + _user + "Tsumo";
        bool isRiichiLine = false;
        bool isRiichi = false;
        GameObject userDiscard = GameObject.Find(userName);
        userDiscard.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        float[] xPos = {-1.25f, -1.7f, -2.15f, -2.6f, -3.05f, -3.5f};
        float[] xRiichiPos = {-1.25f, -1.7f, -2.15f, -2.6f, -3.05f, -3.5f};
        float[] yRiichiPos = {-1.25f, -1.7f, -2.15f, -2.6f, -3.05f, -3.5f};

        float[] yPos = {-0.83f, -0.5f, -0.17f, 0.16f, 0.5f, 0.83f};
        int x = howMany / 6;
        int y = howMany % 6;
        if (isRiichi)
        {
            
        }
        tileSpawner(_tile, 0, new Vector3(yPos[y], xPos[x], 0), new Vector3(0.5f, 0.5f, 0), userDiscard);
        //유저 위치에 맞게 방향 돌리기
        userDiscard.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _user * 90));
    }
    public void doraDisplay(string _tile, int howMany, bool isOpen)
    {
        //도라는 유저 5로 생성해서 타일 생성기에서 뒷면으로 만들기
        int isBack;
        if (isOpen) isBack = 0;
        else isBack = 5;
        GameObject dora = GameObject.Find("Dora");
        dora.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //xPos는 전부 같아서 직접 입력
        float[] yPos = {5.33f, 5.99f, 6.65f, 7.31f, 7.97f};
        tileSpawner(_tile, isBack, new Vector3(yPos[howMany], 4.55f, 0), new Vector3(1f, 1f, 0), dora);
    }
}
//ㄱ 장르 ab커플링