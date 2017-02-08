using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cellType
{
	empty,//May want to rename to air?
	sand //Course, rough, gets everywhere
}

public class GameCell
{
    public int x { get; private set; }
    public int y { get; private set; }

	public bool settled;

	public cellType type;

	public Vector2 velocity;

	public float moveTime;

	public void Set(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public void CreateParticle(cellType spawnType)
	{
		this.type = spawnType;
		velocity = new Vector2(0.0f, -1.0f);
	}
}
