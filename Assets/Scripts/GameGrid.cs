using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
	solid,
	liquid,
	gas,
	empty
}

public enum cellType //Material is used already by steamVR...
{
	empty,
	sand,
	stone,
	lava,
	water,
	plant,
	fire,
	wood,
	root,
	bush,
	steam,
	smoke,
	ice,
	character
}

public struct collision
{
	public cellType other;
	public int location;
}

public struct Tree
{
	public int x;
	public int y;
	public int remainingGrowth;
	public int branchPoint;
	public bool branch;
	public float growthRate;
	public float nextGrow;
}

public class GameGrid : MonoBehaviour
{
	public bool createParticles;//Used for debugging without vive

	public int width;
	public int height;

	public cellType particleType;

	public State particleState;

	public Color[] Colour = new Color[6];

	public GameObject charPrefab;

	//public GameObject cam;//Active particle debug stuff
	Text txt;//Active particle debug stuff

	Cell[,] cells;

	List<Particle> activeParticles = new List<Particle>();
	Particle[,] inactiveParticles;
	public List<Character> characters = new List<Character>();
	List<GameObject> charGO = new List<GameObject>();

	List<Tree> trees = new List<Tree>();

	float delay;//debug temp
	float charDelay;//debug temp
	float offset;//debug temp

	Texture2D gridTexture;

	// Use this for initialization
	void Start()
	{
		CreateGrid();
		CreateCharacter(width, height); //For some reason this has to be where they spawn?
	}

	int CheckRange(int coord, int range)
	{
		int newCoord = coord;
		if (coord < 0)
			newCoord = range - 1;
		if (coord > range - 1)
			newCoord = 0;

		return newCoord;
	}

	void wakeAdjacent(int x, int y, int[] adjCoord)
	{
		if ((cells[x, adjCoord[0]].settled) && (cells[x, adjCoord[0]].particleType != cellType.empty) && (!cells[x, adjCoord[0]].barrier))
		{
			Particle p = inactiveParticles[x, adjCoord[0]];
			p.active = true;
			cells[x, adjCoord[0]].UnSettle();
			inactiveParticles[x, adjCoord[0]] = null;
			activeParticles.Add(p);
		}
		if ((cells[x, adjCoord[1]].settled) && (cells[x, adjCoord[1]].particleType != cellType.empty) && (!cells[x, adjCoord[1]].barrier))
		{
			Particle p = inactiveParticles[x, adjCoord[1]];
			p.active = true;
			cells[x, adjCoord[1]].UnSettle();
			inactiveParticles[x, adjCoord[1]] = null;
			activeParticles.Add(p);
		}
		if ((cells[adjCoord[2], y].settled) && (cells[adjCoord[2], y].particleType != cellType.empty) && (!cells[adjCoord[2], y].barrier))
		{
			Particle p = inactiveParticles[adjCoord[2], y];
			p.active = true;
			cells[adjCoord[2], y].UnSettle();
			inactiveParticles[adjCoord[2], y] = null;
			activeParticles.Add(p);
		}
		if ((cells[adjCoord[3], y].settled) && (cells[adjCoord[3], y].particleType != cellType.empty) && (!cells[adjCoord[3], y].barrier))
		{
			Particle p = inactiveParticles[adjCoord[3], y];
			p.active = true;
			cells[adjCoord[3], y].UnSettle();
			inactiveParticles[adjCoord[3], y] = null;
			activeParticles.Add(p);
		}
	}

