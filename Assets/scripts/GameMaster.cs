using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	public static GameMaster GM;
	public enum GameState { INIT, PLAY, END_ROUND, GAME_OVER };
	private GameState _state;
	private Network tempBrain;
	private GeneticAlgorithm ga;
	public static string enemyTag = "Enemy";
	public static string agentTag = "Agent";
	public int bestGeneration = 0;
	public float bestHeuristic = 0f;
	private int toggleSpeedButtonX = Screen.width - 120;
	private int toggleSpeedButtonY = Screen.height - 60;
	private float heuristic = 0f;
	public Text bestHeuristicText;
	public Text heuristicText;
	public Text generationText;
	public Text bestGenerationText;

	void Awake() {
		// singleton pattern
		if (GM == null) {
			GM = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
		}
		GM.ga = ScriptableObject.CreateInstance<GeneticAlgorithm>();
	}

	// Use this for initialization
	void Start () {
		Initialize();
		GM.ga.Initialize();
		Play();
	}
	
	// Update is called once per frame
	void Update () {
		GM.heuristic += Time.deltaTime;
		GM.heuristicText.text = "Heuristic: " + GM.heuristic;
	}

	void OnGUI(){
		if(GUI.Button(new Rect(toggleSpeedButtonX, toggleSpeedButtonY, 100, 40), "Toggle Speed")){
			if(Time.timeScale == 1.0f) {
				Time.timeScale = 3.0f;
			}
			else {
				Time.timeScale = 1.0f;
			}
		}
	}

	public static GameState GetState() {
		return GM._state;
	}

	public static void KillAgent(GameObject o) {
		// if all agents died end round
		if (o.tag == agentTag) {
			GM.ga.deadAgents++;
		}
		Destroy(o);

		if (GM.ga.deadAgents >= GM.ga.popSize) {
			EndRound();
			GM.ga.deadAgents = 0;
			GM.ga.DoGeneticAlgorithm();

			foreach (Agent a in GM.ga.winners) {
				Debug.Log(a.Heuristic);
			}
			if (GM.ga.winners[0].Heuristic > GM.bestHeuristic) {
				GM.bestHeuristic = GM.ga.winners[0].Heuristic;
				GM.bestGeneration = GM.ga.generation;
			}
			GM.heuristic = 0f;
			GM.generationText.text = "Generation: " + (GM.ga.generation + 1);
			GM.bestHeuristicText.text = "Best Heuristic: " + GM.bestHeuristic;
			GM.bestGenerationText.text = "Best Generation: " + GM.bestGeneration; 
		}
	}

	public static void Initialize() {
		GM._state = GameState.INIT;
	}

	public static void GameOver() {
		GM._state = GameState.GAME_OVER;
	}

	public static void EndRound() {
		GM._state = GameState.END_ROUND;
		GameObject[] gos = GameObject.FindGameObjectsWithTag(enemyTag);
		for (int i = 0; i < gos.Length; i++) {
			Destroy(gos[i]);
		}
		gos = GameObject.FindGameObjectsWithTag(agentTag);
		for (int i = 0; i < gos.Length; i++) {
			Destroy(gos[i]);
		}
	}

	public static void Play() {
		GM._state = GameState.PLAY;
	}
}
