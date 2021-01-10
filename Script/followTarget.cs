using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTarget : MonoBehaviour
{
    public GameObject target;
    private float speed = 1f;
    private Vector3 offset = new Vector3(0, 0, 0);

    void LateUpdate()
    {
        if (Vector3.Distance(transform.position, target.transform.position) >= 0.15)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position - offset, speed * Time.deltaTime);    
    }
}
