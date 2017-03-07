using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum cellType
{
	empty,
	sand,
	stone,
	lava,
	water,
	plant,
    fire,
    obsidian
}

public struct collision
{
	public cellType other;
	public int location;
}

public struct coordPair
{
	public int x;
	public int y;
}

public class GameGrid : MonoBehaviour
{
	public bool createParticles;//Used for debugging without vive

	public int width;
	public int height;

	public cellType particleType;

	public Color[] Colour = new Color[6];

	public GameObject cam;//Active particle debug stuff
	Text txt;//Active particle debug stuff

	Cell[,] cells;

	Dictionary<coordPair, Particle> activeParticles = new Dictionary<coordPair, Particle>();
	Dictionary<coordPair, Particle> inactiveParticles = new Dictionary<coordPair, Particle>();

	float delay;//debug temp
	float offset;//debug temp

	Texture2D gridTexture;

	// Use this for initialization
	void Start ()
	{
		CreateGrid();
	}

	int CheckRange(int coord, int range)
	{
		int newCoord = coord;
		if (coord < 0)
			newCoord = range-1;
		if (coord > range-1)
			newCoord = 0;

		return newCoord;
	}

	void wakeAdj(Cell c, int[] adjCoord)
	{
		if ((cells[c.x, adjCoord[0]].settled) && (cells[c.x, adjCoord[0]].particleType != cellType.empty) && (!cells[c.x, adjCoord[0]].barrier))
		{
			coordPair key = new coordPair() { x = c.x, y = adjCoord[0] };
			Particle p = inactiveParticles[key];
			p.active = true;
			inactiveParticles.Remove(key);
			activeParticles.Add(key, p);
		}
		if ((cells[c.x, adjCoord[1]].settled) && (cells[c.x, adjCoord[1]].particleType != cellType.empty) && (!cells[c.x, adjCoord[1]].barrier))
		{
			coordPair key = new coordPair() { x = c.x, y = adjCoord[1] };
			Particle p = inactiveParticles[key];
			p.active = true;
			inactiveParticles.Remove(key);
			activeParticles.Add(key, p);
		}
		if ((cells[adjCoord[2], c.y].settled) && (cells[adjCoord[2], c.y].particleType != cellType.empty) && (!cells[adjCoord[2], c.y].barrier))
		{
			coordPair key = new coordPair() { x = adjCoord[2], y = c.y };
			Particle p = inactiveParticles[key];
			p.active = true;
			inactiveParticles.Remove(key);
			activeParticles.Add(key, p);
		}
		if ((cells[adjCoord[3], c.y].settled) && (cells[adjCoord[3], c.y].particleType != cellType.empty) && (!cells[adjCoord[3], c.y].barrier))
		{
			coordPair key = new coordPair() { x = adjCoord[3], y = c.y };
			Particle p = inactiveParticles[key];
			p.active = true;
			inactiveParticles.Remove(key);
			activeParticles.Add(key, p);
		}
	}

