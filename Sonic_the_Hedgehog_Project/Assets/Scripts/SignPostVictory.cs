using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignPostVictory : MonoBehaviour
{
    public GameObject timerText;
    public SonicTimer levelTimer;

    //Creates a field in the inspector to place the end of level canvas UI;
    public Canvas endOfLevel;

    //Creates a field in the inspector to hold the SceneLoader.
    public SceneLoader ghzAct1SceneLoader;

    //Declares an Animator variable;
    private Animator signPostAnimator;

    //Bool used to only let the onTrigger event happen once.
    private bool alreadyActivated = false;

    void Start()
    {
        levelTimer = timerText.GetComponent<SonicTimer>();
        //Assign the animator component to the variable.
        signPostAnimator = GetComponent<Animator>();
        //Initializes the signpost to have Dr. Robotnik's picture.
        signPostAnimator.Play("Signpost_Robotnik");
        //Initially disables the end of level UI so it can't be seen.
        endOfLevel.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks for a collision with Sonic and if the if statement has already been run through once.
        if (collision.gameObject.tag == "SonicPlayer" && !alreadyActivated)
        {
            //Sets to true so this if statement may only run once.
            alreadyActivated = true;

            levelTimer.PauseTimer();
            //Twirls the signpost.
            signPostAnimator.Play("Signpost_Twirl");
            //Sets the signpost to the picture of Sonic and activates the end of level UI.
            Invoke("SonicSignpost", 3f);
            //Loads next scene.
            Invoke("LevelEnd", 6f);
        }
    }

    private void SonicSignpost()
    {
        signPostAnimator.Play("Signpost_Sonic");
        endOfLevel.enabled = true;
    }

    private void LevelEnd()
    {
        ghzAct1SceneLoader.LoadNextLevel();
    }
}
