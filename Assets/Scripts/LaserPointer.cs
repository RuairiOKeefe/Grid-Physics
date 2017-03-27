using UnityEngine;
using System.Collections;
using System.IO;

public class LaserPointer : MonoBehaviour
{
    public GameObject laserPrefab;
    public GameObject pointObject;
    public GameObject invCylinder;

    private SteamVR_TrackedObject tracked;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitpoint;
    private bool troo;
    private int mode = 0;
    private int timer;
    private GameObject Fork;

    private SteamVR_Controller.Device Control
    {
        get
        {
            return SteamVR_Controller.Input((int)tracked.index);
        }
    }
    void Awake()
    {
        tracked = GetComponent<SteamVR_TrackedObject>();
    }
    // Use this for initialization
    private void MoveLaser(RaycastHit hit)
    {
        laserTransform.position = Vector3.Lerp(tracked.transform.position, hitpoint, 0.5f);
        laserTransform.LookAt(hitpoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
    }
    private void ShowLaser()
    {
        laser.SetActive(true);
    }
    void Start ()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        laser.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        timer += 1;
        if (Physics.Raycast(tracked.transform.position, transform.forward, out hit, 1000))
        {
            hitpoint = hit.point;
            Vector2 uvCoord = hit.textureCoord;
            MoveLaser(hit);
        }
        if (Control.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && mode == 0)
        {
            mode = 1;
        }
        else if (Control.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && mode == 1)
        {
            mode = 0;
        }
        if (Control.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && troo == false && timer > 5)
        {
            ShowLaser();
            troo = true;
        }
        else if (Control.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && troo == true && timer > 5)
        {
            laser.SetActive(false);
            troo = false;
        }
        if (mode == 1)
        {
            //Control.TriggerHapticPulse(500);
            if (Physics.Raycast(tracked.transform.position, transform.forward, out hit, 1000))
            {
                invCylinder.GetComponent<GameGrid>().CreateParticle(hit.textureCoord.x, hit.textureCoord.y);
                
            }
        }
    }
}
