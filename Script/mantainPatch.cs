using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mantainPatch : MonoBehaviour
{
    public GameObject patch;
    public Transform patchTransform;
    public GameObject rightIndex;
    public GameObject leftIndex;
    public GameObject laser;

    private bool positioned = false;
    private GameObject followedObject = null;
    private Vector3 patchPosition;
    private Quaternion patchRotation;

    void Start()
    {
        patchPosition = patchTransform.position;
        patchRotation = patchTransform.rotation;
    }

    void Update()
    {
        // If the user's index had triggered the patch's collider
        if (followedObject)
        {
            // check if now it's far from the patch
            if (Vector3.Distance(transform.position, followedObject.transform.position) > 0.2f)
            {
                // if so, stop following the finger
                followedObject = null;
                laser.GetComponent<LineRenderer>().enabled = true;
                // If the MeshRenderer is enabled it means the user has applied the patch
                if (GetComponent<MeshRenderer>().enabled)
                {
                    // So the patch was applied and the finger is at a distance above a threshold -> disable the applied patch 
                    GetComponent<MeshRenderer>().enabled = false;
                    GetComponent<AudioSource>().Play();
                    transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = true;
                    positioned = false; // flag the patch as not applied
                    GameObject newPatch = Instantiate(patch, patchPosition, patchRotation); // instantiate a new the patch
                    newPatch.GetComponent<patchWith>().enabled = true;
                }
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        // If the user's index collide with this applied patch
        if (other.tag == "IndexTrigger" || other.tag == "IndexTriggerL")
        {
            // start monitoring, in Update(), the distance between this applied patch and the finger
            followedObject = other.gameObject;
            laser.GetComponent<LineRenderer>().enabled = false;
        }
    }





    public void setFollower(string index)
    {
        if (index.Equals("smartphone_target"))
            followedObject = rightIndex;

        if (index.Equals("smartphone_target_L"))
            followedObject = leftIndex;

        laser.GetComponent<LineRenderer>().enabled = false;
    }

    public bool getPositioned(){
    	return positioned;
    }

    public void setPositioned(){
        positioned = true;
    }
}
