using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class firstLevelManager : MonoBehaviour
{	

	public Canvas BurnInfoCanvas0;
	public Canvas BurnInfoCanvas1;
	public Canvas BurnInfoCanvas2;
	public Canvas BurnInfoCanvas3;
	public Canvas PhoneInfoCanvas0;
	public Canvas CongratulationCanvas;
	public Canvas GameOverCanvas;
	public Canvas ActionCanvas;

	private Canvas CurrentCanvas;

	// we want to know the OVR player position
	public GameObject OVRPlayer;

    // the tutorial scene corresponds to the level 0
    public static int level = 1;
    private int currentUnlockedLevel;
    private string lastCanvas; 

    // it contains at the end of the game the value true or false depending on the fact that we won or lost the level
    private bool hasWon;

    // the change scene script
    public changeScene Change;

    // detect if the player has completed a phone call
    public phoneManager phone;
    // detect if the bottle is colliding with the legCollider
    public bottleCollisionHandler bottleCollision;
    // detect if the bottle is pouring
    public pourLiquid bottlePouringScript;
    // detect if the pan is in the sink
    public patchWith sinkPan;
    // detect if the patch on cindy's leg is applied
    public patchWith legPatch;
    // detect if the blanked is applied
    public patchWith blanketApplied;
    // detect if the stove is turned on
    public switchParticlaSystem stove;
    // fire on pan
    public GameObject fire;

    // detect if the player collided with the fire on the pan
    public playerColliding panFireColliding;
    // detect if the player collided with the fire on the stove
    public playerColliding stoveFireColliding;

    // variables that track the actions of the player during the level
    // we want to know if the player has called
    bool hasCalled = false;
    // we want to know if leg has been cleaned
    bool hasCleaned = false;
    // we want to know if the stove is turned off
    bool hasTurnedOff = false;
    // we want to know if there is a patch on Cindy's leg
    bool hasCoveredLeg = false;
    // we want to know if there is a blanket on Cindy
    bool hasCoveredCindy = false;
    // we want to know if the pan is in the sink
    bool hasSecuredPan = false;
    // we want to know when the player has completed the two canvas of phonecall
    bool hasWhatToDoCall = false;
    bool hasWhenToCall = false;

    // total time of pouring and last time we poured
    float totalTime = 0.0f;
    float lastTime = 0.0f;

    // the penalty assigned to actions out of order or wrong actions. If penalty >=3 the game is over
    int penalty = 0;
    int index = 1;

    // if we saw the phone canvas already
    bool canvasDone = false;

    // utility script
    public utilityScript utility;

    // Start is called before the first frame update
    void Start()
    {	
    	// set the fog
        RenderSettings.fog = true;

        // we retrieve the last unlocked level 
        currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");
        // we check if this level was paused, and in this case we resume from where we left
        lastCanvas = PlayerPrefs.GetString("LastCanvas");
        // if the game was not paused or it was paused before we reached the first checkpoint, we start from the beginning
        if (lastCanvas == ""){
            CurrentCanvas = BurnInfoCanvas0;
        }
        // otherwise we resume from where we left off
        else{
            switch(lastCanvas){
                case "BurnInfoCanvas3":
                    CurrentCanvas = PhoneInfoCanvas0;
                    break;
                case "PhoneInfoCanvas0":
                	CurrentCanvas = null;
                    // placing the player where she / he left off
                    OVRPlayer.transform.position = new Vector3(13.80f, 1.9f, -19.3f);
                    OVRPlayer.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

                    stove.turnOff();
                    Destroy(sinkPan.gameObject);
                    Destroy(legPatch.gameObject);
                    Destroy(blanketApplied.gameObject);

                    hasCleaned = true;
                    hasTurnedOff = true;
                    hasCoveredLeg = true;
                    hasCoveredCindy = true;
                    hasSecuredPan = true;
                    canvasDone = true;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   
    	if ((CurrentCanvas != GameOverCanvas) && (CurrentCanvas != CongratulationCanvas)){
	    	float currentTime = Time.time;
	    	// if the canvas to be displayed is the first we display the canvas and activate the fog effect
	        if (CurrentCanvas == BurnInfoCanvas0){
	        	// activate the canvas
	        	CurrentCanvas.gameObject.SetActive(true);
	        	// activate the fog effect
	        	utility.FogFadingIn();
	        }

	        // if the pan is not in the sink we check if it is now
	        if (hasSecuredPan == false){
	            if (sinkPan.getPositioned() == true){
	                hasSecuredPan = true;
	                utility.goodAction();
	                StartCoroutine(utility.resetInfo());
	            }
	        }
	        // stove turned off
	        if (hasTurnedOff == false){
	            if(stove.isTurnedOn() == false){
	                hasTurnedOff = true;
	                // check previous action
	                if (hasSecuredPan == true){
	                    utility.goodAction();
	                    StartCoroutine(utility.resetInfo());
	                }else{
	                    utility.okAction(1);
	                    penalty++;
	                    StartCoroutine(utility.resetInfo());
	                }
	            }
	        }
	        // clean cindy's leg
	        if(hasCleaned == false){
	            // check if the bottle is above cindy's leg and is pouring liquid
	            if (bottleCollision.isBottleColliding() && bottlePouringScript.isBottlePouring()){
	                // the player is cleaning the leg
	                if (lastTime!= 0.0f){
	                    totalTime += currentTime - lastTime;
	                    lastTime = currentTime;
	                }else{
	                    lastTime = currentTime;
	                }
	                // we update the number displayed to the user each second
	                utility.infoText.color = Color.green;
	                utility.infoText.text = ((int)(totalTime)).ToString();
	                utility.infoText.CrossFadeAlpha(1,0.2f,false);
	            }else{
	                lastTime = 0.0f;
	            }
	            
	            // after 10 seconds the leg is cleaned
	            if (totalTime >=10.0f){
	                hasCleaned = true;
	                utility.infoText.CrossFadeAlpha(0,0.2f,false);
	                // this two actions are important
	                if (hasSecuredPan && hasTurnedOff){
	                    utility.goodAction();
	                    StartCoroutine(utility.resetInfo());
	                }
	                else{
	                    utility.badAction(3);
	                    penalty+=3;
	                    StartCoroutine(utility.resetInfo());
	                }
	            }
	        }

	        if(hasCoveredLeg == false){
	            if (legPatch.getPositioned()){
	                hasCoveredLeg = true;
	                if (hasCleaned == true){
	                    utility.goodAction();
	                    StartCoroutine(utility.resetInfo());
	                }
	                else{
	                    utility.badAction(3);
	                    penalty+=3;
	                    StartCoroutine(utility.resetInfo());
	                }
	            }
	        }

	        if (hasCoveredCindy == false){
	            if (blanketApplied.getPositioned()){
	                hasCoveredCindy = true;
	                if (hasCoveredLeg == true){
	                    utility.goodAction();
	                    StartCoroutine(utility.resetInfo());
	                }
	                else{
	                    utility.badAction(3);
	                    penalty+=3;
	                    StartCoroutine(utility.resetInfo());
	                }
	            }
	        }else if (hasCoveredCindy && (OVRPlayer.transform.position.x >12.0f) && canvasDone == false){
	            // activate the canvas
	            CurrentCanvas.gameObject.SetActive(true);
	            canvasDone =  true;
	            // activate the fog effect
	            utility.FogFadingIn();
	        }  

	        // the null canvas (we already saw the phone canvas). Now we have to check that the phone call is done
	        if (hasWhenToCall == false){
	            if (phone.getCorrectPhoneCall()){
	                hasWhenToCall = true;
	                if (hasCoveredCindy == true){
	                    utility.goodAction();
	                    StartCoroutine(utility.resetInfo());
	                }else{
	                    utility.badAction(3);
	                    penalty+=3;
	                    StartCoroutine(utility.resetInfo());
	                }               
	            }
	        }
	        if (hasWhatToDoCall == false){
	            if (phone.getCorrectWhatToDoCall()){
	                hasWhatToDoCall = true;
	                // we won
	                hasWon = true;  
	                CurrentCanvas = CongratulationCanvas;
	                utility.startCongratulationSound();
	                CurrentCanvas.gameObject.SetActive(true);
	                utility.FogFadingIn();    
	            }
	        }

	        // check if we collide with the fire
	        if (panFireColliding.hasPlayerCollided()== true || stoveFireColliding.hasPlayerCollided()== true){
	        	utility.displayText("You caught fire!");
		        penalty += 3;
		        panFireColliding.setPlayerCollided(false);
		        StartCoroutine(utility.resetInfo());
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
	    }else{
	    	// if we won or lost (the GameOverCanvas or Congratulation canvas is shown) we want to block the player movements
	    	OVRPlayer.GetComponent<OVRPlayerController>().movSpeed = 0f;
            OVRPlayer.GetComponent<OVRPlayerController>().Acceleration = 0f;
            OVRPlayer.GetComponent<OVRPlayerController>().Damping = 0f;
	    }
    }

    public void okButton(){
    	// dissipates the fog and resume game
    	utility.FogFadingOut();
		// deactivating the current canvas
    	CurrentCanvas.gameObject.SetActive(false);
    	utility.startClickOnButtonSound();

        if (CurrentCanvas == BurnInfoCanvas3){
        	// the first checkpoint is after the completion of the burn info canvases
        	PlayerPrefs.SetString("LastCanvas", CurrentCanvas.name);
            utility.checkpointAction();
            StartCoroutine(utility.resetInfo());
        	// set the new canvas without activating it
        	CurrentCanvas = PhoneInfoCanvas0;
        }else if (CurrentCanvas == PhoneInfoCanvas0){
        	// the second checkpoint is after the completion of the phone info canvases
        	PlayerPrefs.SetString("LastCanvas", CurrentCanvas.name);
            utility.checkpointAction();
            StartCoroutine(utility.resetInfo());
        	// set the new canvas without activating it
        	CurrentCanvas = null;
        }
    }

    public void nextCanvas(Canvas nextCanvas){
    	CurrentCanvas.gameObject.SetActive(false);
        CurrentCanvas = nextCanvas;
        utility.startClickOnButtonSound();
        CurrentCanvas.gameObject.SetActive(true);
    }

    public void finishButton(){
    	// informing the changeScene script if we have won or not
    	utility.startClickOnButtonSound();
    	Change.setHasWon(hasWon);
    	Change.teleportToPauseRoom(true);
    }
}
