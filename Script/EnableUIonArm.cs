using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableUIonArm : MonoBehaviour
{
    public GameObject Button;
    public GameObject UIPanel;
    public GameObject leftHand;

    void Update() { 
    
        // angle difference between upwards direction and button's forward direction
        float angle = Vector3.Angle(Vector3.up, Button.transform.forward);


        if (angle >= 0 && angle <= 60)
        {
            Button.SetActive(true);
        }
        else
        {
            Button.SetActive(false);
            UIPanel.SetActive(false);
        }
    }
}
