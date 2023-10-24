using UnityEngine;
public class TileClick : MonoBehaviour
{
    private GamePlay _gamePlay;
    private const float Interval = 0.25f;
    private float _doubleClickedTime = -1.0f;
    private bool _isDoubleClicked = false;

    private void Start()
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
        if((Time.time - _doubleClickedTime) < Interval)
        {
            _isDoubleClicked = true;
            _doubleClickedTime = -1.0f;
        }
        else
        {
            _isDoubleClicked = false;
            _doubleClickedTime = Time.time;
        }
    }

    private void Update()
    {
        if(_isDoubleClicked && _gamePlay.GetIsMyTurn() && tag.Equals("0"))
        {
            string[] tile = name.Split("(");//오브젝트가 ~~(Clone)으로 생성돼서 "("를 기준으로 자름
            int user = int.Parse(tag);
            if(user == 0) _gamePlay.Dahai(tile[0], user);//잘린 문자열이 0번에 들어있어서 tile[0]을 넘김
            _isDoubleClicked = false;
        }
        else _isDoubleClicked = false;
    }

    public void RiichiMover()
    {
        transform.position += new Vector3(0, 0.2f, 0);
    }
    public void RiichiRemover()
    {
        transform.position -= new Vector3(0, 0.2f, 0);
    }
}
