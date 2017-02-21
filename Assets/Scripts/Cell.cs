using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int x { get; private set; }
    public int y { get; private set; }

	public bool settled { get; private set; }

	public cellType particleType { get; private set; }

	public Vector2 velocity = new Vector2(0,0);

	public void Set(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public void SetParticle(cellType particleType, Vector2 velocity)
	{
		this.particleType = particleType;
		this.velocity = velocity;
	}

	public Vector2 GetVelocity()
	{
		return velocity;
	}

	public void Settle()
	{
		this.settled = true;
	}

	public void UnSettle()
	{
		this.settled = false;
	}
}
