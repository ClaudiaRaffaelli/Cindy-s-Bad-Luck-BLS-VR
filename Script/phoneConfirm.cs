using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phoneConfirm : MonoBehaviour
{

    public GameObject phone;
    public int phoneMode;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IndexTrigger" || other.tag == "IndexTriggerL")
        {
            if (phoneMode == 0)
                phone.GetComponent<phoneManager>().checkWhenToCallOptions();
            else
                phone.GetComponent<phoneManager>().checkWhatToDoOptions();
        }
    }
}
