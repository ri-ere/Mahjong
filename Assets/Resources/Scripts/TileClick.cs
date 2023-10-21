using UnityEngine;
public class TileClick : MonoBehaviour
{
    private GamePlay _gamePlay;
    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;
    void Start()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
    }
    private void OnMouseUp()
    {
        // 오브젝트를 클릭했을 때 실행되는 코드
        if (transform.position.y <= -4f)
        {
            // transform.position += new Vector3(0, 0.2f, 0);
        }
        if((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;
        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }

    void Update()
    {
        if(isDoubleClicked && _gamePlay.GetIsMyTurn() && tag.Equals("0"))
        {
            string[] tile = name.Split("(");//오브젝트가 ~~(Clone)으로 생성돼서 "("를 기준으로 자름
            int user = int.Parse(tag);
            if(user == 0) _gamePlay.Dahai(tile[0], user);//잘린 문자열이 0번에 들어있어서 tile[0]을 넘김
            isDoubleClicked = false;
        }
    }
}
