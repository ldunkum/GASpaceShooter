using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//implement logic behind this(i.e. switch)
public enum IndividualType
{
	Example,
	BitFlip
}

public enum MutationType
{
	Random,
	BitFlipAndSwap,
	FullSwap,
	FullBitFlip
}

public class EvolutionState : MonoBehaviour
{
	public int individualSize;
	public int individualMultiplier;
	public int numGenerations;
	public int populationSize;
	public float mutationProbability;
	public float crossoverProbability;
	public int N_cutsCrossover;
	public int preservedIndividualsElitism;
	//ELITISM
	public int tournamentSize = 0;
	private string statsFilename = "log";
	public StatisticsLogger stats;
	public IndividualType typeOfIndividual = IndividualType.Example;
	public MutationType mutationType;

	protected List<Individual> population;
	protected SelectionMethod selection;

	protected int evaluatedIndividuals;

	public int generation;

	public List<Individual> Population {
		get {
			return population;
		}
	}

	public Individual Best {
		get {
			float max = float.MinValue;
			Individual max_ind = null;
			foreach (Individual indiv in population) {
				if (indiv.Fitness > max) {
					max = indiv.Fitness;
					max_ind = indiv;
				}
			}
			return max_ind;
		}
	}

	void Start ()
	{
		generation = 0;

		stats = new StatisticsLogger (statsFilename);
	
		if (tournamentSize == 0)
			selection = new TournamentSelection ();
		else if (tournamentSize > 0) {
			selection = new TournamentSelectionWithTSize (tournamentSize);
		}
	}


	public virtual void InitPopulation ()
	{
		switch (typeOfIndividual) {

		case IndividualType.Example:
			population = new List<Individual> ();

			while (population.Count < populationSize) {
				ExampleIndividual new_ind = new ExampleIndividual (individualSize, individualMultiplier);
				new_ind.Initialize ();
				new_ind.Translate ();
				population.Add (new_ind);
			}
			break;
		case IndividualType.BitFlip:
			population = new List<Individual> ();

			while (population.Count < populationSize) {
				BitFlipIndividual new_ind = new BitFlipIndividual (individualSize, individualMultiplier);
				new_ind.Initialize ();
				new_ind.Translate ();
				population.Add (new_ind);
			}
			break;
		}

	}

	//The Step function assumes that the fitness values of all the individuals in the population have been calculated.
	public virtual void Step ()
	{
		if (generation < numGenerations) {
			List<Individual> new_pop;

			//Store statistics in log
			stats.GenLog (population, generation);

			//Select parents
			new_pop = selection.selectIndividuals (population, populationSize - preservedIndividualsElitism);


			//Crossover
			for (int i = 0; i < populationSize - preservedIndividualsElitism; i += 2) {
				Individual parent1 = new_pop [i];
				Individual parent2 = new_pop [i + 1];
				parent1.Crossover (parent2, crossoverProbability);
			}


			//Mutation and Translation 

			for (int i = 1; i < populationSize - preservedIndividualsElitism; i++) {
				//Debug.Log ("MutationType = " + mutationType + "/nIn int = " + (int)mutationType);
				new_pop [i].MutationType = (int)mutationType; //send public modifier mutationType to Individual
				new_pop [i].Mutate (mutationProbability);
				new_pop [i].Translate ();
			}

			//ELITISM
			List<Individual> elitismIndividuals = new List<Individual> ();


			//sort population by fitness
			population.Sort ((x, y) => y.Fitness.CompareTo (x.Fitness));
			for (int i = 0; i < preservedIndividualsElitism; i++) {
				//Debug.Log(population[i].Fitness);
				new_pop.Add (population [i]);
			}
			//END ELITISM


			//Select new population
			population = new_pop;

			generation++;
		}
	}

	public void FinalLog ()
	{
		stats.GenLog (population, generation);
		stats.FinalLog (mutationProbability, crossoverProbability, tournamentSize, (int)typeOfIndividual, (int)mutationType);
	}

}

