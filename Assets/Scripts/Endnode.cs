using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endnode : MonoBehaviour
{
    public GameObject PrefabEndnode;
 
    private SteamVR_TrackedObject tracked;
    private Transform EndnodeTransform;
    private GameObject node;
    private Vector3 hitpoint;
    private SteamVR_Controller.Device Control
    {
        get
        {
            return SteamVR_Controller.Input((int)tracked.index);
        }
    }
    private void MoveNode(RaycastHit hit)
    {
        EndnodeTransform.position = Vector3.Lerp(tracked.transform.position, hitpoint, 0.5f);
        //EndnodeTransform.LookAt(hitpoint);
       // EndnodeTransform.localScale = new Vector3(EndnodeTransform.localScale.x, EndnodeTransform.localScale.y, hit.distance);
    }
    void Awake()
    {
        tracked = GetComponent<SteamVR_TrackedObject>();
    }
    // Use this for initialization
    void Start()
    {
        node = Instantiate(PrefabEndnode);
        EndnodeTransform = node.transform;
        //node.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(tracked.transform.position, transform.forward, out hit, 1000))
        {
            hitpoint = hit.point;
            MoveNode(hit);
        }
    }
}
