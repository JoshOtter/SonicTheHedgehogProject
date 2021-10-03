using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SonicTimer : MonoBehaviour
{
    public Canvas timeOver;

    //Creates a field in the inspector to link the sonic game object.
    public GameObject sonic;
    //Creates a variable to hold a SonicController_FSM script component.
    private SonicController_FSM sonicScript;

    public TMP_Text timerText;
    public float currentTime = 0;
    public bool timerIsRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        sonicScript = sonic.GetComponent<SonicController_FSM>();
        timerIsRunning = true;
        timeOver.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (currentTime < 599)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime = 599;
                timerIsRunning = false;
            }

            DisplayTime(currentTime);

            if (currentTime >= 599)
            {
                sonicScript.AssessLives();
                if (GameValues.lives > 0)
                {
                    timeOver.enabled = true;
                }
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void PauseTimer()
    {
        timerIsRunning = false;
    }
}
