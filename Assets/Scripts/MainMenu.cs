using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class MainMenu : MonoBehaviour {

    public float timer = 2.0f;
    public AudioSource leverPull;

    public GameObject main;
    public GameObject materials;
    public GameObject options;
    public GameObject controllerMenu;
    public GameObject audioMenu;
    public GameObject videoMenu;

    public GameObject controllerPrefab;
    public GameObject grid;
    Slider audioSlider;
    GameObject Fork;

    // Use this for initialization
    public void Start () {

        Instantiate(controllerPrefab);

        Fork = GameObject.Find("spoke");
    }
	
	// Update is called once per frame
	public void Update ()
    {

        if (audioMenu.activeSelf)
        {
            SliderUpdate();
        }

        //Testing rotation --REMOVE FOR FINAL VERSION
        if (Input.GetKey(KeyCode.F))
        {
            Fork.transform.Rotate(Vector3.forward * 1000 * Time.deltaTime);
        }

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

    public void SliderUpdate()
    {

        GameObject temp = GameObject.Find("AudioSlider");
        GameObject mapping = GameObject.Find("LinearMapping");

        audioSlider = temp.GetComponent<Slider>();

        audioSlider.value = mapping.GetComponent<LinearMapping>().value;

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
            //UnityEditor.EditorApplication.isPlaying = false;
            //This will work in the actual build
            Application.Quit();
        }
        //Added this just so we can reuse ui in game too
        else if(SceneManager.GetActiveScene().name == "Test_Level")
        {
            SceneManager.LoadScene(0);
        }
    }

    public void homeButton(string menu)
    {
        switch (menu)
        {
            case "options":
                options.SetActive(false);
                break;
            case "controllers":
                controllerMenu.SetActive(false);
                break;
            case "audio":
                audioMenu.SetActive(false);
                break;
            case "video":
                videoMenu.SetActive(false);
                break;
        }

        main.SetActive(true);
    }

    public void backButton(string menu)
    {
        switch (menu)
        {
            case "options":
                options.SetActive(false);
                main.SetActive(true);
                break;
            case "controllers":
                controllerMenu.SetActive(false);
                options.SetActive(true);
                break;
            case "audio":
                audioMenu.SetActive(false);
                options.SetActive(true);
                break;
            case "video":
                videoMenu.SetActive(false);
                options.SetActive(true);
                break;
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
