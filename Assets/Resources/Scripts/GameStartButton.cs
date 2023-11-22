using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{
    public void OnClickGameStartBtn()
    {
        SceneManager.LoadScene("MainScene");
    }
}
