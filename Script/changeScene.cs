using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class changeScene : MonoBehaviour
{
    /////// This script allows the loading of a new standalone scene by giving its name as parameter.
    /////// It handles the teleport between the MainMenu to a different scene, from a scene to the MainManu 
    /////// when a button is pressed and the resume of the paused scene from the MainMenu

    // to manage the transition animation between scenes
    public RawImage black;
    public Animator anim;

    string activeScene = "";
    public Button resumeButton;

    // the iteger ID of the level we are in. The tutorial scene is level 0 and so on and so forth
    public int intLevelID;
    private int currentUnlockedLevel;

    private bool hasWon = false;

    public utilityScript utility;

    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        // Create a temporary reference to the current active scene.
        Scene currentActiveScene = SceneManager.GetActiveScene();

        if (currentActiveScene.name != "MainMenu" && OVRInput.Get(OVRInput.Button.Start) == true){
            // If we want to resume the current scene we can now retrieve it:
            PlayerPrefs.SetString("PausedScene", currentActiveScene.name);
            // teleporting to the MainMenu scene when the Start button is pressed with a fade effect
            teleportToPauseRoom();
        }
        if (currentActiveScene.name == "MainMenu" && resumeButton.interactable == true){
            //Fetch name (string) from the PlayerPrefs set in the scene script. If no string exists, the default is ""
            activeScene = PlayerPrefs.GetString("PausedScene");
        }
    }

    public void resumeScene(){
        if (activeScene != ""){
            StartCoroutine(Fading(activeScene));
            activeScene = "";
        }
    }

    public void startLevel(string LevelName)
    {   
        try{
            utility.startClickOnButtonSound();
        }catch{}
        StartCoroutine(Fading(LevelName));
        // if we started the game from a non-resume button we want to start the level from the beginning
        PlayerPrefs.SetString("LastCanvas", "");
    }

    public void teleportToPauseRoom(bool isEnded = false){
        // if we ended the level 
        if (isEnded == true){
            // checking if we have won or lost (hasWon is setted from the manager of the level when we win, otherwise we assume we lost)

            if (hasWon == true){
                // we retrieve the last unlocked level 
                currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");
                // if the last currently unlocked level is the same as the level we just completed, we unlock the next level
                if (currentUnlockedLevel == intLevelID){
                    PlayerPrefs.SetInt("UnlockedLevel", intLevelID+1);       
                }
            }
            //we reset the last canvas because the level has been completed and we won't resume it.
            PlayerPrefs.SetString("LastCanvas", "");
            // there is no scene to be resumed
            PlayerPrefs.SetString("PausedScene", "");
            // click sound because we hit a button to go back to the pause room. If the game is not endend we have simply hit the 
            // Start button instead to go back to the pause room "pausing" the game (no sound required in this case)
            utility.startClickOnButtonSound();
        }
    
        if (activeScene == ""){
            StartCoroutine(Fading("MainMenu"));
        }
    }

    public void loadNextLevel(string levelName){
        // we retrieve the last unlocked level 
        currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");
        // if the last currently unlocked level is the same as the level we just completed, we unlock the next level
        if (currentUnlockedLevel == intLevelID){
            PlayerPrefs.SetInt("UnlockedLevel", intLevelID+1);       
        }
        // there is no scene to be resumed
        PlayerPrefs.SetString("PausedScene", "");
        
        startLevel(levelName);
    }

    public void setHasWon(bool won){
        hasWon = won;
        
    }

    public IEnumerator Fading(string LevelName){
        anim.SetBool("Fade", true);
        // waiting until alpha value is 1
        yield return new WaitUntil(()=>black.color.a==1);
        SceneManager.LoadScene(LevelName, LoadSceneMode.Single);
    }

}