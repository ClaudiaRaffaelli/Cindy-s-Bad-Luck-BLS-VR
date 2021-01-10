using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveToTarget : MonoBehaviour
{
    public GameObject target;
    private float speed = 0.15f;
    private bool move = false;

    /*
    private Transform cachedTransform;
    private Vector3 cachedPosition;


    void Start()
    {
        cachedPosition = GetComponent<Transform>().position;
    }

    
    void FixedUpdate()
    {

        if (Vector3.Distance(transform.position, hand.transform.position) <= 0.25 && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) != 0) {
            if (cachedPosition != GetComponent<Transform>().position)
            {
                transform.position = Vector3.MoveTowards(transform.position, hand.transform.position, speed * Time.deltaTime);
                cachedPosition = transform.position;
            }
        }
        else if (Vector3.Distance(transform.position, target.transform.position) >= 0.50 && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) == 0)
        {
            if (cachedPosition != GetComponent<Transform>().position)
            {
                transform.position = Vector3.MoveTowards(transform.position, source.transform.position, speed * Time.deltaTime);
                cachedPosition = transform.position;
            }
        }

    }
    */

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("DENTRO!");
            move = true;
        }

    }

    void Update()
    {
        if (move && Vector3.Distance(transform.position, target.transform.position) >= 0.25)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }


}
