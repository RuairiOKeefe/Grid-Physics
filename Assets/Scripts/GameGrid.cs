using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
	public int width;
	public int height;

	GameCell[,] cells;


	// Use this for initialization
	void Start () {
		CreateGrid();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		print(cells[1,6].X.ToString()  + ", " + cells[1, 6].Y.ToString());
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

	}
}
