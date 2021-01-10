using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBodyPart : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    public GameObject levelManager;
    public string taskName;
    private float speed = 0.15f;
    private int moveTo = 0;
    private float thresholdDistance = 0.10f;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("DENTRO!");
            moveTo = 1;
            GetComponent<ParticleSystem>().Stop();
        }

    }

    void Update()
    {
        if (moveTo != 0)
        {
            if (moveTo == 1)
            {
                if (Vector3.Distance(transform.position, target1.transform.position) >= thresholdDistance)
                    transform.position = Vector3.MoveTowards(transform.position, target1.transform.position, speed * Time.deltaTime);
                else
                    moveTo = 2;
            }else if (moveTo == 2)
            {
                if (Vector3.Distance(transform.position, target2.transform.position) >= thresholdDistance)
                    transform.position = Vector3.MoveTowards(transform.position, target2.transform.position, speed * Time.deltaTime);
                else
                {
                    // End of the movement
                    moveTo = 0;
                    //Notify the action to the gameManager
                    levelManager.GetComponent<levelManager>().update(taskName);
                }
            }   
        }
    }
}
