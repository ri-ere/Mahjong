using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
	
    private GameState _gameState = new GameState();
	private readonly List<Player> _players = new List<Player>();
	private int _oya;
	private int _nowWind;//gamestate로 옮기기
	public GamePlay gamePlayObject;
    void Start()
    {
        _players.Add(new Player(0));
        _players.Add(new Player(1));
        _players.Add(new Player(2));
        _players.Add(new Player(3));
		_oya = 0;
		_nowWind = 0;

		GameObject gamePlayPrefab = Resources.Load<GameObject>("Prefabs/GamePlay");
		Instantiate(gamePlayPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		gamePlayObject = GameObject.Find("GamePlay(Clone)").GetComponent<GamePlay>();
		// StartCoroutine(MyStartCoroutine());
		
    }

    public List<Player> GetPlayers()
    {
	    return _players;
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
			    if (++_oya == 4)
			    {
				    if (++_nowWind == 2) SceneManager.LoadScene("EndScene");
				    _oya %= 4;
			    }
		    }
		    //gamePlayObject.Destroy();
	    }
    }
    public int GetOya()
    {
	    return _oya;
    }
    public int GetNowWind()
    {
	    return _nowWind;
    }
}
