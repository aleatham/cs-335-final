using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour {

	// Use this for initialization
	private bool jump;
	public float jumpForce = 3f;
	protected Rigidbody2D rb;
	protected SpriteRenderer spR;
	protected bool renderedOnce = false;
	protected Camera cam;
	protected float maxDiag = 7.5f;

	protected Vector2 cEDist;
	private static readonly Color[] colors = {
		Color.black,
		Color.blue,
		Color.cyan,
		Color.grey,
		Color.green,
		Color.magenta,
		Color.red,
		Color.white,
		Color.yellow,
		new Color(1f, 0.45f, 0.66f)
	};
	private int color;
	public int AgentColor {
		set {
			// clamp between 0 and 9
			value = value > 9 ? 9 : value;
			value = value < 0 ? 0 : value;
			// set instance variable
			color = value;
			// set sprite color
			spR.color = colors[color];
		}
		get {
			return color;
		}
	}

    void Awake() {
		Init();
		rb.isKinematic = true;
	}
	
	protected void Init() {
		cam = Camera.main;
		rb = GetComponent<Rigidbody2D>();
		spR = GetComponent<SpriteRenderer>();
	}

	public void RandomColor() {
		color = (int)Random.Range(0f, 9f);
		spR.color = colors[color];
	}

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!renderedOnce && spR.isVisible) {
			renderedOnce = true;
		}
		CheckDeath();
	}

	public void Jump() {
		if (rb.isKinematic && GameMaster.GetState() == GameMaster.GameState.PLAY) {
			rb.isKinematic = false;
		}
		rb.AddForce(new Vector2(0f, jumpForce));
	}

	protected void CheckDeath() {
		if ((!spR.isVisible && renderedOnce)) {
			GameMaster.KillAgent(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == GameMaster.enemyTag) {
			GameMaster.KillAgent(col.gameObject);
			GameMaster.KillAgent(gameObject);
		}
	}

	// Height from bottom
	public float GetHeight() {
		return cam.orthographicSize + transform.position.y;
	}

	public float GetDistFromTop() {
		return cam.orthographicSize - transform.position.y;
	}

	public Vector2 GetClosestEnemyDist() {
		//TODO: we dont care about fish behind the agent
		//TODO: reutrn - if below and + if above to score better
		GameObject[] gos;
		// GameObject closest = null;
		gos = GameObject.FindGameObjectsWithTag(GameMaster.enemyTag);
		float dist = Mathf.Infinity;
		Vector2 position = transform.position;
		foreach (GameObject go in gos) {
			Vector2 dif = (Vector2) (go.transform.position) - position;
			float curDistance = dif.sqrMagnitude;

			// x > -1 makes sure we're only looking at fish in front of the agent
			//TODO: what is body size use that instead of -1
			if (curDistance < dist && dif.x > -1) {
				cEDist = dif;
				// closest = go;
				dist = curDistance;

				// enemy is below agent
				// so invert dist
				if (dif.y < 0) {
					dist = dist * -1;
				}
			}
		}
		// return dist < Mathf.Infinity ? dist : 0;
		return cEDist;
	}

	// This is the training score for the AI to learn from
	public float ScorePosition() {
		float h = GetHeight();
		// just auto jump if we're at the bottom
		// or auto not jump if near the top
		if (h < 3)
			return 1;
		if (h > 7)
			return 0;
		
		GetClosestEnemyDist();
		// if enemy is close enough to react to
		if ((cEDist.x > -1 && cEDist.x < 2f) &&
			(cEDist.y < 1.2f || cEDist.y > -1.2f)) {
			if (cEDist.y > 0)
				return 0;
			else {
				return 1;
			}
		}
		// if we make it down here just fly as normal towards middle
		if (h < 5)
			return 1;

		// otherwise just fall
		return 0;
	}
}
