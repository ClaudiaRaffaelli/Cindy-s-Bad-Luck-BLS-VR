using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patchWith : MonoBehaviour
{
    // There are going to be 2 patch: one in the user's hand and one, invisible at start, on Cindy's body
    // This is the class for the first one, which will be in the scene when the level starts.

    public GameObject patch;  // Reference to the patch allocated on Cindy's body 
    public Material patchMaterial; // Material of the patch (on Cind's body) when it will be apllied
    public Material patchHoverMaterial; // When the patch is in user's hand, the patch on Cindy's body will hover (using this material) to help the user find the correct position for the patch
    private bool positioned = false;
    private bool triggerR = false;
    private bool triggerL = false;
    private bool grabbingR = false;
    private bool grabbingL = false;

    public mantainPatch myMantainPatch;


    void Update()
    {
        
        if (triggerR && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) > 0.2)
            grabbingR = true;
        else
            grabbingR = false;

        if (triggerL && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) > 0.2)
            grabbingL = true;
        else
            grabbingL = false;

        if (grabbingR && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch) <= 0.2)
        {
            triggerR = false;
            grabbingR = false;
        }
        if (grabbingL && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) <= 0.2)
        {
            triggerL = false;
            grabbingL = false;
        }


        if (grabbingR || grabbingL)
        {
            // The patch will start hovering
            patch.GetComponent<MeshRenderer>().enabled = true;
            // In case of multiple materials: assign 'patchHoverMaterial' to all of them 
            Material[] matArray = patch.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < matArray.Length; i++)
            {
                matArray[i] = patchHoverMaterial;
            }
            patch.GetComponent<MeshRenderer>().materials = matArray;
        }
        else
        {
            patch.GetComponent<MeshRenderer>().enabled = false;
        }

    }




    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == patch)
        {
            // If the patch in the user's hand collide with Cindy's patch
            // Change the material of the patch which is already on the body
            // In case of multiple materials: assign 'patchMaterial' to all of them 
            Material[] matArray = patch.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < matArray.Length; i++)
            {
                matArray[i] = patchMaterial;
            }
            patch.GetComponent<MeshRenderer>().materials = matArray;

            try
            {
                patch.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = false; // Disable the blood
                // Increse patch collider's size so the hand does not easily trigger the OnExitTrigger function (in mantainPatch.cs)
                //patch.GetComponent<BoxCollider>().size = new Vector3(0.02f, 0.02f, 0.03f);
                myMantainPatch.setPositioned();
            }
            catch {}
            
            // We set the patch as "positioned"
            positioned = true;
            patch.GetComponent<AudioSource>().Play(); // Play sound effect when patched
            if (patch.GetComponent<mantainPatch>())
                patch.GetComponent<mantainPatch>().setFollower(this.transform.parent.gameObject.name);
            Destroy(this.gameObject); // Destroy the patch in user's hand
        }
        else if (other.gameObject.tag == "IndexTrigger") // If the user pick up the patch (in the scene)
        {
            triggerR = true;

        }
        else if (other.gameObject.tag == "IndexTriggerL")
        {
            triggerL = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "IndexTrigger") // If the user pick up the patch (in the scene)
        {
            triggerR = false;

        }
        if (other.gameObject.tag == "IndexTriggerL")
        {
            triggerL = false;
        }
    }


    public bool getPositioned(){
        return positioned;
    }

}
