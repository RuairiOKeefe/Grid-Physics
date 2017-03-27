using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
	public float x;
	public float y;
	public Vector2 velocity;
	public float terminalVelocity = -500f;//may want to calculate as a function of mass

	private int width;
	private int height;

	public Character(int width, int height)
	{
		this.width = width;
		this.height = height;
		velocity.x = 5.0f;
	}

	public void UpdateX(Vector2[] adjVel, cellType[] adjParticle)
    {
		float attX = x + (velocity.x * Time.deltaTime);
        if (velocity.x < 0)
        {
			if (attX <= (Mathf.Floor(x) - 1))
			{
				for (int i = 6; i < 9; i++)
				{
					if (adjParticle[i] != cellType.empty)
					{
						attX = (Mathf.Floor(x) - 1);
						velocity.x = 5.0f;
					}
				}
			}
			x = attX;
			x = x % width;
		}
        else
        {
			if (attX >= (Mathf.Floor(x) + 1))
			{
				for (int i = 9; i < 12; i++)
				{
					if (adjParticle[i] != cellType.empty)
					{
						attX = (Mathf.Floor(x) + 1);
						velocity.x = -5.0f;
					}
				}
			}
			x = attX;
			x = x % width;
		}

        
    }

    public void UpdateY(Vector2[] adjVel, cellType[] adjParticle)
    {
		float attY = y + (velocity.y*Time.deltaTime);
		if (velocity.y < 0)
        {
			if (attY <= (Mathf.Floor(y) - 1))
			{
				for (int i = 3; i < 6; i++)
				{
					if (adjParticle[i] != cellType.empty)
					{
						attY = (Mathf.Floor(y) - 1);
						this.velocity.y += (adjVel[i].y - this.velocity.y);
					}
				}
			}
			y = attY;
			y = y % height;
		}
        else
        {
			if (attY >= (Mathf.Floor(y) + 1))
			{
				for (int i = 0; i < 3; i++)
				{
					if (adjParticle[i] != cellType.empty)
					{
						attY = (Mathf.Floor(y) + 1);
						this.velocity.y -= (adjVel[i].y - this.velocity.y);
					}
				}
			}
			y = attY;
			y = y % height;
		}
    }

	public void ApplyGravity()
	{
		if (velocity.y > terminalVelocity)
		{
			velocity.y += (-9.8f * 10 * Time.deltaTime);
		}
	}
}
