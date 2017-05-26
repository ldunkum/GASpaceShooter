using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitFlipIndividual : Individual
{


	public int multiplier = 10;

	private int chromosomeSize;
	private int[] chromosome1;
	private bool[] chromosome2;

	public BitFlipIndividual (int size, int mult) : base (size)
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


	/// <summary>
	/// Mutate the specified probability.
	/// </summary>
	/// <param name="probability">Probability from 0.0 to 1.0</param>
	public override void Mutate (float probability)
	{

		//Debug.Log ("MutationType in bitflip = " + mutationType);
		//TODO:check how to accept enum values as parameter

		//mutationType
		//0 -> random
		//1 -> swap for int and bitflip for bool 
		//2 -> swap for everything
		//3 -> full bitflip
		if (mutationType == 0) {
			//Debug.Log ("IN RANDOM MUTATION");
			for (int i = 0; i < chromosomeSize; i++) {
				if (Random.Range (0f, 1f) < probability) {
					chromosome1 [i] = Random.Range (-1, 2);
					chromosome2 [i] = (Random.Range (0, 2) == 1);
				}
			}
		} else if (mutationType == 1) {
			mixedMutation (probability);
		} else if (mutationType == 2) { //swap for int and bool
			fullSwapMutation (probability);
		} else if (mutationType == 3) {//full bitflip(-like) mutation
			fullBitFlipMutation (probability);
		}
	}

	private void mixedMutation (float probability)
	{
		//Debug.Log ("IN ORDER CHANGING MUTATION");
		//order changing mutation
		/* 
		 * Theory/thoughs behind this:
		 * 1.check if mutation is to be applied
		 * 2.calculate random 'pointers' within the size of the chromosome
		 * 3.if they are the same or point to the same value, change until they don't point to the same value anymore
		 * 4.swap values at the pointers
		 * 
		*/
		if (Random.Range (0f, 1f) <= probability) {
			int firstMutationSwapElement = Random.Range (0, chromosomeSize - 1);
			int secondMutationSwapElement = firstMutationSwapElement;
			//while (secondMutationSwapElement == firstMutationSwapElement)
			//	secondMutationSwapElement = Random.Range (0, chromosomeSize - 1);	

			int chromosomeCounter = 0;
			while (chromosome1 [firstMutationSwapElement] == chromosome1 [secondMutationSwapElement]) {
				secondMutationSwapElement = Random.Range (0, chromosomeSize - 1);


				// if it points to the same value in chromosome1, change that value to something random
				if (chromosomeCounter == chromosomeSize - 1) {
					while (chromosome1 [firstMutationSwapElement] == chromosome1 [secondMutationSwapElement])
						chromosome1 [secondMutationSwapElement] = (int)Random.Range (-1, 1);
				}


				/*
 					if (chromosomeCounter == chromosomeSize - 1 && chromosome1 [firstMutationSwapElement] == chromosome1 [secondMutationSwapElement]) {
						chromosome1 [secondMutationSwapElement] = (int)Random.Range (-1, 1);
					} else if (chromosomeCounter == chromosomeSize - 1) {
						int temp = chromosome1 [firstMutationSwapElement];

						chromosome1 [firstMutationSwapElement] = chromosome1 [secondMutationSwapElement];
						chromosome1 [secondMutationSwapElement] = temp;

					}
*/


				chromosomeCounter++;
			}
			if (chromosome1 [firstMutationSwapElement] != chromosome1 [secondMutationSwapElement]) {
				int temp = chromosome1 [secondMutationSwapElement];
				chromosome1 [secondMutationSwapElement] = chromosome1 [firstMutationSwapElement];
				chromosome1 [firstMutationSwapElement] = temp;
			}
		}



		//bit flip mutation
		for (int i = 0; i < chromosomeSize; i++) {
			if (Random.Range (0f, 1f) <= probability) {
				//flips values of chromosome2
				chromosome2 [i] = chromosome2 [i] ? chromosome2 [i] = false : chromosome2 [i] = true; // switch value of chromosome2[i]
			}
		}
	}

	private void fullSwapMutation (float probability)
	{
		int firstIntMutationSwapElement = Random.Range (0, chromosomeSize - 1);
		int secondIntMutationSwapElement = firstIntMutationSwapElement;

		while (secondIntMutationSwapElement == firstIntMutationSwapElement)
			secondIntMutationSwapElement = Random.Range (0, chromosomeSize - 1);

		int firstBoolMutationSwapElement = Random.Range (0, chromosomeSize - 1);
		int secondBoolMutationSwapElement = firstBoolMutationSwapElement;
		while (secondBoolMutationSwapElement == firstBoolMutationSwapElement)
			secondBoolMutationSwapElement = Random.Range (0, chromosomeSize - 1);

		int tempInt = chromosome1 [firstIntMutationSwapElement];
		bool tempBool = chromosome2 [firstBoolMutationSwapElement];

		chromosome1 [firstIntMutationSwapElement] = chromosome1 [secondIntMutationSwapElement];
		chromosome1 [secondIntMutationSwapElement] = tempInt;

		chromosome2 [firstBoolMutationSwapElement] = chromosome2 [secondBoolMutationSwapElement];
		chromosome2 [secondBoolMutationSwapElement] = tempBool;

		/*
		int chromosomeCounter = 0;
		while (chromosome1 [firstIntMutationSwapElement] == chromosome1 [secondIntMutationSwapElement]) {
			secondIntMutationSwapElement = Random.Range (0, chromosomeSize - 1);


			// if it points to the same value in chromosome1, change that value to something random
			if (chromosomeCounter == chromosomeSize - 1) {
				while (chromosome1 [firstIntMutationSwapElement] == chromosome1 [secondIntMutationSwapElement])
					chromosome1 [secondIntMutationSwapElement] = (int)Random.Range (-1, 1);
			}


			/*
 					if (chromosomeCounter == chromosomeSize - 1 && chromosome1 [firstMutationSwapElement] == chromosome1 [secondMutationSwapElement]) {
						chromosome1 [secondMutationSwapElement] = (int)Random.Range (-1, 1);
					} else if (chromosomeCounter == chromosomeSize - 1) {
						int temp = chromosome1 [firstMutationSwapElement];

						chromosome1 [firstMutationSwapElement] = chromosome1 [secondMutationSwapElement];
						chromosome1 [secondMutationSwapElement] = temp;

					}
*/

/*
			chromosomeCounter++;
		}
		chromosomeCounter = 0;
		while (chromosome2 [firstBoolMutationSwapElement] == chromosome2 [secondBoolMutationSwapElement]) {
			secondBoolMutationSwapElement = Random.Range (0, chromosomeSize - 1);


			if (chromosomeCounter == chromosomeSize - 1) {
				while (chromosome2 [firstBoolMutationSwapElement] == chromosome2 [secondBoolMutationSwapElement])
					chromosome2 [secondBoolMutationSwapElement] = (Random.Range (0, 2) == 1);
			}

			chromosomeCounter++;
		}

	*/
	}

	private void fullBitFlipMutation (float probability)
	{
		//Debug.Log ("Full bitflip mutation");
		for (int i = 0; i < chromosomeSize; i++) {
			if (Random.Range (0f, 1f) <= probability) {
				int temp = chromosome1 [i];
				while (chromosome1 [i] == temp) {
					chromosome1 [i] = Random.Range (-1, 2);
				}
			}
			if (Random.Range (0f, 1f) <= probability) {
				chromosome2 [i] = chromosome2 [i] == true ? chromosome2 [i] = false : chromosome2 [i] = true; // switch value of chromosome2[i]
			}
		}
	}

	//1 point
	public override void Crossover (Individual partner, float probability)
	{
		switch (crossoverType) {
		case 0:
			onePointCrossover (partner, probability);
			break;
		case 1:
			n_Crossover (partner, probability);
			break;
		}
	}


	private void onePointCrossover (Individual partner, float probability)
	{
		BitFlipIndividual bitFlipPartner = (BitFlipIndividual)partner;

		//go through chromosomes and alternate the according chromosome from parent1 and parent2 
		int[] newChromosome1 = new int[chromosomeSize];
		int[] newPartnerChromosome1 = new int[chromosomeSize]; 
		bool[] newChromosome2 = new bool[chromosomeSize];
		bool[] newPartnerChromosome2 = new bool[chromosomeSize];


		if (Random.Range (0f, 1f) <= probability) {
			int cutoffPoint = Random.Range (0, chromosomeSize);
			for (int i = 0; i < cutoffPoint; i++) {
				newChromosome1 [i] = chromosome1 [i];
				newChromosome2 [i] = chromosome2 [i];
				newPartnerChromosome1 [i] = bitFlipPartner.chromosome1 [i];
				newPartnerChromosome2 [i] = bitFlipPartner.chromosome2 [i];
			}
			for (int i = cutoffPoint; i < chromosomeSize; i++) {
				newChromosome1 [i] = bitFlipPartner.chromosome1 [i];
				newChromosome2 [i] = bitFlipPartner.chromosome2 [i];
				newPartnerChromosome1 [i] = chromosome1 [i];
				newPartnerChromosome2 [i] = chromosome2 [i];
			}
		}

		chromosome1 = newChromosome1;
		chromosome2 = newChromosome2;

		bitFlipPartner.chromosome1 = newPartnerChromosome1;
		bitFlipPartner.chromosome2 = newPartnerChromosome2;
	}

	//N_Point Crossover
	//Basic theory:
	//1: random probability of happening the crossover between 0f and 1f
	//Loop through all chromossome pairs
	//Pick a random position to cut (n_cuts) ---------> should be dynamically changed in Unity, not in the code
	//Create 2 new chromossomes with the two parts cut (Clone?)
	private void n_Crossover (Individual partner, float probability)
	{
		BitFlipIndividual bitFlipPartner = (BitFlipIndividual)partner;

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
		BitFlipIndividual new_ind = new BitFlipIndividual (totalSize, multiplier);

		chromosome1.CopyTo (new_ind.chromosome1, 0);
		chromosome2.CopyTo (new_ind.chromosome2, 0);

		//new_ind.Translate ();

		new_ind.fitness = 0.0f;
		new_ind.evaluated = false;

		return new_ind;
	}

	public override string ToString ()
	{
		string res = "[BitFlipIndividual] Chromosome1: [";

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
}
