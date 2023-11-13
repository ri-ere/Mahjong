using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private GameObject _chiButton;
    private GameObject _pongButton;
    private GameObject _kanButton;
    private GameObject _ronButton;
    private GameObject _riichiButton;
    private GamePlay _gamePlay;
    // Start is called before the first frame update
    private void Start()
    {
        _chiButton = GameObject.Find("ChiButton");
        _pongButton = GameObject.Find("PongButton");
        _kanButton = GameObject.Find("KanButton");
        _ronButton = GameObject.Find("RonButton");
        _riichiButton = GameObject.Find("RiichiButton");
        _chiButton.SetActive(false);
        _pongButton.SetActive(false);
        _kanButton.SetActive(false);
        _ronButton.SetActive(false);
        _riichiButton.SetActive(false);

    }

    public void ChiBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        Debug.Log("Chi button clicked");
    }
    public void PongBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        Debug.Log("Pong button clicked");
    }
    public void KanBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        Debug.Log("Kan button clicked");
    }
    public void RonBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        Debug.Log("Ron button clicked");
        _gamePlay.UserWin();
    }
    public void RiichiBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        Debug.Log("Riichi button clicked");
    }
    public void ChiBtnActivate()
    {
        _chiButton.SetActive(true);
    }
    public void PongBtnActivate()
    {
        _pongButton.SetActive(true);
    }
    public void KanBtnActivate()
    {
        _kanButton.SetActive(true);
    }
    public void RonBtnActivate()
    {
        _ronButton.SetActive(true);
    }
    public void RiichiBtnActivate()
    {
        _riichiButton.SetActive(true);
    }
    public void AllBtnDeactivate()
    {
        _chiButton.SetActive(false);
        _pongButton.SetActive(false);
        _kanButton.SetActive(false);
        _ronButton.SetActive(false);
        _riichiButton.SetActive(false);
    }
}
