using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCell
{
    public int x { get; private set; }
    public int y { get; private set; }

	public bool settled { get; private set; }

	public cellType particleType { get; private set; }

	public void Set(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public void SetParticle(cellType spawnType)
	{
		this.particleType = spawnType;
	}

	public void Settle()
	{
		this.settled = true;
	}
}
