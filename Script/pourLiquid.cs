using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pourLiquid : MonoBehaviour
{
    public GameObject liquid;
    private bool isPouring = false;

    void Start()
    {
        liquid.GetComponent<ParticleSystem>().enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Vector3.Angle(Vector3.up, transform.up);

        if (angle >= 90)
        {
            liquid.GetComponent<ParticleSystem>().enableEmission = true;
            if(!isPouring)
                GetComponent<AudioSource>().Play();
            isPouring = true;
        }
        else
        {
            liquid.GetComponent<ParticleSystem>().enableEmission = false;
            GetComponent<AudioSource>().Stop();
            isPouring = false;
        }
    }

    public bool isBottlePouring(){
        return isPouring;
    }
}
