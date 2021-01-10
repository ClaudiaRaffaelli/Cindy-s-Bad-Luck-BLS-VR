using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destoryParticleSystem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IndexTrigger" || other.tag == "IndexTriggerL")
        {
            Destroy(this.gameObject);
        }
    }
}
