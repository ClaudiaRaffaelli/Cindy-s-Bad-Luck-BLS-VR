using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overlap_bodyguard_01 : MonoBehaviour
{
    public Transform bodyguard_head;
    public Transform bodyguard_middle_spine;
    public Transform bodyguard_left_leg;
    public Transform bodyguard_right_leg;
    public Transform hologram_head;
    public Transform hologram_middle_spine;
    public Transform hologram_left_leg;
    public Transform hologram_right_leg;
    public double overlap_threshold = 0.1;


    void Update()
    {
        // Debug.Log(body_head.position);

        if ((bodyguard_head.position - hologram_head.position).sqrMagnitude < overlap_threshold)
        {
            Debug.Log("Overlap!");
        }
    }

}
