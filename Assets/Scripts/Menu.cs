using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject Prefabmenu;
    public Camera cam;
    private SteamVR_TrackedObject tracked;
    private Transform menuTrasform;
    private GameObject final_menu;
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
        final_menu = Prefabmenu;
        menuTrasform = final_menu.transform;
        //moveMenu();
        final_menu.SetActive(false);
    }

    //	// Update is called once per frame
    void Update()
    {

        if (Control.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && !final_menu.activeSelf)
        {
            ShowMenu();

        }
        else if (Control.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && final_menu.activeSelf)
        {
            final_menu.SetActive(false);

        }
    }
}
