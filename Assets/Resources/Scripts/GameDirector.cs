using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
	
    private GameState _gameState = new GameState();
	private List<Player> players = new List<Player>();
	private int oya;
	private int nowWind;//gamestate로 옮기기
	public GamePlay gamePlayObject;
    void Start()
    {
        players.Add(new Player(0));
        players.Add(new Player(1));
        players.Add(new Player(2));
        players.Add(new Player(3));
		oya = 0;
		nowWind = 0;

		GameObject gamePlayPrefab = Resources.Load<GameObject>("Prefabs/GamePlay");
		Instantiate(gamePlayPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		gamePlayObject = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
		// StartCoroutine(MyStartCoroutine());
		
    }

    public List<Player> getPlayers()
    {
	    return players;
    }

    void Update()
    {
	    // StartCoroutine(UpdateCoroutine());
	    //게임 시작
	    if (gamePlayObject.IsGameEnd())
	    {
		    Debug.Log("game end");
		    //게임 끝났을때
		    gamePlayObject.GameEnd();
		    if (!gamePlayObject.IsOyaWin())
		    {
			    if (++oya == 4)
			    {
				    if (++nowWind == 2) SceneManager.LoadScene("EndScene");
				    oya %= 4;
			    }
		    }
		    //gamePlayObject.Destroy();
	    }
    }
    IEnumerator MyStartCoroutine()
    {
	    Debug.Log("코루틴 시작");
	    yield return new WaitForSeconds(2f);
	    Debug.Log("2초 후에 출력");
    }
    IEnumerator UpdateCoroutine()
    {
	    Debug.Log("코루틴 시작");
	    yield return new WaitForSeconds(1f);
	    Debug.Log("2초 후에 출력");
    }
    public int getOya()
    {
	    return oya;
    }
    public int getNowWind()
    {
	    return nowWind;
    }
}
