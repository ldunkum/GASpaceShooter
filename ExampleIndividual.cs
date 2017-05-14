using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ExampleIndividual : Individual
{

	public int multiplier = 10;

	private int chromosomeSize;
	private int[] chromosome1;
	private bool[] chromosome2;
	private Dictionary<float, float> trackPoints;
	private object info;

	public ExampleIndividual (int size, int mult) : base (size)
	{
		multiplier = mult;
		chromosomeSize = (int)(size / multiplier);
		chromosome1 = new int[chromosomeSize];
		chromosome2 = new bool[chromosomeSize];
	}

	public override void Initialize ()
	{
		for (int i = 0; i < chromosomeSize; i++) {
			chromosome1 [i] = Random.Range (-1, 2);
			chromosome2 [i] = (Random.Range (0, 2) == 1);
		}

	}

	public override void Mutate (float probability)
	{
		if (mutationType == 0) {
			for (int i = 0; i < chromosomeSize; i++) {
				if (Random.Range (0f, 1f) < probability) {
					chromosome1 [i] = Random.Range (-1, 2);
					chromosome2 [i] = (Random.Range (0, 2) == 1);
				}
			}
		} else {
			throw new System.NotImplementedException ();
		}
	}

	public override void Crossover (Individual partner, float probability)
	{
		//N_Point Crossover
		//Basic theory: 
		//1: random probability of happening the crossover between 0f and 1f
		//Loop through all chromossome pairs
		//Pick a random position to cut (n_cuts) ---------> should be dynamically changed in Unity, not in the code
		//Create 2 new chromossomes with the two parts cut (Clone?)
		ExampleIndividual bitFlipPartner = (ExampleIndividual)partner;

		//Debug.Log (n_cuts + " cuts");

		if (UnityEngine.Random.Range (0f, 1f) > probability) {
			return;
		}
		int crossoverPoint = Mathf.FloorToInt (chromosomeSize / (n_cuts + 1));

		for (int i = crossoverPoint; i < chromosomeSize; i += 2 * crossoverPoint) {
			for (int j = i; j < chromosomeSize && j < i + crossoverPoint; j++) {
				int temp1 = chromosome1 [j];
				bool temp2 = chromosome2 [j];
				chromosome1 [j] = bitFlipPartner.chromosome1 [j];
				chromosome2 [j] = bitFlipPartner.chromosome2 [j];

				bitFlipPartner.chromosome1 [j] = temp1;
				bitFlipPartner.chromosome2 [j] = temp2;

			}
		}


		
	}

	public override void Translate ()
	{
		for (int i = 0; i < chromosomeSize; i++) {
			for (int j = 0; j < multiplier; j++) {
				horizontalMoves [i * multiplier + j] = chromosome1 [i];
				shots [i * multiplier + j] = chromosome2 [i];
			}
		}
	}

	public override Individual Clone ()
	{
		ExampleIndividual new_ind = new ExampleIndividual (totalSize, multiplier);

		chromosome1.CopyTo (new_ind.chromosome1, 0);
		chromosome2.CopyTo (new_ind.chromosome2, 0);

		//new_ind.Translate ();

		new_ind.fitness = 0.0f;
		new_ind.evaluated = false;
		//new_ind.trackPoints = new Dictionary<float,float> (this.trackPoints);

		return new_ind;
	}

	public override string ToString ()
	{
		string res = "[ExampleIndividual] Chromosome1: [";

		for (int i = 0; i < chromosomeSize; i++) {
			res += chromosome1 [i].ToString ();
			if (i != chromosomeSize - 1) {
				res += ",";
			}
		}

		res += "] Chromosome2: [";

		for (int i = 0; i < chromosomeSize; i++) {
			res += chromosome2 [i].ToString ();
			if (i != chromosomeSize - 1) {
				res += ",";
			}
		}
		res += "]";

		return res;
	}



	/*
	//N_Point Crossover
	//Needs complete overhaul
	//Basic theory: 
	//1: random probability of happening the crossover between 0f and 1f
	//Loop through all chromossome pairs
	//Pick a random position to cut (n_cuts) ---------> should be dynamically changed in Unity, not in the code
	//Create 2 new chromossomes with the two parts cut (Clone?)
	void N_Crossover (Individual partner, float probability) {

		if (UnityEngine.Random.Range(0f, 1f) > probability) {
			return;
		}
		//this example always splits the chromosome in half
		int crossoverPoint = Mathf.FloorToInt(chromosomeSize / n_cuts);

		List<Individual> selectedChroms = new List<Individual>();

		for (int j = 0; j< probability; j+= crossoverPoint*2)
		{
			for (int i = j; (i<(j+crossoverPoint))||(i< chromosomeSize); i++)
			{
				float tmp = trackPoints[[i]];
				trackPoints[[i]] = partner.Count[selectedChroms[i]];
				partner.trackPoints[selectedChroms[i]] = tmp;

			}
			return new_ind;
		}
	}
	*/
	/* FLAWED IMPLEMENTATION
	void nCrossover (Individual partner, float probability, int cutsNum)
	{
		int[] cutoffpoints = new int[cutsNum];

		int counter = 0;
		while (cutoffpoints [cutsNum - 1] == null) {
			cutoffpoints [counter] = Random.Range (0, chromosomeSize);
			bool isRepeated = cutoffpoints.Length != cutoffpoints.Distinct ().Count ();
			while (isRepeated) {
				cutoffpoints [counter] = Random.Range (0, chromosomeSize);
			}
			counter++;
		}
	}
	*/

}
