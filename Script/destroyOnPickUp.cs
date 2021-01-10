using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyOnPickUp : MonoBehaviour
{
    // When the finger triggers this object the script will destroy the target object

    public GameObject target;
    public string fingerTagR = "";
    public string fingerTagL = "";
    public GameObject rightHandTarget;
    public GameObject leftHandTarget;


    void OnTriggerEnter(Collider other)
    {
        // If the hand can't grab this object (because it already has a child object grabbed) then don't destroy the parent object
        if ((other.gameObject.tag == fingerTagR && rightHandTarget.transform.childCount == 0) || 
            (other.gameObject.tag == fingerTagL && rightHandTarget.transform.childCount == 0))
        {
            StartCoroutine(destroyCoroutine());
        }
    }


    IEnumerator destroyCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(target);
    }
}
