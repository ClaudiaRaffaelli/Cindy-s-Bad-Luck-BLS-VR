using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class smartphone : MonoBehaviour
{
    public GameObject rightHand;
    public Transform pocket;

    private bool grabbed;
    private bool tutorialDone;

    void Start()
    {
        grabbed = false;
        tutorialDone = false;
        // There are 2 Smartphone in FirstLevel, this if will destroy the one that is already in the player's pocket
        if (SceneManager.GetActiveScene().name == "FirstLevel" || SceneManager.GetActiveScene().name == "Tutorial")
        {
            // We need to find and learn to use the smartphone. So it shouldn't be already in the pocket
            if (this.gameObject.transform.parent.tag == "Player")
                Destroy(this.gameObject);
        }
        else
        {
            // If we are not in the FirstLevel it means we have already done the tutorial. So the phone should be already in the pocket
            tutorialDone = true;
            putInPocket();
        }
    }

    void Update()
    {
        // put the phone back in the pocket
        if(OVRInput.Get(OVRInput.Button.One) && grabbed && tutorialDone)
        {
            putInPocket();
        }
    }

    // When the player's index collide with the smartphone, put the smartphone in player's right hand (according to rightHand object'transform
    // which is a child of the real right hand object), only if it's not already grabbing something.
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IndexTrigger" && rightHand.transform.childCount == 0)
        {
            this.transform.parent = rightHand.transform;
            transform.position = rightHand.transform.position;
            transform.rotation = rightHand.transform.rotation;
            grabbed = true;
        }
    }

    public void setTutorialDone(bool value)
    {
        tutorialDone = value;
    }

    public void setGrabbed(bool value)
    {
        grabbed = value;
    }

    public void putInPocket()
    {
        this.transform.parent = pocket.transform;
        transform.position = pocket.position;
        transform.rotation = pocket.rotation;
        grabbed = false;
    }

    public bool getGrabbed(){
        return grabbed;
    }
}
