using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cellType
{
	empty,//May want to rename to air?
	sand, //Course, rough, gets everywhere
	stone
}

public class GameCell
{
    public int x { get; private set; }
    public int y { get; private set; }

	public bool settled { get; private set; }

	public cellType type { get; private set; }

	public Vector2 velocity { get; private set; }

	public float moveTime { get; private set; }

	public void Set(int x, int y , bool settled, cellType type , Vector2 vel , float move)
	{
		this.x = x;
		this.y = y;
		this.settled = settled;
		this.type = type;
		this.velocity = vel;
		this.moveTime = move;
	}

	public void CreateParticle(cellType spawnType)
	{
		moveTime = 0.5f;
		this.type = spawnType;
		velocity = new Vector2(0.0f, -1.0f);
	}

	public void SetMoveTime(float newTime)
	{
		this.moveTime = newTime;//maybe refactor
	}
}
