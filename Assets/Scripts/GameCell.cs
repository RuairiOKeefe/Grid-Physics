using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCell
{
	//Removed monobehavior because I believe it is not required and reduces performance
    public int X { get; private set; }
    public int Y { get; private set; }

	public void Set(int x, int y)
	{
		X = x;
		Y = y;
	}

}
