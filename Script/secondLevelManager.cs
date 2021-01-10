using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class secondLevelManager : MonoBehaviour
{
	public Canvas HemorrhageInfoCanvas0;
	public Canvas HemorrhageInfoCanvas1;
	public Canvas HemorrhageInfoCanvas2;
	public Canvas HemorrhageInfoCanvas3;
	public Canvas AntishockInfoCanvas0;
	public Canvas AntishockInfoCanvas1;
	public Canvas AntishockInfoCanvas2;
	public Canvas CongratulationCanvas;
	public Canvas GameOverCanvas;
	public Canvas ActionCanvas;
	public Canvas CountdownCanvas;

	public static Canvas CurrentCanvas;

	// we want to know the OVR player position
	public GameObject OVRPlayer;

    // the tutorial scene corresponds to the level 0
    public static int level = 2;
    private int currentUnlockedLevel;
    private string lastCanvas; 

    // the change scene script
    public changeScene Change;
    // it contains at the end of the game the value true or false depending on the fact that we won or lost the level
    private bool hasWon = false;

    // the NPC
    public GameObject jackNPC;
    public GameObject emilyNPC;
    public NPC_logic npcLogicEmily;
    public NPC_logic npcLogicJack; 
    // emily audio
    AudioSource emilyHelp;

    // list of pillow and patch objects
    public List<GameObject> pillowArray;
    public mantainPatch myMantainPatch;

    // ambulance sound for when the rescurers arrive
    public AudioSource ambulanceSound;

    // detect if the player has completed a phone call
    public phoneTrigger triggerPhone;

    // variables that track the actions of the player during the level
    // we want to know if someone has called 112 and who [jack, emily, player]
    bool [] hasCalled = new bool [] {false, false, false};
    bool bad = false;
    // we want to know emily is calm
    bool isCalm = false;
    // we want to know if emily has taken a few steps back or not
    bool isAway = false;
    // we want to know if there is at least a pillow in the right position (below the legs)
    bool pillowInPosition = false;
    // we want to know if Jack has pulled up Cindy's legs
    bool upLegs = false;
    // we want to know when the first patch is applied
    bool thereIsFirstPatch = false;
    // after 20 seconds from the first patch application the rescue will come, 
    // totalElapsedTime holds the remaining time before the arrive of the rescurers
    float totalElapsedTime = 25f;
    // totalMantainedTime, holds the time in which we mantained the patch applied
    float totalMantainedTime = 0f;

    // the penalty assigned to actions out of order or wrong actions. If penalty >=3 the game is over
    int penalty = 0;

    bool first = true;

    // utility script
    public utilityScript utility;

    // Start is called before the first frame update
    void Start()
    {	
    	// setting the fog
        RenderSettings.fog = true;

        // we get Emily audio
        emilyHelp = emilyNPC.GetComponent<AudioSource>();

        // we retrieve the last unlocked level 
        currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");
        // we check if this level was paused, and in this case we resume from where we left
        lastCanvas = PlayerPrefs.GetString("LastCanvas");
        // if the game was not paused or it was paused before we reached the first checkpoint, we start from the beginning
        if (lastCanvas == ""){
            CurrentCanvas = HemorrhageInfoCanvas0;
        }
        // otherwise we resume from where we left off
        else{
            switch(lastCanvas){
                case "HemorrhageInfoCanvas3":
                    CurrentCanvas = AntishockInfoCanvas0;
                    // placing the player where she / he left off
                    OVRPlayer.transform.position = new Vector3(16.70f, 1.9f, -7.74f);
                    break;
                case "AntishockInfoCanvas2":
                	CurrentCanvas = ActionCanvas;
                    // placing the player where she / he left off
                    OVRPlayer.transform.position = new Vector3(10.60f, 1.9f, -2.0f);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   

        if ((CurrentCanvas != GameOverCanvas) && (CurrentCanvas != CongratulationCanvas)){
        	float currentTime = Time.time;
        	// if the canvas to be displayed is the first, and the player reached the kitchen, 
        	// we display the canvas and activate the fog effect
            if ((CurrentCanvas == HemorrhageInfoCanvas0) && (OVRPlayer.transform.position.y < 3.5f )
            	&& (OVRPlayer.transform.position.z >-9.5f) && (first == true)){
            	// activate the canvas
            	CurrentCanvas.gameObject.SetActive(true);
            	// activate the fog effect
                utility.FogFadingIn();
            	// we pause Emily audio
            	emilyHelp.Pause();
            	first = false;
            }
            // if the canvas to be displayed is the first of the antishock position, and the player reached the living room, 
        	// we display the canvas and activate the fog effect
            if ((CurrentCanvas == AntishockInfoCanvas0) && (OVRPlayer.transform.position.x < 12.9f ) && (OVRPlayer.transform.position.y < 3.5f )
            	&& (OVRPlayer.transform.position.z >-8.0f) && (first == true)){
            	// activate the canvas
            	CurrentCanvas.gameObject.SetActive(true);
            	// activate the fog effect
                utility.FogFadingIn();
            	// we pause Emily audio
            	emilyHelp.Pause();
            	first = false;
            }
            // Here we check if the actions are made, and if are made in the right ordering. Some mistakes are allowed. 
            // Depending on the severity of the mistakes, different penalties are assigned.
            // The maximum penalty allowed is 2, above this value is Game Over.
            
            // if Emily was not calm, we check if she is now
            if (isCalm == false){
                if (npcLogicEmily.isCalm() == true){
                    isCalm=true;
                    utility.goodAction();
                    StartCoroutine(utility.resetInfo());
                }
            }
            
            // we also check if emily has taken a few steps back is she was not already away
            if (isAway == false){
                if (npcLogicEmily.isAway() == true){
                    isAway = true;
                    utility.goodAction();
                    StartCoroutine(utility.resetInfo());
                }
            }


            
            // if nobody has called, we check if someone has now
            if (hasCalled.Contains(true)!=true){
                if(npcLogicEmily.isCalling() == true){
                    hasCalled[1]=true;
                }else if(npcLogicJack.isCalling() == true){
                    hasCalled[0]=true;
                }else if(triggerPhone.getatEar() == true){
                    hasCalled[2]=true;
                    // if the player has called we need to give a penalty, because since there are other people around, they
                    // should make the call
                    penalty +=2;
                }

                if(hasCalled.Contains(true)){
                    // if emily is not calm nor emily has taken a few steps back, we increment the penalty, 
                    //because we have not respected the right ordering of actions
                    if (isCalm == false && isAway == false){
                        penalty +=2;
                        utility.badAction(2);
                        StartCoroutine(utility.resetInfo());
                    }else if(isCalm == false){
                        penalty ++;
                        utility.okAction(1);
                        StartCoroutine(utility.resetInfo());
                    }else if (isAway == false){
                        penalty ++;
                        utility.okAction(1);
                        StartCoroutine(utility.resetInfo());
                    }else{
                        if (hasCalled[2] == false){
                            // the player made no mistakes
                            utility.goodAction();
                            StartCoroutine(utility.resetInfo());
                        }else{
                            // tha player called on his own
                            utility.badAction(2);
                            StartCoroutine(utility.resetInfo());
                        }    
                    }
                }
            }else if (bad == false){
                // if jack has called we check that nobody else has too, otherwise is a bad error -> gameover
                if(hasCalled[0]==true){
                    if((npcLogicEmily.isCalling() == true) || (triggerPhone.getatEar() == true)){
                        bad = true;
                    }
                }else if(hasCalled[1]==true){
                    if((npcLogicJack.isCalling() == true) || (triggerPhone.getatEar() == true)){
                        bad = true;
                    }
                }else if(hasCalled[2]==true){
                    if((npcLogicEmily.isCalling() == true) || (npcLogicJack.isCalling() == true)){
                        bad = true;
                    }
                }
                // if someone else has called, it's an error
                if(bad == true){
                    penalty +=3;
                    utility.badAction(3);
                    StartCoroutine(utility.resetInfo());
                }
            }
               
            // we check if Jack has pulled Cindy's legs up or if we have positioned at least a pillow under the legs
            if ((upLegs == false) && (pillowInPosition == false)){
                // first we check if Jack has bulled Cindy's legs up
                if (npcLogicJack.hasPulledLegs() == true){
                    upLegs = true;
                }
                // we check if at least a pillow has been released under the legs region
                foreach (GameObject pillow in pillowArray){
                    if ((pillow.transform.position.x<=11.55) && (pillow.transform.position.x>=10.4)
                        && (pillow.transform.position.z<=-3.75) && (pillow.transform.position.z >=-4.8)
                        && (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger))==0){
                        
                        pillowInPosition = true;
                    }
                }
                // if there have been changes we check if the previous actions were already been made, if not we have to set a penalty
                if (upLegs == true || pillowInPosition == true){
                    if (hasCalled.Contains(true)!=true){
                        // calling 112 was an important action to be made
                        penalty +=3;
                        utility.badAction(3);
                        StartCoroutine(utility.resetInfo());
                    }else{
                        utility.goodAction();
                        StartCoroutine(utility.resetInfo());
                    }
                }
            }

            // we check if the first patch is applied if it wasn't already
            if (thereIsFirstPatch == false){
                if (myMantainPatch.getPositioned() == true){
                    thereIsFirstPatch = true;

                    // we check if the previous actions were already been made, if not we have to set a penalty
                    if (upLegs == false && pillowInPosition == false){
                        // even raising the legs was an important action
                        penalty +=2;
                        if (hasCalled.Contains(true)!=true){
                            // calling 112 was an important action to be made
                            penalty +=3;
                            utility.badAction(3);
                            StartCoroutine(utility.resetInfo());
                        }else{
                            utility.badAction(2);
                            StartCoroutine(utility.resetInfo());
                        }
                    }else{
                        utility.goodAction();
                        StartCoroutine(utility.resetInfo());
                    }

                    if (penalty <3){
                        // we display the text on the canvas, telling the user of the countdown
                        StartCoroutine(startCountdown());
                        StartCoroutine(utility.resetInfo());

                        // the rescurers are on their way, and we turn up the sirens
                        ambulanceSound.Play();
                    }
                }
            }
            // show the canvas if the countdown has started
            if (thereIsFirstPatch == true){
                // updating the time
                totalElapsedTime -= Time.deltaTime;
                if((int)(totalElapsedTime) <=20){
                    if ((int)(totalElapsedTime)>=0){
                        utility.infoText.text = ((int)(totalElapsedTime)).ToString();
                        utility.infoText.CrossFadeAlpha(1,0.2f,false);
                        if (myMantainPatch.getPositioned() == true){
                            // the patch is currently applied
                            utility.infoText.color = Color.green;
                            totalMantainedTime += Time.deltaTime;
                        }else{
                            // the patch has been removed and still not re-applied yet
                            utility.infoText.color = Color.red;
                        }
                    }else{
                        // the patch is still applied at the moment of end of countdown and it has been applied for more then 10 sec
                        if (myMantainPatch.getPositioned() == true && hasWon == false && totalMantainedTime>=10f){
                            hasWon = true;
                            // we activate the game over canvas to comunicate it to the player
                            CurrentCanvas = CongratulationCanvas;
                            utility.startCongratulationSound();
                            CurrentCanvas.gameObject.SetActive(true);
                            utility.FogFadingIn();
                        }else{
                            // we lost
                            penalty +=3;
                        }       
                    }  
                }
            }

            // the game is over if the penalty is too high (we check if it is != true because if the player has already won the other wrong
            // actions are ignored)
            if (penalty >=3 && hasWon !=true){
                hasWon = false; // even though hasWon is already false by default
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

        if (CurrentCanvas == HemorrhageInfoCanvas3){
        	// the first checkpoint is after the completion of the bleeding info canvases
        	PlayerPrefs.SetString("LastCanvas", CurrentCanvas.name);
            utility.checkpointAction();
            StartCoroutine(utility.resetInfo());
        	// we play once again the audio of Emily
        	emilyHelp.Play(0);
        	// set the new canvas without activating it
        	CurrentCanvas = AntishockInfoCanvas0;
        }
        if (CurrentCanvas == AntishockInfoCanvas2){
        	// the second checkpoint is after the completion of the antishock canvases
        	PlayerPrefs.SetString("LastCanvas", CurrentCanvas.name);
            utility.checkpointAction();
            StartCoroutine(utility.resetInfo());
        	// we play once again the audio of Emily
        	emilyHelp.Play(0);
        	// set the new canvas without activating it 
        	CurrentCanvas = ActionCanvas;
        }
        first = true;
    }

    public void finishButton(){
    	// informing the changeScene script if we have won or not
        utility.startClickOnButtonSound();
    	Change.setHasWon(hasWon);
    	Change.teleportToPauseRoom(true);
    }

    IEnumerator startCountdown(){
    	CountdownCanvas.gameObject.SetActive(true);
    	yield return new WaitForSecondsRealtime(5);
    	CountdownCanvas.gameObject.SetActive(false);
    }


}
