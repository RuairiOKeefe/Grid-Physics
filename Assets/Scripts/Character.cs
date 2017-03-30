using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
	public float x;
	public float y;
	public float maxSpeed = 20;
	public Vector2 velocity;
	public float terminalVelocity = -50f;//may want to calculate as a function of mass

	private int width;
	private int height;
	private bool tryJump;
	private bool jumpCD;
	private float jumpTimer;
	public float initX;

	public Character(int width, int height)
	{
		this.width = width;
		this.height = height;
		velocity.x = maxSpeed;
		y = height - 100;
		x = width - 100;
	}

	public void UpdateX(Vector2[] adjVel, cellType[] adjParticle)
    {
		float attX = x + (velocity.x * Time.deltaTime);
        if (velocity.x < 0)
        {
			for (int i = 6; i < 9; i++)
			{
				if (adjParticle[i] != cellType.empty)
				{
					attX = (Mathf.Floor(x));
					tryJump = true;
				}
			}
			x = attX;
			if (x < 0)
				x = width - 1;
		}
        else
        {
			for (int i = 9; i < 12; i++)
			{
				if (adjParticle[i] != cellType.empty)
				{
					attX = (Mathf.Floor(x));
					tryJump = true;
				}
			}
			x = attX;
			if (x > width - 1)
				x = 0;
		}

        
    }

    public void UpdateY(Vector2[] adjVel, cellType[] adjParticle)
    {
		float attY = y + (velocity.y*Time.deltaTime);
		if (velocity.y < 0)
        {
			for (int i = 3; i < 6; i++)
			{
				if (adjParticle[i] != cellType.empty)
				{
					attY = (Mathf.Floor(y));
					this.velocity.y += (adjVel[i].y - this.velocity.y);
				}
			}
			y = attY;
			if (y < 0)
				y = height - 1;
		}
        else
        {
			for (int i = 0; i < 3; i++)
			{
				if (adjParticle[i] != cellType.empty)
				{
					attY = (Mathf.Floor(y));
					this.velocity.y -= (adjVel[i].y - this.velocity.y);
				}
			}
			y = attY;
			if (y > height - 1)
				y = 0;
		}
    }

	public void ApplyGravity()
	{
		if (velocity.y > terminalVelocity)
		{
			velocity.y += (-9.8f * 10 * Time.deltaTime);
		}
	}

	public void Jump(cellType[] adjParticle)
	{
		if (tryJump && !jumpCD)
		{
			jumpTimer = Time.time + 2.0f;
			initX = x;
			jumpCD = true;
		}
		if (tryJump)
		{
			if (jumpTimer > Time.time)
			{
				if (adjParticle[3] != cellType.empty || adjParticle[4] != cellType.empty || adjParticle[5] != cellType.empty)
				{
					velocity.y = 60.0f;
				}
			}
			else
			{
				if(initX-1 < x && x < initX+1)
					velocity.x = -velocity.x;
				tryJump = false;
				jumpCD = false;
			}
		}
	}
}
