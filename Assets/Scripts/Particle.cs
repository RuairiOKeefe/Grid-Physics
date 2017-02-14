using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
	public int x { get; private set; }
	public int y { get; private set; }
	public bool active = true;
	public Vector2 velocity { get; private set; }
	public cellType particleType { get; private set; }

	public int prevX { get; private set; }
	public int prevY { get; private set; }

	private float moveTimeX;
	private float moveTimeY;
	private bool timingOut = false;
	private float inactiveTime;
	private int width;
	private int height;

	public Particle()
	{
		
	}

	public Particle(int x, int y, cellType particleType, Vector2 velocity, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.particleType = particleType;
		this.velocity = velocity;
		active = true;
		prevX = x;
		prevY = y;
		this.width = width;
		this.height = height;
	}

	public void SetParticle(int x, int y, cellType particleType, Vector2 velocity, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.particleType = particleType;
		this.velocity = velocity;
		active = true;
		prevX = x;
		prevY = y;
		this.width = width;
		this.height = height;
	}

	public void SetLocation(int x, int y, Vector2 velocity)
	{
		this.x = x;
		this.y = y;
		this.velocity = velocity;
		prevX = x;
		prevY = y;
	}

	public void NextMove()
	{
		moveTimeX = 10 / Mathf.Abs(velocity.x);
		moveTimeY = 10 / Mathf.Abs(velocity.y);
	}

	public void Update() //Will need to add collisions
	{
		if (active)
		{
			if (velocity.x == 0 && velocity.y == 0) //If not moving check to see if it is timing out, if not set a timer, if it is, check if the time is up and if it is make this inactive
			{
				if (!timingOut)
				{
					inactiveTime = Time.time + 1.0f;
					timingOut = true;
				}

				if (timingOut && inactiveTime <= Time.time)
				{
					active = false;
				}
				return;
			}
			else //If it is moving make sure it is not timing out
			{
				if (timingOut)
					timingOut = false;
			}

			if (moveTimeX <= Time.time)
			{
				if (velocity.x != 0)
				{
					prevX = x;
					if (velocity.x < 0)
					{
						x--;
						if (x < 0)
							x = width;
					}
					else
					{
						x++;
						if (x > width)
							x = 0;
					}
				}
			}

			if (moveTimeY <= Time.time)
			{
				if (velocity.y != 0)
				{
					prevY = y;
					if (velocity.y < 0)
					{
						y--;
						if (y < 0)
							y = height;
					}
					else
					{
						y++;
						if (y > height)
							y = 0;
					}
				}
			}
			NextMove();
		}
	}
}
