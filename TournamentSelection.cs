using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TournamentSelection : SelectionMethod
{

	//********************************HOW IT WORKS:*****************************
	//
	public float k = 1f;
	//How many parents should be allowed to compete is the value of k
	//The most common value for k seems to be = 2 so that you return good individuals more often than bad ones
	//But in such a way that it doesn't keep picking the same individuals over and over again


	//int k = 2;
	public TournamentSelection (float k) : base ()
	{
		this.k = k;
	}


	public override List<Individual> selectIndividuals (List<Individual> oldpop, int num)
	{
		return tournamentSelection (oldpop, num);
	}


	private List<Individual> tournamentSelection (List<Individual> oldpop, int num)
	{
		List<Individual> selectedInds = new List<Individual> ();
		int popsize = oldpop.Count;
		int counter = 0;

		for (int i = 0; i < num; i++) {
			//make sure selected individuals are different
			Individual ind = oldpop [Random.Range (0, popsize)];
			Individual ind2 = oldpop [Random.Range (0, popsize)];

			while (ind2 == ind)
				ind2 = oldpop [Random.Range (0, popsize)];

			if (Random.Range (0f, 1f) < k) {
				if (ind.Fitness > ind2.Fitness) {
//					if (counter < 10) {
//						Debug.Log ("Ind 1 is fitter\n" + "Fitness of Ind 1: " + ind.Fitness + "    \nFitness of Ind 2: " + ind2.Fitness);
//						counter++;	
//					}

					selectedInds.Add (ind.Clone ());
				} else {
//					if (counter < 10) {
//						Debug.Log ("Ind 2 is fitter\n" + "Fitness of Ind 1: " + ind.Fitness + "    \nFitness of Ind 2: " + ind2.Fitness);
//						counter++;	
//					}
					selectedInds.Add (ind2.Clone ());
				}
			} else {
				if (ind.Fitness > ind2.Fitness) {
					selectedInds.Add (ind2.Clone ());
				} else {
					selectedInds.Add (ind.Clone ());
				}
			}
		}
		//we return copys of the selected individuals
		return selectedInds;
	}
}
