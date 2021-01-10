using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class thirdLevelManager : MonoBehaviour
{

	public Canvas CarAccidentInfoCanvas0;
	public Canvas CarAccidentInfoCanvas1;
    public Canvas CarAccidentInfoCanvas2;
    public Canvas DangerInfoCanvas0;
    public Canvas MotorcyclistCanvas0;
    public Canvas CongratulationCanvas;
    public Canvas GameOverCanvas;
    public Canvas ActionCanvas;

    public static Canvas CurrentCanvas;

    // we want to know the OVR player position
    public GameObject OVRPlayer;

    bool first = true;
    int currentUnlockedLevel;
    string lastCanvas; 

    // the change scene script
    public changeScene Change;
    // it contains at the end of the game the value true or false depending on the fact that we won or lost the level
    bool hasWon;

    // detect if the player has completed a phone call
    public phoneTrigger triggerPhone;
    // detect cindy position
    public pickUpBody pickCindy;
    // detect if the bandage has been placed
    public patchWith patch;
    // detect if the triangles have been placed
    public List<patchWith> patchWithTriangles;
    // detect cindy's position
    public GameObject cindy;
    // detect when the player collides with the motorbiker
    public playerColliding motorbikerCollides;
    // detect if the player leaves the area (flee the scene)
    public playerColliding leaveCollides;
    // detect if the player collides with the car 
    public playerColliding carCollides;

    // variables that track the actions of the player during the level
    // we check that the motorcyclist is breathing
    bool isBreathing = false;
    // we want to know if the player has called
    bool hasCalled = false;
    // we want to know if the bandage has been placed on cindy's head
    bool bandagePlaced = false;
    // we want to know if the road triangles have been placed
    bool [] hasTriangles = new bool [] {false, false};

    // the penalty assigned to actions out of order or wrong actions. If penalty >=3 the game is over
    int penalty = 0;

    // utility script
    public utilityScript utility;

    // Start is called before the first frame update
    void Start()
    {
        // setting the fog
        RenderSettings.fog = true;

        // we retrieve the last unlocked level 
        currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");
        // we check if this level was paused, and in this case we resume from where we left
        lastCanvas = PlayerPrefs.GetString("LastCanvas");
        // if the game was not paused or it was paused before we reached the first checkpoint, we start from the beginning
        if (lastCanvas == ""){
            CurrentCanvas = CarAccidentInfoCanvas0;
        }
        // otherwise we resume from where we left off
        else{
            switch(lastCanvas){
                case "CarAccidentInfoCanvas2":
                    CurrentCanvas = DangerInfoCanvas0;
                    break;
                case "DangerInfoCanvas0":
                    CurrentCanvas = MotorcyclistCanvas0;
                    // placing the player where she / he left off
                    OVRPlayer.transform.position = new Vector3(161.40f, 1.78f, 179f);
                    break;
                case "MotorcyclistCanvas0":
                    CurrentCanvas = null;
                    // placing the player where she / he left off
                    OVRPlayer.transform.position = new Vector3(157.20f, 1.78f, 153f);
                    OVRPlayer.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

                    // setting Cindy in the right position
                    cindy.transform.position = pickCindy.safeZone.transform.position;
            		cindy.transform.eulerAngles = pickCindy.safeZone.transform.eulerAngles;
            		pickCindy.setIsSafe(true);
            		pickCindy.transform.GetChild(0).transform.parent = pickCindy.transform.parent;
                    Destroy(pickCindy.safeZone.gameObject);
                    Destroy(pickCindy.gameObject);
                    break;
            }
        
        }
        
        Invoke("delayPhoneTriggerInitialization", 1.0f);
    }

    void delayPhoneTriggerInitialization()
    {
        triggerPhone.setAtEar(false);
    }

    // Update is called once per frame
    void Update()
    {   
        if ((CurrentCanvas != GameOverCanvas) && (CurrentCanvas != CongratulationCanvas)){
            // if the canvas to be displayed is the first we display it and activate the fog effect
            if ((CurrentCanvas == CarAccidentInfoCanvas0) && (first == true)){
                // activate the canvas
                CurrentCanvas.gameObject.SetActive(true);
                // activate the fog effect
                utility.FogFadingIn();

                first = false;
            }
            // if the canvas to be displayed is the danger canvas, and the player reached Cindy's car, 
            // we display the canvas and activate the fog effect
            if ((CurrentCanvas == DangerInfoCanvas0) && (OVRPlayer.transform.position.z <179.0f) && (first == true)){
                // activate the canvas
                CurrentCanvas.gameObject.SetActive(true);
                // activate the fog effect
                utility.FogFadingIn();

                first = false;
            }

            // if the canvas to be displayed is the motorcyclist canvas, the player put Cindy in the safe zone, and the player is looking 
            // in the motorcyclist direction, we display the canvas and activate the fog effect
            if ((CurrentCanvas == MotorcyclistCanvas0) && (Vector3.Angle(OVRPlayer.transform.forward, Vector3.forward) <= 60.0) 
                && (first == true) && (pickCindy.getIsSafe() == true)){ 
                // activate the canvas
                CurrentCanvas.gameObject.SetActive(true);
                // activate the fog effect
                utility.FogFadingIn();

                first = false;
            }

            if(isBreathing == false){
                // we check if we checked if now the motorcyclist is breathing
                if (motorbikerCollides.hasPlayerCollided() == true){
                    isBreathing = true;
                    motorbikerCollides.setPlayerCollided(false);
                    // we check that the previous actions have been made
                    if (CurrentCanvas == null){
                        utility.goodAction();
                        StartCoroutine(utility.resetInfo());
                    }
                    else{
                        penalty +=2;
                        utility.badAction(2);
                        StartCoroutine(utility.resetInfo());
                    }
                }
            }

            if (hasTriangles.Contains(false)){
                // we check that the previous actions have been made
                if (isBreathing == true){
                    if (hasTriangles[0] == false){
                        // if the first triangle has been set
                        if (patchWithTriangles[0].getPositioned() == true){
                            hasTriangles[0]=true;
                            utility.goodAction();
                            StartCoroutine(utility.resetInfo());
                        }
                    }
                    if(hasTriangles[1] == false){
                        // if the second triangle has been set
                        if (patchWithTriangles[1].getPositioned() == true){
                            hasTriangles[1]=true;
                            utility.goodAction();
                            StartCoroutine(utility.resetInfo());
                        }
                    }
                }
                else{
                    // if we place a triangle before the right time it's game over, because we haven't check the motorcyclist
                    if ((patchWithTriangles[0].getPositioned() == true) || (patchWithTriangles[1].getPositioned()== true)){
                        // game over
                        penalty+=3;
                        utility.badAction(3);
                        StartCoroutine(utility.resetInfo());
                    }
                }       
            }

            // we check if it has been made the phone call
            if (hasCalled == false){
                if (triggerPhone.getatEar() == true){
                    hasCalled = true;
                    // reset the phoneTrigger
                    triggerPhone.setAtEar(false);
                    // check if it was the right time to make the call: the triangles have been set
                    
                    if (!hasTriangles.Contains(false)){ 
                        utility.goodAction();
                        StartCoroutine(utility.resetInfo());
                    }else{
                        penalty +=2;
                        utility.badAction(2);
                        StartCoroutine(utility.resetInfo());
                    }
                    
                }
            }
            if (bandagePlaced == false){
                if (patch.getPositioned() == true){
                    bandagePlaced = true;
                    // check if it was the right time to place the bandage
                    if (hasCalled == false){
                        penalty +=3;
                        utility.badAction(3);
                        StartCoroutine(utility.resetInfo());
                    }
                    else if (hasTriangles.Contains(false)){
                        penalty +=1;
                        utility.okAction(1);
                        StartCoroutine(utility.resetInfo());
                    }
                    else{
                        utility.goodAction();
                        StartCoroutine(utility.resetInfo());
                        hasWon = true;
                        utility.startCongratulationSound();
                        CurrentCanvas = CongratulationCanvas;
                    }
                    CurrentCanvas.gameObject.SetActive(true);
                    utility.FogFadingIn();
                }
            }

            // check if we collide with the fire
            if (carCollides.hasPlayerCollided()== true){
                utility.displayText("You caught fire!");
                penalty += 3;
                carCollides.setPlayerCollided(false);
                StartCoroutine(utility.resetInfo());
            }

            // check if we flee the scene
            if (leaveCollides.hasPlayerCollided()== true){
                utility.displayText("You fleed!");
                penalty += 3;
                leaveCollides.setPlayerCollided(false);
                StartCoroutine(utility.resetInfo());
            }

            // check if the player calls more than one time, it is game over
            if (hasCalled == true){
                if (triggerPhone.getatEar() == true){
                    penalty+=3;
                    utility.badAction(3);
                    StartCoroutine(utility.resetInfo());
                }
            }

            // the game is over if the penalty is too high (we check if it is != true because if the player has already won the other wrong
            // actions are ignored)
            if (penalty >=3 && hasWon !=true){
                hasWon = false;
                // we activate the game over canvas to comunicate it to the player
                CurrentCanvas = GameOverCanvas;
                utility.startGameOverSound();
                CurrentCanvas.gameObject.SetActive(true);
                utility.FogFadingIn();
            }
        } 
        else{
            // if we won or lost (the GameOverCanvas or Congratulation canvas is shown) we want to block the player movements
            OVRPlayer.GetComponent<OVRPlayerController>().movSpeed = 0f;
            OVRPlayer.GetComponent<OVRPlayerController>().Acceleration = 0f;
            OVRPlayer.GetComponent<OVRPlayerController>().Damping = 0f;
        }
    }

    public void nextCanvas(Canvas nextCanvas){
    	CurrentCanvas.gameObject.SetActive(false);
        CurrentCanvas = nextCanvas;
        utility.startClickOnButtonSound();
        CurrentCanvas.gameObject.SetActive(true);
    }
    

    public void okButton(){
        // dissipates the fog and resume game
        utility.FogFadingOut();
        // deactivating the current canvas
        CurrentCanvas.gameObject.SetActive(false);

        if (CurrentCanvas == CarAccidentInfoCanvas2){
            // the first checkpoint is after the completion of the car accident info canvases
            PlayerPrefs.SetString("LastCanvas", CurrentCanvas.name);
            utility.checkpointAction();
            StartCoroutine(utility.resetInfo());
            // set the new canvas without activating it
            CurrentCanvas = DangerInfoCanvas0;
        }else if (CurrentCanvas == DangerInfoCanvas0){
            // the second checkpoint is after the completion of the canvas to take cindy in safezone
            PlayerPrefs.SetString("LastCanvas", CurrentCanvas.name);
            utility.checkpointAction();
            StartCoroutine(utility.resetInfo());
            // set the new canvas without activating it 
            CurrentCanvas = MotorcyclistCanvas0;
        }else if (CurrentCanvas == MotorcyclistCanvas0){
            // the third checkpoint is after taking cindy in the safezone
            PlayerPrefs.SetString("LastCanvas", CurrentCanvas.name);
            utility.checkpointAction();
            StartCoroutine(utility.resetInfo());
            // set the new canvas without activating it 
            CurrentCanvas = null;
        }
        first = true;
    }

    public void finishButton(){
        // informing the changeScene script if we have won or not
        utility.startClickOnButtonSound();
        Change.setHasWon(hasWon);
        Change.teleportToPauseRoom(true);
    }


}
