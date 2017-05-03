public abstract class Individual
{

	public float[] horizontalMoves;
	public float[] verticalMoves;
	public bool[] shots;

	protected int totalSize;
	protected float fitness;
	protected bool evaluated;
	protected int mutationType = 3;

	public int MutationType {
		get { return mutationType; }
		set { mutationType = value; }
	}

	public int Size {
		get { return totalSize; }
	}

	public float Fitness {
		get { return fitness; }
		set {
			fitness = value;
			evaluated = true;
		}
	}

	public bool Evaluated {
		get { return evaluated; }
	}

	public Individual (int size)
	{
		totalSize = size;
		fitness = 0f;
		evaluated = false;
		horizontalMoves = new float[size];
		verticalMoves = new float[size];
		shots = new bool[size];
	}

	//override on each specific individual class
	public abstract void Initialize ();

	public abstract void Mutate (float probability);

	public abstract void Crossover (Individual partner, float probability);

	public abstract void Translate ();

	public abstract Individual Clone ();
}
