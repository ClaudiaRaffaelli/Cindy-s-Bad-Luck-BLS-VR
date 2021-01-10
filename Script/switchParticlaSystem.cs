using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchParticlaSystem : MonoBehaviour
{
    // Switch on and off an array of particle systems

    public GameObject[] particleSystemObject;
    public bool start = false;

    private bool toggle;

    void Start()
    {
        toggle = start;
        for (int i=0; i< particleSystemObject.Length; i++)
            particleSystemObject[i].GetComponent<ParticleSystem>().enableEmission = toggle;
        // Play/stop sound
        if (toggle)
            GetComponent<AudioSource>().Play();
        else
            GetComponent<AudioSource>().Stop();

        toggle = !toggle;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "IndexTrigger" || other.gameObject.tag == "IndexTriggerL")
        {
            for (int i = 0; i < particleSystemObject.Length; i++)
                particleSystemObject[i].GetComponent<ParticleSystem>().enableEmission = toggle;
            // Play/stop sound
            if (toggle)
                GetComponent<AudioSource>().Play();
            else
                GetComponent<AudioSource>().Stop();

            toggle = !toggle;
        }
    }

    public bool isTurnedOn(){
        return !toggle;
    }

    public void turnOff(){
        for (int i = 0; i < particleSystemObject.Length; i++)
                particleSystemObject[i].GetComponent<ParticleSystem>().enableEmission = toggle;
        toggle = !toggle;
    }
}
