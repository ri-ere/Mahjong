using UnityEngine;

public class RiichiButton : MonoBehaviour
{
    private GameObject _riichiButton;
    // Start is called before the first frame update
    private void Start()
    {
        _riichiButton = GameObject.Find("RiichiButton");
        // _riichiButton.SetActive(false);
        
    }
    
    public void RiichiBtnClicked()
    {
        Debug.Log("riichi button clicked");
    }
}
