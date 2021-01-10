using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInstructionsArm : MonoBehaviour
{
    public GameObject UIPanel;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IndexTrigger")
        {
            bool isActive = UIPanel.activeSelf;
            UIPanel.SetActive(!isActive);
        }
    }

    public void trigger()
    {
        bool isActive = UIPanel.activeSelf;
        UIPanel.SetActive(!isActive);
    }
}
