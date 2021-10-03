using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzBomber : MonoBehaviour
{
    //Creates a field in the inspector to link the sonic game object.
    public GameObject sonic;
    //Creates a variable to hold a SonicController_FSM script component.
    private SonicController_FSM sonicScript;
    //Creates field in the inspector to link the associated bomb spawner.
    public GameObject bombSpawner;
    //Creates field in the inspector to link the bomb prefab.
    public GameObject buzzBomb;
    //Creates an array field that will store the sprite renderers for the Buzz Bomber's child objects.
    public SpriteRenderer[] buzzBomberChildAnimations;
    //Creates fields in the inspector to place the Buzz Bomber's animator, sprite renderer, circle collider, and capsule collider.
    public Animator buzzBomberAnimator;
    public SpriteRenderer buzzBomberSprite;
    public CircleCollider2D buzzBomberCircleCollider;
    public CapsuleCollider2D buzzBomberCapsuleCollider;

    //Creates an array field in the inspector to place the two waypoints used by the Buzz Bomber.
    public Transform[] waypoints = new Transform[2];
    //Creates a field in the inspector to place the Buzz Bomber's BombSpawn child object to set the location from which it spawns its bombs.
    public Transform spawnPoint;

    //Set's the bool value to true to enable the Move() function to keep running as long as the Buzz Bomber isn't dead.
    public bool canMove = true;

    //Set's the initial speed of the Buzz Bomber to zero so it doesn't move until Sonic triggers its movement.
    public float speed = 0f;
    
    //Used to traverse through the waypoints array.
    int current = 0;

    void Start()
    {
        //Collects the necessary components.
        sonicScript = sonic.GetComponent<SonicController_FSM>();
        buzzBomberChildAnimations = GetComponentsInChildren<SpriteRenderer>();
        buzzBomberAnimator = GetComponent<Animator>();
        buzzBomberSprite = GetComponent<SpriteRenderer>();
        buzzBomberCapsuleCollider = GetComponent<CapsuleCollider2D>();
        buzzBomberCircleCollider = GetComponent<CircleCollider2D>();
        //Turns off the firing animation for the bomb.
        buzzBomberChildAnimations[2].enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Checks if the Buzz Bomber collides with sonic
        if (collision.gameObject.tag == "SonicPlayer")
        {
            //Check's if sonic is currently spinning
            if (sonicScript.isDeadly)
            {
                //Hault's movement and prevents any Invoke statments from activating.
                speed = 0;
                canMove = false;
                //Plays the explosion animation, deactivates all unnecessary sprites, and destroys the object when its finished.
                buzzBomberAnimator.Play("Enemy_Explosion");
                buzzBomberChildAnimations[1].enabled = false;
                buzzBomberChildAnimations[2].enabled = false;
                buzzBomberChildAnimations[3].enabled = false;
                Invoke("Destroyed", 0.3f);
            }           
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Checks to see if it can still move (it can't if it's been hit by sonic while he is spinning)
        if (canMove)
        {
            //Calls the Move() function every frame but only starts when the BuzzBomberTrigger sets the Buzz Bomber's speed to 5;
            Move();
        }
    }

    private void Move()
    {
        //Begins moving the Buzz Bomber to the current waypoint.
        transform.position = Vector2.MoveTowards(transform.position, waypoints[current].transform.position, speed * Time.deltaTime);
        //Checks if the Buzz Bomber's position is equal to the current waypoint's position.
        if (Vector2.Distance(waypoints[current].transform.position, transform.position) == 0f)
        {
            //Set's speed back to zero to stop the Buzz Bomber from moving.
            speed = 0;
            //If the current waypoint is the first waypoint, it changes the location of certain animations and the bomb spawner object in relation to the Buzz Bomber
            //It also reverts the animation's sprite renderers flipX setting to false so they face the correct way.
            if (current == 0)
            {
                buzzBomberSprite.flipX = false;
                buzzBomberChildAnimations[1].transform.localPosition = new Vector2(0.17f, -0.06f);
                buzzBomberChildAnimations[1].flipX = false;
                buzzBomberChildAnimations[2].transform.localPosition = new Vector2(-0.2f, -0.27f);
                buzzBomberChildAnimations[2].flipX = false;
                bombSpawner.transform.localPosition = new Vector2(-0.2f, -0.27f);
            }
            //If the current waypoint is the second waypoint, it changes the location of certain animations and the bomb spawner object in relation to the Buzz Bomber
            //It also flips the animation's sprite renderers so they face the correct way.
            else if (current == 1)
            {
                buzzBomberSprite.flipX = true;
                buzzBomberChildAnimations[1].transform.localPosition = new Vector2(-0.17f, -0.06f);
                buzzBomberChildAnimations[1].flipX = true;
                buzzBomberChildAnimations[2].transform.localPosition = new Vector2(0.2f, -0.27f);
                buzzBomberChildAnimations[2].flipX = true;
                bombSpawner.transform.localPosition = new Vector2(0.2f, -0.27f);
            }
            //Adds one to current so the next time Move() is called, it targets the second waypoint.
            current++;
            //If the value of current becomes 2 or more, it is reset to 0 so the Buzz Bomber can continue patrolling between the two waypoints.
            if (current >= waypoints.Length)
            {
                current = 0;
            }
            //This series of Invoked functions control the timing of animations, shooting its bomb, and continuing its movement.
            Invoke("BuzzBomberEngineOff", 0.0f);
            Invoke("BuzzBomberAims", 0.5f);
            Invoke("BuzzBomberShoots", 0.8f);
            Invoke("BuzzBomberStopsShooting", 1.0f);
            Invoke("Shoot", 1f);
            Invoke("BuzzBomberNormal", 1.2f);
            Invoke("BuzzBomberEngineOn", 1.4f);
            Invoke("BuzzBomberContinues", 1.4f);
        }
    }

    //Enables the sprite renderer for the Buzz Bomber's Shooting animation.
    private void BuzzBomberShoots()
    {
        if (canMove)
        {
            buzzBomberChildAnimations[2].enabled = true;
        }    
    }

    //Disables the sprite renderer for the Buzz Bomber's Shooting animation.
    private void BuzzBomberStopsShooting()
    {
        if (canMove)
        {
            buzzBomberChildAnimations[2].enabled = false;
        }        
    }

    //Enables the sprite renderer for the Buzz Bomber's Engine animation.
    private void BuzzBomberEngineOn()
    {
        if (canMove)
        {
            buzzBomberChildAnimations[1].enabled = true;
        }        
    }

    //Disables the sprite renderer for the Buzz Bomber's Engine animation.
    private void BuzzBomberEngineOff()
    {
        if (canMove)
        {
            buzzBomberChildAnimations[1].enabled = false;
        }       
    }

    //Plays the Buzz Bomber's normal animation.
    private void BuzzBomberNormal()
    {
        if (canMove)
        {
            buzzBomberAnimator.Play("BuzzBomberNormal");
            buzzBomberCapsuleCollider.enabled = true;
            buzzBomberCircleCollider.enabled = false;
        }        
    }

    //Plays the Buzz Bomber's aiming animation.
    private void BuzzBomberAims()
    {
        if (canMove)
        {
            buzzBomberAnimator.Play("BuzzBomberAims");
            buzzBomberCapsuleCollider.enabled = false;
            buzzBomberCircleCollider.enabled = true;
        }       
    }

    //Set's the speed of the Buzz Bomber back to 5 so it can continue moving to the next waypoint.
    private void BuzzBomberContinues()
    {
        if (canMove)
        {
            speed = 5f;
        }        
    }

    //Instantiates the buzzBomb prefab and set's its position and rotation.
    void Shoot()
    {
        if (canMove)
        {
            //Depending on which way the Buzz Bomber is facing, the rotation of the buzzBomb is altered so that it shoots in the correct direction.
            if (current == 0)
            {
                Instantiate(buzzBomb, spawnPoint.position, Quaternion.Euler(0, 0, 135));
            }
            else if (current == 1)
            {
                Instantiate(buzzBomb, spawnPoint.position, Quaternion.Euler(0, 0, 45));
            }
        }        
    }

    //Destroys the game object.
    private void Destroyed()
    {
        Destroy(gameObject);
    }
}
