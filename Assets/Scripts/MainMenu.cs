using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public float timer = 2.0f;
    public AudioSource leverPull;

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
                timer = 2.0f;
            }
        }
        else
        {
            timer = 2.0f;
        }
    }

    public void exitButton()
    {
        if(SceneManager.GetActiveScene().name == "Main Menu")
        {
            print("You quit the application");
            //This is just for the editor
            UnityEditor.EditorApplication.isPlaying = false;
            //This will work in the actual build
            Application.Quit();
        }
        //Added this just so we can reuse ui in game too
        else if(SceneManager.GetActiveScene().name == "Test_Level")
        {
            print("You quit to main menu");
            SceneManager.LoadScene(0);
        }
        //Little debugging thing
        else
        {
            print("Yo, its broken");
        }
    }
}
