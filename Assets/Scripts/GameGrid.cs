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

public enum Substance //Material is used already by steamVR...
{
	Empty,
	Sand,
	Stone,
	Lava,
	Water,
	Plant,
	Fire,
	Wood,
	Root,
	Bush,
	Steam,
	Smoke,
	Ice,
	Character
}

public struct Collide
{
	public Substance other;
	public int location;
}

public struct Tree
{
	public Particle currentParticle;
	public int remainingGrowth;
	public float growthRate;
	public float nextGrow;
}

public class GameGrid : MonoBehaviour
{
	public bool createParticles;//Used for debugging without vive

	public int width;
	public int height;

	public Substance particleType;

	public State particleState;

	public Color[] Colour = new Color[6];

	public GameObject charPrefab;

	public GameObject cam;//Active particle debug stuff
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

	void wakeAdj(Cell c, int[] adjCoord)
	{
		if ((cells[c.x, adjCoord[0]].settled) && (cells[c.x, adjCoord[0]].particleType != Substance.Empty) && (!cells[c.x, adjCoord[0]].barrier))
		{
			Particle p = inactiveParticles[c.x, adjCoord[0]];
			p.active = true;
			cells[c.x, adjCoord[0]].UnSettle();
			cells[c.x, adjCoord[0]].SetParticle(Substance.Empty, new Vector2(0.0f, 0.0f));
			inactiveParticles[c.x, adjCoord[0]] = null;
			activeParticles.Add(p);
		}
		if ((cells[c.x, adjCoord[1]].settled) && (cells[c.x, adjCoord[1]].particleType != Substance.Empty) && (!cells[c.x, adjCoord[1]].barrier))
		{
			Particle p = inactiveParticles[c.x, adjCoord[1]];
			p.active = true;
			cells[c.x, adjCoord[1]].UnSettle();
			cells[c.x, adjCoord[1]].SetParticle(Substance.Empty, new Vector2(0.0f, 0.0f));
			inactiveParticles[c.x, adjCoord[1]] = null;
			activeParticles.Add(p);
		}
		if ((cells[adjCoord[2], c.y].settled) && (cells[adjCoord[2], c.y].particleType != Substance.Empty) && (!cells[adjCoord[2], c.y].barrier))
		{
			Particle p = inactiveParticles[adjCoord[2], c.y];
			p.active = true;
			cells[adjCoord[2], c.y].UnSettle();
			cells[adjCoord[2], c.y].SetParticle(Substance.Empty, new Vector2(0.0f, 0.0f));
			inactiveParticles[adjCoord[2], c.y] = null;
			activeParticles.Add(p);
		}
		if ((cells[adjCoord[3], c.y].settled) && (cells[adjCoord[3], c.y].particleType != Substance.Empty) && (!cells[adjCoord[3], c.y].barrier))
		{
			Particle p = inactiveParticles[adjCoord[3], c.y];
			p.active = true;
			cells[adjCoord[3], c.y].UnSettle();
			cells[adjCoord[3], c.y].SetParticle(Substance.Empty, new Vector2(0.0f, 0.0f));
			inactiveParticles[adjCoord[3], c.y] = null;
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
					Particle p = new Particle(i, j, Substance.Stone, State.solid, new Vector2(0.0f, 0.0f), width, height);
					p.active = false;
					inactiveParticles[i, j] = p;
					cells[i, j].Settle();
					cells[i, j].SetBarrier();
					cells[i, j].SetParticle(Substance.Stone, new Vector2(0, 0));
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
					Particle p = new Particle(i, j, Substance.Stone, State.solid, new Vector2(0.0f, 0.0f), width, height);
					p.active = false;
					inactiveParticles[i, j] = p;
					cells[i, j].Settle();
					cells[i, j].SetParticle(Substance.Stone, new Vector2(0, 0));
				}

				if (j > 70 && j < 81)
				{
					int rand = Random.Range(0, 10);
					if (rand + (j - 76) > 5)
					{
						Particle p = new Particle(i, j, Substance.Sand, State.solid, new Vector2(0.0f, 0.0f), width, height);
						p.active = false;
						inactiveParticles[i, j] = p;
						cells[i, j].Settle();
						cells[i, j].SetParticle(Substance.Sand, new Vector2(0, 0));
					}
					else
					{
						Particle p = new Particle(i, j, Substance.Stone, State.solid, new Vector2(0.0f, 0.0f), width, height);
						p.active = false;
						inactiveParticles[i, j] = p;
						cells[i, j].Settle();
						cells[i, j].SetParticle(Substance.Stone, new Vector2(0, 0));
					}
				}

				if (j > 80 && j < 150)
				{
					Particle p = new Particle(i, j, Substance.Stone, State.solid, new Vector2(0.0f, 0.0f), width, height);
					p.active = false;
					inactiveParticles[i, j] = p;
					cells[i, j].Settle();
					cells[i, j].SetParticle(Substance.Sand, new Vector2(0, 0));
				}
			}
		}
	}

	public void ChangeType(Substance type)
	{
		particleType = type;

		if (type == Substance.Empty)
			particleState = State.empty;
		else
			if (type == Substance.Fire || type == Substance.Smoke || type == Substance.Steam)
				particleState = State.gas;
			else
				if (type == Substance.Water || type == Substance.Lava)
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

		if (cells[gridX, gridY].particleType == Substance.Empty)
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
			int randX = gridX + Random.Range(-8, 8);
			int randY = gridY + Random.Range(-8, 8);

			if (cells[randX, randY].particleType == Substance.Empty)
			{
				activeParticles.Add(new Particle(randX, randY, particleType, particleState, new Vector2(0.0f, -9.8f), width, height));
				cells[randX, randY].SetParticle(particleType, new Vector2(0.0f, -9.8f));
			}
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
			cells[p.x, p.y].SetParticle(Substance.Empty, new Vector2(0.0f, 0.0f));
			cells[p.x, p.y].Settle();
			gridTexture.SetPixel(p.x, p.y, Colour[(int)Substance.Empty]);
			Vector2[] adjVel = new Vector2[4]; //Adjacent velocities. Up Down Left Right
			Substance[] adjParticle = new Substance[4]; //Adjacent particles. Up Down Left Right
			int[] adjCoord = new int[4];

			Collisions col = new Collisions();

			Collide xColl;
			Collide yColl;

			adjCoord[0] = CheckRange((p.y + 1), height);
			adjCoord[1] = CheckRange((p.y - 1), height);

			adjVel[0] = cells[p.x, adjCoord[0]].velocity;
			adjVel[1] = cells[p.x, adjCoord[1]].velocity;

			adjParticle[0] = cells[p.x, adjCoord[0]].particleType;
			adjParticle[1] = cells[p.x, adjCoord[1]].particleType;

			yColl = p.UpdateY(adjVel, adjParticle);

			gridTexture.SetPixel(p.previousX, p.previousY, Colour[(int)Substance.Empty]);
			cells[p.previousX, p.previousY].SetParticle(Substance.Empty, new Vector2(0.0f, 0.0f));
			cells[p.previousX, p.previousY].Settle();

			adjCoord[2] = CheckRange((p.x - 1), width);
			adjCoord[3] = CheckRange((p.x + 1), width);

			adjVel[2] = cells[adjCoord[2], p.y].velocity;
			adjVel[3] = cells[adjCoord[3], p.y].velocity;

			adjParticle[2] = cells[adjCoord[2], p.y].particleType;
			adjParticle[3] = cells[adjCoord[3], p.y].particleType;
			xColl = p.UpdateX(adjVel, adjParticle);

			cells[p.previousX, p.previousY].SetParticle(Substance.Empty, new Vector2(0.0f, 0.0f));
			cells[p.x, p.y].SetParticle(p.particleType, p.velocity);


			adjCoord[0] = CheckRange((p.y + 1), height);
			adjCoord[1] = CheckRange((p.y - 1), height);

			adjVel[0] = cells[p.x, adjCoord[0]].velocity;
			adjVel[1] = cells[p.x, adjCoord[1]].velocity;

			adjParticle[0] = cells[p.x, adjCoord[0]].particleType;
			adjParticle[1] = cells[p.x, adjCoord[1]].particleType;

			adjCoord[2] = CheckRange((p.x - 1), width);
			adjCoord[3] = CheckRange((p.x + 1), width);

			adjVel[2] = cells[adjCoord[2], p.y].velocity;
			adjVel[3] = cells[adjCoord[3], p.y].velocity;

			adjParticle[2] = cells[adjCoord[2], p.y].particleType;
			adjParticle[3] = cells[adjCoord[3], p.y].particleType;

			if (p.particleType == Substance.Root && p.velocity == new Vector2(0.0f, 0.0f))
			{
				Tree newTree = new Tree();
				trees.Add(newTree);
				newTree.currentParticle = p;
				newTree.remainingGrowth = 20;
				newTree.growthRate = 0.4f;
				newTree.nextGrow = Time.time + newTree.growthRate;
				p.particleType = Substance.Wood;
			}

			p.IdleCheck(adjVel, adjParticle);

			if (xColl.other != Substance.Empty || yColl.other != Substance.Empty)
			{
				Substance collidedType = Substance.Empty, other1 = Substance.Empty, other2 = Substance.Empty;
				if (yColl.location == 0)
				{
					collidedType = Search_Collided(p, 0, 1);
					other1 = Search_Collided(p, -1, 0);
					other2 = Search_Collided(p, 1, 0);
				}
				if (yColl.location == 1)
				{
					collidedType = Search_Collided(p, 0, -1);
					other1 = Search_Collided(p, -1, 0);
					other2 = Search_Collided(p, 1, 0);

				}
				if (xColl.location == 2)
				{
					collidedType = Search_Collided(p, -1, 0);
					other1 = Search_Collided(p, -1, 0);
					other2 = Search_Collided(p, 1, 0);
				}
				else if (xColl.location == 3)
				{
					collidedType = Search_Collided(p, 1, 0);
					other1 = Search_Collided(p, -1, 0);
					other2 = Search_Collided(p, 1, 0);
				}
				col.check(p, collidedType);
				col.check(p, other1);
				col.check(p, other2);
			}

			gridTexture.SetPixel(p.previousX, p.previousY, Colour[(int)Substance.Empty]);
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
			SetCharacterHitBox((int)(Mathf.Floor(c.x)), (int)(Mathf.Floor(c.y)), Substance.Empty);
			Vector2[] adjVel = new Vector2[12];
			Substance[] adjParticle = new Substance[12];
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
			SetCharacterHitBox((int)(Mathf.Floor(c.x)), (int)(Mathf.Floor(c.y)), Substance.Character);
			float relX = c.x;
			float relY = c.y;
			if (c.x != 0)
				relX = c.x / width;
			if (c.y != 0)
				relY = c.y / height;
			charGO[characters.IndexOf(c)].GetComponent<MoveCharacter>().MoveChar(relX, relY, c.movingLeft);
		}
	}

	public void SetCharacterHitBox(int x, int y, Substance boxType)
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

	// Update is called once per frame
	void FixedUpdate()
	{
		txt = cam.GetComponent<Text>();
		txt.text = "Active Particles: " + activeParticles.Count;
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

		/*if (charDelay <= Time.time)
		{
			CreateCharacter(width, height);
			charDelay = Time.time + 10.0f;
		}*/

		UpdateActiveParticles();
		UpdateCharacters();
		gridTexture.Apply();

		GetComponent<Renderer>().material.mainTexture = gridTexture;
		foreach (Tree t in trees)
		{

		}
		gridTexture.Apply();

		GetComponent<Renderer>().material.mainTexture = gridTexture;

	}

	public Substance Search_Collided(Particle current, int x, int y)
	{
		Substance newP;
		if ((current.x + x) < 0)
		{
			newP = cells[width - 1, current.y + y].particleType;
		}
		else if (current.x + x >= width)
		{
			newP = cells[0, current.y + y].particleType;
		}
		else if ((current.y + y) >= height)
		{
			newP = cells[current.x + x, 0].particleType;
		}
		else if ((current.y + y) < 0)
		{
			newP = cells[current.x + x, (height - 1)].particleType;
		}
		else
		{
			newP = cells[current.x + x, current.y + y].particleType;
		}
		return newP;
	}

	public void Growing(Tree plant)
	{
		
	}
}
