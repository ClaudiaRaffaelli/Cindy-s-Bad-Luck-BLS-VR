using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class phoneManager : MonoBehaviour
{


    public GameObject whenToCall_obj;
    public GameObject whatToDo_obj;
    public GameObject confirmButtonWhenToCall;
    public GameObject confirmButtonWhatToDo;
    public Material[] answerMaterials;
    public GameObject smartphone;
    public GameObject[] whenToCallPlates;
    public GameObject[] whatToDoPlates;
    public GameObject player;

    private bool[] whenToCallOptions;
    private bool[] whatToDoOptions;
    private bool[] TTwhenToCallOptions = { true, false, true, true, false, true, false, false, false };
    private bool[] TTwhatToDoOptions = { true, false, false, true, false, false, true, false, true};

    private bool correctPhoneTutorial = false;
    private bool correctWhatToDo = false;

    void Start()
    {
        whenToCallOptions = new bool[9];
        whatToDoOptions = new bool[9];
        for (int i = 0; i < 9; i++)
        {
            whenToCallOptions[i] = false;
            whatToDoOptions[i] = false;
        }
        whenToCall_obj.SetActive(false);
        whatToDo_obj.SetActive(false);

    }


    public void update(int option, int index)
    {
        if (option == 0)
            whenToCallOptions[index] = !whenToCallOptions[index];
        else
            whatToDoOptions[index] = !whatToDoOptions[index];
    }




    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IndexTrigger" && SceneManager.GetActiveScene().name == "FirstLevel")
        {
            whenToCall_obj.SetActive(true);
            whenToCall_obj.transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
    }




    public void checkWhenToCallOptions()
    {
        bool correct = true;

        for (int i=0; i<whenToCallOptions.Length; i++)
        {
            if (whenToCallOptions[i] != TTwhenToCallOptions[i])
            {
                correct = false;
                if (whenToCallOptions[i]) //only if the user selected this plate, inform him that this answer was wrong
                    whenToCallPlates[i].GetComponent<Renderer>().material = answerMaterials[0];
            }
            else
            {
                if (whenToCallOptions[i]) //only if the user selected this plate, inform him that this answer was correct
                    whenToCallPlates[i].GetComponent<Renderer>().material = answerMaterials[1];
            }
        }
        if (correct)
        {
            Destroy(whenToCall_obj);
            correctPhoneTutorial = true;
        }
        else
        {
            confirmButtonWhenToCall.GetComponent<Renderer>().material = answerMaterials[0];
            confirmButtonWhenToCall.GetComponent<Animation>().Play("phoneConfirm");
        }
            
    }

    public void checkWhatToDoOptions()
    {
        bool correct = true;

        for (int i = 0; i < whatToDoOptions.Length; i++)
        {
            if (whatToDoOptions[i] != TTwhatToDoOptions[i])
            {
                correct = false;
                if (whatToDoOptions[i]) //only if the user selected this plate, inform him that this answer was wrong
                    whatToDoPlates[i].GetComponent<Renderer>().material = answerMaterials[0];
            }
            else
            {
                if (whatToDoOptions[i]) //only if the user selected this plate, inform him that this answer was correct
                    whatToDoPlates[i].GetComponent<Renderer>().material = answerMaterials[1];
            }

        }
        if (correct)
        {
            Destroy(whatToDo_obj);
            correctWhatToDo = true;
        }
        else
        {
            confirmButtonWhatToDo.GetComponent<Renderer>().material = answerMaterials[0];
            confirmButtonWhatToDo.GetComponent<Animation>().Play("phoneConfirm");
        }

    }

    public bool getCorrectPhoneCall(){
        return correctPhoneTutorial;
    }

    public bool getCorrectWhatToDoCall(){
        return correctWhatToDo;
    }

}
