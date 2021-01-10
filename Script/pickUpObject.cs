using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpObject : MonoBehaviour
{
    public GameObject rightHandTarget;
    public GameObject leftHandTarget;
    public bool hasRigidbody = false;
    public Transform trackingSpace; // used to calculate the velocity of the object when dropped (if this object has a rigidbody)
    // Save object's original coordinates and parent
    private GameObject oldParent;
    private Vector3 oldPosition;
    private Quaternion oldRotation;
    private bool pickedUpR; // picked up by right hand
    private bool pickedUpL; // picked up by left hand
    private bool rightHandPositioned;
    private bool leftHandPositioned;
    private bool resetPosition; // Flag if the object is being repositioned (through vector3.lerp() )
    private bool createRigidbody = false;
    private bool rigidbodyMechanic; // True if this object had a rigidbody at start

    void Start()
    {
        oldParent = null;
        pickedUpR = false;
        pickedUpL = false;
        rightHandPositioned = false;
        leftHandPositioned = false;
        resetPosition = false;
        saveOldCoordinates(); // save Object's original position
        GetComponent<TrailRenderer>().enabled = false;
        rigidbodyMechanic = hasRigidbody;
    }


    void Update()
    {
        // Grab with right hand
        if (rightHandPositioned && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) > 0.2)
        {
            this.transform.parent = rightHandTarget.transform;
            transform.position = rightHandTarget.transform.position;
            transform.rotation = rightHandTarget.transform.rotation;
            pickedUpR = true;
            pickedUpL = false;
            rightHandPositioned = false;
            leftHandPositioned = false;
            resetPosition = false;
            GetComponent<TrailRenderer>().enabled = false;

            // when the object is grabbed we don't want it to have a rigidbody
            if (hasRigidbody)
            {
                Destroy(GetComponent<Rigidbody>());
                hasRigidbody = false;
                createRigidbody = true;
            }
        }

        // Grab with left hand
        if (leftHandPositioned && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) > 0.2)
        {
            this.transform.parent = leftHandTarget.transform;
            transform.position = leftHandTarget.transform.position;
            transform.rotation = leftHandTarget.transform.rotation;
            pickedUpR = false;
            pickedUpL = true;
            rightHandPositioned = false;
            leftHandPositioned = false;
            resetPosition = false;
            GetComponent<TrailRenderer>().enabled = false;

            // when the object is grabbed we don't want it to have a rigidbody
            if (hasRigidbody)
            {
                Destroy(GetComponent<Rigidbody>()); // Destroy the rigidbody component when the object is grabbed
                hasRigidbody = false;
                createRigidbody = true;
            }
        }

        // Release the object
        if((OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) <= 0.2 && pickedUpR)
            || (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) <= 0.2 && pickedUpL))
        {
            // put the object as child of its original parent
            if (oldParent)
                this.transform.parent = oldParent.transform;
            else // otherwise just let the hand detach its child (this object)
                this.transform.parent.DetachChildren();


            GetComponent<TrailRenderer>().enabled = true; // enable the trail renderer attached to this object

            if (createRigidbody)
            {
                GetComponent<TrailRenderer>().enabled = false; // disable the trail renderer if this object has rigidbody physics
                this.gameObject.AddComponent<Rigidbody>(); // Add the rigidbody component again when the object is released
                if (pickedUpR)
                {
                    GetComponent<Rigidbody>().velocity = trackingSpace.rotation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
                    GetComponent<Rigidbody>().angularVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
                }
                else
                {
                    GetComponent<Rigidbody>().velocity = trackingSpace.rotation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
                    GetComponent<Rigidbody>().angularVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);
                }
                createRigidbody = false;
                hasRigidbody = true;
            }

            if (pickedUpR) pickedUpR = false;
            if (pickedUpL) pickedUpL = false;
            resetPosition = true; // start resetting its original position and rotation

        }
    }

    void FixedUpdate() { 
        
        if (resetPosition && !rigidbodyMechanic)
        {
            this.transform.position = Vector3.Lerp(transform.position, oldPosition, Time.deltaTime * 5f);
            this.transform.rotation = Quaternion.Lerp(transform.rotation, oldRotation, Time.deltaTime * 5f);
        }

    }


    // When the player's index collide with this object put it in player's right hand
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IndexTrigger" && rightHandTarget.transform.childCount == 0)
        {
            rightHandPositioned = true;
            leftHandPositioned = false;
        }
        else if (other.tag == "IndexTriggerL" && leftHandTarget.transform.childCount == 0)
        {
            leftHandPositioned = true;
            rightHandPositioned = false;
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.tag == "IndexTrigger" && rightHandTarget.transform.childCount == 0)
        {
            rightHandPositioned = false;
        }
        else if (other.tag == "IndexTriggerL" && leftHandTarget.transform.childCount == 0)
        {
            leftHandPositioned = false;
        }
    }


    // Save object original coordinates so if the user presses B or Y this object can come back to its original position
    void saveOldCoordinates()
    {
        // If the object was already picked up from its original position (by any hand)
        // then we must not overwrite those coordinates
        if (!pickedUpR && !pickedUpL) 
        {
            if (transform.parent != null)
                oldParent = this.transform.parent.gameObject;
            oldPosition = this.transform.position;
            oldRotation = this.transform.rotation;
        }
    }

    public bool isPickedUp(){
        return (pickedUpR || pickedUpL);
    }


}
