using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public float timer = 2.0f;
    public AudioSource leverPull;

    public GameObject main;
    public GameObject materials;
    public GameObject options;
    public GameObject controllerMenu;
    public GameObject audioMenu;
    public GameObject videoMenu;

    public GameObject grid;

	// Use this for initialization
	public void Start () {

	}
	
	// Update is called once per frame
	public void Update () {

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


    public void sandboxButton()
    {
        SceneManager.LoadScene(1);
    }

    //Used in main menu only
    public void optionsButton()
    {

        //Close main menu
        if (main.activeSelf)
        {
            main.SetActive(false);
        }

        options.SetActive(true);
    }

    //Controller settings menu
    public void controllerButton()
    {

        //Close options menu
        if (options.activeSelf)
        {
            options.SetActive(false);
        }

        controllerMenu.SetActive(true);
    }

    public void audioButton()
    {
        //Close options menu
        if (options.activeSelf)
        {
            options.SetActive(false);
        }

        audioMenu.SetActive(true);
    }

    public void videoButton()
    {
        //Close options menu
        if (options.activeSelf)
        {
            options.SetActive(false);
        }

        videoMenu.SetActive(true);
    }

    public void exitButton()
    {
        if(SceneManager.GetActiveScene().name == "ActualMainMenu")
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

        if (main.activeSelf)
        {
            main.SetActive(false);
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


    //Used in UI options
    public void optionsButtonUI()
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

    public void switchParticle(string type)
    {
        cellType cType = (cellType)System.Enum.Parse(typeof(cellType), type);
        (grid.GetComponent<GameGrid>() as GameGrid).ChangeType(cType);
    }
}
