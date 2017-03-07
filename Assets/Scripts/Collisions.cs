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
        }
    }      


    public void FireBehaviour()
    {

    }


    public void WaterCollisions(Particle water, cellType other)
    {
        switch (other)
        {
            case cellType.lava:
                other = cellType.stone;
                water.particleType = cellType.stone;
                break;
        }
    }
    public void LavaCollisions(Particle lava, cellType other)
    {
        switch (other)
        {
            case cellType.water:
                other = cellType.stone;
                lava.particleType = cellType.stone;
                break;
        }
    }
    public void PlantCollsions(Particle plant , cellType other)
    {
        switch (other)
        {
            case cellType.water:
                plant.particleType = cellType.wood_base;
                break;
            case cellType.wood:
                plant.particleType = cellType.wood;
                break;
            case cellType.wood_base:
                plant.particleType = cellType.wood;
                break;

        }

    }
}
