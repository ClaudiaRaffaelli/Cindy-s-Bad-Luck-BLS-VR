using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seizure : MonoBehaviour
{
    public GameObject handHelper;
    public GameObject legHelper;
    public GameObject pull1Helper;
    public GameObject pull2Helper;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    void Update()
    {
        // When the user has moved cindy's arm and leg (through handHelper and legHelper), activate these last 2 objects to complete 
        // the safe position animation
        if (GetComponent<Animator>().GetBool("arm") && GetComponent<Animator>().GetBool("leg"))
        {
            pull1Helper.SetActive(true);
            pull2Helper.SetActive(true);
            // finally destroy this component because no more necessary
            Destroy(this.gameObject.GetComponent<seizure>());
        }
    }

    public void startSeizure(int seconds){
        // Stop the seizure animation after X seconds
        StartCoroutine(DelayedAnimation(seconds));
    }


    IEnumerator DelayedAnimation(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<Animator>().SetBool("seizure", false);
        // When the seizure ends, activate these objects which help user to put Cindy in the safe position
        handHelper.SetActive(true);
        legHelper.SetActive(true);
    }
}
