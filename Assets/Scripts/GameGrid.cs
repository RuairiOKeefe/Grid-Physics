using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
	public int width;
	public int height;

	public Color emptyCol;
	public Color sandCol;
	public Color stoneCol;

	GameCell[,] cells;

	Simulator simulator = new Simulator();

	Texture2D gridTexture;

	// Use this for initialization
	void Start ()
	{
		CreateGrid();
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
				if (i == j)
				{
					cells[i, j].CreateParticle(cellType.sand);
				}
			}
		}
	}

	public void CreateParticle(float x, float y)//Create; Polymorphise. Whats the difference? **Should pass type too**
	{

		// x and y must be in range 0-1
		//may add random effect to "spray" particles 
		int gridX = Mathf.RoundToInt(x * width);
		int gridY = Mathf.RoundToInt(y * width);

		cells[gridX, gridY].CreateParticle(cellType.sand);

	}

	
	// Update is called once per frame
	void FixedUpdate ()
	{
		cells = simulator.Simulate(ref cells);

		foreach(GameCell c in cells)
		{
			if (c.type == cellType.empty)
			{
				gridTexture.SetPixel(c.x, c.y, emptyCol);
			}
			if (c.type == cellType.sand)
			{
				gridTexture.SetPixel(c.x, c.y, sandCol);
			}
		}
		gridTexture.Apply();

		GetComponent<Renderer>().material.mainTexture = gridTexture;
	}
}
