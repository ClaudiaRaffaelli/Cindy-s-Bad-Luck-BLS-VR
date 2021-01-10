using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class fourthLevelManager : MonoBehaviour
{
	public Canvas SizureInfoCanvas0;
	public Canvas SizureInfoCanvas1;
	public Canvas SizureInfoCanvas2;
	public Canvas CongratulationCanvas;
	public Canvas GameOverCanvas;
	public Canvas ActionCanvas;
	public Canvas WatchCanvas;
    public Canvas GrabLightsCanvas;

	public static Canvas CurrentCanvas;

	// we want to know the OVR player position
	public GameObject OVRPlayer;

    // the tutorial scene corresponds to the level 0
    public static int level = 4;
    private int currentUnlockedLevel;
    private string lastCanvas; 

    // the change scene script
    public changeScene Change;
    // it contains at the end of the game the value true or false depending on the fact that we won or lost the level
    private bool hasWon;

    // the NPC
    public NPC_logic npcLogicJack; 

    // list of pillows
    public List<patchWith> pillowArray;
    // list of wood 
    public List<GameObject> woodArray;
    public GameObject axe;
    // the collider box around cindy to detect if the objects are too close
    public Collider boxCollider;
    // to detect if cindy has been positioned in the safe position
    public GameObject cindy;
    // to start the seizure event
    public seizure seizureScript;
    // detect if the player has completed a phone call
    public phoneTrigger triggerPhone;
    // to update the time on the watch
    public TextMeshProUGUI myWatchTime;
    // detect if the player leaves the area (flee the scene)
    public playerColliding leaveCollides;
    // detect if the user grabs one of the two first lights on cindy
    public path pathLeg;
    public path pathShoulder;

    // variables that track the actions of the player during the level
    // we want to know if someone has called 112 and who [jack, player]
    bool [] hasCalled = new bool [] {false, false};
    // we want to know if jack has taken a few steps back or not
    bool isAway = false;
    // we want to know if the pillows are in the right position (below the head)
    bool pillowsInPosition = false;
    // we want to know if the woods and axe are being moved
    bool objectsMoved = false;
    // we check if cindy is on the side
    bool sideCindy = false;
    // we check if jack has helped cindy going to bed
    bool toBed = false;
    // we check if the user has already grabbed one of the two first lights on cindy
    bool hasGrabbedLights = false;
    // the time of the seizure
    int seizureTime;
    // the time of start of the seizure
    float startTime = 0f;

    // the penalty assigned to actions out of order or wrong actions. If penalty >=3 the game is over
    int penalty = 0;
    
    // utility script
    public utilityScript utility;


    // Start is called before the first frame update
    void Start()
    {	
    	// setting the fog
        RenderSettings.fog = true;

        System.Random rnd = new System.Random();
        // generate a random number between 3 and 7 (number of minutes of the seizure)
		seizureTime = rnd.Next(3, 8);

        // we retrieve the last unlocked level 
        currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");
        // we check if this level was paused, and in this case we resume from where we left
        lastCanvas = PlayerPrefs.GetString("LastCanvas");
        
        // if the game was not paused or it was paused before we reached the first checkpoint, we start from the beginning
        if (lastCanvas == ""){
            CurrentCanvas = SizureInfoCanvas0;
        }
        // otherwise we resume from where we left off
        else{
        	// the last canvas is SizureInfoCanvas3
            CurrentCanvas = null;
            // the seizure and time start (with *15 we are assuming that a minute lasts 15 seconds)
	        seizureScript.startSeizure(seizureTime*15);
	        startTime = Time.time;
	        // we show the canvas of information for the seizure
	        StartCoroutine(startWatchCanvas());
            StartCoroutine(utility.resetInfo());
        }
    }

    // Update is called once per frame
    void Update()
    {   
    	float currentTime = Time.time;
    	// if the canvas to be displayed is the first, and the player reached the kitchen, 
    	// we display the canvas and activate the fog effect
        if (CurrentCanvas == SizureInfoCanvas0){
        	// activate the canvas
        	CurrentCanvas.gameObject.SetActive(true);
        	// activate the fog effect
        	utility.FogFadingIn();

        }
        else if ((CurrentCanvas != GameOverCanvas) && (CurrentCanvas != CongratulationCanvas)){
        	// check that the actions are being made in the right order

        	// we check if jack has taken a few steps back is he was not already away
            if (isAway == false){
            	if (npcLogicJack.isAway() == true){
            		isAway = true;
            		utility.goodAction();
            		StartCoroutine(utility.resetInfo());
            	}
            }
            // we check if we moved the woods and axe away from cindy
            if(objectsMoved == false){
            	// if now the objects have been moved
            	if ((!boxCollider.bounds.Contains(woodArray[0].transform.position)) && 
            		(!boxCollider.bounds.Contains(woodArray[1].transform.position)) &&
            		(!boxCollider.bounds.Contains(axe.transform.position))){
            		objectsMoved = true;
            		if (isAway == true){
            			utility.goodAction();
            			StartCoroutine(utility.resetInfo());
            		}else{
            			utility.okAction(1);
            			penalty++;
            			StartCoroutine(utility.resetInfo());
            		}
            	}	
            }
            // we check if we moved the pillows in position
            if(pillowsInPosition == false){
            	if(pillowArray[0].getPositioned() && pillowArray[1].getPositioned() && pillowArray[2].getPositioned()){
            		pillowsInPosition = true;
            		if(objectsMoved == true){
            			utility.goodAction();
            			StartCoroutine(utility.resetInfo());
            		}else{
            			// cindy could get hurt if we don't move away the objects
            			utility.badAction(3);
            			penalty+=3;
            			StartCoroutine(utility.resetInfo());
            		}
            	}
            }
            // we check if we moved on the side cindy
            if (sideCindy == false){
            	// the last actions of moving cindy are completed
            	if (cindy.GetComponent<Animator>().GetBool("pull1") && cindy.GetComponent<Animator>().GetBool("pull2")){
            		sideCindy = true;
            		if (pillowsInPosition == true){
						utility.goodAction();
            			StartCoroutine(utility.resetInfo());
            		}else{
            			// not all pillows have been placed
            			utility.badAction(2);
            			penalty+=2;
            			// if the pillows are not positioned, we skipped the control on the objects moved and we check that too
            			if (objectsMoved == false){
            				penalty+=3;
            				utility.badAction(3);
            			}
            			StartCoroutine(utility.resetInfo());
            		}
            	}
            }
            
            // if nobody has called the ambulance we check if somebody did now
            if (!hasCalled.Contains(true)){
            	if (npcLogicJack.isCalling() == true){
            		hasCalled[0] = true;
            	}else if(triggerPhone.getatEar() == true){
            		hasCalled[1] = true;
            	}
            	// if someone made a call we check if it was necessary to make it (more than 5 minute seizure), and if it was the right time
            	if (hasCalled.Contains(true)){
            		if (sideCindy == true && seizureTime >=5){
						utility.goodAction();
            			StartCoroutine(utility.resetInfo());
            			hasWon = true;
            			// we activate the game over canvas to comunicate it to the player
		            	CurrentCanvas = CongratulationCanvas;
                        utility.startCongratulationSound();
		        		CurrentCanvas.gameObject.SetActive(true);
		            	utility.FogFadingIn();
            		}else{
            			utility.badAction(3);
            			penalty+=3;
            			StartCoroutine(utility.resetInfo());
            		}
            	}    	
            }
            // if jack has not helped cindy yet we check if it has now
            if (toBed == false){
                if (npcLogicJack.isToBed() == true){
                    toBed = true;
                    if(sideCindy == true && seizureTime <=5){
                        utility.goodAction();
                        StartCoroutine(utility.resetInfo());
                        hasWon = true;
                        // we activate the game over canvas to comunicate it to the player
                        CurrentCanvas = CongratulationCanvas;
                        utility.startCongratulationSound();
                        CurrentCanvas.gameObject.SetActive(true);
                        utility.FogFadingIn();
                    }else{
                        utility.badAction(3);
                        penalty+=3;
                        StartCoroutine(utility.resetInfo());
                    }
                }
            }

            // we update the time displayed on the canvas
            if ((int)(currentTime - startTime) <= seizureTime*15){
            	if ((int)(currentTime - startTime) == seizureTime*15){
            		myWatchTime.color = Color.green;
            	}
            	myWatchTime.text=((int)(currentTime - startTime)/15).ToString()+ " min";
            }

            // check if we flee the scene
            if (leaveCollides.hasPlayerCollided()== true){
                utility.displayText("You fleed!");
                penalty += 3;
                leaveCollides.setPlayerCollided(false);
                StartCoroutine(utility.resetInfo());
            }

            // show the instructions to move cindy when the user grabs one of the first lights
            if(hasGrabbedLights == false){
                if (pathLeg.isShowingGuideline() || pathShoulder.isShowingGuideline()){
                    hasGrabbedLights = true;
                    StartCoroutine(startGrabLightsCanvas());
                    StartCoroutine(utility.resetInfo());
                }
            }
            // the game is over if the penalty is too high 
            // (we check hasWon bacause maybe we have already won the game but the player walking, has left the scene, but we still have won)
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
        utility.startClickOnButtonSound();

        if (CurrentCanvas == SizureInfoCanvas2){
        	// the first checkpoint is after the completion of the bleeding info canvases
        	PlayerPrefs.SetString("LastCanvas", CurrentCanvas.name);
            utility.checkpointAction();
            StartCoroutine(utility.resetInfo());
        	// set the new canvas without activating it
        	CurrentCanvas = null;

        	// the seizure and time start (with *15 we are assuming that a minute lasts 15 seconds)
	        seizureScript.startSeizure(seizureTime*15);
	        startTime = Time.time;

	        StartCoroutine(startWatchCanvas());
            StartCoroutine(utility.resetInfo());
        }
    }

    public void finishButton(){
    	// informing the changeScene script if we have won or not
        utility.startClickOnButtonSound();
    	Change.setHasWon(hasWon);
    	Change.teleportToPauseRoom(true);
    }

    
    IEnumerator startWatchCanvas(){
    	WatchCanvas.gameObject.SetActive(true);
    	yield return new WaitForSecondsRealtime(10);
    	WatchCanvas.gameObject.SetActive(false);
    }

    IEnumerator startGrabLightsCanvas(){
        utility.FogFadingIn();
        GrabLightsCanvas.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(10);
        GrabLightsCanvas.gameObject.SetActive(false);
        utility.FogFadingOut();
    }

}
