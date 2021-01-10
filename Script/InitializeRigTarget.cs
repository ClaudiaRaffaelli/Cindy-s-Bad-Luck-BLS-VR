using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeRigTarget : MonoBehaviour
{
    public Transform target;
    private float dist;

    void Update()
    {
        dist = Vector3.Distance(transform.position, target.position);
        if (dist > 0.5) transform.position = target.position;
    }
}
