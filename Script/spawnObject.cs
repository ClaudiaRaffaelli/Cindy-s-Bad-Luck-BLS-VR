using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnObject : MonoBehaviour
{
    public GameObject oggetto;
    private bool canSpawn = false;

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) != 0)
        {
            canSpawn = true;
        }
        else
        {
            canSpawn = false;
        }
    }

    void LateUpdate()
    {
        if (canSpawn)
        {
            Instantiate(oggetto, transform.position, Quaternion.identity);
            canSpawn = false;
        }
    }
}
