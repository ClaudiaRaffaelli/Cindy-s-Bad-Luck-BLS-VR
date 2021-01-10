using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpLegs : MonoBehaviour
{
    public string tag1, tag2; // Objects (i.a. hands) which will interact with the capsule trigger 
    public float constraintX, constraintY, constraintZ_min, constraintZ_max; // Movement constraints

    private bool followRightHand = false; // It decides if the capsules follow the right hand or not
    private bool followLeftHand = false; // It decides if the capsules follow the left hand or not
    private GameObject followedObject; // Tha hand which will be followed by the capsule


    void Update()
    {
        if ((followRightHand && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) != 0) ||
            (followLeftHand && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) != 0)) // follow the correct hand
        {
            transform.position = followedObject.transform.position;
        }
        else
            GetComponent<Rigidbody>().useGravity = true; // Drop down the legs
        // Respect the constraints
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, constraintX, constraintX), Mathf.Clamp(transform.localPosition.y, constraintY, constraintY), Mathf.Clamp(transform.localPosition.z, constraintZ_min, constraintZ_max));
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == tag1 || other.tag == tag2) // If left/right hand triggers the capsule
        {
            // Follow that hand
            if(other.tag == tag1)
                followRightHand = true;
            if(other.tag == tag2)
                followLeftHand = true;
            followedObject = other.gameObject;
            GetComponent<Rigidbody>().useGravity = false;            
        }
    }


    void OnTriggerExit(Collider other)
    {
        // Stop following
        if (other.tag == tag1 || other.tag == tag2)
        {
            followRightHand = false;
            followLeftHand = false;
        }
    }
}
