using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
	public int x;
	public int y;
	public bool active = true;
	public Vector2 velocity;
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

	public void AttemptX(Vector2[] adjVel, cellType[] adjParticle)
    {
		prevX = x;
        if (velocity.x < 0)
        {

			if (adjParticle[2] != cellType.empty)
			{
				this.velocity.x -= (adjVel[2].x - this.velocity.x);
			}
			else
			{
				x--;
				if (x < 0)
					x = width-1;
			}

			if (velocity.x != 0)
				moveTimeX = 10 / Mathf.Abs(velocity.x);
			return;
		}
        else
        {
			if (adjParticle[3] != cellType.empty)
			{
				this.velocity.x -= (adjVel[3].x - this.velocity.x);
			}
			else
			{
				x++;
				if (x > width-1)
					x = 0;
			}
			if (velocity.x != 0)
				moveTimeX = 10 / Mathf.Abs(velocity.x);
			return;
		}

        
    }

    public void AttemptY(Vector2[] adjVel, cellType[] adjParticle)
    {
        prevY = y;
		if (velocity.y < 0)
        {
			if (adjParticle[1] != cellType.empty)
			{
				this.velocity.y -= (adjVel[1].y - this.velocity.y);
			}
			else
			{
				y--;
				if (y < 0)
					y = height-1;
			}
			if (velocity.y != 0)
				moveTimeY = 10 / Mathf.Abs(velocity.y);
			return;
		}
        else
        {
			if (adjParticle[0] != cellType.empty)
			{
				this.velocity.y -= (adjVel[0].y - this.velocity.y);
			}
			else
			{
				y++;
				if (y > height-1)
					y = 0;
			}
			if(velocity.y != 0)
				moveTimeY = 10 / Mathf.Abs(velocity.y);
			return;
		}
    }

	public void Update(Vector2[] adjVel, cellType[] adjParticle) //Will need to fix collisions
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
                    AttemptX(adjVel, adjParticle);
				}
			}

			if (moveTimeY <= Time.time)
			{
				if (velocity.y != 0)
				{
                    AttemptY(adjVel, adjParticle);
				}
			}
		}
	}
}
