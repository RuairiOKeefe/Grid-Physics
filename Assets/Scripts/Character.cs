using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
	public float x;
	public float y;
	public Vector2 velocity;
	public float terminalVelocity = -500f;//may want to calculate as a function of mass

	//private float moveTimeX;
	//private float moveTimeY;
	private int width;
	private int height;

	public Character()
	{
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
		}

        
    }

    public void UpdateY(Vector2[] adjVel, cellType[] adjParticle)
    {
		float attY = y + velocity.y;
		if (velocity.y < 0)
        {
			if (attY <= (Mathf.Floor(y) - 1))
			{
				for (int i = 3; i < 6; i++)
				{
					if (adjParticle[i] != cellType.empty)
					{
						attY = (Mathf.Floor(y) - 1);
					}
				}
			}
			y = attY;
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
					}
				}
			}
			y = attY;
		}
    }

}
