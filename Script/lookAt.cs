using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAt : MonoBehaviour
{
    public GameObject canvasObject;
    public Transform target;


    void Update()
    {
        // Constantly loot at the target
        transform.LookAt(target, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Disable canvas
            canvasObject.GetComponent<Canvas>().enabled = false;
            // Or destroy this object completly
            Destroy(this.gameObject);
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Not necessary if the object is destroyed
            canvasObject.GetComponent<Canvas>().enabled = true;
        }
    }
}