	void CreateGrid()
	{
		cells = new Cell[width, height]; //Create an multidimensional array of cells to fit the grid
		inactiveParticles = new Particle[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Cell cell = new Cell();
				cell.Set(x, y);
				cells[x, y] = cell;
			}
		}
		gridTexture = new Texture2D(width, height, TextureFormat.ARGB32, true);
		gridTexture.filterMode = FilterMode.Point;

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (j < 2)
				{
					Particle p = new Particle(i, j, cellType.stone, State.solid, new Vector2(0.0f, 0.0f), width, height);
					p.active = false;
					inactiveParticles[i, j] = p;
					cells[i, j].Settle();
					cells[i, j].SetBarrier();
					cells[i, j].SetParticle(cellType.stone, new Vector2(0, 0));
				}
			}
		}

		FillGrid();

		foreach (Cell c in cells)
		{
			gridTexture.SetPixel(c.x, c.y, Colour[(int)c.particleType]);
			if (c.velocity.x == 0 && c.velocity.y == 0)
			{
				c.Settle();
			}
		}
	}

	public void FillGrid()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (j > 1 && j < 71)
				{
					Particle p = new Particle(i, j, cellType.stone, State.solid, new Vector2(0.0f, 0.0f), width, height);
					p.active = false;
					inactiveParticles[i, j] = p;
					cells[i, j].Settle();
					cells[i, j].SetParticle(cellType.stone, new Vector2(0, 0));
				}

				if (j > 70 && j < 81)
				{
					int rand = Random.Range(0, 10);
					if (rand + (j - 76) > 5)
					{
						Particle p = new Particle(i, j, cellType.sand, State.solid, new Vector2(0.0f, 0.0f), width, height);
						p.active = false;
						inactiveParticles[i, j] = p;
						cells[i, j].Settle();
						cells[i, j].SetParticle(cellType.sand, new Vector2(0, 0));
					}
					else
					{
						Particle p = new Particle(i, j, cellType.stone, State.solid, new Vector2(0.0f, 0.0f), width, height);
						p.active = false;
						inactiveParticles[i, j] = p;
						cells[i, j].Settle();
						cells[i, j].SetParticle(cellType.stone, new Vector2(0, 0));
					}
				}

				if (j > 80 && j < 150)
				{
					Particle p = new Particle(i, j, cellType.sand, State.solid, new Vector2(0.0f, 0.0f), width, height);
					p.active = false;
					inactiveParticles[i, j] = p;
					cells[i, j].Settle();
					cells[i, j].SetParticle(cellType.sand, new Vector2(0, 0));
				}
			}
		}
	}

	public void ChangeType(cellType type)
	{
		particleType = type;

		if (type == cellType.empty)
			particleState = State.empty;
		else
			if (type == cellType.fire || type == cellType.smoke || type == cellType.steam)
			particleState = State.gas;
		else
				if (type == cellType.water || type == cellType.lava)
			particleState = State.liquid;
		else
			particleState = State.solid;
	}

	public bool CreateParticle(float x, float y)//Create; Polymorphise. Whats the difference? -R Huh turns out` in the end we are creating them... -R
	{
		// x and y must be in range 0-1
		//may add random effect to "spray" particles 
		int gridX = Mathf.RoundToInt(x * width);//***************************  TEST FLOORING  ***************************
		int gridY = Mathf.RoundToInt(y * height);

		if (cells[gridX, gridY].particleType == cellType.empty)
		{
			activeParticles.Add(new Particle(gridX, gridY, particleType, particleState, new Vector2(0.0f, -9.8f), width, height));//Adjust spawn velocity for gases?
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool SprayParticles(float x, float y)//Create; Polymorphise. Whats the difference? -R Huh turns out` in the end we are creating them... -R
	{
		int gridX = Mathf.RoundToInt(x * width);
		int gridY = Mathf.RoundToInt(y * height);

		for (int i = 0; i < 20; i++)
		{
			int randX = CheckRange(gridX + Random.Range(-8, 8), width);
			int randY = CheckRange(gridY + Random.Range(-8, 8), height);

			activeParticles.Add(new Particle(randX, randY, particleType, particleState, new Vector2(0.0f, -9.8f), width, height));
		}

		return true;
	}

	public void CreateCharacter(int x, int y)
	{
		characters.Add(new Character(x, y));
		charGO.Add(Instantiate(charPrefab));
	}

	public void UpdateActiveParticles()
	{
		for (int i = activeParticles.Count - 1; i > 0; i--)
		{
			Particle p = activeParticles[i];
			cells[p.x, p.y].SetParticle(cellType.empty, new Vector2(0.0f, 0.0f));
			cells[p.x, p.y].Settle();
			gridTexture.SetPixel(p.x, p.y, Colour[(int)cellType.empty]);
			int[] adjacentCoordinates = new int[4];
			Vector2[] adjacentVel = new Vector2[4]; //Adjacent velocities. Up Down Left Right
			cellType[] adjacentParticle = new cellType[4]; //Adjacent particles. Up Down Left Right


			Collisions collisions = new Collisions();

			collision xCollision;
			collision yCollision;

			adjacentCoordinates[0] = CheckRange((p.y + 1), height);
			adjacentCoordinates[1] = CheckRange((p.y - 1), height);

			adjacentVel[0] = cells[p.x, adjacentCoordinates[0]].velocity;
			adjacentVel[1] = cells[p.x, adjacentCoordinates[1]].velocity;

			adjacentParticle[0] = cells[p.x, adjacentCoordinates[0]].particleType;
			adjacentParticle[1] = cells[p.x, adjacentCoordinates[1]].particleType;

			yCollision = p.UpdateY(adjacentVel, adjacentParticle);

			gridTexture.SetPixel(p.previousX, p.previousY, Colour[(int)cellType.empty]);
			cells[p.previousX, p.previousY].SetParticle(cellType.empty, new Vector2(0.0f, 0.0f));
			cells[p.previousX, p.previousY].Settle();

			adjacentCoordinates[2] = CheckRange((p.x - 1), width);
			adjacentCoordinates[3] = CheckRange((p.x + 1), width);

			adjacentVel[2] = cells[adjacentCoordinates[2], p.y].velocity;
			adjacentVel[3] = cells[adjacentCoordinates[3], p.y].velocity;

			adjacentParticle[2] = cells[adjacentCoordinates[2], p.y].particleType;
			adjacentParticle[3] = cells[adjacentCoordinates[3], p.y].particleType;
			xCollision = p.UpdateX(adjacentVel, adjacentParticle);

			cells[p.previousX, p.previousY].SetParticle(cellType.empty, new Vector2(0.0f, 0.0f));
			cells[p.x, p.y].SetParticle(p.particleType, p.velocity);


			adjacentCoordinates[0] = CheckRange((p.y + 1), height);
			adjacentCoordinates[1] = CheckRange((p.y - 1), height);

			adjacentVel[0] = cells[p.x, adjacentCoordinates[0]].velocity;
			adjacentVel[1] = cells[p.x, adjacentCoordinates[1]].velocity;

			adjacentParticle[0] = cells[p.x, adjacentCoordinates[0]].particleType;
			adjacentParticle[1] = cells[p.x, adjacentCoordinates[1]].particleType;

			adjacentCoordinates[2] = CheckRange((p.x - 1), width);
			adjacentCoordinates[3] = CheckRange((p.x + 1), width);

			adjacentVel[2] = cells[adjacentCoordinates[2], p.y].velocity;
			adjacentVel[3] = cells[adjacentCoordinates[3], p.y].velocity;

			adjacentParticle[2] = cells[adjacentCoordinates[2], p.y].particleType;
			adjacentParticle[3] = cells[adjacentCoordinates[3], p.y].particleType;

			if (p.particleType == cellType.root && p.velocity == new Vector2(0.0f, 0.0f))
			{
				Tree newTree = new Tree();
				newTree.x = p.x;
				newTree.y = p.y;
				newTree.remainingGrowth = 50 + Random.Range(-30, 30);
				newTree.branchPoint = (newTree.remainingGrowth / 2) + Random.Range(-newTree.remainingGrowth / 2 + 1, newTree.remainingGrowth / 2);
				newTree.growthRate = 0.6f;
				newTree.nextGrow = Time.time + newTree.growthRate;
				p.particleType = cellType.wood;
				trees.Add(newTree);
			}
			p.IdleCheck(adjacentVel, adjacentParticle);

			if (xCollision.other != cellType.empty || yCollision.other != cellType.empty)
			{
				Particle collidedType = new Particle(), other1 = new Particle(), other2 = new Particle();
				if (yCollision.location == 0)
				{
					collidedType = SearchCollided(p, 0, 1);
					other1 = SearchCollided(p, -1, 0);
					other2 = SearchCollided(p, 1, 0);
				}
				if (yCollision.location == 1)
				{
					collidedType = SearchCollided(p, 0, -1);
					other1 = SearchCollided(p, -1, 0);
					other2 = SearchCollided(p, 1, 0);

				}
				if (xCollision.location == 2)
				{
					collidedType = SearchCollided(p, 1, 0);
					other1 = SearchCollided(p, 0, -1);
					other2 = SearchCollided(p, 0, 1);
				}
				else if (xCollision.location == 3)
				{
					collidedType = SearchCollided(p, -1, 0);
					other1 = SearchCollided(p, 0, -1);
					other2 = SearchCollided(p, 0, 1);
				}

				if (collisions.check(p, collidedType))
				{
					adjacentCoordinates[0] = CheckRange((p.y + 1), height);
					adjacentCoordinates[1] = CheckRange((p.y - 1), height);
					adjacentCoordinates[2] = CheckRange((p.x - 1), width);
					adjacentCoordinates[3] = CheckRange((p.x + 1), width);

					wakeAdjacent(p.x, p.y, adjacentCoordinates);
				}
				if (collisions.check(p, other1))
				{
					adjacentCoordinates[0] = CheckRange((p.y + 1), height);
					adjacentCoordinates[1] = CheckRange((p.y - 1), height);
					adjacentCoordinates[2] = CheckRange((p.x - 1), width);
					adjacentCoordinates[3] = CheckRange((p.x + 1), width);

					wakeAdjacent(p.x, p.y, adjacentCoordinates);
				}
				if (collisions.check(p, other2))
				{
					adjacentCoordinates[0] = CheckRange((p.y + 1), height);
					adjacentCoordinates[1] = CheckRange((p.y - 1), height);
					adjacentCoordinates[2] = CheckRange((p.x - 1), width);
					adjacentCoordinates[3] = CheckRange((p.x + 1), width);

					wakeAdjacent(p.x, p.y, adjacentCoordinates);
				}
			}

			gridTexture.SetPixel(p.previousX, p.previousY, Colour[(int)cellType.empty]);
			gridTexture.SetPixel(p.x, p.y, Colour[(int)p.particleType]);

			if (p.active)
			{
				cells[p.x, p.y].UnSettle();
			}
			if (p.active == false)
			{
				cells[p.x, p.y].Settle();
				this.inactiveParticles[p.x, p.y] = p;
				activeParticles.Remove(p);
			}
		}
	}

	public void UpdateCharacters()
	{
		foreach (Character c in characters)
		{
			SetCharacterHitBox((int)(Mathf.Floor(c.x)), (int)(Mathf.Floor(c.y)), cellType.empty);
			Vector2[] adjVel = new Vector2[12];
			cellType[] adjParticle = new cellType[12];
			Vector2[] adjCoord = new Vector2[12];

			c.ApplyGravity();

			adjCoord[0] = new Vector2(CheckRange((int)Mathf.Floor(c.x) - 1, width), CheckRange((int)Mathf.Floor(c.y) + 2, height));
			adjCoord[1] = new Vector2(CheckRange((int)Mathf.Floor(c.x), width), CheckRange((int)Mathf.Floor(c.y) + 2, height));
			adjCoord[2] = new Vector2(CheckRange((int)Mathf.Floor(c.x) + 1, width), CheckRange((int)Mathf.Floor(c.y) + 2, height));

			adjCoord[3] = new Vector2(CheckRange((int)Mathf.Floor(c.x) - 1, width), CheckRange((int)Mathf.Floor(c.y) - 2, height));
			adjCoord[4] = new Vector2(CheckRange((int)Mathf.Floor(c.x), width), CheckRange((int)Mathf.Floor(c.y) - 2, height));
			adjCoord[5] = new Vector2(CheckRange((int)Mathf.Floor(c.x) + 1, width), CheckRange((int)Mathf.Floor(c.y) - 2, height));

			for (int i = 0; i < 6; i++)
			{
				adjParticle[i] = cells[(int)(adjCoord[i].x), (int)(adjCoord[i].y)].particleType;
				adjVel[i] = cells[(int)(adjCoord[i].x), (int)(adjCoord[i].y)].velocity;
			}

			c.Jump(adjParticle);
			c.UpdateY(adjVel, adjParticle);

			adjCoord[6] = new Vector2(CheckRange((int)Mathf.Floor(c.x) - 2, width), CheckRange((int)Mathf.Floor(c.y) - 1, height));
			adjCoord[7] = new Vector2(CheckRange((int)Mathf.Floor(c.x) - 2, width), CheckRange((int)Mathf.Floor(c.y), height));
			adjCoord[8] = new Vector2(CheckRange((int)Mathf.Floor(c.x) - 2, width), CheckRange((int)Mathf.Floor(c.y) + 1, height));

			adjCoord[9] = new Vector2(CheckRange((int)Mathf.Floor(c.x) + 2, width), CheckRange((int)Mathf.Floor(c.y) - 1, height));
			adjCoord[10] = new Vector2(CheckRange((int)Mathf.Floor(c.x) + 2, width), CheckRange((int)Mathf.Floor(c.y), height));
			adjCoord[11] = new Vector2(CheckRange((int)Mathf.Floor(c.x) + 2, width), CheckRange((int)Mathf.Floor(c.y) + 1, height));

			for (int i = 6; i < 12; i++)
			{
				adjParticle[i] = cells[(int)(adjCoord[i].x), (int)(adjCoord[i].y)].particleType;
				adjVel[i] = cells[(int)(adjCoord[i].x), (int)(adjCoord[i].y)].velocity;
			}
			c.UpdateX(adjVel, adjParticle);
			SetCharacterHitBox((int)(Mathf.Floor(c.x)), (int)(Mathf.Floor(c.y)), cellType.character);
			float relX = c.x;
			float relY = c.y;
			if (c.x != 0)
				relX = c.x / width;
			if (c.y != 0)
				relY = c.y / height;
			charGO[characters.IndexOf(c)].GetComponent<MoveCharacter>().MoveChar(relX, relY, c.movingLeft);
		}
	}

	public void SetCharacterHitBox(int x, int y, cellType boxType)
	{
		int[] xRange = new int[3];
		int[] yRange = new int[3];

		xRange[0] = CheckRange(x - 1, width);
		xRange[1] = x;
		xRange[2] = CheckRange(x + 1, width);

		yRange[0] = CheckRange(y - 1, height);
		yRange[1] = y;
		yRange[2] = CheckRange(y + 1, height);

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				cells[xRange[i], yRange[j]].SetParticle(boxType, new Vector2(0.0f, 0.0f));
				gridTexture.SetPixel(xRange[i], yRange[j], Colour[(int)boxType]);
			}
		}
	}

	void FixedUpdate()
	{
		//txt = cam.GetComponent<Text>();
		//txt.text = "Active Particles: " + activeParticles.Count;
		if (delay <= Time.time && createParticles)
		{

			if (CreateParticle(offset, 0.8f))
			{
				delay = Time.time + 0.2f;//Modify to change frequency of particles
			}
			else
			{
				offset = (offset += (1.0f / width)) % 1;
			}
		}
		UpdateActiveParticles();

		/*if (charDelay <= Time.time)
		{
			CreateCharacter(width, height);
			charDelay = Time.time + 10.0f;
		}*/

		UpdateActiveParticles();
		UpdateCharacters();
		gridTexture.Apply();

		GetComponent<Renderer>().material.mainTexture = gridTexture;
		for (int i = trees.Count - 1; i >= 0; i--)
		{
			trees[i] = Grow(trees[i]);
			if (trees[i].remainingGrowth <= 0)
			{
				trees.Remove(trees[i]);
			}
		}
		gridTexture.Apply();

		GetComponent<Renderer>().material.mainTexture = gridTexture;

	}

	public Particle SearchCollided(Particle current, int x, int y)
	{
		Particle newP;
		if ((current.x + x) < 0)
		{
			newP = new Particle(width - 1, current.y + y, cells[width - 1, current.y + y].particleType);
		}
		else if (current.x + x >= width)
		{
			newP = new Particle(0, current.y + y, cells[0, current.y + y].particleType);
		}
		else if ((current.y + y) >= height)
		{
			newP = new Particle(current.x + x, 0, cells[current.x + x, 0].particleType);
		}
		else if ((current.y + y) < 0)
		{
			newP = new Particle(current.x + x, (height - 1), cells[current.x + x, (height - 1)].particleType);
		}
		else
		{
			newP = new Particle(current.x + x, current.y + y, cells[current.x + x, current.y + y].particleType);
		}
		return newP;
	}

	public void CreateTree(int x, int y, int remainingGrowth)
	{
		Tree newTree = new Tree();
		newTree.x = x;
		newTree.y = y;
		newTree.remainingGrowth = remainingGrowth + Random.Range(-remainingGrowth / 2, remainingGrowth / 2);
		newTree.branchPoint = (newTree.remainingGrowth / 2) + Random.Range(-newTree.remainingGrowth / 2 + 1, newTree.remainingGrowth / 2);
		newTree.growthRate = 0.6f;
		newTree.nextGrow = Time.time + newTree.growthRate;
		trees.Add(newTree);
	}

	public Tree Grow(Tree tree)
	{
		int[] xRange = new int[3];
		int[] yRange = new int[3];

		xRange[0] = CheckRange(tree.x - 1, width);
		xRange[1] = tree.x;
		xRange[2] = CheckRange(tree.x + 1, width);

		yRange[0] = CheckRange(tree.y - 1, height);
		yRange[1] = tree.y;
		yRange[2] = CheckRange(tree.y + 1, height);

		cellType growthType = cellType.wood;

		if (tree.remainingGrowth < 5)
			growthType = cellType.bush;

		if (Time.time < tree.nextGrow)
		{
			if (cells[xRange[1], yRange[2]].particleType == cellType.empty && !tree.branch)
			{
				tree = AttemptGrowth(tree, xRange[1], yRange[2], growthType);
			}
			else
			{
				if (cells[xRange[0], yRange[2]].particleType == cellType.empty && cells[xRange[2], yRange[2]].particleType == cellType.empty)
				{
					int rand = Random.Range(0, 2);
					switch (rand)
					{
						case 0:
							tree = AttemptGrowth(tree, xRange[0], yRange[2], growthType);
							break;
						case 1:
							tree = AttemptGrowth(tree, xRange[2], yRange[2], growthType);
							break;
					}
				}
				else
				{
					if (cells[xRange[0], yRange[2]].particleType == cellType.empty)
					{
						tree = AttemptGrowth(tree, xRange[0], yRange[2], growthType);
					}
					else
						if (cells[xRange[2], yRange[2]].particleType == cellType.empty)
					{
						tree = AttemptGrowth(tree, xRange[2], yRange[2], growthType);
					}
					else
					{
						if (cells[xRange[0], yRange[1]].particleType == cellType.empty && cells[xRange[2], yRange[1]].particleType == cellType.empty)
						{
							int rand = Random.Range(0, 2);
							switch (rand)
							{
								case 0:
									tree = AttemptGrowth(tree, xRange[0], yRange[1], growthType);
									break;
								case 1:
									tree = AttemptGrowth(tree, xRange[2], yRange[1], growthType);
									break;
							}
						}
						else
						{
							if (cells[xRange[0], yRange[1]].particleType == cellType.empty)
							{
								tree = AttemptGrowth(tree, xRange[0], yRange[1], growthType);
							}
							else
								if (cells[xRange[2], yRange[1]].particleType == cellType.empty)
							{
								tree = AttemptGrowth(tree, xRange[2], yRange[1], growthType);
							}
						}
					}
				}
			}
			if (tree.remainingGrowth == tree.branchPoint)
			{
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 2; j++)
					{
						if (i != 1 && cells[xRange[i], yRange[j]].particleType == cellType.empty)
							CreateTree(xRange[i], yRange[j], tree.remainingGrowth);
					}
				}
				tree.branch = true;
				tree.branchPoint = tree.remainingGrowth / 2 + Random.Range(-tree.remainingGrowth / 2 + 1, tree.remainingGrowth / 2);
				if (tree.branchPoint < 2)
					tree.branchPoint = 100;
			}
			tree.nextGrow = Time.time + tree.growthRate;
		}

		return tree;
	}

	public Tree AttemptGrowth(Tree tree, int x, int y, cellType growthType)
	{
		inactiveParticles[x, y] = new Particle(x, y, growthType, State.solid, true);
		cells[x, y].SetParticle(growthType, new Vector2(0.0f, 0.0f));
		gridTexture.SetPixel(x, y, Colour[(int)growthType]);
		tree.remainingGrowth = tree.remainingGrowth - 1;
		tree.x = x;
		tree.y = y;
		return tree;
	}
}
