using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {
	Spaceship spaceship;
	Background background;
	private Text EnergyText;

	// Use this for initialization
	void Start () {
		spaceship = GetComponent<Spaceship>();
		background = FindObjectOfType<Background>();
		if (spaceship.canShot) {
			StartCoroutine("AutoShot");
		}

		EnergyText = GameObject.Find("Energy").GetComponent<Text>();
		SetEnergyText(spaceship.hp);
	}

	// Update is called once per frame
	void Update () {
		float x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		float y = CrossPlatformInputManager.GetAxisRaw("Vertical");
		Vector2 direction = new Vector2(x, y);
		Move(direction);
		UpdateEnergyText();
	}

	private IEnumerator AutoShot()
	{
		while(true) {
			Shot(transform);
			GetComponent<AudioSource>().Play();
			yield return new WaitForSeconds(spaceship.shotDelay);
		}
	}

        public void Shot(Transform origin)
        {
                Instantiate(spaceship.bullet, origin.position, origin.rotation);
        }

	void OnTriggerEnter2D(Collider2D c)
	{
		Manager manager = FindObjectOfType<Manager>();

		// プレイ中じゃない場合はダメージを受けない
		if (manager.IsPlaying() == false) {
			return;
		}

		string layerName = LayerMask.LayerToName(c.gameObject.layer);

		if (layerName == "Bullet(Enemy)") {
			Bullet bullet = c.GetComponent<Bullet>();
			spaceship.hp -= bullet.power;
			Destroy(c.gameObject);
		} else if (layerName == "Enemy") {
			spaceship.hp = 0; // 体当たりは即死
		}

		if (layerName == "Bullet(Enemy)" || layerName == "Enemy") {
			if (spaceship.hp <= 0) {
				SetEnergyText(0);
				manager.GameOver();
				spaceship.Explosion();
				Destroy(gameObject);
			} else {
				spaceship.GetAnimator().SetTrigger("Damage");
			}
		}
	}

	public void Move(Vector2 direction)
	{
		Vector2 scale = background.transform.localScale;
		Vector2 min = scale * -0.5f;
		Vector2 max = scale * 0.5f;

		Vector2 pos = transform.position;
		pos += direction * spaceship.speed * Time.deltaTime;

		// 移動制限
		pos.x = Mathf.Clamp(pos.x, min.x, max.x);
		pos.y = Mathf.Clamp(pos.y, min.y * 0.7f, max.y * 0.6f); // 上下の移動を少し狭める
		transform.position = pos;
	}

	void UpdateEnergyText()
	{
		int textEnergy = int.Parse(EnergyText.text);
		if (textEnergy > spaceship.hp) {
			SetEnergyText(textEnergy - 1);
		} else if (textEnergy < spaceship.hp) {
			SetEnergyText(textEnergy + 1);
		}
	}

	void SetEnergyText(int energy)
	{
		EnergyText.text = energy.ToString();
	}
}
