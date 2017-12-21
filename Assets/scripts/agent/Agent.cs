using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
	public float[] _inputs;
	private float decision_rate = 0f;
	private float decision_cd = 0f;
	public Network _brain;
	private AgentController controller;
	private static string prefabFileName = "Prefabs/Agent";
	
	private float heuristic = 0f;
	public float Heuristic { get { return heuristic; } set { heuristic = value; } }

	void Awake() {
		_brain = new Network(4, 6, 1);
		// Get all other game objects marked as agent
		// ignore collisions with them, or things get messy
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Agent");
		foreach (GameObject go in gos) {
			Physics2D.IgnoreCollision(go.transform.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
		}
		Initialize();
	}

	public void Initialize() {
		controller = transform.GetComponent<AgentController>();
	}

	public static Agent Spawn() {
		GameObject go = (GameObject)Instantiate(Resources.Load(prefabFileName));
		return go.GetComponent<Agent>();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GetState() != GameMaster.GameState.PLAY) {
			return;
		}

		if (decision_cd <= 0) {
			decision_cd = decision_rate;
			UseBrain();
		} else {
			decision_cd -= Time.deltaTime;
		}
		if (transform.gameObject != null) {
			heuristic += Time.deltaTime;
		}
	}

	void UseBrain() {
		// get distance from bottom
		float height = controller.GetHeight();
		float distFromTop = controller.GetDistFromTop();
		// float closest_enemy = controller.GetClosestEnemyDist();
		Vector2 cDist = controller.GetClosestEnemyDist();

		_inputs = new float [] { distFromTop, height, cDist.x, cDist.y };
		_brain.Respond(_inputs);
		float score = controller.ScorePosition();
		_brain.Train(new float[] {score});
		Neuron[] outputs = _brain.Outputs;
		if (outputs[0].Output > 0.5) {
			this.Jump();
		}
	}

	public void SetColor(int color) {
		controller.AgentColor = color;
	}

	public int GetColor() {
		return controller.AgentColor;
	}

	public void SetRandomColor() {
		controller.RandomColor();
	}

	void Jump() {
		this.controller.Jump();
	}

}
