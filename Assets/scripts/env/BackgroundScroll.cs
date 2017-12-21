using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour {

	public float hSpeed = 3.0f;
	public Transform bg1;
	public Transform bg2;

	private Camera cam;
	private float spriteWidth = 0f;
	public int offset = 2;

	void Awake() {
		cam = Camera.main;
	}

	// Use this for initialization
	void Start () {
		// initialize the width of the sprite
		SpriteRenderer sR = bg1.GetComponent<SpriteRenderer>();
		spriteWidth = sR.sprite.bounds.size.x * bg1.transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GetState() == GameMaster.GameState.PLAY) {
			float camXVisible = cam.orthographicSize * Screen.width / Screen.height + offset;
			float bg1Edge = (bg1.transform.position.x + spriteWidth/2);
			float bg2Edge = (bg2.transform.position.x + spriteWidth/2);

			if (bg1Edge <= -(camXVisible) && bg1Edge < bg2Edge) {
				Move(bg1);
			}
			else if (bg2Edge <= -(camXVisible) && bg2Edge < bg1Edge) {
				Move(bg2);
			}

			// Scroll the entire background
			Vector2 newPos = transform.position;
			newPos.x -= hSpeed * Time.deltaTime;
			transform.position = newPos;
		}
	}

	void Move(Transform t) {
		Vector2 v = t.position;
		// leapfrog over sprite
		v.x += spriteWidth * 2.0f;
		t.position = v;
	}
}
