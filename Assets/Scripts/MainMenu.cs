using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public float timer = 2.0f;
    public AudioSource leverPull;
    public GameObject materials;
    public GameObject options;

    public GameObject grid;


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
            //This is just for the editor
            UnityEditor.EditorApplication.isPlaying = false;
            //This will work in the actual build
            Application.Quit();
        }
        //Added this just so we can reuse ui in game too
        else if(SceneManager.GetActiveScene().name == "Test_Level")
        {
            SceneManager.LoadScene(0);
        }
    }

    //Function for materials button in UI
    public void materialsButton()
    {
        //Close options menu if its active 
        if (options.activeSelf)
        {
            options.SetActive(false);
        }

        if (!materials.activeSelf)
        {
            materials.SetActive(true);
        }
        else
        {
            materials.SetActive(false);
        }
    }

    public void optionsButton()
    {
        //Close materials menu if its active 
        if (materials.activeSelf)
        {
            materials.SetActive(false);
        }

        if (!options.activeSelf)
        {
            options.SetActive(true);
        }
        else
        {
            options.SetActive(false);
        }
    }

    public void changeMaterial(int particle){
        switch (particle)
        {
            case 0:
                break;

        }
    }
}
