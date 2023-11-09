using UnityEngine;

public class RiichiButton : MonoBehaviour
{
    private GameObject _riichiButton;
    private GamePlay _gamePlay;
    // Start is called before the first frame update
    private void Start()
    {
        _riichiButton = GameObject.Find("RiichiButton");
        _riichiButton.SetActive(false);

    }
    
    public void RiichiBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        Debug.Log("riichi button clicked");
    }
}
