# SonicTheHedgehogProject
## Introduction
After completing The Tech Academy's C# and Unity course, I spent two weeks on a live project in which I built a level from a classic game of my chosing in Unity. I picked Sonic the Hedgehog, because I it was the first video game I played as a kid and I thought it would be a fun challenge. Over the course of the project, I worked under the supervision of my live project manager to get a working version of the first level of the game built into their classic arcade game project. I finished four user stories over the course of the week including creating my basic scenes, building the level and designing Sonic's behaviors, adding enemies and their behaviors, and creating a working gameplay model with working win and loss scenarios available to the player.

I quickly came to appreciate the complexity of Sonic the Hedgehog's character movement and the different states in which he is able to interact with enemies and the environment. I still have plenty of work to do to make this a truly faithful remake of the original game, but I am very proud of what I was able to accomplish over the course of the two week project. I am particularly happy with the finite state machine I used to create Sonic's controller as well as the scripts for several of the classic Sonic enemies. Certain aspects of the project were more difficult than others, but I enjoyed the challenge and had a lot of fun figuring out solutions to the various problems that came up. I learned a lot about Unity and its features, including its Animation capabilities, many of the available components and how to chose the right one for the situation, the cinemachine package, and it's 2D physics system. 

The project was built using Unity 2020.3.2f1. I want to thank Joseph Judge since I used many of the sprites from his "I Can't Believe it's Not Sonic 1!" project. I also want to not that this project features a scene loader built by The Tech Academy that I will eventually replace with my own to better suit the project. Other than the one script related to the scene loader, all the script in this project was written solely by me.
## Story 1 - The Basic Scenes
I created 3 scenes so far for this project. The Title Menu, the Green Hill Zone Act 1 scene, and an End Menu. Rather than implementing buttons to chose what to do between the scenes, I chose to use the get GetKeyDown() function for both the beginning and ending scenes with onscreen instruction for the player. 

