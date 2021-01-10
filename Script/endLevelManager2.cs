using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endLevelManager2 : MonoBehaviour
{
	public changeScene Change;
	public Canvas Credits;
    public ParticleSystem confetti;
    public GameObject Cindy;
    public GameObject Jack;
    public GameObject Emily;
    public GameObject Liam;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(displayCredits(5));
        StartCoroutine(dance(7));
        StartCoroutine(exitScene(69));   
    }


    IEnumerator displayCredits (int delay){
    	yield return new WaitForSeconds(delay);
    	Credits.gameObject.SetActive(true);
        confetti.Play();
        GetComponent<AudioSource>().Play();
    }


    IEnumerator dance(int delay)
    {
        yield return new WaitForSeconds(delay);
        Cindy.GetComponent<Animator>().SetBool("dance", true);
        Jack.GetComponent<Animator>().SetBool("dance", true);
        Emily.GetComponent<Animator>().SetBool("dance", true);
        Liam.GetComponent<Animator>().SetBool("dance", true);
    }


    IEnumerator exitScene(int delay){
    	yield return new WaitForSeconds(delay);
    	Change.teleportToPauseRoom();
    }
}
