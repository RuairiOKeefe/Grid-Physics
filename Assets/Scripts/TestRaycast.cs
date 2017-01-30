using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRaycast : MonoBehaviour {

    public GameObject gridObject;
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            //if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, 1000, layerMask = "grid",)
        }
		
	}
}
