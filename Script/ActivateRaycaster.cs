using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRaycaster : MonoBehaviour
{

    public GameObject canvas;
    public GameObject laserPointer;
    public GameObject rightHandTarget;

    void FixedUpdate()
    {
        // returns a float of the Hand Trigger’s current state on the Right Oculus Touch controller.
        if ((OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) != 0) && (rightHandTarget.transform.childCount == 0))
        {
            canvas.GetComponent<OVRRaycaster>().enabled = true;
            laserPointer.active = true;
        }
        else
        {
            canvas.GetComponent<OVRRaycaster>().enabled = false;
            laserPointer.active = false;
        }
    }
}
