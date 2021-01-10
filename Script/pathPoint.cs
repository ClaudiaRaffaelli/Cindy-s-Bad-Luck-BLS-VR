using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathPoint : MonoBehaviour
{
    public GameObject handPathManager;

    private string pointID;

    // Start is called before the first frame update
    void Start()
    {
        pointID = gameObject.name;
    }


    void OnTriggerEnter(Collider other)
    {
        // if the hand is in position and it's grabbing
        if ((other.tag == "IndexTrigger" && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) >= 0.7)
                            || (other.tag == "IndexTriggerL" && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) >= 0.7))
        {
            handPathManager.GetComponent<path>().notify(pointID);
            //GetComponent<MeshRenderer>().enabled = false;
        }
    }

    //TODO: check if this trigger is usefull
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "IndexTrigger" || other.tag == "IndexTriggerL")
        {
            //GetComponent<MeshRenderer>().enabled = true;
        }
    }

}
