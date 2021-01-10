using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class utilityScript : MonoBehaviour
{
	// texture images for the action canvas to change
    public Texture good;
    public Texture ok;
    public Texture bad;
    public Texture checkpoint;
    public RawImage infoImage;
    public TextMeshProUGUI infoText;

    public AudioSource goodSound;
    public AudioSource badSound;
    public AudioSource checkpointSound;

    public AudioSource congratulationSound;
    public AudioSource gameOverSound;

    public AudioSource clickButtonSound;


    // Start is called before the first frame update
    void Start()
    {
        Color fixedColor = infoImage.color;
		fixedColor.a = 1;
		infoImage.color = fixedColor;
		infoImage.CrossFadeAlpha(0f, 0f, true);
		fixedColor = infoText.color;
		fixedColor.a = 1;
		infoText.color = fixedColor;
		infoText.CrossFadeAlpha(0f, 0f, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayText(string mytext){
		// the target alpha is 1, opaque
	    float alpha = 1;

	    // the text is set
	    infoText.text = mytext;
	    infoText.color = Color.red;
	    infoText.CrossFadeAlpha(alpha,1f,false);
	    // and so is the info image
	    infoImage.texture = bad;
	    infoImage.CrossFadeAlpha(alpha,1f,false);
	}

    public void goodAction(){
		// the target alpha is 1, opaque
    	float alpha = 1;

    	goodSound.Play(0);
    	infoImage.texture = good;
		infoImage.CrossFadeAlpha(alpha, 1f, false);
    }

    public void okAction(int penal){
    	// the target alpha is 1, opaque
    	float alpha = 1;

    	badSound.Play(0);
    	// the text is set
    	infoText.text = "-" + penal.ToString();
    	infoText.color = Color.yellow;
    	infoText.CrossFadeAlpha(alpha,1f,false);
    	// and so is the info image
    	infoImage.texture = ok;
		infoImage.CrossFadeAlpha(alpha,1f,false);
    }

    public void badAction(int penal){
    	// the target alpha is 1, opaque
    	float alpha = 1;

    	badSound.Play(0);
    	// the text is set
    	infoText.text = "-" + penal.ToString();
    	infoText.color = Color.red;
    	infoText.CrossFadeAlpha(alpha,1f,false);
    	// and so is the info image
    	infoImage.texture = bad;
 		infoImage.CrossFadeAlpha(alpha,1f,false);
    }

    public void checkpointAction(){
        // the target alpha is 1, opaque
        float alpha = 1;

        checkpointSound.Play(0);
        // set the info image
        infoImage.texture = checkpoint;
        infoImage.CrossFadeAlpha(alpha,1f,false);
    }

    public IEnumerator resetInfo(){
    	// wait for 2 seconds
    	yield return new WaitForSecondsRealtime(2);
    	// the target alpha is 0, transparent
    	float alpha = 0;

    	// the text is resetted
    	infoText.CrossFadeAlpha(alpha, 1f, false);
    	// and so is the info image
    	infoImage.CrossFadeAlpha(alpha, 1f, false);
    }

    public void FogFadingIn()
    {
        StartCoroutine(FogFadingInCoroutine());
    }

    public void FogFadingOut(){
        StartCoroutine(FogFadingOutCoroutine());
        RenderSettings.fogDensity = 0.9f;
    }
    public void FogFadingInFirstTutorial()
    {
        StartCoroutine(FogFadingInTutorialCoroutine());
    }

    IEnumerator FogFadingInCoroutine()
    {
        do{
           RenderSettings.fogDensity += 0.4f * Time.deltaTime;
           yield return null;
        }while (RenderSettings.fogDensity < 0.9f);
        RenderSettings.fogDensity = 0.9f;
    }

    IEnumerator FogFadingInTutorialCoroutine()
    {
        do{
           RenderSettings.fogDensity += 0.4f * Time.deltaTime;
           yield return null;
        }while (RenderSettings.fogDensity < 0.4f);
        RenderSettings.fogDensity = 0.4f;
    }

    IEnumerator FogFadingOutCoroutine()
    {
        do{
            RenderSettings.fogDensity -= 0.4f * Time.deltaTime;
            yield return null;
        }while (RenderSettings.fogDensity >= 0f);
        RenderSettings.fogDensity=0f;
    }

    public void startCongratulationSound(){
    	congratulationSound.Play(0);
    }

    public void startGameOverSound(){
		gameOverSound.Play(0);
    }

    public void startClickOnButtonSound(){
    	clickButtonSound.Play(0);
    }
}
