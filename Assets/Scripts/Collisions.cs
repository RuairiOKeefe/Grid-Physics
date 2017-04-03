using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions
{
    public void check(Particle first , Substance second)
    {
        switch (first.particleType)
        {
            case Substance.Water:
                WaterCollisions(first, second);
                break;
            case Substance.Lava:
                LavaCollisions(first, second);
                break;
            case Substance.Plant:
                PlantCollsions(first, second);
                break;
            case Substance.Wood:
                WoodCollisions(first, second);
                break;
            case Substance.Root:
                Wood_BaseCollsions(first, second);
                break;
            case Substance.Fire:
                FireCollisions(first, second);
                break;
            case Substance.Ice:
                IceCollisions(first, second);
                break;
            case Substance.Steam:
                SteamCollisions(first, second);
                break;
        }
    }      


    public void FireCollisions(Particle fire, Substance other)
    {
        switch (other)
        {
            case Substance.Wood:
                fire.particleType = Substance.Fire;
                break;
            case Substance.Root:
                fire.particleType = Substance.Fire;
                break;
            case Substance.Plant:
                fire.particleType = Substance.Fire;
                break;
            case Substance.Water:
                fire.particleType = Substance.Steam;
                break;
            case Substance.Ice:
                fire.particleType = Substance.Water;
                break;
        }
    }


    public void WaterCollisions(Particle water, Substance other)
    {
        switch (other)
        {
            case Substance.Lava:
                water.particleType = Substance.Stone;
                break;
            case Substance.Fire:
                water.particleType = Substance.Steam;
                break;
            case Substance.Ice:
                water.particleType = Substance.Ice;
                break;
        }
    }
    public void LavaCollisions(Particle lava, Substance other)
    {
        switch (other)
        {
            case Substance.Water:
                lava.particleType = Substance.Stone;
                break;
        }
    }
    public void PlantCollsions(Particle plant , Substance other)
    {
        switch (other)
        {
            case Substance.Water:
                plant.particleType = Substance.Root;
                break;
            case Substance.Wood:
                plant.particleType = Substance.Wood;
                break;
            case Substance.Root:
                plant.particleType = Substance.Wood;
                break;
            case Substance.Fire:
                plant.particleType = Substance.Fire;
                break;
            case Substance.Lava:
                plant.particleType = Substance.Fire;
                break;
        }

    }
    public void WoodCollisions(Particle wood, Substance other)
    {
        switch (other)
        {
            case Substance.Fire:
                wood.particleType = Substance.Fire;
                break;
            case Substance.Lava:
                wood.particleType = Substance.Fire;
                break;
        }

    }
    public void Wood_BaseCollsions(Particle wood_again, Substance other)
    {
        switch (other)
        {
            case Substance.Fire:
                wood_again.particleType = Substance.Smoke;
                break;
            case Substance.Lava:
                wood_again.particleType = Substance.Smoke;
                break;
        }
    }
    public void IceCollisions(Particle Ice, Substance other)
    {
        switch (other)
        {
            case Substance.Fire:
                Ice.particleType = Substance.Water;
                break;
            case Substance.Lava:
                Ice.particleType = Substance.Water;
                break;
            case Substance.Water:
                Ice.particleType = Substance.Water;
                break;
        }

    }
    public void SteamCollisions(Particle steam, Substance other)
    {
        switch (other)
        {
            case Substance.Water:
                steam.particleType = Substance.Water;
                break;
            case Substance.Ice:
                steam.particleType = Substance.Water;
                break;
        }
    }
}
