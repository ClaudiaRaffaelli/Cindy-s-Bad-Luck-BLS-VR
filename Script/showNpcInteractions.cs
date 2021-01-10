using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showNpcInteractions : MonoBehaviour
{
    public List<GameObject> options;
    private float alpha;
    private bool visiblity;

    void Start()
    {
        alpha = 0f;
        visiblity = false;
        toggleOptions();
    }

    public void toggleOptions()
    {
        foreach (GameObject option in options)
        {
            option.GetComponent<CanvasGroup>().alpha = alpha;
            option.GetComponent<CanvasGroup>().blocksRaycasts = visiblity;
        }

        if (alpha == 0f)
            alpha = 1f;
        else
            alpha = 0f;
        visiblity = !visiblity;
    }

    public bool getVisibility(){
        return visiblity;
    }

}
