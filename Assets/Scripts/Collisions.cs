using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions
{
	public bool check(Particle first, Particle second)
	{
		switch (first.particleType)
		{
			case cellType.water:
				return WaterCollisions(first, second);
			case cellType.lava:
				return LavaCollisions(first, second);
			case cellType.plant:
				return PlantCollsions(first, second);
			case cellType.wood:
				return WoodCollisions(first, second);
			case cellType.root:
				return Wood_BaseCollsions(first, second);
			case cellType.fire:
				return FireCollisions(first, second);
			case cellType.ice:
				return IceCollisions(first, second);
			case cellType.steam:
				return SteamCollisions(first, second);
			case cellType.smoke:
				return SmokeCollisions(first, second);
		}
		return false;
	}


	public bool FireCollisions(Particle fire, Particle other)
	{
		switch (other.particleType)
		{
			case cellType.wood:
				other.particleType = cellType.fire;
				other.particleState = State.gas;
				return true;
			case cellType.root:
				fire.particleType = cellType.fire;
				fire.particleState = State.gas;
				return true;
			case cellType.plant:
				other.particleType = cellType.fire;
				other.particleState = State.gas;
				return true;
			case cellType.water:
				other.particleType = cellType.steam;
				other.particleState = State.gas;
				return true;
			case cellType.ice:
				other.particleType = cellType.water;
				other.particleState = State.gas;
				return true;
		}
		return false;
	}


	public bool WaterCollisions(Particle water, Particle other)
	{
		switch (other.particleType)
		{
			case cellType.lava:
				water.particleType = cellType.steam;
				water.particleState = State.gas;
				other.particleType = cellType.stone;
				other.particleState = State.solid;
				return true;
			case cellType.fire:
				water.particleType = cellType.steam;
				water.particleState = State.gas;
				return true;
			case cellType.ice:
				water.particleType = cellType.ice;
				water.particleState = State.solid;
				return true;
		}
		return false;
	}
	public bool LavaCollisions(Particle lava, Particle other)
	{
		switch (other.particleType)
		{
			case cellType.water:
				lava.particleType = cellType.stone;
				lava.particleState = State.solid;
				other.particleType = cellType.steam;
				other.particleState = State.gas;
				return true;
			case cellType.wood:
				other.particleType = cellType.fire;
				other.particleState = State.gas;
				return true;
			case cellType.root:
				other.particleType = cellType.fire;
				other.particleState = State.gas;
				return true;
			case cellType.plant:
				other.particleType = cellType.fire;
				other.particleState = State.gas;
				return true;
			case cellType.ice:
				other.particleType = cellType.water;
				other.particleState = State.liquid;
				return true;
		}
		return false;
	}
	public bool PlantCollsions(Particle plant, Particle other)
	{
		switch (other.particleType)
		{
			case cellType.water:
				plant.particleType = cellType.root;
				plant.particleState = State.solid;
				return true;
			case cellType.wood:
				plant.particleType = cellType.wood;
				plant.particleState = State.solid;
				return true;
			case cellType.root:
				plant.particleType = cellType.wood;
				plant.particleState = State.solid;
				return true;
			case cellType.fire:
				plant.particleType = cellType.fire;
				plant.particleState = State.gas;
				return true;
			case cellType.lava:
				plant.particleType = cellType.fire;
				plant.particleState = State.gas;
				return true;
		}
		return false;
	}
	public bool WoodCollisions(Particle wood, Particle other)
	{
		switch (other.particleType)
		{
			case cellType.fire:
				int rand = Random.Range(0, 2);
				if (rand == 0)
				{
					wood.particleType = cellType.fire;
					wood.particleState = State.gas;
				}
				else
				{
					wood.particleType = cellType.smoke;
					wood.particleState = State.gas;
				}
				return true;
			case cellType.lava:
				int rander = Random.Range(0, 2);
				if (rander == 0)
				{
					wood.particleType = cellType.fire;
					wood.particleState = State.gas;
				}
				else
				{
					wood.particleType = cellType.smoke;
					wood.particleState = State.gas;
				}
				return true;
			case cellType.smoke:
				int randerer = Random.Range(0, 2);
				if (randerer == 0)
				{
					wood.particleType = cellType.fire;
					wood.particleState = State.gas;
				}
				else
				{
					wood.particleType = cellType.smoke;
					wood.particleState = State.gas;
				}
				return true;
		}
		return false;
	}
	public bool Wood_BaseCollsions(Particle wood_again, Particle other)
	{
		switch (other.particleType)
		{
			case cellType.fire:
				wood_again.particleType = cellType.smoke;
				wood_again.particleState = State.gas;
				return true;
			case cellType.lava:
				wood_again.particleType = cellType.smoke;
				wood_again.particleState = State.gas;
				return true;
		}
		return false;
	}
	public bool IceCollisions(Particle Ice, Particle other)
	{
		switch (other.particleType)
		{
			case cellType.fire:
				Ice.particleType = cellType.water;
				Ice.particleState = State.liquid;
				return true;
			case cellType.lava:
				Ice.particleType = cellType.water;
				Ice.particleState = State.liquid;
				return true;
			case cellType.water:
				Ice.particleType = cellType.water;
				Ice.particleState = State.liquid;
				return true;
		}
		return false;
	}
	public bool SteamCollisions(Particle steam, Particle other)
	{
		switch (other.particleType)
		{
			case cellType.water:
				steam.particleType = cellType.water;
				steam.particleState = State.liquid;
				return true;
			case cellType.ice:
				steam.particleType = cellType.water;
				steam.particleState = State.liquid;
				return true;
		}
		return false;
	}
	public bool SmokeCollisions(Particle smoke, Particle other)
	{
		switch (other.particleType)
		{
			case cellType.wood:
				other.particleType = cellType.fire;
				other.particleState = State.gas;
				return true;
			case cellType.root:
				other.particleType = cellType.fire;
				other.particleState = State.gas;
				return true;
			case cellType.plant:
				other.particleType = cellType.fire;
				other.particleState = State.gas;
				return true;
		}
		return false;
	}
}
