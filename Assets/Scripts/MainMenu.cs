using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public float timer = 3.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.localPosition.z < -0.700 )
        {
            timer -= Time.deltaTime;

            if(timer <= 0.0f)
            {
                SceneManager.LoadScene(1);
            }
        }
        else
        {
            timer = 3.0f;
        }
    }
}
