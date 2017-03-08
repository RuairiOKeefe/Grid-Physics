using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject Prefabmenu;
    public Camera cam;
    //This is a test object which is in the scene, its a menu object that works with raycasting
    public GameObject Test;
    private SteamVR_TrackedObject tracked;
    private Transform menuTrasform;
    private GameObject final_menu;
    private bool mode;
    private int modes;
    private Vector3 hitpoint;
    public cellType cells;

    private SteamVR_Controller.Device Control
    {
        get
        {
            return SteamVR_Controller.Input((int)tracked.index);
        }
    }
    private void moveMenu()
    {
        Vector3 temp_pos = tracked.transform.position;
        temp_pos.y += 0.3f;
        //temp_pos.x += 0.3f;
        menuTrasform.position = temp_pos;
        //menuTrasform.LookAt(menuTrasform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
    private void ShowMenu()
    {
        final_menu.SetActive(true);
    }
    void Awake()
    {
        tracked = GetComponent<SteamVR_TrackedObject>();
    }
    //    // Use this for initialization
    void Start()
    {
        final_menu = Instantiate(Prefabmenu);
        menuTrasform = final_menu.transform;
        moveMenu();
        final_menu.SetActive(false);
    }

    //	// Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Material touched = Resources.Load("LAser", typeof(Material)) as Material;
        Material notTouched = Resources.Load("Point Shader", typeof(Material)) as Material;
        modes += 1;
        int layerMask = LayerMask.GetMask("UI");

        //Test object used for raycasting, it will work with the menus, we just need to be able to access them
        if (Control.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (Physics.Raycast(tracked.transform.position, transform.forward, out hit, 1000, layerMask))
            {
                hitpoint = hit.point;
                Test.GetComponent<Renderer>().material = touched;

            }
        }
        else if (Control.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Test.GetComponent<Renderer>().material = notTouched;
        }
        if (Control.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && mode == false && modes > 5)//probably doesnt need timer any more
        {
            ShowMenu();
            mode = true;
            modes = 0;
        }
        else if (Control.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && mode == true && modes > 5)
        {
            final_menu.SetActive(false);
            mode = false;
            modes = 0;
        }
    }
}
