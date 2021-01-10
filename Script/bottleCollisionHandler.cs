using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottleCollisionHandler : MonoBehaviour
{

    private bool isColliding = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.name == "Bottle"){
            isColliding = true;
		}
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.name == "Bottle"){
            isColliding = false;
        }
    }

    public bool isBottleColliding(){
        return isColliding;
    }
}
