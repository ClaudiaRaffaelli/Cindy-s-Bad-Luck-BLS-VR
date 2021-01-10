using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstNode : MonoBehaviour
{
    public GameObject handPathManager;

    private bool handInPositionR;
    private bool handInPositionL;
    private bool active;


    void Start()
    {
        handInPositionR = false;
        handInPositionL = false;
        active = true;
    }

    void Update()
    {
        // if the hand is in position and it's grabbing
        if ((handInPositionR && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) >= 0.7)
                            ||(handInPositionL && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) >= 0.7))
        {
            //if (active)
            handPathManager.GetComponent<path>().notify(gameObject.name);
            handPathManager.GetComponent<path>().setActive(true); // activate the path Update() method
            //GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false); // Disable particle system (flame)
            //active = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IndexTrigger")
        {
            handInPositionR = true;
        }
        if ( other.tag == "IndexTriggerL")
        {
            handInPositionL = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "IndexTrigger")
        {
            handInPositionR = false;
        }
        if (other.tag == "IndexTriggerL")
        {
            handInPositionL = false;
        }
    }
}
