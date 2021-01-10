using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footseteps : MonoBehaviour
{

    private CharacterController cc;
    private float Timer = 0.0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cc.isGrounded == true && cc.velocity.magnitude > 2f && GetComponent<AudioSource>().isPlaying == false)
        {
            if (Timer > 0.3f) // Play the step sound every 0.3 seconds
            {
                GetComponent<AudioSource>().volume = Random.Range(0.8f, 1);
                GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.1f);
                GetComponent<AudioSource>().Play();
                Timer = 0.0f;
            }

            Timer += Time.deltaTime;
        }
    }
}
