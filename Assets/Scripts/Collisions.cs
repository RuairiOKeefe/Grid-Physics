using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions
{
    public void FireBehaviour()
    {

    }


    public void WaterCollisions(Particle water, Particle other)
    {
        switch (other.particleType)
        {
            case cellType.lava:
                other.particleType = cellType.stone;
                water.particleType = cellType.stone;
                break;
        }
    }
    public void LavaCollisions(Particle lava, Particle other)
    {
        switch (other.particleType)
        {
            case cellType.water:
                other.particleType = cellType.stone;
                lava.particleType = cellType.stone;
                break;
        }
    }
}
