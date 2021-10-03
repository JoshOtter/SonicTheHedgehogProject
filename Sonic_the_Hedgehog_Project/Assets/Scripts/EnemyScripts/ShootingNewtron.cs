using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingNewtron : MonoBehaviour
{
    //Sets the activate bool to false (this will be changed to true by the ShootingNewtronTrigger when sonic collides with it.
    public bool activate = false;

    //This bool makes it so the behavior of this newtron can only activate once, depsite being attached to the Update function.
    public bool alreadyActivated = false;

    //Declares a transform variable to hold the bomb spawn child object.
    public Transform newtronBombSpawn;

    //Declares a game object variable to hold the bomb prefab.
    public GameObject newtronBomb;

    //Declares a SpriteRenderer variable that can be assigned in the inspector.
    public SpriteRenderer shootingNewtronSprite;

    //Declares an Animator variable that can be assigned in the inspector.
    public Animator shootingNewtronAnimator;

    private void Start()
    {
        //Disables the sprite and animator on startup.
        shootingNewtronSprite.enabled = false;
        shootingNewtronAnimator.enabled = false;
    }


    void Update()
    {
        //Calls the WakeUp() function if the trigger has been activated.
        if (activate)
        {
            WakeUp();
        }
    }

    private void WakeUp()
    {
        //Checks the bool value to see if this script has already been activated once.
        if (!alreadyActivated)
        {
            //Set's this bool to true so this function can only be used this first time.
            alreadyActivated = true;
            //Activates the sprite and animator.
            shootingNewtronSprite.enabled = true;
            shootingNewtronAnimator.enabled = true;
            //Shoots the bomb during animation when it's mouth is open.
            Invoke("Shoot", 2f);
            //Destroys the object at the end of its animation.
            Invoke("Destroyed", 5f);
        }
    }

    private void Shoot()
    {
        //spawns the bomb at the spawn position and set's its rotation.
        Instantiate(newtronBomb, newtronBombSpawn.position, Quaternion.Euler(0, 0, 0));
    }

    //Destroys the game object.
    private void Destroyed()
    {
        Destroy(gameObject);
    }
}
