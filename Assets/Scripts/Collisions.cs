using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions
{
    public void check(Particle first , Particle second)
    {
        switch (first.particleType)
        {
            case cellType.water:
                WaterCollisions(first, second);
                break;
            case cellType.lava:
                LavaCollisions(first, second);
                break;
            case cellType.plant:
                PlantCollsions(first, second);
                break;
            case cellType.wood:
                WoodCollisions(first, second);
                break;
            case cellType.root:
                Wood_BaseCollsions(first, second);
                break;
            case cellType.fire:
                FireCollisions(first, second);
                break;
            case cellType.ice:
                IceCollisions(first, second);
                break;
            case cellType.steam:
                SteamCollisions(first, second);
                break;
            case cellType.smoke:
                SmokeCollisions(first, second);
                break;
        }
    }      


    public void FireCollisions(Particle fire, Particle other)
    {
        switch (other.particleType)
        {
            case cellType.wood:
                other.particleType = cellType.fire;
                break;
            case cellType.root:
                fire.particleType = cellType.fire;
                break;
            case cellType.plant:
                other.particleType = cellType.fire;
                break;
            case cellType.water:
                other.particleType = cellType.steam;
                break;
            case cellType.ice:
                other.particleType = cellType.water;
                break;
        }
    }


    public void WaterCollisions(Particle water, Particle other)
    {
        switch (other.particleType)
        {
            case cellType.lava:
                water.particleType = cellType.steam;
                other.particleType = cellType.stone;
                break;
            case cellType.fire:
                water.particleType = cellType.steam;
                break;
            case cellType.ice:
                water.particleType = cellType.ice;
                break;

        }
    }
    public void LavaCollisions(Particle lava, Particle other)
    {
        switch (other.particleType)
        {
            case cellType.water:
                lava.particleType = cellType.stone;
                other.particleType = cellType.steam;
                break;
            case cellType.wood:
                other.particleType = cellType.fire;
                break;
            case cellType.root:
                other.particleType = cellType.fire;
                break;
            case cellType.plant:
                other.particleType = cellType.fire;
                break;
            case cellType.ice:
                other.particleType = cellType.water;
                break;
        }
    }
    public void PlantCollsions(Particle plant , Particle other)
    {
        switch (other.particleType)
        {
            case cellType.water:
                plant.particleType = cellType.root;
                break;
            case cellType.wood:
                plant.particleType = cellType.wood;
                break;
            case cellType.root:
                plant.particleType = cellType.wood;
                break;
            case cellType.fire:
                plant.particleType = cellType.fire;
                break;
            case cellType.lava:
                plant.particleType = cellType.fire;
                break;
        }

    }
    public void WoodCollisions(Particle wood, Particle other)
    {
        switch (other.particleType)
        {
            case cellType.fire:
                int rand = Random.Range(0, 2);
                if(rand == 0)
                {
                    wood.particleType = cellType.fire;
                }
                else
                {
                    wood.particleType = cellType.smoke;
                }
                break;
            case cellType.lava:
                int rander = Random.Range(0, 2);
                if (rander == 0)
                {
                    wood.particleType = cellType.fire;
                }
                else
                {
                    wood.particleType = cellType.smoke;
                }
                break;
            case cellType.smoke:
                int randerer = Random.Range(0, 2);
                if (randerer == 0)
                {
                    wood.particleType = cellType.fire;
                }
                else
                {
                    wood.particleType = cellType.smoke;
                }
                break;
        }

    }
    public void Wood_BaseCollsions(Particle wood_again, Particle other)
    {
        switch (other.particleType)
        {
            case cellType.fire:
                wood_again.particleType = cellType.smoke;
                break;
            case cellType.lava:
                wood_again.particleType = cellType.smoke;
                break;
        }
    }
    public void IceCollisions(Particle Ice, Particle other)
    {
        switch (other.particleType)
        {
            case cellType.fire:
                Ice.particleType = cellType.water;
                break;
            case cellType.lava:
                Ice.particleType = cellType.water;
                break;
            case cellType.water:
                Ice.particleType = cellType.water;
                break;
        }

    }
    public void SteamCollisions(Particle steam, Particle other)
    {
        switch (other.particleType)
        {
            case cellType.water:
                steam.particleType = cellType.water;
                break;
            case cellType.ice:
                steam.particleType = cellType.water;
                break;
        }
    }
    public void SmokeCollisions(Particle smoke, Particle other)
    {
        switch (other.particleType)
        {
            case cellType.wood:
                other.particleType = cellType.fire;
                break;
            case cellType.root:
                other.particleType = cellType.fire;
                break;
            case cellType.plant:
                other.particleType = cellType.fire;
                break;
        }
    }
}