    void CreateGrid()
    {
		cells = new Cell[width, height]; //Create an multidimensional array of cells to fit the grid

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Cell cell = new Cell();
				cell.Set(x, y);
				cells[x, y] = cell;
			}
		}
		gridTexture = new Texture2D(width, height, TextureFormat.ARGB32, true);
		gridTexture.filterMode = FilterMode.Point;

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (j < 2)
				{
					coordPair key = new coordPair() { x = i, y = j };
					Particle p = new Particle(i, j, cellType.stone, new Vector2(0.0f, 0.0f), width, height);
					p.active = false;
					inactiveParticles.Add(key, p);
					cells[i, j].Settle();
					cells[i, j].SetBarrier();
					cells[i, j].SetParticle(cellType.stone, new Vector2(0,0));
				}
			}
		}

		foreach (Cell c in cells)
		{
			gridTexture.SetPixel(c.x, c.y, Colour[(int)c.particleType]);
			if (c.velocity.x == 0 && c.velocity.y == 0)
			{
				c.Settle();
			}
		}
	}

	public void ChangeType(cellType type)
	{
		particleType = type;
	}

	public bool CreateParticle(float x, float y)//Create; Polymorphise. Whats the difference?
	{
		// x and y must be in range 0-1
		//may add random effect to "spray" particles 
		int gridX = Mathf.RoundToInt(x * width);
		int gridY = Mathf.RoundToInt(y * height);

		if (cells[gridX, gridY].particleType == cellType.empty)
		{
			coordPair key = new coordPair() { x = gridX, y = gridY };
			activeParticles.Add(key ,new Particle(gridX, gridY, particleType, new Vector2(0.0f, -9.8f), width, height));
			return true;
		}
		else
		{
			return false;
		}
	}

	
	// Update is called once per frame
	void FixedUpdate ()
	{
		txt = cam.GetComponent<Text>();
		txt.text = "Active Particles: " + activeParticles.Count;
		if(delay <= Time.time && createParticles)
		{

			if (CreateParticle(offset, 0.8f))
			{
				delay = Time.time + 0.2f;//Modify to change frequency of particles
			}
			else
			{
				offset += 1.0f / width;
			}

			if (offset >= 1.0f)
				offset = 0.0f;
		}

		foreach (KeyValuePair<coordPair, Particle> kp in activeParticles)
		{
			Particle p = kp.Value;
 
            Vector2[] adjVel = new Vector2[4]; //Adjacent velocities. Up Down Left Right
			cellType[] adjParticle = new cellType[4]; //Adjacent particles. Up Down Left Right
			int[] adjCoord = new int[4];

            Collisions col = new Collisions();
            collision xColl;
			collision yColl;
			adjCoord[0] = CheckRange((p.y+1), height);
			adjCoord[1] = CheckRange((p.y-1), height);
			
			adjVel[0] = cells[p.x, adjCoord[0]].velocity;
			adjVel[1] = cells[p.x, adjCoord[1]].velocity;

			adjParticle[0] = cells[p.x, adjCoord[0]].particleType;
			adjParticle[1] = cells[p.x, adjCoord[1]].particleType;

			yColl = p.UpdateY(adjVel, adjParticle);
			cells[p.prevX, p.prevY].SetParticle(cellType.empty, new Vector2(0.0f, 0.0f));
			cells[p.prevX, p.prevY].Settle();
			cells[p.prevX, p.prevY].SetParticle(cellType.empty, new Vector2(0.0f, 0.0f));
			cells[p.x, p.y].SetParticle(p.particleType, p.velocity);

            adjCoord[2] = CheckRange((p.x-1), width);
			adjCoord[3] = CheckRange((p.x+1), width);

			adjVel[2] = cells[adjCoord[2], p.y].velocity;
			adjVel[3] = cells[adjCoord[3], p.y].velocity;

			adjParticle[2] = cells[adjCoord[2], p.y].particleType;
			adjParticle[3] = cells[adjCoord[3], p.y].particleType;
			xColl = p.UpdateX(adjVel, adjParticle);
          
            cells[p.prevX, p.prevY].SetParticle(cellType.empty, new Vector2(0.0f, 0.0f));
			cells[p.prevX, p.prevY].Settle();
			cells[p.x, p.y].SetParticle(p.particleType, p.velocity);

			adjCoord[0] = CheckRange((p.y + 1), height);
			adjCoord[1] = CheckRange((p.y - 1), height);
			adjCoord[2] = CheckRange((p.x - 1), width);
			adjCoord[3] = CheckRange((p.x + 1), width);

			if (xColl.other != cellType.empty || yColl.other != cellType.empty)
            {
				wakeAdj(cells[p.x, p.y], adjCoord);
                cellType collidedType  = cellType.empty , other1 = cellType.empty, other2 = cellType.empty;
                if (yColl.location == 0)
                {
                    collidedType = Search_Collided(p, 0, 1);
                    other1 = Search_Collided(p, -1, 0);
                    other2 = Search_Collided(p, 1, 0);
                }
                if(yColl.location == 1)
                {
                    collidedType = Search_Collided(p, 0, -1);
                    other1 = Search_Collided(p, -1, 0);
                    other2 = Search_Collided(p, 1, 0);
                    
                }
                if (xColl.location == 2)
                {
                    collidedType = Search_Collided(p, -1, 0);
                    other1 = Search_Collided(p, -1, 0);
                    other2 = Search_Collided(p, 1, 0);
                }
                else if (xColl.location == 3)
                {
                    collidedType = Search_Collided(p, 1, 0);
                    other1 = Search_Collided(p, -1, 0);
                    other2 = Search_Collided(p, 1, 0);
                }
                col.check(p, collidedType);
                col.check(p, other1);
                col.check(p, other2);
            }
            gridTexture.SetPixel(p.prevX, p.prevY, Colour[(int)cellType.empty]);
            gridTexture.SetPixel(p.x, p.y, Colour[(int)p.particleType]);

            if (p.active)
			{
				cells[p.x, p.y].UnSettle();
			}
			if (p.active == false)
			{
				cells[p.x, p.y].Settle();
				inactiveParticles.Add(kp.Key, p);
				activeParticles.Remove(kp.Key);
			}
		}
		gridTexture.Apply();

		GetComponent<Renderer>().material.mainTexture = gridTexture;
	}

    public cellType Search_Collided(Particle current , int x , int y)
    {
        cellType newP;
        if ((current.x + x) < 0)
        {
            newP = cells[width - 1, current.y + y].particleType;
        }
        else if (current.x + x >= width)
        {
             newP = cells[0, current.y + y].particleType;
        }
        else if ((current.y + y) >= height)
        {
             newP = cells[current.x + x, 0].particleType;
        }
        else if ((current.y + y) < 0)
        {
             newP = cells[current.x + x, (height - 1)].particleType;
        }
        else
        {
            newP = cells[current.x + x, current.y + y].particleType;
        }
        return newP;
    }
}
