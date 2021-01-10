using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpBody : MonoBehaviour
{
    public GameObject cindy;
    public GameObject safeZone;
    public GameObject OVRPlayerController; 

    public GameObject rightHand;
    public GameObject leftHand;

    public LayerMask raycastIgnoreLayer; // Ignore player layer when firing raycast
    public GameObject raycasterHand;

    static bool rightHandTrigger = false; // Check if user's right hand is in the right position
    static bool leftHandTrigger = false; // Check if user's left hand is in the right position
    static Vector3 resetPosition; // When the user release the grab button Cindy must return on the floor  
    static Quaternion resetRotation; // and with the correct rotation (depends on OVRPlayerController's rotation)
    static bool cindyIsSafe, cindyReleased; // Check if Cindy is the safezone and if she's released down on the floor

    private GameObject followedRightHand; // When grabbed, cindy is going to follow this object (right hand's index)
    private GameObject oldParent; // When released, cindy come back to her old parent object, and reset position/rotation according to this old parent object
    private bool rightHandMeshToggle; // Just a boolean to fix a bug which enable/disable rightHandTrigger'mesh randomly. With this boolean it works fine


    void Start()
    {
        // This script is shared beetwen 3 objects to reduce the number of different scripts
        // PickUpTriggerR is the "manager" and if possible it will be the only one executing this script
        if (gameObject.name == "PickUpTriggerR")
        {
            resetPosition = gameObject.transform.position;
            resetRotation = gameObject.transform.rotation;
            cindyIsSafe = false;
            cindyReleased = false;
            safeZone.SetActive(false); // The safezone won't be visible until Cindy's grabbed
            oldParent = transform.parent.gameObject;
            rightHandMeshToggle = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // This IF is required so that these lines below won't be executed 3 times every frame (by the 3 different objects)
        if (gameObject.name == "PickUpTriggerR")
        {
            // If the hands are in the right position and they're grabbing (hand and index are triggered)
            if (leftHandTrigger && rightHandTrigger && 
                OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) != 0 &&
                    OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) != 0 &&
                    OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.9 &&
                    OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) >= 0.9)
            {
                this.gameObject.transform.parent = followedRightHand.transform; // pickUpTriggerR's parent become user's right index
                safeZone.SetActive(true); // Activate the safeZone visibility
                transform.GetChild(2).gameObject.SetActive(true); // Activate particle system to help user find the safe zone
                OVRPlayerController.GetComponent<OVRPlayerController>().SetMoveScaleMultiplier(0.3f); //Slow player's speed
                cindy.GetComponent<Animator>().SetBool("pickedUp", true); // Change cindy's animation to picked up
                GameObject ous = Instantiate(new GameObject(), raycasterHand.transform); // ousiliar child object to make the raycaster laser disappear
                ous.name = "ous";
                rightHand.GetComponent<MeshRenderer>().enabled = false; // Visual bugs require these two lines in order to be sure that the hands'mesh stay false
                leftHand.GetComponent<MeshRenderer>().enabled = false;
                if (!cindyIsSafe) // If cindy has not triggered the safeZone
                {
                    // Calculate and save the position and rotation of cindy in the case that in the next frame the user drops her down (not in the safeZone)

                    // Calculate the distance from pickUpTriggerR (cindy) to the floor in order to calculate her Y coordinate
                    RaycastHit hit = new RaycastHit();
                    float distanceToGround = 0f;
                    if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1000f, ~raycastIgnoreLayer, QueryTriggerInteraction.Ignore))
                    {
                        distanceToGround = hit.distance;
                    }
                    // adjust (+ 0.3) her Y coordinate so that she does't go too much down
                    resetPosition = new Vector3(transform.position.x, transform.position.y - distanceToGround + 0.3f, transform.position.z);

                    // The rotation to apply to Cindy (remember, only if she drops down the next frame) depends on the OVRPlayerController's rotation
                    resetRotation = Quaternion.AngleAxis(OVRPlayerController.transform.eulerAngles.y - transform.eulerAngles.y, OVRPlayerController.transform.up) * transform.rotation;
                    resetRotation = Quaternion.AngleAxis(OVRPlayerController.transform.eulerAngles.z - transform.eulerAngles.z + 200, OVRPlayerController.transform.forward) * resetRotation;
                    resetRotation = Quaternion.AngleAxis(OVRPlayerController.transform.eulerAngles.x - transform.eulerAngles.x - 10, OVRPlayerController.transform.right) * resetRotation;

                }
            }

            // If the user drops down Cindy
            if (cindy.GetComponent<Animator>().GetBool("pickedUp") && (!leftHandTrigger || !rightHandTrigger ||
                OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) <= 0.2 ||
                    OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) <= 0.2))
            {
                // Change back pickUpTriggerR's parent so that the reset position and rotation can be applied according to its old parent,
                // Without this line pickUpTriggerR (Cindy) would remain attached to the user's index finger
                this.gameObject.transform.parent = oldParent.transform;

                // Drop down Cindy using the last position and rotation calculated above or in setResetTransform() 
                // which is called when Cindy triggers the safeZone
                transform.position = resetPosition;
                transform.rotation = resetRotation;


                cindyReleased = true;
                safeZone.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false); // Disable particle system which was helping the user find the safeZone
                OVRPlayerController.GetComponent<OVRPlayerController>().SetMoveScaleMultiplier(1f); // Reset player's speed
                cindy.GetComponent<Animator>().SetBool("pickedUp", false);
                rightHand.GetComponent<MeshRenderer>().enabled = true;
                leftHand.GetComponent<MeshRenderer>().enabled = true;

                foreach (Transform child in raycasterHand.transform)
                {
                    if(child.name.Equals("ous"))
                        Destroy(child.gameObject);
                }

                // If the user drops her down in the safeZone
                if (cindyIsSafe)
                {
                    cindy.GetComponent<Animator>().SetBool("pickedUp", false);
                    // Change Cindy's parent because PickUpTriggerR is going to be destroyed (as the other objects which are using this script
                    // because no more necessary). 
                    transform.GetChild(0).transform.parent = transform.parent;
                    Destroy(this.gameObject);
                }
            }

            // Manage the hand's mesh renderer 
            if (leftHandTrigger) leftHand.GetComponent<MeshRenderer>().enabled = false;
            if (rightHandTrigger) rightHand.GetComponent<MeshRenderer>().enabled = rightHandMeshToggle;
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (gameObject.name == "safeZone" && other.gameObject.name == "PickUpTriggerR")
        {
            // When cindy reaches the safe zone change her resetPosition and resetRotation
            setResetTransform(this.gameObject.transform.position, this.gameObject.transform.rotation);
            cindyIsSafe = true;
            leftHandTrigger = false; // Force the user to drop down Cindy
            rightHandTrigger = false;
            rightHandMeshToggle = !rightHandMeshToggle; // Just to fix that visual bug explained early
            
        }

        // When the right hand is in the correct position
        if (gameObject.name == "PickUpTriggerR" && other.tag == "IndexTrigger")
        {
            rightHandTrigger = true;
            rightHandMeshToggle = !rightHandMeshToggle;
            followedRightHand = other.gameObject;
        }

        // When the left hand is in the correct position
        if (gameObject.name == "PickUpTriggerL" && other.tag == "IndexTriggerL")
        {
            leftHandTrigger = true;
        }

        // When the user triggers handAnimation manager activate the animation
        if (gameObject.name == "handsAnimation" && other.tag == "IndexTrigger")
        {
            GetComponent<Animator>().SetTrigger("Active");
        }

    }


    void OnTriggerExit(Collider other)
    {
        // When the hands exit from their correct position
        if (gameObject.name == "PickUpTriggerR" && other.tag == "IndexTrigger")
        {
            rightHandTrigger = false;
            rightHandMeshToggle = !rightHandMeshToggle;
        }
        if (gameObject.name == "PickUpTriggerL" && other.tag == "IndexTriggerL")
        {
            leftHandTrigger = false;
        }

    }


    public void setResetTransform(Vector3 newPosition, Quaternion newRotation)
    {
        resetPosition = newPosition;
        resetRotation = newRotation;
    }

    public bool getIsSafe(){
        // in order to know if the player has placed Cindy in the right position
        return cindyIsSafe;
    }

    public void setIsSafe(bool safe){
        cindyIsSafe = safe;
    }
}
