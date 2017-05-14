using UnityEngine;
using System.Collections;
using System.Collections.Generic;        
using UnityEngine.UI;


public class SimulationInfo 
{
	public GameObject sim;
	public GameController gamec;
	public PlayerController playerc;

	public SimulationInfo(GameObject sim, GameController gamec, PlayerController playerc){
		this.sim = sim;
		this.gamec = gamec;
		this.playerc = playerc;
	}

}
public class EvolvingSpaceShooter : MonoBehaviour
{
	public static EvolvingSpaceShooter instance = null;              
  
	public GameObject simPrefab;

	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;

	public Camera mainCamera;
	public Camera bestCamera;

	private List<SimulationInfo> simsInfo;
	private SimulationInfo bestSimInfo;
	private EvolutionState evolEngine;

	public int levelNumber = 0;

	private  Level lvl;
	private int sims_done = 0;
	private bool bestFinished = true;
	private bool allFinished = false;

	//Awake is always called before any Start functions
	void Awake()
	{
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (gameObject);    
		}

		DontDestroyOnLoad(gameObject);

		scoreText.text = "";
		if (restartText) {
			restartText.text = "1/1";
		}

		if (gameOverText) {
			gameOverText.text = "";
		}

		evolEngine= this.GetComponentInParent<EvolutionState> ();

		BatchmodeConfig.HandleArgs (evolEngine, this);

		lvl = new Level ();
		lvl.load (levelNumber);

		evolEngine.populationSize = 50;
		evolEngine.individualSize = lvl.calcNumberOfMoves ();
		evolEngine.InitPopulation();

		init ();
	}




	public void createSimulationGrid (int numberOfSimsNeeded)
	{
		simsInfo = new List<SimulationInfo> ();
		/// SIMULATION GRID! 
		/// cam config:  position -5 10 -5    Ortho size 80 
		/// 50 ...

		int cols = 10;
		int lines = numberOfSimsNeeded / 10;
		float game_width = 16; 
		float game_height = 31;
		int cols_done = 0;
		for (int i = -5; cols_done < cols; i++, cols_done++) {
			for (int j = 0; j < lines; j++) {
				GameObject tmp = Instantiate(simPrefab, transform.position + new Vector3(i * game_width, 0, j * -game_height ), transform.rotation);
				simsInfo.Add(new SimulationInfo( tmp, 
					tmp.GetComponentInChildren<GameController>(),
					tmp.GetComponentInChildren<PlayerController>())
				);

			}
		}

	}


	public void initSimulations(){
		int index = 0 ;
		foreach (SimulationInfo info in simsInfo) {
			info.gamec.id = index;
			info.gamec.lvl = lvl;
			info.gamec.spawnValues.y = 0 ;
			info.gamec.spawnValues.z = 20 ;
			info.gamec.running = false;
			info.gamec.enabled = true;
			index++;
		}
	}

	public void updatePlayers(){
		
		foreach (SimulationInfo info in simsInfo) {
		
			///  with EvolutionState
			info.playerc.testIndividual = evolEngine.Population [info.gamec.id];
			info.playerc.running = false;
			info.playerc.testIndividual.Translate ();

		}
			
	}

	public void startSimulations()
	{
		foreach (SimulationInfo info in simsInfo) {
			info.gamec.initGame ();
			info.playerc.enabled = true;
			info.playerc.restart ();
			info.playerc.running = true;
		}
	}

	void initBestSim(){
		mainCamera.enabled = false;
		bestCamera.enabled = true;
		bestFinished = false;

		GameObject bestSim = Instantiate(simPrefab, transform.position, transform.rotation);


		bestSimInfo = new SimulationInfo(
			bestSim,
			bestSim.GetComponentInChildren<GameController>(),
			bestSim.GetComponentInChildren<PlayerController>()
		);

		bestSimInfo.playerc.testIndividual = evolEngine.Best;
		bestSimInfo.gamec.id = 1337;
		bestSimInfo.gamec.lvl = lvl;
		bestSimInfo.gamec.spawnValues.y = 0 ;
		bestSimInfo.gamec.spawnValues.z = 20 ;
		bestSimInfo.gamec.running = true;
		bestSimInfo.gamec.enabled = true;

		// start player
		bestSimInfo.gamec.initGame ();
		bestSimInfo.playerc.enabled = true;
		bestSimInfo.playerc.restart ();
		bestSimInfo.playerc.running = true;
	}

	public void Update(){
		int i = 0;
		sims_done = 0;
		foreach (SimulationInfo info in simsInfo) {

			if (info.gamec.gameOver || info.gamec.stageCleared) {
				sims_done++;
				// assign fitness equal to the gamescore!
				info.playerc.testIndividual.Fitness = (float)info.gamec.score;

				// destroy it !
				Destroy (info.sim);
			}

			i++;
		}

		if (bestSimInfo != null && (bestSimInfo.gamec.gameOver || bestSimInfo.gamec.stageCleared)) {
			Destroy (bestSimInfo.sim);
			bestSimInfo = null;
			bestFinished = true;
		}

		restartText.text = "Generation: " + evolEngine.generation + "/" + evolEngine.numGenerations + "\nSims Done: " + sims_done + " / " + simsInfo.Count;
	}
		
	public void FixedUpdate(){
		if (sims_done == simsInfo.Count && bestFinished) {
			if (evolEngine.generation < evolEngine.numGenerations) {

				// Perform an evolutionary algorithm step
				evolEngine.Step ();

				// Init grids and start simulations again
				init ();
				sims_done = 0;
			} else {
				gameOverText.text = "Number of Generations reached. Fitness of Best Solution : " + evolEngine.Best.Fitness;
				if (!allFinished) {
					Debug.Log ("Best individual: " + evolEngine.Best.ToString ());
					evolEngine.FinalLog ();
					allFinished = true;
				}

				if (BatchmodeConfig.batchmode) {
					Application.Quit ();
				}

				//Play best solution
				initBestSim ();
			}
		}

	}

	void init(){
		createSimulationGrid (evolEngine.populationSize);
		initSimulations();
		updatePlayers ();
		startSimulations ();
	}
}

