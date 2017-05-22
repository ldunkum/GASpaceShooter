using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentSelectionWithTSize : SelectionMethod
{

	private int tournamentSize;

	public TournamentSelectionWithTSize (int  size)
	{
		tournamentSize = size;	
	}

	public override List<Individual> selectIndividuals (List<Individual> oldpop, int num)
	{
		List<Individual> selectedInds = new List<Individual> ();

		while (selectedInds.Count < num) {
			List<Individual> tournament = new List<Individual> ();
			for (int i = 0; i < tournamentSize; i++) {
				Individual randomIndividual = oldpop [Random.Range (0, oldpop.Count)];

				while (tournament.Contains (randomIndividual)) {
					randomIndividual = oldpop [Random.Range (0, oldpop.Count)];
				}
				tournament.Add (randomIndividual);

			}
			tournament.Sort ((x, y) => y.Fitness.CompareTo (x.Fitness));
			selectedInds.Add (tournament [0].Clone ());
		}
		return selectedInds;
	}
}