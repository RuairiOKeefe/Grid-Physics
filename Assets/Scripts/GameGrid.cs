using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cellType
{
	empty,
	sand,
	stone,
	lava,
	water,
	plant
}

public struct collision
{
	public cellType other;
	public int location;
}

public class GameGrid : MonoBehaviour
{
	public int width;
	public int height;

	public cellType particleType;

	public Color[] Colour = new Color[6];

	Cell[,] cells;

	HashSet<Particle> activeParticles = new HashSet<Particle>();
	HashSet<Particle> inactiveParticles = new HashSet<Particle>();

	float delay;//debug temp
	float offset;//debug temp

	float spawnDelay;

	Texture2D gridTexture;

	// Use this for initialization
	void Start ()
	{
		CreateGrid();
	}

	int CheckRange(int coord, int range)
	{
		int newCoord = coord;
		if (coord < 0)
			newCoord = range-1;
		if (coord > range-1)
			newCoord = 0;

		return newCoord;
	}

    void CreateGrid()
    {
		cells = new Cell[width, height]; //Create an multidimensional array of cells to fit the grid

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
				if (j < 2 )
				{
					cells[i, j].SetParticle(particleType, new Vector2(0,0)); //May want to set as sand
				}
			}
		}
	}

	public void ChangeType(cellType type)
	{
		particleType = type;
	}

	public bool CreateParticle(float x, float y)//Create; Polymorphise. Whats the difference?
	{
		// x and y must be in range 0-1
		//may add random effect to "spray" particles 
		int gridX = Mathf.RoundToInt(x * width);
		int gridY = Mathf.RoundToInt(y * height);

		if (cells[gridX, gridY].particleType == cellType.empty && spawnDelay <= Time.time)
		{
			activeParticles.Add(new Particle(gridX, gridY, particleType, new Vector2(0.0f, -9.8f), width, height));
			spawnDelay = Time.time + 0.02f;
			return true;
		}
		else
		{
			return false;
		}
	}

	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(delay <= Time.time)
		{
			if (CreateParticle(offset, 0.8f))
				delay = Time.time + 0.1f;
			else
			{
				offset += 1.0f / width;
			}
		}
		
		foreach (Particle p in activeParticles)
		{
			Vector2[] adjVel = new Vector2[4]; //Adjacent velocities. Up Down Left Right
			cellType[] adjParticle = new cellType[4]; //Adjacent particles. Up Down Left Right
			int[] adjCoord = new int[4];

			collision xColl;
			collision yColl;
			adjCoord[0] = CheckRange((p.y+1), height);
			adjCoord[1] = CheckRange((p.y-1), height);
			
			adjVel[0] = cells[p.x, adjCoord[0]].velocity;
			adjVel[1] = cells[p.x, adjCoord[1]].velocity;

			adjParticle[0] = cells[p.x, adjCoord[0]].particleType;
			adjParticle[1] = cells[p.x, adjCoord[1]].particleType;

			yColl = p.UpdateY(adjVel, adjParticle);

			cells[p.prevX, p.prevY].SetParticle(cellType.empty, new Vector2(0.0f, 0.0f));
			cells[p.x, p.y].SetParticle(p.particleType, p.velocity);

			adjCoord[2] = CheckRange((p.x-1), width);
			adjCoord[3] = CheckRange((p.x+1), width);

			adjVel[2] = cells[adjCoord[2], p.y].velocity;
			adjVel[3] = cells[adjCoord[3], p.y].velocity;

			adjParticle[2] = cells[adjCoord[2], p.y].particleType;
			adjParticle[3] = cells[adjCoord[3], p.y].particleType;

			xColl = p.UpdateX(adjVel, adjParticle);

			cells[p.prevX, p.prevY].SetParticle(cellType.empty, new Vector2(0.0f, 0.0f));
			cells[p.x, p.y].SetParticle(p.particleType, p.velocity);

			if (p.active)
			{
				cells[p.x, p.y].UnSettle();
			}
			if (p.active == false)
			{
				inactiveParticles.Add(p);
				activeParticles.Remove(p);
			}
		}
		

		foreach (Cell c in cells)
		{
			if (!c.settled)
			{
				gridTexture.SetPixel(c.x, c.y, Colour[(int)c.particleType]);
				if (c.velocity.x == 0 && c.velocity.y == 0)
				{
					c.Settle();
				}
			}
		}
		gridTexture.Apply();

		GetComponent<Renderer>().material.mainTexture = gridTexture;
	}
}
