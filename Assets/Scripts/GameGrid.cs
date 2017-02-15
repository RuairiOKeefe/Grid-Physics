using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cellType
{
	empty,
	sand,
	stone
}

public class GameGrid : MonoBehaviour
{
	public int width;
	public int height;

	public cellType particleType;

	public Color[] Colour = new Color[6];

	GameCell[,] cells;

	List<Particle> particles = new List<Particle>();

	Simulator simulator = new Simulator();//May be unecessary

	float delay;//temp

	Texture2D gridTexture;

	// Use this for initialization
	void Start ()
	{
		CreateGrid();
	}

	int CheckRange(int coord, int range)
	{
		if (coord < 0)
			coord = range-1;
		if (coord > range-1)
			coord = 0;

		return coord;
	}

    void CreateGrid()
    {
		cells = new GameCell[width, height]; //Create an multidimensional array of cells to fit the grid

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				GameCell cell = new GameCell();
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
				if (j < 64 || (i == 64 && j == 128) || ((i % 2 == 0) && j == 64))
				{
					cells[i, j].SetParticle(particleType, new Vector2(0,0)); //May want to set as sand
				}
			}
		}
	}

	public void CreateParticle(float x, float y)//Create; Polymorphise. Whats the difference?
	{
		// x and y must be in range 0-1
		//may add random effect to "spray" particles 
		int gridX = Mathf.RoundToInt(x * width);
		int gridY = Mathf.RoundToInt(y * height);

		//cells[gridX, gridY].SetParticle(particleType);
		particles.Add(new Particle(gridX, gridY, particleType, new Vector2(0, -9.8f), width, height));//This seems to cause some lag, about 2 seconds worth maybe create in advance?

	}

	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(Input.GetKeyDown("e") && delay < Time.time)
		{
			CreateParticle(0.25f, 0.8f);
			delay = Time.time + 2.0f;
		}

		foreach (Particle p in particles)
		{
			//This bit is duuuuuuumb, refine?
			Vector2[] adjVel = new Vector2[4]; //Adjacent velocities. Up Down Left Right
			cellType[] adjParticle = new cellType[4]; //Adjacent particles. Up Down Left Right
			int[] adjCoord = new int[4];
			adjCoord[0] = CheckRange(p.y++, height);
			adjCoord[1] = CheckRange(p.y--, height);
			adjCoord[2] = CheckRange(p.x--, width);
			adjCoord[3] = CheckRange(p.x++, width);

			print(adjCoord[2] + ", " + p.y);
			adjVel[0] = cells[p.x, adjCoord[0]].velocity;
			adjVel[1] = cells[p.x, adjCoord[1]].velocity;
			adjVel[2] = cells[adjCoord[2], p.y].velocity;
			adjVel[3] = cells[adjCoord[3], p.y].velocity;

			adjParticle[0] = cells[p.x, adjCoord[0]].particleType;
			adjParticle[1] = cells[p.x, adjCoord[1]].particleType;
			adjParticle[2] = cells[adjCoord[2], p.y].particleType;
			adjParticle[3] = cells[adjCoord[3], p.y].particleType;

			p.Update(adjVel, adjParticle);
			cells[p.prevX, p.prevY].SetParticle(cellType.empty, new Vector2(0.0f, 0.0f));
		}

		foreach (Particle p in particles)
		{
			cells[p.x, p.y].SetParticle(p.particleType, p.velocity);
		}
		

		foreach (GameCell c in cells)
		{
			if (!c.settled)
			{
				gridTexture.SetPixel(c.x, c.y, Colour[(int)c.particleType]);
				//c.Settle();
			}
		}
		gridTexture.Apply();

		GetComponent<Renderer>().material.mainTexture = gridTexture;
	}
}
