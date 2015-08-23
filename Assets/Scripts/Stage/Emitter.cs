using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour {
	public GameObject[] waves;
	public GameObject[] bossWaves;
	private int forceWaveNumber = -1; // テスト用wave矯正指定フラグ
	private Manager manager;
	private int totalWave;
	public int bossApperWave;
	private Message message;

	// Use this for initialization
	void Start () {
		message = FindObjectOfType<Message>();
		totalWave = 0;
		manager = FindObjectOfType<Manager>();
		StartCoroutine("AddEnemy");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator AddEnemy() {
		if (waves.Length == 0) {
			yield break;
		}
		while (true) {
			while (manager.IsPlaying() == false) {
				yield return new WaitForEndOfFrame();
			}

			// ボス出現処理
			if (totalWave == bossApperWave) {
				StartCoroutine("ApperBoss");
			}

			int randomNumber = 0;
			if (forceWaveNumber >= 0) {
				randomNumber = forceWaveNumber;
			} else {
				randomNumber = Random.Range (0, waves.Length);
			}

			// 出現位置ランダム変更
			Vector2 tempPos = transform.position;
			tempPos.x += Random.Range(-1f, 1f);

			GameObject wave = (GameObject)Instantiate(waves[randomNumber], tempPos, Quaternion.identity);
			totalWave += 1;
			wave.transform.parent = transform;
			while(wave.transform.childCount != 0) {
				yield return new WaitForEndOfFrame();
			}
			Destroy(wave);
		}
	}

	private IEnumerator ApperBoss()
	{
		// ボス出現演出
		message.ShowMessagesConstantTime("WARNING !", "BOSS APPEARANCE", 3);
		yield return new WaitForSeconds(3);

		// 種類は1種類決め打ち
		Instantiate(bossWaves[0], transform.position, Quaternion.identity);

		yield break;
	}
}
