using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motobug : MonoBehaviour
{
    //Creates a field in the inspector to link the sonic game object.
    public GameObject sonic;
    //Creates a variable to hold a SonicController_FSM script component.
    private SonicController_FSM sonicScript;
    //Creates an array field that will store the sprite renderers for the motobug's child objects.
    public SpriteRenderer[] motobugChildAnimations;
    //Creates fields in the inspector to place the motobug's animator and sprite renderer.
    public Animator motobugAnimator;
    public SpriteRenderer motobugSprite;

    //Creates an array field in the inspector to place the two waypoints used by the motobug.
    public Transform[] waypoints = new Transform[2];

    //Set's the bool value to true to enable the Move() function to keep running as long as the motobug isn't dead.
    public bool canMove = true;

    //Set's the actual speed of the motobug when the speed variable is assigned its value.
    public float motobugSpeed = 3f;

    //Set's the initial speed of the motobug to zero so it doesn't move until Sonic triggers its movement.
    public float speed = 0f;

    //Used to traverse through the waypoints array.
    int current = 0;

    void Start()
    {
        sonicScript = sonic.GetComponent<SonicController_FSM>();
        motobugChildAnimations = GetComponentsInChildren<SpriteRenderer>();
        motobugAnimator = GetComponent<Animator>();
        motobugSprite = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SonicPlayer")
        {
            if (sonicScript.isDeadly)
            {
                speed = 0;
                canMove = false;
                motobugAnimator.Play("Enemy_Explosion");
                motobugChildAnimations[1].enabled = false;
                Invoke("Destroyed", 0.3f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //Calls the Move() function every frame but only starts when the motobugTrigger sets the Motobug's speed to 5;
            Move();
        }
    }

    private void Move()
    {
        //Begins moving the motobug to the current waypoint.
        transform.position = Vector2.MoveTowards(transform.position, waypoints[current].transform.position, speed * Time.deltaTime);
        //Checks if the motobug's position is less than .2 from the current waypoint's position.
        if (Vector2.Distance(waypoints[current].transform.position, transform.position) < 0.2f)
        {
            //Set's speed back to zero to stop the motobug from moving.
            speed = 0;
            //If the current waypoint is the first waypoint, it changes the location of certain animations and the bomb spawner object in relation to the Buzz Bomber
            //It also reverts the animation's sprite renderers flipX setting to false so they face the correct way.
            if (current == 0)
            {
                motobugSprite.flipX = true;
                motobugChildAnimations[1].transform.localPosition = new Vector2(-0.25f, 0f);
            }
            //If the current waypoint is the second waypoint, it changes the location of certain animations and the bomb spawner object in relation to the Buzz Bomber
            //It also flips the animation's sprite renderers so they face the correct way.
            else if (current == 1)
            {
                motobugSprite.flipX = false;
                motobugChildAnimations[1].transform.localPosition = new Vector2(0.25f, 0f);
            }
            //Adds one to current so the next time Move() is called, it targets the second waypoint.
            current++;
            //If the value of current becomes 2 or more, it is reset to 0 so the motobug can continue patrolling between the two waypoints.
            if (current >= waypoints.Length)
            {
                current = 0;
            }
            //This series of Invoked functions control the timing of animations, shooting its bomb, and continuing its movement.
            Invoke("MotobugEngineOff", 0.0f);
            Invoke("MotobugStop", 0.0f);
            Invoke("MotobugMoving", 1.0f);
            Invoke("MotobugEngineOn", 1.0f);
            Invoke("MotobugContinues", 1.0f);
        }
    }

    //Enables the sprite renderer for the motobug's smoke animation.
    private void MotobugEngineOn()
    {
        if (canMove)
        {
            //Debug.Log("Engine Activated");
            motobugChildAnimations[1].enabled = true;
        }
    }

    //Disables the sprite renderer for the motobug's smoke animation.
    private void MotobugEngineOff()
    {
        if (canMove)
        {
            motobugChildAnimations[1].enabled = false;
        }
    }

    private void MotobugStop()
    {
        if (canMove)
        {
            motobugAnimator.Play("Motobug_Stopped");
        }
    }

    //Plays the motobug's moving animation.
    private void MotobugMoving()
    {
        if (canMove)
        {
            //Debug.Log("Moving animation activated");
            motobugAnimator.Play("Motobug_Moving");
        }        
    }

    //Set's the speed of the motobug back to 5 so it can continue moving to the next waypoint.
    private void MotobugContinues()
    {
        if (canMove)
        {
            speed = motobugSpeed;
        }
    }

    //Destroys the game object.
    private void Destroyed()
    {
        Destroy(gameObject);
    }
}
