using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {

	private SpriteRenderer spR;
	private Sprite[] sprites;
	private int curFrame = 0;
	public bool loop;
	public float frameSeconds = 1;
	private float deltaTime = 0;
	public string filename;

	// Use this for initialization
	void Start () {
		spR = GetComponent<SpriteRenderer>();
		sprites = Resources.LoadAll<Sprite>("sprites/" + filename);
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime += Time.deltaTime;
		while (deltaTime >= frameSeconds) {
			deltaTime -= frameSeconds;
			curFrame++;
			if (loop) {
				curFrame %= sprites.Length;
			} else if (curFrame >= sprites.Length) {
				// Maybe delete the object at this point?
				// Or handle that in logic of object being destroyed
				curFrame = sprites.Length - 1;
			}
		}

		spR.sprite = sprites[curFrame];
	}
}
