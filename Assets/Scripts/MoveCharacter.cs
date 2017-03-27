using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour {

	public void MoveChar(float x, float y)
	{
		transform.position = new Vector3(this.transform.position.x, (y * 3.14f), transform.position.z);
		transform.rotation = Quaternion.AngleAxis(x*360 -180, Vector3.up);
	}
}
