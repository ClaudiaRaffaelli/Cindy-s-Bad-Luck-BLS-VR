using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: destroy this script when unnecessary

public class CarloPull : MonoBehaviour
{

    public GameObject Cindy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y+0.78f, transform.localPosition.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x+70, transform.eulerAngles.y, transform.eulerAngles.z);
            GetComponent<Animator>().SetTrigger("MoveLegsUp");
            Cindy.GetComponent<Animator>().SetTrigger("MoveLegsUp");
        }
    }
}
