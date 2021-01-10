using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPC_logic : MonoBehaviour
{
    public GameObject targetBackwardPosition;
    public GameObject targetForwardPosition;
    public Canvas canvasInteractions;
    public GameObject smartphone;
    // These attributes are necessary if the NPC can pull up Cindy's legs
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftLegPullUpTarget;
    public GameObject rightLegPullUpTarget;
    // These attributes are necessary if the NPC can make some noise
    public AudioClip helpAudio;
    public AudioClip panicAudio;
    private Animator animator;

    private bool firstPullUpLegsCall = true; // Some weird bug requires pullUpLegs() to be called 2 times. This variable is used to do that
    private bool move_forward = false; // Check when the NPC need to move forward

    private bool calm = false;
    private bool back = false;
    private bool legsUp = false;
    private bool toBed = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        // If there are audio clips available start playing the "panic" audio clip
        if (GetComponent<AudioSource>())
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = panicAudio;
            audioSource.Play();
            audioSource.PlayOneShot(helpAudio, 1f); // The second variable is the volume (from 0 to 1)
        }
    }

    void Update()
    {
        // When the npc reaches the target (distance below a threshold) he stop moving
        if (Vector3.Distance(transform.position, targetBackwardPosition.transform.position) < 1f)
        {
            animator.SetBool("walking", false);
        }
        // If the NPC was moving forward and reaches the target he can start pull up Cind's legs
        else if (move_forward && Vector3.Distance(transform.position, targetForwardPosition.transform.position) < 0.5f)
        {
            move_forward = false;

            animator.SetBool("forward", false);
            GetComponent<RigBuilder>().enabled = true;
            pullUpLegs();
        }

    }

    public void goBack()
    {
        transform.LookAt(targetBackwardPosition.transform, Vector3.up);
        transform.Rotate(0, 180, 0);
        animator.SetBool("phoneCall", false);
        animator.SetBool("walking", true);
        // remove the option once it has been selected
        canvasInteractions.GetComponent<showNpcInteractions>().toggleOptions();
        canvasInteractions.GetComponent<showNpcInteractions>().options.RemoveAt(0);
        back = true;
    }

    public void phoneCall()
    {
        animator.SetBool("walking", false);
        animator.SetBool("phoneCall", true);
        // put the smartphone in the npc's hand
        smartphone.GetComponent<MeshRenderer>().enabled = true;
        // remove the option once it has been selected
        canvasInteractions.GetComponent<showNpcInteractions>().toggleOptions();
        try
        {
            canvasInteractions.GetComponent<showNpcInteractions>().options.RemoveAt(1);
        }
        catch
        {
            canvasInteractions.GetComponent<showNpcInteractions>().options.RemoveAt(0);
        }
    }


    public void pullUpLegs()
    {
        // Change target's parent
        leftLegPullUpTarget.transform.parent = rightHand.transform;
        rightLegPullUpTarget.transform.parent = leftHand.transform;

        // Before changing their position it is necessary to destroy the rigid body (I don't know why)
        Destroy(rightLegPullUpTarget.GetComponent<Rigidbody>());
        Destroy(leftLegPullUpTarget.GetComponent<Rigidbody>());

        // Reset targets's position so they're overlapping the hands
        leftLegPullUpTarget.transform.localPosition = new Vector3(0, 0, 0);
        rightLegPullUpTarget.transform.localPosition = new Vector3(0, 0, 0);

        // I don't know why but it is necessary to call this function 2 times (with a delay in between) to make it work
        if (firstPullUpLegsCall)
        {
            firstPullUpLegsCall = false;
            Invoke("pullUpLegs", 0.1f);
        }
        legsUp = true;
    }

    public void bed()
    {
        toBed = true;

        // remove the option once it has been selected
        canvasInteractions.GetComponent<showNpcInteractions>().toggleOptions();
        try
        {
            canvasInteractions.GetComponent<showNpcInteractions>().options.RemoveAt(2);
        }
        catch
        {
            try
            {
                canvasInteractions.GetComponent<showNpcInteractions>().options.RemoveAt(1);
            }
            catch
            {
                canvasInteractions.GetComponent<showNpcInteractions>().options.RemoveAt(0);
            }
        }
    }


    public void moveForward()
    {
        move_forward = true;
        transform.LookAt(targetForwardPosition.transform, Vector3.up);
        animator.SetBool("phoneCall", false);
        animator.SetBool("forward", true);
        // remove all the options
        canvasInteractions.GetComponent<showNpcInteractions>().toggleOptions();
        canvasInteractions.GetComponent<showNpcInteractions>().options.Clear();
    }


    public void keepCalm()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        // remove the option once it has been selected
        canvasInteractions.GetComponent<showNpcInteractions>().toggleOptions();
        try
        {
            canvasInteractions.GetComponent<showNpcInteractions>().options.RemoveAt(2);
        }
        catch
        {
            try
            {
                canvasInteractions.GetComponent<showNpcInteractions>().options.RemoveAt(1);
            }
            catch
            {
                canvasInteractions.GetComponent<showNpcInteractions>().options.RemoveAt(0);
            }
        }
        calm = true;
    }


    public bool isCalling()
    {
        return animator.GetBool("phoneCall");
    }

    public bool isCalm(){
        return calm;
    }

    public bool isAway(){
        return back;
    }

    public bool hasPulledLegs(){
        return legsUp;
    }

    public bool isToBed(){
        return toBed;
    }
}
