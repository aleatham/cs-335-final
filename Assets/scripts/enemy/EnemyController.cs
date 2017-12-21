using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : AgentController {
	public float minSpeed = 1f;
	public float maxSpeed = 20f;
	public float minScale = 1f;
	public float maxScale = 4f;
	private float hSpeed;
	public float scale;

	void Awake() {
		// TODO: difficulty goes up over time
		scale = Random.Range(minScale, maxScale);
		hSpeed = Random.Range(minSpeed, maxSpeed);
		Init();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!renderedOnce && spR.isVisible) {
			renderedOnce = true;
		}
		if (GameMaster.GetState() == GameMaster.GameState.PLAY) {
			// Scroll the entire background
			Vector2 newPos = transform.position;
			newPos.x -= hSpeed * Time.deltaTime;
			transform.position = newPos;
		}
		CheckDeath();
	}
}
