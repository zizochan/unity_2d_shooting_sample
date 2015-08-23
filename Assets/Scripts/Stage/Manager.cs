using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	public GameObject player;
	private bool isPlaying = false;
	private Message message;

	// Use this for initialization
	void Start () {
		message = FindObjectOfType<Message>();
		GameStart();
	}

	void GameStart()
	{
		isPlaying = true;
		Instantiate(player, player.transform.position, player.transform.rotation);
	}

        void OnGUI()
        {
                if (IsPlaying() == false && Event.current.type == EventType.MouseDown) {
			FindObjectOfType<Score>().Save();
                        GoToTitleScene();
                }
        }

	public void GameOver()
	{
		message.ShowMessages("GAME OVER", "touch to return title");
		StartCoroutine("GameClose");
	}

	public bool IsPlaying()
	{
		return isPlaying;
	}

	public void GoToTitleScene()
	{
		Application.LoadLevel ("Title");
	}

	public void GameClear()
	{
		GameClose();
		message.ShowMessages("GAME CLEAR", "congratulations!!");
		StartCoroutine("GameClose");
	}

	// 終了処理
        private IEnumerator GameClose()
	{
		isPlaying = false;
                yield return new WaitForSeconds(8);
		FindObjectOfType<Score>().Save();
                GoToTitleScene();
	}
}
