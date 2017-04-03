using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions
{
    public void check(Particle first , cellType second)
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
        }
    }      


    public void FireCollisions(Particle fire, cellType other)
    {
        switch (other)
        {
            case cellType.wood:
                fire.particleType = cellType.fire;
                break;
            case cellType.root:
                fire.particleType = cellType.fire;
                break;
            case cellType.plant:
                fire.particleType = cellType.fire;
                break;
            case cellType.water:
                fire.particleType = cellType.steam;
                break;
            case cellType.ice:
                fire.particleType = cellType.water;
                break;
        }
    }


    public void WaterCollisions(Particle water, cellType other)
    {
        switch (other)
        {
            case cellType.lava:
                water.particleType = cellType.stone;
                break;
            case cellType.fire:
                water.particleType = cellType.steam;
                break;
            case cellType.ice:
                water.particleType = cellType.ice;
                break;
        }
    }
    public void LavaCollisions(Particle lava, cellType other)
    {
        switch (other)
        {
            case cellType.water:
                lava.particleType = cellType.stone;
                break;
        }
    }
    public void PlantCollsions(Particle plant , cellType other)
    {
        switch (other)
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
    public void WoodCollisions(Particle wood, cellType other)
    {
        switch (other)
        {
            case cellType.fire:
                wood.particleType = cellType.fire;
                break;
            case cellType.lava:
                wood.particleType = cellType.fire;
                break;
        }

    }
    public void Wood_BaseCollsions(Particle wood_again, cellType other)
    {
        switch (other)
        {
            case cellType.fire:
                wood_again.particleType = cellType.smoke;
                break;
            case cellType.lava:
                wood_again.particleType = cellType.smoke;
                break;
        }
    }
    public void IceCollisions(Particle Ice, cellType other)
    {
        switch (other)
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
    public void SteamCollisions(Particle steam, cellType other)
    {
        switch (other)
        {
            case cellType.water:
                steam.particleType = cellType.water;
                break;
            case cellType.ice:
                steam.particleType = cellType.water;
                break;
        }
    }
}
