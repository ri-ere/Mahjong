using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TileClick : MonoBehaviour
{
    private GamePlay _gamePlay;
    void Start()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
    }
    private void OnMouseDown()
    {
        // 오브젝트를 클릭했을 때 실행되는 코드
        string[] tile = this.name.Split("(");//오브젝트가 ~~(Clone)으로 생성돼서 "("를 기준으로 자름
        int user = int.Parse(this.tag);
        _gamePlay.dahai(tile[0], user);//잘린 문자열이 0번에 들어있어서 tile[0]을 넘김
    }
}
