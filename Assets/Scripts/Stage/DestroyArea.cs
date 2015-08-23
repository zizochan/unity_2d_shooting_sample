using UnityEngine;
using System.Collections;

public class DestroyArea : MonoBehaviour {
	private Score scoreObject;

	void Start()
	{
		scoreObject = FindObjectOfType<Score>();
	}

	void OnTriggerExit2D(Collider2D c)
	{
		string layerName = LayerMask.LayerToName(c.gameObject.layer);
		if (layerName == "Enemy") {
			// ボスは再出現させるためにここでは消さない
			if (c.GetComponent<Enemy>().isBoss) {
				return;
			// ザコを逃したペナルティとして難易度上昇する
			} else {
				scoreObject.RiseGameLevelByEnemyEscapePenalty();
			}
		}

		Destroy(c.gameObject);
	}
}
