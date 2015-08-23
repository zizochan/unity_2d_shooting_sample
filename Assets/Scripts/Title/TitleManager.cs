using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

        void OnGUI()
        {
                if (Event.current.type == EventType.MouseDown) {
                        GameStart();
                }
        }

	void GameStart()
	{
		Application.LoadLevel ("Stage");
	}
}
