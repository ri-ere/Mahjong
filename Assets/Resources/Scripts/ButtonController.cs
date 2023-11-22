using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    private GameObject _returnButton;
    private GameObject _chiButton;
    private GameObject _pongButton;
    private GameObject _kanButton;
    private GameObject _ronButton;
    private GameObject _riichiButton;
    private GameObject _passButton;
    private GamePlay _gamePlay;
    // Start is called before the first frame update
    private void Start()
    {
        _returnButton = GameObject.Find("ReturnButton");
        _chiButton = GameObject.Find("ChiButton");
        _pongButton = GameObject.Find("PongButton");
        _kanButton = GameObject.Find("KanButton");
        _ronButton = GameObject.Find("RonButton");
        _riichiButton = GameObject.Find("RiichiButton");
        _passButton = GameObject.Find("PassButton");
        _returnButton.SetActive(false);
        _chiButton.SetActive(false);
        _pongButton.SetActive(false);
        _kanButton.SetActive(false);
        _ronButton.SetActive(false);
        _riichiButton.SetActive(false);
        _passButton.SetActive(false);

    }

    public void ReturnBtnClicked()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void ChiBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        _gamePlay.OnClickedChiButton();
    }
    public void PongBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        _gamePlay.OnClickedPongButton();
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
        _gamePlay.OnClickedRonButton();
    }
    public void RiichiBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        Debug.Log("Riichi button clicked");
        _gamePlay.OnClickedRiichiButton();
    }
    public void PassBtnClicked()
    {
        _gamePlay = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
        Debug.Log("Pass button clicked");
        _gamePlay.OnClickedPassButton();
    }
    public void ReturnBtnActivate()
    {
        _returnButton.SetActive(true);
    }
    public void ChiBtnActivate()
    {
        _chiButton.SetActive(true);
        _passButton.SetActive(true);
    }
    public void PongBtnActivate()
    {
        _pongButton.SetActive(true);
        _passButton.SetActive(true);
    }
    public void KanBtnActivate()
    {
        _kanButton.SetActive(true);
        _passButton.SetActive(true);
    }
    public void RonBtnActivate()
    {
        _ronButton.SetActive(true);
        _passButton.SetActive(true);
    }
    public void RiichiBtnActivate()
    {
        _riichiButton.SetActive(true);
    }
    public void PassBtnActivate()//안쓸듯?
    {
        _passButton.SetActive(true);
    }
    public void AllBtnDeactivate()
    {
        _chiButton.SetActive(false);
        _pongButton.SetActive(false);
        _kanButton.SetActive(false);
        _ronButton.SetActive(false);
        _riichiButton.SetActive(false);
        _passButton.SetActive(false);
    }
}
