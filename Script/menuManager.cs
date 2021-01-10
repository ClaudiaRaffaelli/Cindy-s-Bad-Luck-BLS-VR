using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{

	public Canvas MenuCanvas;
	public Canvas LevelsCanvas;
	public Button resumeButton;
    private string activeScene;

    // unlockedLevels contains the last currently unlocked level
    private int unlockedLevels;
    public Button TutorialButton;
    public Button FirstLevelButton;
    public Button SecondLevelButton;
    public Button ThirdLevelButton;
    public Button FourthLevelButton;
    public Button EndLevelButton;

    public utilityScript utility;
    public changeScene change;

    // Start is called before the first frame update
    void Start()
    {  

    }

    // Update is called once per frame
    void Update()
    {
    	// if the user comes from a level scene, the scene can be resumed
    	if (MenuCanvas.gameObject.activeSelf == true){
            //Fetch name (string) from the PlayerPrefs set in the scene script. If no string exists, the default is ""
            activeScene = PlayerPrefs.GetString("PausedScene");
    		// resumeLevel has been set in the provenance scene
            if (activeScene != ""){
    			resumeButton.interactable = true;
    		}

    	}
        if (LevelsCanvas.gameObject.activeSelf == true){
            // getting the last currently unlocked level
            unlockedLevels = PlayerPrefs.GetInt("UnlockedLevel");
            switch (unlockedLevels){
                case 0:
                    TutorialButton.interactable = true;
                    break;
                case 1:
                    TutorialButton.interactable = true;
                    FirstLevelButton.interactable = true;
                    break;
                case 2:
                    TutorialButton.interactable = true;
                    FirstLevelButton.interactable = true;
                    SecondLevelButton.interactable = true;
                    break;
                case 3:
                    TutorialButton.interactable = true;
                    FirstLevelButton.interactable = true;
                    SecondLevelButton.interactable = true;
                    ThirdLevelButton.interactable = true;
                    break;
                case 4:
                    TutorialButton.interactable = true;
                    FirstLevelButton.interactable = true;
                    SecondLevelButton.interactable = true;
                    ThirdLevelButton.interactable = true;
                    FourthLevelButton.interactable = true;
                    break;
                case 5:
                    TutorialButton.interactable = true;
                    FirstLevelButton.interactable = true;
                    SecondLevelButton.interactable = true;
                    ThirdLevelButton.interactable = true;
                    FourthLevelButton.interactable = true;
                    EndLevelButton.interactable = true;
                    break;
            }
        } 
    }

    public void showLevels(){
        MenuCanvas.gameObject.SetActive(false);
        LevelsCanvas.gameObject.SetActive(true);
        utility.startClickOnButtonSound();
    }

    public void showMainMenu(){
        LevelsCanvas.gameObject.SetActive(false);
        MenuCanvas.gameObject.SetActive(true);
        utility.startClickOnButtonSound();
    }

    public void quitApplication(){
        utility.startClickOnButtonSound();
        Application.Quit();     
    }

    public void resumeGame(){
    	utility.startClickOnButtonSound();
    	change.resumeScene();
    }
}
