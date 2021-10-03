using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crabmeat : MonoBehaviour
{
    //Creates a field in the inspector to link the sonic game object.
    public GameObject sonic;
    //Creates a variable to hold a SonicController_FSM script component.
    private SonicController_FSM sonicScript;
    //Creates fields in the inspector to place the crabmeat's animator and sprite renderer.
    public Animator crabmeatAnimator;
    public SpriteRenderer crabmeatSprite;

    //Creates an array in which the crabmeat's spawnpoint child objects will be places.
    public Transform[] bombSpawnPoints = new Transform[2];

    //Instantiates a copy of the crabmeat bomb prefab.
    public GameObject crabmeatBomb;

    //Creates an array field in the inspector to place the two waypoints used by the crabmeat.
    public Transform[] waypoints = new Transform[2];

    //Set's the bool value to true to enable the Move() function to keep running as long as the crabmeat isn't dead.
    public bool canMove = true;

    //Set's the actual speed of the crabmeat when the speed variable is assigned its value.
    public float crabmeatSpeed = 1f;

    //Set's the initial speed of the crabmeat to 3 so it can start moving immediately.
    public float speed = 1f;

    //Used to traverse through the waypoints array.
    int current = 0;

    void Start()
    {
        //Collects the necessary components at startup.
        sonicScript = sonic.GetComponent<SonicController_FSM>();
        crabmeatAnimator = GetComponent<Animator>();
        crabmeatSprite = GetComponent<SpriteRenderer>();
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
                //Plays the explosion animation and destroys the object when its finished.
                crabmeatAnimator.Play("Enemy_Explosion");
                Invoke("Destroyed", 0.3f);
            }
        }
    }

    void Update()
    {
        //Checks to see if it can still move (it can't if it's been hit by sonic while he is spinning)
        if (canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        //Begins moving the crabmeat to the current waypoint.
        transform.position = Vector2.MoveTowards(transform.position, waypoints[current].transform.position, speed * Time.deltaTime);
        //Checks if the crabmeat's position is less than .2 from the current waypoint's position.
        if (Vector2.Distance(waypoints[current].transform.position, transform.position) < 0.2f)
        {
            //Set's speed to zero to stop the crabmeat from moving.
            speed = 0;
            //If the current waypoint is the first waypoint, it changes the location of certain animations and the bomb spawner object in relation to the Buzz Bomber
            //It also reverts the animation's sprite renderers flipX setting to false so they face the correct way.
            if (current == 0)
            {
                crabmeatSprite.flipX = true;
            }
            //If the current waypoint is the second waypoint, it changes the location of certain animations and the bomb spawner object in relation to the Buzz Bomber
            //It also flips the animation's sprite renderers so they face the correct way.
            else if (current == 1)
            {
                crabmeatSprite.flipX = false;
            }
            //Adds one to current so the next time Move() is called, it targets the second waypoint.
            current++;
            //If the value of current becomes 2 or more, it is reset to 0 so the motobug can continue patrolling between the two waypoints.
            if (current >= waypoints.Length)
            {
                current = 0;
            }
            //This series of Invoked functions control the timing of animations, shooting its bomb, and continuing its movement.
            Invoke("CrabmeatStop", 0.0f);
            Invoke("CrabmeatShoots", 0.5f);
            Invoke("Shoot", 0.5f);
            Invoke("CrabmeatWalk", 1.0f);
            Invoke("CrabmeatContinues", 1.0f);
        }
    }

    //Plays the stop animation.
    private void CrabmeatStop()
    {
        crabmeatAnimator.Play("Crabmeat_Stopped");
    }

    //Plays the walk animation.
    private void CrabmeatWalk()
    {
        crabmeatAnimator.Play("Crabmeat_Walking_Right");
    }

    //Plays the shoot animation.
    private void CrabmeatShoots()
    {
        crabmeatAnimator.Play("Crabmeat_Shoots");
    }

    //Set's the speed of the crabmeat back to 5 so it can continue moving to the next waypoint.
    private void CrabmeatContinues()
    {
        if (canMove)
        {
            speed = crabmeatSpeed;
        }
    }

    //Instantiates the crabmeat bomb prefab and set's its position and rotation.
    void Shoot()
    {
        if (canMove)
        {
            Instantiate(crabmeatBomb, bombSpawnPoints[0].position, Quaternion.Euler(0, 0, 15));
            Instantiate(crabmeatBomb, bombSpawnPoints[1].position, Quaternion.Euler(0, 0, 345));
        }
    }

    //Destroys the game object.
    private void Destroyed()
    {
        Destroy(gameObject);
    }
}