![image](https://user-images.githubusercontent.com/87107050/135880820-388d01c1-8ab9-4f80-82ba-2bdc4ae5067b.png)
![image](https://user-images.githubusercontent.com/87107050/135881294-b2a985b4-7bc0-4f8f-a6ab-bf671bdc5dde.png)

The transition from the game scene to the End Menu occures when Sonic has lost his last life or makes it to the end of the level.

![SonicGameOver](https://user-images.githubusercontent.com/87107050/135884539-2919abf4-08f0-4e70-bbcf-3a06354e8c31.gif)
![SonicLevelEnd](https://user-images.githubusercontent.com/87107050/135885684-5e1a7afa-d9fc-4677-909f-bdb05403fddf.gif)

I will be adding more images and animations to the Title Menu and End Menu as I continue working on this project.
## Story 2 - The Level and Sonic's Controller
### The Level
In order to build Act 1 of the Green Hill Zone, I used the sprites I found in Joseph Judge's project and added 2D edge colliders and a few polygon colliders to make prefabs I could drag and drop into position to create the level. I also included 2D platform effectors for almost every 2D collider since Sonic is able to jump through most surfaces from below them.

![image](https://user-images.githubusercontent.com/87107050/135888105-fe9d955d-10f1-42e4-8a55-0bbe68a2e37f.png)

I then added each prefab to construct the terrain for the entire level.

![image](https://user-images.githubusercontent.com/87107050/135888471-b07d4b57-16df-4933-b0bd-e32ffc4ebf31.png)

### Sonic's Controller
After playing around with the original Sonic the Hedgehog game, I realized his controls were much more complex that I had first thought when I chose the project, so I decided to make a finite state machine so the controller's code could be more organized and easily extendable. In my main Sonic Controller script, I set up my basic process to transfer control over sonic's available actions to his various state scripts:

    private void Start()
    {
        TransitionToState(IdleState);
    }
    
    private void Start()
    {
        TransitionToState(IdleState);       
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //This calls the OnCollisionEnter2D method for the currentState.
        currentState.OnCollisionEnter2D(this);
    }
    
With the Idle state as the starting default state, I then added the ability to move into other states based on user input:

    public class SonicIdleState : SonicBaseState
    {
    public override void EnterState(SonicController_FSM player)
    {
        //Enables the capsule collider and disables the circle collider.
        player.CircleCollider2d.enabled = false;
        player.CapsuleCollider2d.enabled = true;

        //Activates the Idle animation.
        player.SetAnimation("Sonic_Idle");

        player.isDeadly = false;
    }

    public override void OnCollisionEnter2D(SonicController_FSM player)
    {
        
    }

    public override void Update(SonicController_FSM player)
    {
        //Checks to see if sonic is grounded via raycast and for user input.
        if (player.IsGrounded() && Input.GetButtonDown("Jump"))
        {  
            //Gives sonic a new vertical velocity while maintaining his current horizontal velocity.
            player.RigidBody2d.velocity = new Vector2(player.RigidBody2d.velocity.x, player.jumpForce);
            //Transitions control to the JumpingState script.
            player.TransitionToState(player.JumpingState);
        }

        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            //Gives sonic a positive velocity of initialSpeed while maintaining his current vertical velocity.
            player.RigidBody2d.velocity = new Vector2(player.initialSpeed, player.RigidBody2d.velocity.y);
            //Transitions control to the RunningRightState script.
            player.TransitionToState(player.RunningRightState);
        }

        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            //Gives sonic a negative velocity of initialSpeed while maintaining his current vertical velocity.
            player.RigidBody2d.velocity = new Vector2(-player.initialSpeed, player.RigidBody2d.velocity.y);
            //Transitions control to the RunningLeftState script.
            player.TransitionToState(player.RunningLeftState);
        }

        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            //Transistions control to the DuckingState script.
            player.TransitionToState(player.DuckingState);
        }

        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            //Transitions control to the LookingUpState script.
            player.TransitionToState(player.LookingUpState);
        }
    }
    }

Besides the abstract base state, Sonic has 11 currently working states with some of them dependent on others to access. For example, Sonic can only move into his rolling state from his running or jumping states and not directly from his idle state:

    public class SonicRunningRightState : SonicBaseState
    {
    public override void EnterState(SonicController_FSM player)
    {
        //Enables Sonic's capsule and box colliders and disables the circle collider.
        player.CircleCollider2d.enabled = false;
        player.CapsuleCollider2d.enabled = true;
        //player.BoxCollider2d.enabled = true;

        //Activates the running animation.
        player.SetAnimation("Sonic_Running");

        //Makes Sonic's sprite unreversed for rightward movement.
        player.ReverseSprite(false);

        player.isDeadly = false;
    }

    public override void OnCollisionEnter2D(SonicController_FSM player)
    {
        
    }

    public override void Update(SonicController_FSM player)
    {
        //Adjusts animation based on Sonic's speed.
        player.SpeedCheck();

        //Checks if Sonic is grounded and the jump button has been pressed.
        if (player.IsGrounded() && Input.GetButtonDown("Jump"))
        {
            //Gives Sonic a new vertical velocity while leaving the horizontal velocity the same.
            player.RigidBody2d.velocity = new Vector2(player.RigidBody2d.velocity.x, player.jumpForce);
            //Gives control to the JumpingRightState script.
            player.TransitionToState(player.JumpingRightState);
        }

        //Checks to see if the player is standing next to a collider and 
        //if one of the left movement keys is being pressed.
        if (player.IsPushing() && (Input.GetKey("d") || Input.GetKey("right")))
        {
            //Gives control to the PushingState script.
            player.TransitionToState(player.PushingState);
        }

        //Checks to see if the player continues to press the movement key related to this state.
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            //When the updatedSpeed is equal to the initialSpeed variable, it is increased 
            //by the acceleration value multiplied by Time.detlaTime.
            if (player.updatedSpeed == player.initialSpeed)
            {
                player.updatedSpeed = player.initialSpeed + player.acceleration * Time.deltaTime;
            }
            //When the updatedSpeed is above the initialSpeed variable, it is increased 
            //by the acceleration value multiplied by Time.deltaTime.
            else if (player.updatedSpeed > player.initialSpeed)
            {
                player.updatedSpeed += player.acceleration * Time.deltaTime;
            }

            //When the updatedSpeed goes beyond the maxSpeed value, it resets it to maxSpeed.
            if (player.updatedSpeed >= player.maxSpeed)
            {
                player.updatedSpeed = player.maxSpeed;
            }

            //Gives Sonic an updated horizontal velocity while keeping the vertical velocity the same.
            player.RigidBody2d.velocity = new Vector2(player.updatedSpeed, player.RigidBody2d.velocity.y);
        }
        //When the player is not holding down the movement key, sonic slows down.
        //This ensures his animations change with his speed.
        else
        {
            if (player.updatedSpeed > player.initialSpeed)
            {
                player.updatedSpeed -= player.acceleration * Time.deltaTime;
            }
        }

        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            //Reset's the updatedSpeed variable before switching to moving the opposite direction.
            player.updatedSpeed = player.initialSpeed;
            player.RigidBody2d.velocity = new Vector2(-8, player.RigidBody2d.velocity.y);
            //Gives control to the RunningLeftState script.
            player.TransitionToState(player.RunningLeftState);
        }

        if (player.updatedSpeed <= player.initialSpeed)
        {
            //When sonic slows down, the updatedSpeed variable is reset, so he doesn't 
            //take off at full speed next time he starts running.
            player.updatedSpeed = player.initialSpeed;
            //Gives control back to the IdleState script.
            player.TransitionToState(player.IdleState);
        }

        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            //When the player hits the down key, it switches control to the RollingLeftState script.
            player.TransitionToState(player.RollingRightState);
        }
    }
    }
    
