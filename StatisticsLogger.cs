using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StatisticsLogger
{

	public Dictionary<int,float> bestFitness;
	public Dictionary<int,float> meanFitness;
	public Dictionary<int,float> standardFitness;
	public Dictionary<int,float> worstFitness;

	private string filename;
	private StreamWriter logger;


	public StatisticsLogger (string name)
	{
		filename = name;
		bestFitness = new Dictionary<int,float> ();
		meanFitness = new Dictionary<int,float> ();
		standardFitness = new Dictionary<int, float> ();
		worstFitness = new Dictionary<int, float> ();

	}

	//saves fitness info and writes to console
	public void GenLog (List<Individual> pop, int currentGen)
	{
		float avgSquaredFitness = 0;
		pop.Sort ((x, y) => y.Fitness.CompareTo (x.Fitness));

		bestFitness.Add (currentGen, pop [0].Fitness);
		meanFitness.Add (currentGen, 0f);
		worstFitness.Add (currentGen, pop [pop.Count - 1].Fitness);

		foreach (Individual ind in pop) {
			meanFitness [currentGen] += ind.Fitness;
		}
		meanFitness [currentGen] /= pop.Count;

		foreach (Individual ind in pop) {
			avgSquaredFitness += Mathf.Pow (ind.Fitness - meanFitness [currentGen], 2);
		}

		float a = Mathf.Sqrt (avgSquaredFitness / pop.Count);
		standardFitness.Add (currentGen, a);
		//standardFitness = raiz quad [somatorio (ind.Fitness - meanFitness) ao quadrado / n_elementos];
		//The square root of the sum for each element of each element minus the avarage math.pow dividing by the number of elements

		Debug.Log ("generation: " + currentGen + "\tbest: " + bestFitness [currentGen] + "\tmean: " + meanFitness [currentGen] + "\n" + "\tstandard:" + standardFitness [currentGen] + "\tworst:" + worstFitness [currentGen]);
		//Debug.Log ("generation: " + currentGen + "\t solution: " + pop [0].ToString ());
	}

	//writes to file
	public void FinalLog (float mutationProbability, float crossoverProbability, int tournamentSize, int typeOfIndividual, int mutationType, int crossoverType)
	{
		string localFileName = filename;
		int numberOfLogs = 0;

		if (!Directory.Exists ("logs"))
			Directory.CreateDirectory ("logs");
			
		do {
			localFileName = filename + numberOfLogs;
			//localFileName.Insert(localFileName.IndexOf("."), numberOfLogs.ToString());
			numberOfLogs++;
			//Debug.Log (localFileName + ".txt");
			//Debug.Log(File.Exists ("logs/" + localFileName + ".txt"));
		} while(File.Exists ("logs/" + localFileName + ".txt"));
		localFileName = localFileName + ".txt";

		logger = File.CreateText ("logs/" + localFileName);

		logger.WriteLine ("Type of Individual: " + typeOfIndividual + "\nMutationType: " + mutationType + "\nCrossoverType: " + crossoverType + "\nMutation probability: " + mutationProbability + "\nCrossover probability: " + crossoverProbability + "\nTournament size: " + tournamentSize);

		//writes with the following format: generation, bestfitness, meanfitness, standardFitness
		for (int i = 0; i < bestFitness.Count; i++) {
			logger.WriteLine (i + " : " + bestFitness [i] + " : " + worstFitness [i] + " : " + meanFitness [i] + " : " + standardFitness [i]);
		}


		//worstFitness
		/*for (int i = 0; i < worstFitness.Count; i++) {
			logger.WriteLine (i + "," + worstFitness [i] + "," + meanFitness [i] + "," + standardFitness [i]);
		}
		*/
		logger.Close ();
	}
}

//standard and worst, fitness is score, maen é a média
//get the mean | number - mean | result ao quadrado | mean de todos os resultados | raiz quadrada dessa media | media - desvio pad. e media + desvio pad.