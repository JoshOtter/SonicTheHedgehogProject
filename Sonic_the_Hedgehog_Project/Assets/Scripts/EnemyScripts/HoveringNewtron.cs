using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringNewtron : MonoBehaviour
{
    //Creates a field in the inspector to link the sonic game object.
    public GameObject sonic;
    //Creates a variable to hold a SonicController_FSM script component.
    private SonicController_FSM sonicScript;
    //Creates an array field that will store the sprite renderers for the Newtron's child objects.
    public SpriteRenderer[] hoveringNewtronChildAnimations;
    //Creates fields in the inspector to place the Newtron's animator, sprite renderer, RigidBody2D, and CapsuleCollider2D.
    public Animator hoveringNewtronAnimator;
    public SpriteRenderer hoveringNewtronSprite;
    public Rigidbody2D hoveringNewtronRb2d;
    public CapsuleCollider2D capsuleCollider;

    //Creates an array field in the inspector to place the two waypoints used by the Newtron.
    public Transform[] waypoints = new Transform[2];

    //Set's the bool value to true to enable the Move() function to keep running as long as the Newtron isn't dead.
    public bool canMove = true;

    //Set's the actual speed of the Newtron when the speed variable is assigned its value.
    public float hoveringNewtronSpeed = 3f;

    //Set's the initial speed of the Newtron to zero so it doesn't move until it performs its first few animations.
    public float speed = 0f;

    //Set's a bool value to be used to activate the Newtron's actions.
    public bool activate = false;

    //This bool prevents the newtron from deciding whether to move left or right more than once, so that it doesn't switch when the player moves to the other side of it.
    private bool pathChosen = false;
    //This bool sets the destination of the newtron based on one of two waypoints.
    private bool movingLeft;

    void Start()
    {
        //Gathers some required components.
        sonicScript = sonic.GetComponent<SonicController_FSM>();
        hoveringNewtronChildAnimations = GetComponentsInChildren<SpriteRenderer>();
        hoveringNewtronAnimator = GetComponent<Animator>();
        hoveringNewtronSprite = GetComponent<SpriteRenderer>();
        hoveringNewtronRb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        //Initially starts with the collider off and the rigidbody set to kinematic, so it can't be hit by the player and doesn't fall during it's beginning animations.
        capsuleCollider.enabled = false;
        hoveringNewtronRb2d.isKinematic = true;
        //Initially disables the newtron sprite and its engine sprite so it can be invisible.
        hoveringNewtronSprite.enabled = false;
        hoveringNewtronChildAnimations[1].enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Checks if the crabmeat collides with sonic
        if (collision.gameObject.tag == "SonicPlayer")
        {
            //Check's if sonic is currently spinning
            if (sonicScript.isDeadly)
            {
                //Hault's movement and prevents any Invoke statments from activating.
                speed = 0;
                canMove = false;
                //Plays the explosion animation, deactivates the engine sprite, and destroys the object.
                hoveringNewtronAnimator.Play("Enemy_Explosion");
                hoveringNewtronChildAnimations[1].enabled = false;
                Invoke("Destroyed", 0.3f);
            }
        }
    }

    void Update()
    {
        //Checks to see if it can still move (it can't if it's been hit by sonic while he is spinning)
        if (activate)
        {
            Move();
        }
    }

    private void Move()
    {   
        //Checks which direction the sprite faces and initially starts moving based on the NewtronMove() function below.
        if (!movingLeft)
        {
            //This continues the movement from NewtronMove().
            transform.position = Vector2.MoveTowards(transform.position, waypoints[0].transform.position, speed * Time.deltaTime);

            //This destroys the newtron object once it reaches the waypoint destination.
            if (Vector2.Distance(waypoints[0].transform.position, transform.position) < 1f)
            {
                Invoke("Destroyed", 0f);
            }
        }
        else
        {
            //This continues the movement from NewtronMove().
            transform.position = Vector2.MoveTowards(transform.position, waypoints[1].transform.position, speed * Time.deltaTime);

            //This destroys the newtron object once it reaches the waypoint destination.
            if (Vector2.Distance(waypoints[1].transform.position, transform.position) < 1f)
            {
                Invoke("Destroyed", 0f);
            }
        }
        //Enables the newtron sprite so the initial animation can appear.
        hoveringNewtronSprite.enabled = true;
        //Starts the movement animation.
        Invoke("HoveringNewtronMoving", 0.8f);
        //Enables the collider and rigidbody so it can move along the terrain surface.
        Invoke("ActivateNewtronBody", 0.8f);
        //Picks its direction and begins the initial movement.
        Invoke("NewtronMove", 1.8f);
    }

    private void HoveringNewtronMoving()
    {
        if (canMove)
        {
            hoveringNewtronAnimator.Play("Hovering_Newtron_Moving");
        }       
    }

    private void ActivateNewtronBody()
    {
        if (canMove)
        {
            hoveringNewtronRb2d.isKinematic = false;
            capsuleCollider.enabled = true;
        }        
    }

    private void NewtronMove()
    {
        if (canMove)
        {
            //Enables the engine sprite
            hoveringNewtronChildAnimations[1].enabled = true;
            //sets the speed
            speed = hoveringNewtronSpeed;
            //checks if this if statment has been activated once before, and if so, it won't activate again.
            if (!pathChosen)
            {
                //Checks where sonic is in relation to the newtron on the x axis.
                if (sonic.transform.position.x > transform.position.x)
                {
                    //flips the sprites and repositions the child sprite for rightward movement.
                    hoveringNewtronSprite.flipX = true;
                    hoveringNewtronChildAnimations[1].flipX = true;
                    hoveringNewtronChildAnimations[1].transform.localPosition = new Vector2(-0.2f, -0.02f);
                    //Begins the movement toward the chosen waypoint.
                    transform.position = Vector2.MoveTowards(transform.position, waypoints[0].transform.position, speed * Time.deltaTime);
                    //Sets this bool to true so this function won't be fully called again.
                    pathChosen = true;
                    //Sets this bool for the Move() function to be able continue movement to the correct waypoint.
                    movingLeft = false;
                }
                else
                {
                    //Begins the movement toward the chosen waypoint.
                    transform.position = Vector2.MoveTowards(transform.position, waypoints[1].transform.position, speed * Time.deltaTime);
                    //Sets this bool to true so this function won't be fully called again.
                    pathChosen = true;
                    //Sets this bool for the Move() function to be able continue movement to the correct waypoint.
                    movingLeft = true;
                }
            }
        }        
    }

    //Destroys the game object.
    private void Destroyed()
    {
        Destroy(gameObject);
    }
}
