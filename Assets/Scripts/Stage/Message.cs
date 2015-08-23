using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Message : MonoBehaviour {
	public Text mainMessageText;
	public Text subMessageText;
	private IEnumerator coroutine;

	// Use this for initialization
	void Start () {
		HiddenMessages();
	}

	// Update is called once per frame
	void Update () {
	}

	public void HiddenMessages()
	{
		mainMessageText.enabled = false;
		subMessageText.enabled = false;
	}

	public void ShowMainMessage(string message)
	{
		mainMessageText.text = message;
		mainMessageText.enabled = true;
	}

	public void ShowSubMessage(string message)
	{
		subMessageText.text = message;
		subMessageText.enabled = true;
	}

	public void ShowMessages(string mainMessage, string subMessage)
	{
		// 二重実行防止
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}

		if (mainMessage != "") {
			ShowMainMessage(mainMessage);
		}
		if (subMessage != "") {
			ShowSubMessage(subMessage);
		}
	}

	public void ShowMessagesConstantTime(string mainMessage, string subMessage, int seconds)
	{
		ShowMessages(mainMessage, subMessage);
		coroutine = _ShowMessagesConstantTime(mainMessage, subMessage, seconds);
		StartCoroutine(coroutine);
	}

	private IEnumerator _ShowMessagesConstantTime(string mainMessage, string subMessage, int seconds)
	{
		yield return new WaitForSeconds(seconds);
		HiddenMessages();
	}
}
