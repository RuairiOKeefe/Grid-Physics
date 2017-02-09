using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator{


	private GameCell newCell = null;

	public GameCell Simulate(ref GameCell cell, int width, int height)
	{
		//Currently completely broken (is a prototype anyhow) need to rethink
		newCell = new GameCell();


		if (cell.moveTime < Time.time)
		{
			int newY = cell.y - 1;
			if (newY < 0)
			{
				newY = 255;
			}

			newCell.Set(cell.x, cell.y, cell.settled, cell.type, cell.velocity, 1);
			newCell.SetMoveTime(Time.time - cell.velocity.y);//hacky because y vel is currently always -1
		}

		return newCell;
	}
}
