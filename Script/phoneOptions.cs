using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phoneOptions : MonoBehaviour
{
    public GameObject phone;
    public Material selectedMaterial;
    public Material notSelectedMaterial;
    public int indexOption;
    public int phoneMode;
    private bool selected = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IndexTrigger" || other.tag == "IndexTriggerL")
        {
            if (!selected)
            {
                selected = true;
                GetComponent<Renderer>().material = selectedMaterial;
                GetComponent<Animation>().enabled = false;
            }
            else
            {
                selected = false;
                GetComponent<Renderer>().material = notSelectedMaterial;
                GetComponent<Animation>().enabled = true;
            }
            phone.GetComponent<phoneManager>().update(phoneMode, indexOption);
        }
    }
}
