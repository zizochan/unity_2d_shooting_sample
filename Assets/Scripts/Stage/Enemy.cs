using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	Spaceship spaceship;
        Background background;
	public int point = 100;
	public bool isBoss = false;
	private Score scoreObject;
	private float gameLevelRate;
	private Manager manager;

	// Use this for initialization
	void Start () {
		spaceship = GetComponent<Spaceship>();
                background = FindObjectOfType<Background>();
		manager = FindObjectOfType<Manager>();
		scoreObject = FindObjectOfType<Score>();
		gameLevelRate = scoreObject.GetGameLevelRate();
		ReflectGameLevel();
		Move(transform.up * -1);
		if (spaceship.canShot) {
			StartCoroutine("AutoShot");
		}
	}

	// Update is called once per frame
	void Update () {
		checkMoveRestriction();
	}

	private IEnumerator AutoShot() {
		while(true) {
			for (int i = 0; i < transform.childCount; i ++) {
				Transform shotPosition = transform.GetChild(i);
				Shot(shotPosition, gameLevelRate);
			}
			yield return new WaitForSeconds(spaceship.shotDelay);
		}
	}

        public void Shot(Transform origin, float rate = 1f)
        {
                GameObject newBullet = Instantiate(spaceship.bullet, origin.position, origin.rotation) as GameObject;
        	Bullet bulletScript = newBullet.GetComponent<Bullet>();
        	bulletScript.speed  = (int)((bulletScript.speed * rate) + spaceship.speed);
        	bulletScript.power  = (int)(bulletScript.power * rate);
        }

	void OnTriggerEnter2D(Collider2D c)
	{
		if (manager.IsPlaying() == false) {
			return;
		}

		string layerName = LayerMask.LayerToName(c.gameObject.layer);
		if (layerName != "Bullet(Player)") {
			return;
		}

		Bullet bullet = c.transform.parent.GetComponent<Bullet>();
		spaceship.hp -= bullet.power;

		Destroy(c.gameObject);

		if (spaceship.hp <= 0) {
			if (isBoss) {
				// 先にゲームクリアフラグを立てる必要がある
				manager.GameClear();
			}
			scoreObject.AddPoint(point);
			spaceship.Explosion();
			Destroy(gameObject);
		} else {
			spaceship.GetAnimator().SetTrigger("Damage");
		}
	}

	public void Move(Vector2 direction)
	{
		GetComponent<Rigidbody2D>().velocity = direction * spaceship.speed;
	}

	void ReflectGameLevel()
	{
		float gameLevelRate = scoreObject.GetGameLevelRate();
		spaceship.speed     *= gameLevelRate;
		spaceship.shotDelay /= gameLevelRate;
	}

        // 移動制限
	private void checkMoveRestriction()
	{
                Vector2 scale = background.transform.localScale;
                Vector2 min = scale * -0.5f;
                Vector2 max = scale * 0.5f;
                Vector2 pos = transform.position;

		// ボスは下まで行っても画面上部から再出現させる
		if (isBoss && pos.y < min.y * 1.5) {
                        pos.x += Random.Range(-2f, 2f);
			pos.y = scale.y * 0.7f;
                	transform.position = pos;
			transform.rotation = Quaternion.Euler(0, 0, Random.Range(-30, 30));
			Move(transform.up * -1);
		}

		// 画面端制御
		if (pos.x < min.x || pos.x > max.x) {
                	pos.x = Mathf.Clamp(pos.x, min.x, max.x);
                	transform.position = pos;

			// 反転
			transform.rotation = Quaternion.Inverse(transform.rotation);
			Move(transform.up * -1);
		}
	}
}
