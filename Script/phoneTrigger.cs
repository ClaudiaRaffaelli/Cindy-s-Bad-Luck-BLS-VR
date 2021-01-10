using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class phoneTrigger : MonoBehaviour
{
    public GameObject spawnObject;
    public GameObject spawnCoordinates;
    public GameObject pocket;
    public GameObject smartPhone;
    public GameObject rightHand;

    private Vector3 old_position, old_rotation;
    private bool atEar = false;


    void Update()
    {
        // keep the pocket below this phoneTrigger box. 
        // phoneTrigger box collider corrispond to the player ear.
        // So according to the player's ear position put the pocket such that it always follows the player height
        pocket.transform.position = new Vector3(transform.position.x, transform.position.y - 1.2f , transform.position.z);
        // Calculate the angle between the hand and pocket. If it's below 30 degrees (or in player's hand) then show the smartphone 
        float angle = Vector3.Angle(-pocket.transform.up, rightHand.transform.forward);
        if (angle <= 30 || smartPhone.GetComponent<smartphone>().getGrabbed())
        {
            smartPhone.GetComponent<MeshRenderer>().enabled = true;
            smartPhone.GetComponent<smartphone>().enabled = true;
        }
        else
        {
            smartPhone.GetComponent<MeshRenderer>().enabled = false;
            smartPhone.GetComponent<smartphone>().enabled = false;
        }
    }


    // When the Smartphone "collide" the ear
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Smartphone")
        {
            // Play phone dial sound
            GetComponent<AudioSource>().Play();
            // the smartphone has collided the ear
            atEar = true;
            // put the smartphone back in the pocket
            other.gameObject.GetComponent<smartphone>().putInPocket();
            other.gameObject.GetComponent<smartphone>().setTutorialDone(true);
            other.gameObject.GetComponent<smartphone>().setGrabbed(false);

            // if we are in the tutorial or in the first level we want to spawn an object
            if (SceneManager.GetActiveScene().name == "FirstLevel" || SceneManager.GetActiveScene().name == "Tutorial")
            {
                // save the position of spawnCoordinates so it can go back to the original position after this script
                old_position = spawnCoordinates.transform.position;
                old_rotation = spawnCoordinates.transform.eulerAngles;

                // make spawnCoordinates's Y near the player's (head) Y, so the object will spawn near the player's head
                spawnCoordinates.transform.position = new Vector3(
                    spawnCoordinates.transform.position.x,
                    transform.position.y - 0.5f,
                    spawnCoordinates.transform.position.z);

                // adjust the angle
                spawnCoordinates.transform.eulerAngles = new Vector3(
                    0,
                    spawnCoordinates.transform.eulerAngles.y,
                    0
                );


                // spawn the object
                spawnObject.transform.position = spawnCoordinates.transform.position;
                spawnObject.transform.rotation = spawnCoordinates.transform.rotation;
                spawnObject.SetActive(true);

                // reset spawnCoordinates transform
                spawnCoordinates.transform.position = old_position;
                spawnCoordinates.transform.eulerAngles = old_rotation;
            }

        }
    }


    public void triggerCall()
    {
        // the smartphone has collided the ear
        atEar = true;

        // save the position of spawnCoordinates so it can go back to the original position after this script
        old_position = spawnCoordinates.transform.position;
        old_rotation = spawnCoordinates.transform.eulerAngles;

        // make spawnCoordinates's Y near the player's (head) Y, so the object will spawn near the player's head
        spawnCoordinates.transform.position = new Vector3(
            spawnCoordinates.transform.position.x,
            transform.position.y - 0.5f,
            spawnCoordinates.transform.position.z);

        // adjust the angle
        spawnCoordinates.transform.eulerAngles = new Vector3(
            0,
            spawnCoordinates.transform.eulerAngles.y,
            0
        );

        // spawn the object
        spawnObject.transform.position = spawnCoordinates.transform.position;
        spawnObject.transform.rotation = spawnCoordinates.transform.rotation;
        spawnObject.SetActive(true);

        // reset spawnCoordinates transform
        spawnCoordinates.transform.position = old_position;
        spawnCoordinates.transform.eulerAngles = old_rotation;
            
    }


    public bool getatEar(){
        return atEar;
    }

    public void setAtEar(bool ear){
        atEar =ear;
    }

}
