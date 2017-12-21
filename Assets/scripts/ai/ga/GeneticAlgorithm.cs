using System;
using System.Collections;
using UnityEngine;

public class GeneticAlgorithm : ScriptableObject {

	public Agent[] agents;
	public Agent[] winners;
	public int popSize = 10;
	public float mRate = 0.03f;
	public int deadAgents = 0;
	public int numWinners = 5;
	public int generation = 0;

	public void Initialize() {
		CreatePopulation();
	}

	public void DoGeneticAlgorithm() {
        generation++;
        GameMaster.Initialize();
		System.Random r = new System.Random();

        // get top numWinners from the population
        winners = Selection();
        for (int i = 0; i < popSize; i++)
        {
            Agent parentA = winners[r.Next(0, numWinners)];
            Agent parentB = winners[r.Next(0, numWinners)];
            Agent child = Crossover(parentA, parentB);
            Mutate(child);
            agents[i] = child;

        }
        GameMaster.Play();
    }

    void Mutate(Agent c) {
		System.Random r = new System.Random();
		if (r.NextDouble() < mRate) {
			int g1 = r.Next(0, c._brain.Neurons().Length);
			int g2 = r.Next(0, c._brain.Neurons().Length);
			float temp = c._brain.Neurons()[g1].bias;
			c._brain.Neurons()[g1].bias = c._brain.Neurons()[g2].bias;
			c._brain.Neurons()[g2].bias = temp;
			// instead of swapping synapse weights, we'll swap actual bias
			// for (int i = 0; i < c._brain.Neurons()[g1]._weights.Length; i ++) {
            // 	c._brain.Neurons()[g1]._weights[i] = (float) (r.NextDouble());
			// }
		}
		c.SetRandomColor();
	}

	Agent Crossover(Agent a, Agent b) {
		System.Random r = new System.Random();
		Agent child = Agent.Spawn();
		int cLength = a._brain.Neurons().Length;
		int crossPoint = r.Next(0, cLength);
		bool aFirst = r.Next(0, 1) == 1;

		//child._brain = aFirst ? a._brain.Copy() : b._brain.Copy();
		for (int i = 0; i < crossPoint; i++) {
			if (aFirst) {
				child._brain.Neurons()[i].bias = a._brain.Neurons()[i].bias;
				child._brain.Neurons()[i]._weights = a._brain.Neurons()[i]._weights;
			} else {
				child._brain.Neurons()[i].bias = b._brain.Neurons()[i].bias;
				child._brain.Neurons()[i]._weights = b._brain.Neurons()[i]._weights;
			}
		}
		for (int i = crossPoint; i < cLength; i++) {
			if (aFirst) {
				child._brain.Neurons()[i].bias = b._brain.Neurons()[i].bias;
				child._brain.Neurons()[i]._weights = b._brain.Neurons()[i]._weights;
			} else {
				child._brain.Neurons()[i].bias = a._brain.Neurons()[i].bias;
				child._brain.Neurons()[i]._weights = a._brain.Neurons()[i]._weights;
			}
		}
		child.SetColor(a.GetColor());

		return child;
	}

	Agent[] Selection() {
		IComparer hComparer = new HeuristicComparer();
		System.Array.Sort(agents, hComparer);

		Agent[] temp = new Agent[numWinners];
		System.Array.Copy(agents, 0, temp, 0, numWinners);

		return temp;
	}

	void CreatePopulation() {
		agents = new Agent[popSize];
		for (int i = 0; i < popSize; i++) {
			agents[i] = Agent.Spawn();
		}
	}
}

public class HeuristicComparer : System.Collections.IComparer {
	public int Compare(object x, object y) {
		Agent a = (Agent)x;
		Agent a2 = (Agent)y;
		return a2.Heuristic.CompareTo(a.Heuristic);
	}
}
