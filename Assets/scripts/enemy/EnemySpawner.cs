using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public Transform spawnPointsParent;
	public float spawnRate = 2f;
	public GameObject enemy;
	public float spawnCountdown;
	protected Transform[] spawnPoints;

	void Awake() {
		spawnPoints = spawnPointsParent.GetComponentsInChildren<Transform>();
		spawnCountdown = spawnRate;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GetState() == GameMaster.GameState.PLAY) {
			if (spawnCountdown <= 0) {
				// spawn -> reset countdown
				SpawnEnemy();
				spawnCountdown = spawnRate;
			} else {
				spawnCountdown -= Time.deltaTime;
			}
		}
	}

	void SpawnEnemy() {
		Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
		Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	}
}
