using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class firstTimeTutorialManager : MonoBehaviour
{
    public Canvas WelcomeCanvas;
    public Canvas TestWalkCanvas;
    public Canvas TestViewCanvas;
    public Canvas TestPauseButtonCanvas;
    public Canvas TestButtonCanvas;
    public Button TryButton;
    Canvas CurrentCanvas;
    public Canvas MenuCanvas;
    public static bool firstTime = true;

    public utilityScript utility;

    // Start is called before the first frame update
    void Start()
    {
        // setting the fog
        RenderSettings.fog = true;

        if (firstTime == true){
            CurrentCanvas = WelcomeCanvas;
            CurrentCanvas.gameObject.SetActive(true);
            // initializing the preference regarding the paused scene
            PlayerPrefs.SetString("PausedScene", "");

            if (PlayerPrefs.GetString("FirstTime") == ""){
                // we save the fact that this is not the first time we enter the game.
                // it will be used to avoid watching the tutorial each time we load the game
                PlayerPrefs.SetString("FirstTime", "false");
                PlayerPrefs.SetInt("UnlockedLevel", 0);
                utility.FogFadingInFirstTutorial();
            }
            else{
                //This is not the first time we enter the game and we don't want to show the tutoral each time
                startGame();
            }
            
        }else{
            // this is not the first time we end up in the menu room and we don't want to show the tutorial
            CurrentCanvas = MenuCanvas;
            CurrentCanvas.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {   
        // if this is the first time we end up in the game we want to show the tutorial first
        if (firstTime == true ){
            if (CurrentCanvas == WelcomeCanvas && OVRInput.Get(OVRInput.Button.One)==true){
                CurrentCanvas.gameObject.SetActive(false);
                CurrentCanvas = TestWalkCanvas;
                CurrentCanvas.gameObject.SetActive(true);

            }
            else if (CurrentCanvas ==TestWalkCanvas){
                Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                if (primaryAxis.y != 0 || primaryAxis.x !=0){
                    CurrentCanvas.gameObject.SetActive(false);
                    CurrentCanvas = TestViewCanvas;
                    CurrentCanvas.gameObject.SetActive(true);
                }
            }
            else if (CurrentCanvas == TestViewCanvas){
                Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
                if (primaryAxis.x != 0){
                    CurrentCanvas.gameObject.SetActive(false);
                    CurrentCanvas = TestPauseButtonCanvas;
                    CurrentCanvas.gameObject.SetActive(true);
                }
            }
            else if (CurrentCanvas == TestPauseButtonCanvas && OVRInput.Get(OVRInput.Button.Start) == true){
                CurrentCanvas.gameObject.SetActive(false);
                CurrentCanvas = TestButtonCanvas;
                CurrentCanvas.gameObject.SetActive(true);
            }
        }  
    }

    public void startGame(){
        utility.FogFadingOut();
        CurrentCanvas.gameObject.SetActive(false);
        CurrentCanvas = MenuCanvas;
        CurrentCanvas.gameObject.SetActive(true);
        firstTime = false;

    }

}


