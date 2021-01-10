using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLevelManager : MonoBehaviour
{

    public AudioSource cindy_thankyou;
    public AudioSource car_engine;
    public AudioSource car_accident;
    public changeScene Change;

    // Start is called before the first frame update
    void Start()
    {   
        StartCoroutine(cindy_thankyou_delay(3));
        StartCoroutine(car_engine_delay(5));
        StartCoroutine(car_accident_delay(11));
        StartCoroutine(load_scene(20));
    }

    IEnumerator cindy_thankyou_delay(int delay)
    {
        yield return new WaitForSeconds(delay);
        cindy_thankyou.Play();
    }

    IEnumerator car_engine_delay(int delay)
    {
        yield return new WaitForSeconds(delay);
        car_engine.Play();
    }

    IEnumerator car_accident_delay(int delay)
    {
        yield return new WaitForSeconds(delay);
        car_accident.Play();
    }

    IEnumerator load_scene(int delay)
    {
        yield return new WaitForSeconds(delay);
        Change.startLevel("endLevel_2");
    }
}
