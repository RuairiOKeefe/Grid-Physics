using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
	public int x;
	public int y;
	public bool active = true;
	public Vector2 velocity;
	public cellType particleType;

	public int prevX { get; private set; }
	public int prevY { get; private set; }

	private float moveTimeX;
	private float moveTimeY;
	private bool timingOut = false;
	private float inactiveTime;
	private int width;
	private int height;

	private float shiftDelay;

	public Particle()
	{
		
	}
    public Particle(int x , int y , cellType particleType)
    {
        this.x = x;
        this.y = y;
        this.particleType = particleType;
        this.velocity = new Vector2(0.0f, 0.0f);
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

	public collision AttemptX(Vector2[] adjVel, cellType[] adjParticle)
    {
		prevX = x;
		collision coll = new collision();
		coll.other = cellType.empty;
        if (velocity.x < 0)
        {
			if (adjParticle[2] != cellType.empty)
			{
				defaultCollision(false, false, adjVel[2]);
				coll = new collision();
				coll.other = adjParticle[2];
				coll.location = 2;
			}
			else
			{
				x--;
				if (x < 0)
					x = width-1;
			}

			if (velocity.x != 0)
				moveTimeX = 10 / Mathf.Abs(velocity.x);
			return coll;
		}
        else
        {
			if (adjParticle[3] != cellType.empty)
			{
				defaultCollision(false, true, adjVel[3]);
				coll = new collision();
				coll.other = adjParticle[3];
				coll.location = 3;
			}
			else
			{
				x++;
				if (x > width-1)
					x = 0;
			}
			if (velocity.x != 0)
				moveTimeX = 10 / Mathf.Abs(velocity.x);
			return coll;
		}

        
    }

    public collision AttemptY(Vector2[] adjVel, cellType[] adjParticle)
    {
        prevY = y;
		collision coll = new collision();
		coll.other = cellType.empty;
		if (velocity.y < 0)
        {
			if (adjParticle[1] != cellType.empty)
			{
				defaultCollision(true, false, adjVel[1]);
				coll = new collision();
				coll.other = adjParticle[1];
				coll.location = 1;
			}
			else
			{
				y--;
				if (y < 0)
					y = height-1;
			}
			if (velocity.y != 0)
				moveTimeY = 10 / Mathf.Abs(velocity.y);
			return coll;
		}
        else
        {
			if (adjParticle[0] != cellType.empty)
			{
				defaultCollision(true, true, adjVel[0]);
				coll = new collision();
				coll.other = adjParticle[0];
				coll.location = 0;
			}
			else
			{
				y++;
				if (y > height-1)
					y = 0;
			}
			if(velocity.y != 0)
				moveTimeY = 10 / Mathf.Abs(velocity.y);
			return coll;
		}
    }

	public collision UpdateX(Vector2[] adjVel, cellType[] adjParticle) 
	{
		collision coll = new collision();
		coll.other = cellType.empty;

		//if (this.particleType == cellType.water || this.particleType == cellType.lava)//May want to create liquid bool for particles
		//	LiquidShift(adjParticle);

		if (active)
		{
			if (moveTimeX <= Time.time && (velocity.x != 0))
			{
				return AttemptX(adjVel, adjParticle);
			}
		}
		return coll;
	}

	public collision UpdateY(Vector2[] adjVel, cellType[] adjParticle)
	{
		collision coll = new collision();
		coll.other = cellType.empty;
		if (active)
		{
			ApplyGravity();

			if (moveTimeY <= Time.time && (velocity.y != 0))
			{
				return AttemptY(adjVel, adjParticle);
			}

			if (velocity.x == 0 && velocity.y == 0) //If not moving check to see if it is timing out, if not set a timer, if it is, check if the time is up and if it is make this inactive
			{
				if (!timingOut)
				{
					inactiveTime = Time.time + 3.0f;
					timingOut = true;
				}

				if (timingOut && inactiveTime <= Time.time)
				{
					velocity = new Vector2(0.0f, 0.0f);
					active = false;
				}
				return coll;
			}
			else //If it is moving make sure it is not timing out
			{
				if (timingOut)
					timingOut = false;
			}
		}
		return coll;
	}

	public void ApplyGravity()
	{
		if (velocity.y != -9.8f)
		{
			velocity.y += (-9.8f * Time.deltaTime);
		}
	}

	public void LiquidShift(cellType[] adjParticle)
	{
		float speed = 0.0f;
		if (particleType == cellType.water)
			speed = 5.0f;
		if (particleType == cellType.lava)
			speed = 2.0f;
		if (adjParticle[1] != cellType.empty)
		{
			if (shiftDelay < Time.time)
			{
				if (adjParticle[2] == cellType.empty && adjParticle[3] == cellType.empty)
				{
					int rand = Random.Range(0, 2);
					switch (rand)
					{
						case 0:
							velocity.x = -speed;
							break;
						case 1:
							velocity.x = speed;
							break;
					}
				}
				else
				{
					if (adjParticle[2] == cellType.empty)
					{
						velocity.x = -speed;
					}
					else
						if (adjParticle[3] == cellType.empty)
						{
							velocity.x = speed;
						}
						else
						{
							velocity.x = 0.0f;
						}
				}
				shiftDelay = Time.time + 0.2f;
			}
			velocity.y = -9.8f;
		}
	}

	public void defaultCollision(bool yAxis, bool positive, Vector2 adjVel)
	{
		if (yAxis)
		{
			if (positive)
			{
				this.velocity.y -= (adjVel.y - this.velocity.y);
			}
			else
			{
				this.velocity.y += (adjVel.y - this.velocity.y);
			}
		}
		else
		{
			{
				if (positive)
				{
					this.velocity.x -= (adjVel.x - this.velocity.x);
				}
				else
				{
					this.velocity.x += (adjVel.x - this.velocity.x);
				}
			}
		}
	}
}
