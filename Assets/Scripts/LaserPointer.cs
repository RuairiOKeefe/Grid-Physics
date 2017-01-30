using UnityEngine;
using System.Collections;
using System.IO;

public class LaserPointer : MonoBehaviour
{
    private SteamVR_TrackedObject tracked;
    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitpoint;
    private bool troo;
    private Vector3 prevlocation;
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
	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast(tracked.transform.position, transform.forward, out hit, 1000))
        {
            hitpoint = hit.point;
            MoveLaser(hit);
        }

        if (Control.GetPress(SteamVR_Controller.ButtonMask.Trigger) && !troo && (prevlocation != hitpoint)) 
        { 
            if (Physics.Raycast(tracked.transform.position , transform.forward , out hit , 1000))
            { 
                troo = true;
                ShowLaser();
               // MoveLaser(hit);
            }

        }
        else if (!Control.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            laser.SetActive(false);
            troo = false;
        }
        prevlocation = hitpoint; 
    }
}
