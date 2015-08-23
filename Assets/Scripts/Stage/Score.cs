using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {
	public Text scoreText;
	public Text highScoreText;
	public Text gameLevelText;
	private int score;
	private int highScore;
	private float gameLevel;
	private string highScoreKey = "highScore";
	private const int minGameLevel = 1;
	private const int maxGameLevel = 1000;
	private const float gameLevelRate = 0.1f; // gameLevel1毎の難易度上昇倍率
	private const float enemyEscapeRiseLevel = 0.3f; // ザコを画面下に逃した時のレベル上昇値
	private Manager manager;

	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<Manager>();
		Initialize();
	}

	// Update is called once per frame
	void Update () {
		if (highScore < score) {
			highScore = score;
		}
		scoreText.text = score.ToString();
		highScoreText.text = highScore.ToString();
	}

	private void Initialize()
	{
		score = 0;
		highScore = PlayerPrefs.GetInt(highScoreKey, 0);
		gameLevel = 1;
		setGameLevelText();
	}

	public void AddPoint(int point)
	{
		int beforeScore = score;
		score += point;
		UpdateGameLevel(beforeScore, score);
	}

	void UpdateGameLevel(int beforeScore, int afterScore)
	{
		float unit = 1000f; // この点数毎に1level up
		float diffLevel = (afterScore - beforeScore) / unit;
		if (diffLevel > 0) {
			AddGameLevel(diffLevel);
		}
	}

        public float GetGameLevelRate()
        {
                return (float)((GetGameLevel() - 1) * gameLevelRate + 1);
        }

	public void Save()
	{
		PlayerPrefs.SetInt(highScoreKey, highScore);
		PlayerPrefs.Save();
	}

	public float GetGameLevel()
	{
		return gameLevel;
	}

	public void AddGameLevel(float level)
	{
		if (manager.IsPlaying() == false) {
			return;
		}

		gameLevel += level;
		if (gameLevel > maxGameLevel) {
		  gameLevel = maxGameLevel;
		}
		setGameLevelText();
	}

	public void setGameLevelText()
	{
		gameLevelText.text = ((int)gameLevel).ToString();
	}

	public void ReduceGameLevel(int level)
	{
		gameLevel -= level;
		if (gameLevel < minGameLevel) {
		  gameLevel = minGameLevel;
		}
	}

	public void RiseGameLevelByEnemyEscapePenalty()
	{
		AddGameLevel(enemyEscapeRiseLevel);
	}
}