One of the features in his runnings states that I am very happy with is his acceleration and deacceleration based on whether or not the player is holding down one of the movement keys with his animation changing based on his current speed. The above is just one example of the different state scripts I wrote for Sonic. Feel free to take a look at the others within the scripts folder. Here's a clip that shows Sonic accelerating and shifting into his rolling state:

![SonicRollingInTunnel](https://user-images.githubusercontent.com/87107050/135913450-43c4ad8d-0e74-4d7e-9396-415505712351.gif)

## Story 3 - Enemies and Their Behaviors
I started the third story by putting together the enemy sprites and animations into prefabs and added script for each one. The challenging thing about this story was that each enemy had slightly different behaviors that had to be accounted for in their scripts. Some required triggers to activate when Sonic drew near. Some had to use kinematic rigidbodies, while others had to be dynamic. Some had to be able to travel across colliders while others had to pass through them. Each enemy had its own set of behaviors that came with new challenges. Here are a couple examples to contrast:

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
    //Creates fields in the inspector to place the Buzz Bomber's animator, 
    //sprite renderer, circle collider, and capsule collider.
    public Animator buzzBomberAnimator;
    public SpriteRenderer buzzBomberSprite;
    public CircleCollider2D buzzBomberCircleCollider;
    public CapsuleCollider2D buzzBomberCapsuleCollider;

    //Creates an array field in the inspector to place the two waypoints used by the Buzz Bomber.
    public Transform[] waypoints = new Transform[2];
    //Creates a field in the inspector to place the Buzz Bomber's BombSpawn child 
    //object to set the location from which it spawns its bombs.
    public Transform spawnPoint;

    //Set's the bool value to true to enable the Move() function to keep 
    //running as long as the Buzz Bomber isn't dead.
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
                //Plays the explosion animation, deactivates all unnecessary sprites, 
                //and destroys the object when its finished.
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
            //Calls the Move() function every frame but only starts when the BuzzBomberTrigger 
            //sets the Buzz Bomber's speed to 5;
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
            //If the current waypoint is the first waypoint, it changes the location of certain 
            //animations and the bomb spawner object in relation to the Buzz Bomber
            //It also reverts the animation's sprite renderers so they face the correct way.
            if (current == 0)
            {
                buzzBomberSprite.flipX = false;
                buzzBomberChildAnimations[1].transform.localPosition = new Vector2(0.17f, -0.06f);
                buzzBomberChildAnimations[1].flipX = false;
                buzzBomberChildAnimations[2].transform.localPosition = new Vector2(-0.2f, -0.27f);
                buzzBomberChildAnimations[2].flipX = false;
                bombSpawner.transform.localPosition = new Vector2(-0.2f, -0.27f);
            }
            //If the current waypoint is the second waypoint, it changes the location of certain 
            //animations and the bomb spawner object in relation to the Buzz Bomber
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
            //If the value of current becomes 2 or more, it is reset to 0 so the Buzz Bomber 
            //can continue patrolling between the two waypoints.
            if (current >= waypoints.Length)
            {
                current = 0;
            }
            //This series of Invoked functions control the timing of animations, 
            //shooting its bomb, and continuing its movement.
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
            //Depending on which way the Buzz Bomber is facing, the rotation of the buzzBomb 
            //is altered so that it shoots in the correct direction.
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
    
The Buzz Bomber script above is activated when Sonic hits a trigger and sets the enemy's movement speed variable. It then travels back and forth between two waypoints, turning and shooting at Sonic each time.

![Buzz Bomber Movement](https://user-images.githubusercontent.com/87107050/135916279-7576c98a-de36-4d38-a1c4-328642c58298.gif)

The Chopper, on the other hand, had to use gravity to produce the intended jumping affect, and yet it couldn't collide with the bridge colliders. I had to create script to catch the enemy mid fall and then send it jumping again after a brief interval, and I had to use the IsTrigger property for its colliders so that it could move through the various barriers.

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

    //Creates a float that can be adjusted that decides how soon the 
    //chopper initially jumps upon loading.
    public float jumpStart = 0f;

    //Creates a float that can be adjusted to lengthen the time between jumps.
    public float keepJumping = 1f;

    //Declares a transform variable for the jumping point and the point 
    //where the chopper's movement is haulted.
    public Transform jumpPoint;
    public Transform stopPoint;

    //Declares a bool used in a check to move the chopper back to its 
    //jumping spot in the JumpAgain() function.
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
            //Moves the chopper back to it's initial jumping point, which is far enough away 
            //to not trigger the above if statement.
            transform.position = new Vector2(jumpPoint.position.x, jumpPoint.position.y);
            //Set's jumping to false so that this if statement can't run again while the chopper 
            //is moving to the jumping point.
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
                //Set's the chopper's body to kinematic so it doesn't move while 
                //playing the explosion animation.
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
    
![Chopper](https://user-images.githubusercontent.com/87107050/135917520-1f3e2792-8b5c-4388-9e42-155569221978.gif)

Feel free to check out my other enemy scripts. As I continue working on this game, I will come back to these scripts to update their behaviors to be immitate the original game even more faithfully. I'm excited to keep learning about Unity so I can apply even better code to this game and my future projects. In the meantime, I am glad to know that I can use Unity functionality and logic to make a close approximation.

## Story 4 - Gameplay Model
In the last story during the live project, I created the HUD, established Sonic's lives, created the end of zone sign trigger and created an initial system for collecting and losing rings. I've already linked some gifs above showing the game over and victory overlays that pop up when triggered. I also created a timer that causes Sonic to lose a life when it reaches ten minutes. 

While I still have some adjustments I want to make to the ring loss system, I was quite happy to be able to implement that functionality in its current state. Sonic also has the ability to retrieve his lost rings for a time period before they are destroyed. Here's the script from Sonic's controller showing how I instantiate the lost rings he is currently carrying when he takes damage:

    private void LoseRings()
    {
        for (int i = 0; i < rings; i++)
        {
            int randomSpawnPoint = random.Next(0, 5);
            int randomDirection = random.Next(1, 3) == 1 ? random.Next(1, 46) : random.Next(315, 360);
            Instantiate(lostRing, lostRingSpawnPoints[randomSpawnPoint].position, Quaternion.Euler(0, 0, randomDirection));
        }
        rings = 0;
    }
    
![Sonic Losing Rings](https://user-images.githubusercontent.com/87107050/135920931-70d4e663-a989-46e9-8606-d9340b962ade.gif)

## Conclusion

- During this live project, I learned a lot about Unity and became much more comfortable writing C# code to apply to specific situations. However, I know there is much more to learn and practice, and I am looking forward to that process. 

- I learned to pace myself in order to build the basic features within the time limits of a two week sprint in order to create an initial program that can be fine tuned at later stage in development. I had the opportunity to participate in daily stand-up meetings and give account of my progress, my goals, and my roadblocks to my team and project manager. 

- I learned how to do research to find solutions to the problems I was facing, but I also learned how to recognize when I needed to ask for help from my team and manager. 

- I learned a love for game development in its challenges and its victories. Working on this project showed me how fun and fulfilling making games can be and gave me more drive to learn and grow as much as I can in this field.
