using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{

    public List<string> tasks = new List<string>();
    private int currentTask;

    void Start()
    {
        currentTask = 0;
    }

    public void update(string taskCompleted)
    {
        if (tasks[currentTask] != taskCompleted || (currentTask+1) == tasks.Count)
            gameOver();
        currentTask++;
    }

    private void gameOver()
    {
        // the game is over, we return to the Pause Room with no possibility to resume the game
        PlayerPrefs.SetString("PausedScene", ""); 
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
