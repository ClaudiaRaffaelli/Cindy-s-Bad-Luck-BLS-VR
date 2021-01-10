using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleHelper : MonoBehaviour
{

    public GameObject particleSystem; // The particle system to control
    private bool triggerR = false; // If the right index has collided with this object
    private bool triggerL = false; // If the left index has collided with this object
    private GameObject followedObject = null; // When the user collide to this object with his index finger, save the reference to that index

    void Update()
    {

        if (followedObject) // If there is a refernece to the finger who was trying to grab this object
        {
            if (Vector3.Distance(followedObject.transform.position, transform.position) < 0.1f) // and its distance is below 0.1f
            {
                if ((triggerR && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) > 0.2) // and the hand is grabbing
            || (triggerL && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) > 0.2))
                {
                    particleSystem.SetActive(true); // turn on the particle system
                }
            }
            else
            {
                followedObject = null; // if the finger is far enough delete the reference to that object
            }

        }
        else
        {
            particleSystem.SetActive(false); // disable the particle system if not following any object (i.e. you don't have a reference to any object)
            triggerR = false;
            triggerL = false;
        }
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IndexTrigger")
        {
            triggerR = true;
            followedObject = other.gameObject;
        }
        if ( other.tag == "IndexTriggerL")
        {
            triggerL = true;
            followedObject = other.gameObject;
        }
    }

}
