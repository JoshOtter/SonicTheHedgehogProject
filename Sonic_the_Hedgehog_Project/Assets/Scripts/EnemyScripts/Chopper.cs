using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopper : MonoBehaviour
{
    //Creates a field in the inspector to link the sonic game object.
    public GameObject sonic;
    //Creates a variable to hold a SonicController_FSM script component.
    private SonicController_FSM sonicScript;

    //Creates a field in the inspector to place chopper's rigidbody2D.
    public Rigidbody2D chopperRb2d;

    //Creates a field in the inspector to place chopper's animator.
    public Animator chopperAnimator;

    //Set's the force at which the fish will keep jumping in the air.
    public float jumpForce = 22.8f;

    //Creates a float that can be adjusted that decides how soon the chopper initially jumps upon loading.
    public float jumpStart = 0f;

    //Creates a float that can be adjusted to lengthen the time between jumps.
    public float keepJumping = 1f;

    //Declares a transform variable for the jumping point and the point where the chopper's movement is haulted.
    public Transform jumpPoint;
    public Transform stopPoint;

    //Declares a bool used in a check to move the chopper back to its jumping spot in the JumpAgain() function.
    public bool jumping = true;

    void Start()
    {
        //Collects the necessary components.
        sonic = GameObject.Find("Sonic");
        sonicScript = sonic.GetComponent<SonicController_FSM>();
        chopperAnimator = GetComponent<Animator>();
        chopperRb2d = GetComponent<Rigidbody2D>();
        //Activates the initial jump.
        Invoke("Jump", jumpStart);
    }

    private void Update()
    {
        //Allows the chopper to continue jumping
        JumpAgain();
    }

    void Jump()
    {
        //Sets the jumping bool to true so the if statement in the JumpAgain() function can activate.
        jumping = true;
        //Set's the chopper's gravity.
        chopperRb2d.gravityScale = 3f;
        //Causes the chopper's initial jump.
        chopperRb2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    void JumpAgain()
    {
        //Checks if the chopper is close enough to the stopping point and if it is currently jumping.
        if (Vector2.Distance(stopPoint.transform.position, transform.position) < 1f && jumping == true)
        {
            //Set's the velocity to zero.
            chopperRb2d.velocity = Vector2.zero;
            //Set's gravity to zero.
            chopperRb2d.gravityScale = 0f;
            //Moves the chopper back to it's initial jumping point, which is far enough away not to trigger the above if statement.
            transform.position = new Vector2(jumpPoint.position.x, jumpPoint.position.y);
            //Set's jumping to false so that this if statement can't run again while the chopper is moving to the jumping point.
            jumping = false;
            //Calls the Jump function once again after a brief wait.
            Invoke("Jump", keepJumping);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks if the crabmeat collides with sonic
        if (collision.gameObject.tag == "SonicPlayer")
        {
            //Check's if sonic is currently spinning
            if (sonicScript.isDeadly)
            {
                //Set's the chopper's body to kinematic so it doesn't move while playing the explosion animation.
                chopperRb2d.isKinematic = true;
                chopperAnimator.Play("Enemy_Explosion");
                Invoke("Destroyed", 0.3f);
            }
        }
    }

    //destroys the game object.
    private void Destroyed()
    {
        Destroy(gameObject);
    }
}