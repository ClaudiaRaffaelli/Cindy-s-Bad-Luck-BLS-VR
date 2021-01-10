using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class path : MonoBehaviour
{
    public GameObject cindy;
    public string trigger; // The value to trigger in Cindy's animator through this object
    public List<GameObject> particleSystems = new List<GameObject>(); // list of all particle systems which will guide the user to perform the correct movement
    //public GameObject Log; // debug
    
    private List<string> pointsName = new List<string>(); // Save all points's (this object children) name
    private List<Vector3> pointsPosition = new List<Vector3>(); // and their position
    private int currentPoint; // the next point expected from this object
    private float resetTime; // after this time the count will reset
    private float timer;
    private bool guideLine; // when true the particle systems will be activated
    private bool firstPoint;
    private int nextGuideLinePosition;
    private bool active; // When true the Update method will be executed. this parameter is set to true by the first node of this path



    // Start is called before the first frame update
    void Awake()
    {
        // Save all points's information
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject point = transform.GetChild(i).gameObject;
            point.name = "point" + i;
            pointsName.Add("point"+i);
            pointsPosition.Add(point.transform.position);
        }
        currentPoint = 0;
        resetTime = 3.0f;
        timer = 0.0f;
        guideLine = false; // When true the particleSystems will start
        firstPoint = true; // used to reset particleSystems's position in the particleSystems list before they start their new path
    }

    void Start()
    {
        foreach (GameObject particleSystem in particleSystems)
            resetGuideLine(particleSystem);
        //guideLine = true;   //debug
        //active = true;  //debug
    }

    void resetGuideLine(GameObject particleSystem)
    {
        particleSystem.SetActive(false); // Particle system's emission depends on its movement, so disable it before resetting its position
        particleSystem.transform.position = pointsPosition[0]; // All particle systems start from the first point
        particleSystem.SetActive(true);
        nextGuideLinePosition = 1;
    }
    

    
    void Update()
    {
        // Manage the particle systems
        if (active)
        {
            timer += Time.deltaTime;

            if (timer >= resetTime)
            {
                // reset the counter
                currentPoint = 0;
                timer = 0;
                guideLine = true;
            }

            if (guideLine)
            {
                // Do this only the first time when the guideline starts
                if (firstPoint)
                {
                    // reset all particleSystem's position
                    foreach (GameObject particleSystem in particleSystems)
                        resetGuideLine(particleSystem);
                    firstPoint = false;
                }

                int completedPath = 0; // Each particleSystem will increase this value by 1 when it reaches the target node in the path
                foreach (GameObject particleSystem in particleSystems)
                {
                    // if the particle system didn't reach the target
                    if (particleSystem.transform.position != pointsPosition[nextGuideLinePosition])
                    {
                        // move it towards the target
                        particleSystem.transform.position = Vector3.MoveTowards(particleSystem.transform.position, pointsPosition[nextGuideLinePosition], Time.deltaTime * 1f);
                    }
                    else{
                        completedPath++; // Target (next node) reached
                    }
                    
                }
                if (completedPath == particleSystems.Count) // When all particleSystem reached their terget node
                {
                    nextGuideLinePosition = (nextGuideLinePosition + 1) % pointsPosition.Count; // set the next target index in pointsPosition list
                    if (nextGuideLinePosition == 0) // If the particleSystems reached the last node of the path then disable the guideline
                    {
                        guideLine = false;
                        firstPoint = true;
                    }
                }
            }


        }


    }

    public void notify(string pointID)
    {
        if (pointID.Equals(pointsName[currentPoint])) // Check if the pointID is equal to te expected point (currentPoint)
        {
            currentPoint++;
            //Log.GetComponent<TMPro.TextMeshProUGUI>().SetText(currentPoint.ToString());
            timer = 0;
        }
        // otherwise reset counterPoint
        else if (pointID.Equals(pointsName[0])) //reset to 1 if the user keep triggering the first node (so the next expected node will be 1)
        {
            currentPoint = 1;
        }
        else // otherwise reset it to 0 (so the next expected node will be 0)
        {
            currentPoint = 0;
        }
        

        if (currentPoint == pointsName.Count) // If the user has followed the path correctly and reached the last node
        {
            cindy.GetComponent<Animator>().SetBool(trigger, true); // activate the trigger in the animator
            Destroy(this.gameObject); // no more necessary

            //TOOD: gestire le due mani
        }
    }

    public void setActive(bool pActive)
    {
        active = pActive;
        guideLine = pActive;
    }

    public bool isShowingGuideline(){
        return guideLine;
    }
}
