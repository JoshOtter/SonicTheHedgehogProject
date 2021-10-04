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
    
One of the features in his runnings states that I am very happy with is his acceleration and deacceleration based on whether or not the player is holding down one of the movement keys with his animation changing based on his current speed. The above is just one example of the different state scripts I wrote for Sonic. Feel free to take a look at the others within the scripts folder.
