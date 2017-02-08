using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator{

	public GameCell[,] Simulate(ref GameCell[,] cells)
	{
        //Currently completely broken (is a prototype anyhow) need to rethink
		GameCell[,] newGrid = cells;
		foreach (GameCell c in cells)
		{
			if (c.moveTime < Time.time)
			{
				int newY = c.y - 1;
				if (newY < 0)
					newY = 256;
				newGrid[c.x, newY].Equals(c);
				newGrid[c.x, newY].moveTime = Time.time + cells[c.x, newY].moveTime;
			}
		}

		return newGrid;
	}
}
