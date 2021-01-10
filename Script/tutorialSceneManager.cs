using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class tutorialSceneManager : MonoBehaviour
{
	public Canvas WelcomeCanvas;
	public Canvas HelpCanvas0;
	public Canvas HelpCanvas1;
	public Canvas HelpCanvas2;
	public Canvas PhoneCanvas0;
	public Canvas PhoneCanvas1;
    public Canvas PhoneCanvas2;
    public Canvas PhoneCanvas3;
    public Canvas PhoneCanvas4;
    public Canvas NPCCanvas0;
    public Canvas NPCCanvas1;
    public Canvas NPCCanvas2;
    public Canvas NPCCanvas3;
    public Canvas MoveObjectsCanvas0;
    public Canvas MoveObjectsCanvas1;
    public Canvas MoveObjectsCanvas2;
    public Canvas SymbolsCanvas0;
    public Canvas SymbolsCanvas1;
    public Canvas FinishCanvas;
	public static Canvas CurrentCanvas;

    public utilityScript utility;

	public GameObject Instructions;
	public GameObject Phone;
	public GameObject PhoneManager;
	public GameObject PhoneTable;
    public GameObject BreadTable;
    public GameObject Bread;
    public GameObject BreadSafezone;

    // the tutorial scene corresponds to the level 0
    public static int level = 0;
    public static bool isCompleted = false;
    public int currentUnlockedLevel;
    public changeScene changeSceneScript;
    string lastCanvas; 

    // to detect when we grab the phone
    bool grabbed;
    public smartphone phoneScript;
    public GameObject rightHand;

    // detect when we place the phone near the ear
    bool atEar;
    public phoneTrigger triggerPhone;

    // detect if the player has completed the phone call
    public phoneManager phoneManagerScript;
    bool correctPhoneCall;

    // to detect if the player has picked up the bread
    public pickUpObject pickUpScript;
    // to detect it the bread is placed
    public patchWith placedScript;

    // in order to detect when the player clicks on the npc
    public showNpcInteractions npcInteractions;
    public GameObject jackNPC;
    bool visibility = true;
    bool first1 = true;
    bool first2 = true;
    bool first3 = true;
    bool first4 = true;

    public ParticleSystem tableConfetti;
    public ParticleSystem jackConfetti;
    public ParticleSystem breadTableConfetti;

    // to detect if the player clicks on an npc interaction
    public NPC_logic npcLogic;

    // Start is called before the first frame update
    void Start()
    {
        // we retrieve the last unlocked level 
        currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");
        // we check if this level was paused, and in this case we resume from where we left
        lastCanvas = PlayerPrefs.GetString("LastCanvas");
        // if the game was not paused we start from the beginning
        if (lastCanvas == ""){
            CurrentCanvas = WelcomeCanvas;
        }
        // otherwise we resume from where we left off
        else{
            switch(lastCanvas){
                case "WelcomeCanvas":
                    CurrentCanvas = WelcomeCanvas;
                    break;
                case "HelpCanvas0":
                    CurrentCanvas = HelpCanvas0;
                    break;
                case "HelpCanvas1":
                    CurrentCanvas = HelpCanvas1;
                    break;
                case "HelpCanvas2":
                    CurrentCanvas = HelpCanvas2;
                    break;
                case "PhoneCanvas0":
                    CurrentCanvas = PhoneCanvas0;
                    Phone.gameObject.SetActive(false);
        			PhoneManager.gameObject.SetActive(false);
					jackNPC.gameObject.SetActive(false);
                    break;
                case "PhoneCanvas1":
                    CurrentCanvas = PhoneCanvas1;
                    Instructions.gameObject.SetActive(false);
					PhoneTable.gameObject.SetActive(true);
            		jackNPC.gameObject.SetActive(false);
                    break;
                case "PhoneCanvas2":
                    CurrentCanvas = PhoneCanvas2;
                    Instructions.gameObject.SetActive(false);
		            Phone.gameObject.SetActive(true);
		            PhoneManager.gameObject.SetActive(true);
					jackNPC.gameObject.SetActive(false);
                    // placing the smartphone in hand
                    Phone.transform.parent = rightHand.transform;
                    Phone.transform.position = rightHand.transform.position;
                    Phone.transform.rotation = rightHand.transform.rotation;
                    grabbed = true;
                    break;
                case "PhoneCanvas3":
                    CurrentCanvas = PhoneCanvas3;
                    Instructions.gameObject.SetActive(false);
		            Phone.gameObject.SetActive(true);
		            PhoneManager.gameObject.SetActive(true);
		            PhoneTable.gameObject.SetActive(false);
		            jackNPC.gameObject.SetActive(false);
                    // putting the phone in the pocket 
                    phoneScript.putInPocket();
                    // Opening the phone call canvas while giving the time for the other script to properly initialize
                    Invoke("delayTriggerCall", 1.0f);
                    break;
                case "PhoneCanvas4":
                    CurrentCanvas = PhoneCanvas4;
                    Instructions.gameObject.SetActive(false);
		            Phone.gameObject.SetActive(true);
		            PhoneManager.gameObject.SetActive(true);
		            PhoneTable.gameObject.SetActive(false);
		            jackNPC.gameObject.SetActive(false);
                    // putting the phone in the pocket 
                    phoneScript.putInPocket();
                    break;
                case "NPCCanvas0":
                    CurrentCanvas = NPCCanvas0;
                    Instructions.gameObject.SetActive(false);
					PhoneTable.gameObject.SetActive(false);
                    break;
                case "NPCCanvas1":
                	CurrentCanvas = NPCCanvas1;
                	Instructions.gameObject.SetActive(false);
            		Phone.gameObject.SetActive(false);
		            PhoneManager.gameObject.SetActive(false);
		            PhoneTable.gameObject.SetActive(false);
		            jackNPC.gameObject.SetActive(true);
		            // setting the proper visibility on the canvas of interactions
                	delayCanvas();
                	break;
                case "NPCCanvas2":
                	CurrentCanvas = NPCCanvas2;
                	Instructions.gameObject.SetActive(false);
		            Phone.gameObject.SetActive(false);
		            PhoneManager.gameObject.SetActive(false);
		            PhoneTable.gameObject.SetActive(false);
		            jackNPC.gameObject.SetActive(true);
                	// opening the interactions canvas while giving the time for the other script to properly initialize
                	Invoke("delayShowNPCInteractions", 1.0f);
                	break;
                case "NPCCanvas3":
                	CurrentCanvas = NPCCanvas3;
                	Instructions.gameObject.SetActive(false);
            		Phone.gameObject.SetActive(false);
            		PhoneManager.gameObject.SetActive(false);
            		PhoneTable.gameObject.SetActive(false);
            		jackNPC.gameObject.SetActive(true);
 					// close the interactions canvas while giving the time for the other script to properly initialize
                	Invoke("delayShowNPCInteractions", 1.0f);
                	// starting the phone call while giving the time for the other script to properly initialize
                	Invoke("delayStartPhoneCall", 1.0f);
                	break;
                case "MoveObjectsCanvas0":
                    CurrentCanvas = MoveObjectsCanvas0;
                    Instructions.gameObject.SetActive(false);
                    Phone.gameObject.SetActive(false);
                    PhoneManager.gameObject.SetActive(false);
                    jackNPC.gameObject.SetActive(false);
                    break;
                case "MoveObjectsCanvas1":
                    CurrentCanvas = MoveObjectsCanvas1;
                    Instructions.gameObject.SetActive(false);
                    Phone.gameObject.SetActive(false);
                    PhoneManager.gameObject.SetActive(false);
                    BreadTable.gameObject.SetActive(true);
                    PhoneTable.gameObject.SetActive(true);
                    jackNPC.gameObject.SetActive(false);
                    Bread.gameObject.SetActive(true);
                    BreadSafezone.gameObject.SetActive(true);
                    break;
                case "MoveObjectsCanvas2":
                    CurrentCanvas = MoveObjectsCanvas2;
                    Instructions.gameObject.SetActive(false);
                    Phone.gameObject.SetActive(false);
                    PhoneManager.gameObject.SetActive(false);
                    BreadTable.gameObject.SetActive(true);
                    PhoneTable.gameObject.SetActive(true);
                    jackNPC.gameObject.SetActive(false);
                    //Bread.gameObject.SetActive(true);
                    BreadSafezone.gameObject.SetActive(true);

		            // Change the material of the safezone bread
		            // In case of multiple materials: assign 'patchMaterial' to all of them 
		            Material[] matArray = placedScript.patch.GetComponent<MeshRenderer>().materials;
		            for (int i = 0; i < matArray.Length; i++)
		            {
		                matArray[i] = placedScript.patchMaterial;
		            }
		            placedScript.patch.GetComponent<MeshRenderer>().materials = matArray;


                    break;
                case "SymbolsCanvas0":
                    CurrentCanvas = SymbolsCanvas0;
                    Instructions.gameObject.SetActive(false);
                    Phone.gameObject.SetActive(false);
                    PhoneManager.gameObject.SetActive(false);
                    PhoneTable.gameObject.SetActive(false);
                    break;
                case "SymbolsCanvas1":
                    CurrentCanvas = SymbolsCanvas1;
                    Instructions.gameObject.SetActive(false);
                    Phone.gameObject.SetActive(false);
                    PhoneManager.gameObject.SetActive(false);
                    PhoneTable.gameObject.SetActive(false);
                    break;
                case "FinishCanvas":
                	CurrentCanvas = FinishCanvas;
                	Instructions.gameObject.SetActive(false);
		            Phone.gameObject.SetActive(false);
		            PhoneManager.gameObject.SetActive(false);
		            PhoneTable.gameObject.SetActive(false);
                	break;
            }
        }
        CurrentCanvas.gameObject.SetActive(true);
    }

    void delayTriggerCall()
    {
        triggerPhone.triggerCall();
    }

   	void delayShowNPCInteractions(){
   		// making the interactions canvas visible
   		npcInteractions.toggleOptions();
   	}

   	void delayStartPhoneCall(){
   		npcLogic.phoneCall();
   	}

   	void delayCanvas(){
   		if (npcInteractions.getVisibility() == false){
   			npcInteractions.toggleOptions();
   		}
   	}

    // Update is called once per frame
    void Update()
    {   
        if (CurrentCanvas == HelpCanvas0 || CurrentCanvas == HelpCanvas1 || CurrentCanvas == HelpCanvas2){
        	Instructions.gameObject.SetActive(true);
        	Phone.gameObject.SetActive(false);
        	PhoneManager.gameObject.SetActive(false);
            PhoneTable.gameObject.SetActive(false);
            jackNPC.gameObject.SetActive(false);
        }
        else if (CurrentCanvas == PhoneCanvas0){
        	Instructions.gameObject.SetActive(false);
            PhoneTable.gameObject.SetActive(true);
            if (first1 == true){
				tableConfetti.Play(true);
				first1 = false;
            }       
        }
        else if (CurrentCanvas == PhoneCanvas1){
            // activating the objects needed with this canvas
        	Phone.gameObject.SetActive(true);
        	if (first2 == true){
				tableConfetti.Play(true);
				first2 = false;
        	}
        	PhoneManager.gameObject.SetActive(true);
            
            // detect when we grab the phone
            grabbed = phoneScript.getGrabbed();
            if (grabbed == true){
                nextCanvas(PhoneCanvas2);
            }
        }
        else if (CurrentCanvas == PhoneCanvas2){        
            PhoneTable.gameObject.SetActive(false);
            // detect when we place the phone near the ear
            atEar = triggerPhone.getatEar();
            if (atEar == true){
                phoneScript.putInPocket();
                nextCanvas(PhoneCanvas3);
            }
        }
        else if (CurrentCanvas == PhoneCanvas3){

            // get confirm that the phone call is completed
            correctPhoneCall = phoneManagerScript.getCorrectPhoneCall();
            if (correctPhoneCall == true){
                nextCanvas(PhoneCanvas4);
            }
        }
        else if (CurrentCanvas == PhoneCanvas4){
        	// do nothing and waits for the Next push button
        }
        else if (CurrentCanvas == NPCCanvas0){
            Phone.gameObject.SetActive(false);
            PhoneManager.gameObject.SetActive(false);
            jackNPC.gameObject.SetActive(true);
            if (first3 == true){
            	jackConfetti.Play(true);
            	first3 = false;
            }
           
        }
        else if (CurrentCanvas == NPCCanvas1){
        	// detect when the user clicks on the npc and set the visibility of the canvas of interactions to false (is visible)
     		visibility = npcInteractions.getVisibility();
            if (visibility == false){
            	nextCanvas(NPCCanvas2);
            }
        }
        else if (CurrentCanvas == NPCCanvas2){
            // detect when the player 
            if (npcLogic.isCalling() == true){
            	nextCanvas(NPCCanvas3);
            }
        }
        else if (CurrentCanvas == NPCCanvas3){
           // do nothing and waits for the Next push button
        }
        else if (CurrentCanvas == MoveObjectsCanvas0){
            // display the two tables and the bread
            jackNPC.gameObject.SetActive(false);
            PhoneTable.gameObject.SetActive(true);
            BreadTable.gameObject.SetActive(true);
            Bread.gameObject.SetActive(true);
            BreadSafezone.gameObject.SetActive(true);
            if (first4 == true){
                tableConfetti.Play(true);
                breadTableConfetti.Play(true);
                first4 = false;
            }
            if (pickUpScript.isPickedUp() == true){
                nextCanvas(MoveObjectsCanvas1);
            }
        }
        else if (CurrentCanvas == MoveObjectsCanvas1){
            if (placedScript.getPositioned() == true){
                nextCanvas(MoveObjectsCanvas2);
            }
        }
        else if (CurrentCanvas == MoveObjectsCanvas2){
            // do nothing and waits for the Next push button
        }
        else if (CurrentCanvas == SymbolsCanvas0){
            PhoneTable.gameObject.SetActive(false);
            BreadTable.gameObject.SetActive(false);
            BreadSafezone.gameObject.SetActive(false);

            // do nothing and waits for the Next push button
        }
        else if (CurrentCanvas ==SymbolsCanvas1){
            // do nothing and waits for the Next push button

        }
        else if (CurrentCanvas == FinishCanvas){
            // waits for Ok push button
            // we "won" the level and we want to communicate it to the changeScene script
            changeSceneScript.setHasWon(true);

        }
    }

    public void nextCanvas(Canvas nextCanvas){
    	CurrentCanvas.gameObject.SetActive(false);
        CurrentCanvas = nextCanvas;
        utility.startClickOnButtonSound();
        PlayerPrefs.SetString("LastCanvas", CurrentCanvas.name);
        CurrentCanvas.gameObject.SetActive(true);
    }
}


