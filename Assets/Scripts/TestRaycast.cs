using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRaycast : MonoBehaviour {

    public GameObject pointObject;
	public GameObject invCylinder;
	public GameObject gridPlane;

	// Update is called once per frame
	void FixedUpdate ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
			if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 1000))
			{
				Vector2 uvCoord = hit.textureCoord;
				Vector3 spawnLocation = new Vector3((hit.textureCoord.x*5) - 2.5f, (hit.textureCoord.y*5) - 2.5f, gridPlane.transform.position.z);
				Instantiate(pointObject, spawnLocation, this.transform.rotation);
			}
        }
		
	}
}
