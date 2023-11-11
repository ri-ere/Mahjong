using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private GameObject _riichiButton;
    private GameObject _ronButton;
    private GamePlay _gamePlay;
    // Start is called before the first frame update
    private void Start()
    {
        _riichiButton = GameObject.Find("RiichiButton");
        _ronButton = GameObject.Find("RonButton");
        _riichiButton.SetActive(false);
        _ronButton.SetActive(false);

    }
    public void RonBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        Debug.Log("ron button clicked");
        _gamePlay.GameEnd();
    }
    public void RiichiBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        Debug.Log("riichi button clicked");
    }
}
