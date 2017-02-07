using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
	public int width;
	public int height;
	Texture2D gridTexture;

	GameCell[,] cells;

	Simulator simulator = new Simulator();



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
	}

	void CreateParticle(float x, float y)//Create; Polymorphise. Whats the difference? **Should pass type too**
	{
		//may add random effect to "spray" particles 
		int gridX = Mathf.RoundToInt(x * width);
		int gridY = Mathf.RoundToInt(y * width);

		cells[gridX, gridY].CreateParticle(cellType.sand);

	}

	
	// Update is called once per frame
	void FixedUpdate ()
	{
		simulator.Simulate(ref cells);
		//print(cells[1,6].x.ToString()  + ", " + cells[1, 6].y.ToString() + " type: " + cells[1,6].type.ToString());

		for (int i = 0; i < 128; i += 2)
		{
			for (int j = 0; j < 128; j += 2)
			{
				cells[i, j].CreateParticle(cellType.sand);
			}
		}

		foreach(GameCell c in cells)
		{
			if (c.type == cellType.empty)
			{
				gridTexture.SetPixel(c.x, c.y, Color.black);
			}
			if (c.type == cellType.sand)
			{
				gridTexture.SetPixel(c.x, c.y, Color.yellow);
			}
		}
		gridTexture.Apply();

		GetComponent<Renderer>().material.mainTexture = gridTexture;
	}
}
