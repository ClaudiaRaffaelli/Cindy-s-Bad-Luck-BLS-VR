using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerColliding : MonoBehaviour
{
	private bool hasCollided = false; 

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "IndexTrigger")
        {
            hasCollided = true;
        }
    }

    public bool hasPlayerCollided(){
    	return hasCollided;
    }

    public void setPlayerCollided(bool collide){
    	hasCollided = collide;
    }
}
