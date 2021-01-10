using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disapperHand : MonoBehaviour
{

    public GameObject hand;


    void Update()
    {
        if(transform.childCount > 0)
        {
            hand.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        else
        {
            hand.GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
    }
}
