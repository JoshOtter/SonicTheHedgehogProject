using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SonicController_FSM : MonoBehaviour
{
    //These are used for the raycast ground and tunnel checks at the bottom of this class.
    [SerializeField] private LayerMask terrainLayerMask;
    [SerializeField] private LayerMask tunnelLayerMask;

    public TMP_Text currentLives;
    public TMP_Text currentRings;
    public TMP_Text currentScore;

    public Canvas gameOver;

    public GameObject lostRing;

    System.Random random = new System.Random();

    public Transform[] lostRingSpawnPoints = new Transform[5];

    private Vector2 hurtJump;
    
    //These variables are used to determine Sonic's initial movement speed, and accelerate him to his max speed.
    public float initialSpeed = 5f;
    public float maxSpeed = 15f;
    public float acceleration = 5f;
    public float updatedSpeed = 5f;

    //This is used to make Sonic jump the same height as the original game.
    public float jumpForce = 14.9f;

    //This is used to keep track of Sonic's score throughout the level.
    public int score = 0;

    //This is used to keep track of how many rings Sonic has in his possession.
    public int rings = 0;

    //This is used to check if Sonic is currently dead, to prevent him from switching states after dying.
    public bool isDead = false;

    //This is used to check if Sonic is spinning, either in a jump or a roll, so he can destroy enemies on collision.
    public bool isDeadly = false;

    //These are used to set variables to hold the instantiated components attached to Sonic.
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;
    public CapsuleCollider2D CapsuleCollider2d { get { return capsuleCollider; } }
    //private BoxCollider2D boxCollider;
    //public BoxCollider2D BoxCollider2d { get { return boxCollider; } }
    private CircleCollider2D circleCollider;
    public CircleCollider2D CircleCollider2d { get { return circleCollider; } }
    public Rigidbody2D rb2d;
    public Rigidbody2D RigidBody2d { get { return rb2d; } }
    

    //This is used to reference the current active state for each function.
    private SonicBaseState currentState;

    public SonicSceneController SonicRespawn;

    //This instantiates objects for each of Sonic's states.
    public readonly SonicIdleState IdleState = new SonicIdleState();
    public readonly SonicRunningRightState RunningRightState = new SonicRunningRightState();
    public readonly SonicRunningLeftState RunningLeftState = new SonicRunningLeftState();
    public readonly SonicJumpingState JumpingState = new SonicJumpingState();
    public readonly SonicJumpingRightState JumpingRightState = new SonicJumpingRightState();
    public readonly SonicJumpingLeftState JumpingLeftState = new SonicJumpingLeftState();
    public readonly SonicDuckingState DuckingState = new SonicDuckingState();
    public readonly SonicRollingRightState RollingRightState = new SonicRollingRightState();
    public readonly SonicRollingLeftState RollingLeftState = new SonicRollingLeftState();
    public readonly SonicPushingState PushingState = new SonicPushingState();
    public readonly SonicLookingUpState LookingUpState = new SonicLookingUpState();

    private void Awake()
    {
        //These collect Sonic's components within the variables.
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        //boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        //This sets Sonic's inital animation to "Idle"
        SetAnimation("Sonic_Idle");

        //This disables Sonic's circle collider when the program is first run.
        circleCollider.enabled = false;
    }

    private void Start()
    {
        //Sets the currentState to IdleState.
        TransitionToState(IdleState);

        gameOver.enabled = false;

        if (GameValues.lives <= 0)
        {
            GameValues.lives = 3;
        }
    }

    void Update()
    {
        if (!isDead)
        {
            //This calls the Update method within the currentState.
            currentState.Update(this);
        }
       
        DisplayLives(GameValues.lives);
        DisplayRings(rings);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SonicRing")
        {
            rings++;
        }
        else if (collision.gameObject.tag == "SonicEnemyBullet")
        {
            if (rings > 0)
            {
                LoseRings();
                GetHurt();
            }
            else
            {
                AssessLives();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SonicPit")
        {
            isDead = true;
            AssessLives();
        }
        else if (collision.gameObject.tag == "SonicEnemy")
        {
            if (!isDeadly)
            {
                if (rings > 0)
                {
                    LoseRings();
                    GetHurt();
                }
                else
                {
                    AssessLives();
                }
            }          
        }
        else if (collision.gameObject.tag == "SonicSpikes")
        {
            if (rings > 0)
            {
                LoseRings();
                GetHurt();
            }
            else
            {
                AssessLives();
            }
        }
        else
        {
            //This calls the OnCollisionEnter2D method for the currentState.
            currentState.OnCollisionEnter2D(this);
        }
        
    }

    public void TransitionToState(SonicBaseState state)
    {
        //This takes the name of the state to be entered and switches control to that state's script.
        currentState = state;
        currentState.EnterState(this);
    }

    public void SetAnimation(string newAnimation)
    {
        //This sets the appropriate animations for each state based on the animation names.
        animator.Play(newAnimation);
    }

    public void ReverseSprite(bool flip)
    {
        //This flips the animating sprite when sonic moves backwards.
        spriteRenderer.flipX = flip;
    }

    public bool IsGrounded()
    {
        //This sets an extra length for the raycast to extend.
        float extraHeight = 0.1f;

        //Sends out a raycast from the center of Sonic's capsuleCollider to check for a collider beneath him.
        RaycastHit2D raycastHit = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.down, capsuleCollider.bounds.extents.y + extraHeight, terrainLayerMask);
        //Debug.DrawRay(capsuleCollider.bounds.center, Vector2.down * (capsuleCollider.bounds.extents.y + extraHeight));
        //Debug.Log(raycastHit.collider);

        //Returns true as long as the raycast is hitting a surface below him.
        return raycastHit.collider != null;
    }

    public bool IsGroundedSpin()
    {
        //This sets an extra length for the raycast to extend.
        float extraHeight = 0.1f;

        //Sends out a raycast from the center of Sonic's circleCollider when he is rolling to check for a collider beneath him.
        RaycastHit2D raycastHit = Physics2D.Raycast(circleCollider.bounds.center, Vector2.down, circleCollider.bounds.extents.y + extraHeight, terrainLayerMask);
        //Debug.DrawRay(circleCollider.bounds.center, Vector2.down * (circleCollider.bounds.extents.y + extraHeight));
        //Debug.Log(raycastHit.collider);

        //Returns true as long as the raycast is hitting a surface below him.
        return raycastHit.collider != null;
    }

    public bool IsNotInTunnel()
    {
        //This sets an extra length for the raycast to extend.
        float extraDistance = 0.5f;

        //Sends out two raycasts from the center of Sonic's circleCollider, one above and one to the right, to check for colliders when he is in a tunnel.
        RaycastHit2D raycastTopHit = Physics2D.Raycast(circleCollider.bounds.center, Vector2.up, circleCollider.bounds.extents.y + extraDistance, tunnelLayerMask);
        RaycastHit2D raycastSideHit = Physics2D.Raycast(circleCollider.bounds.center, Vector2.right, circleCollider.bounds.extents.x + extraDistance, tunnelLayerMask);
        //Debug.DrawRay(circleCollider.bounds.center, Vector2.right * (circleCollider.bounds.extents.x + extraDistance));
        //Debug.DrawRay(circleCollider.bounds.center, Vector2.up * (circleCollider.bounds.extents.y + extraDistance));
        //Debug.Log(raycastTopHit.collider);

        //Returns true as long as neither raycast hits another surface.
        return raycastTopHit.collider == null && raycastSideHit.collider == null;
    }

    public bool IsPushing()
    {
        //This sets an extra length for the raycast to extend.
        float extraLength = 0.05f;

        //Sends out two raycasts from the center of Sonic's capsuleCollider, one to the left and one to the right, to check for colliders he is standing next to.
        RaycastHit2D rightCheck = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.right, capsuleCollider.bounds.extents.x + extraLength, terrainLayerMask);
        RaycastHit2D leftCheck = Physics2D.Raycast(capsuleCollider.bounds.center, Vector2.left, capsuleCollider.bounds.extents.x + extraLength, terrainLayerMask);

        //Returns true if either of the raycast hits another surface.
        return rightCheck.collider != null || leftCheck.collider != null;

    }

    public void SpeedCheck()
    {
        //When Sonic's speed increases above 12, his animation changes to fast running.
        if (updatedSpeed > 12f)
        {
            SetAnimation("Sonic_Run_Fast");
        }
        //When Sonic's speed is 12 or below, his animation switches back to normal running.
        else if (updatedSpeed <= 12f)
        {
            SetAnimation("Sonic_Running");
        }
    }

    public void AssessLives()
    {
        if (!isDead)
        {
            isDead = true;
            GameValues.lives--;
            Debug.Log("You just lost a life!");
        }
        
        if (GameValues.lives > 0)
        {
            isDead = true;
            SonicDead();
        }
        else
        {
            isDead = true;
            GameOver();
        }
    }

    private void GetHurt()
    {
        float xDirection;
        if (spriteRenderer.flipX == false)
        {
            xDirection = -5f;
        }
        else
        {
            xDirection = 5f;
        }
        hurtJump = new Vector2(xDirection, 10f);
        rb2d.AddRelativeForce(hurtJump, ForceMode2D.Impulse);                    
    }

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

    public void SonicDead()
    {
        SetAnimation("Sonic_Dead");
        //boxCollider.enabled = false;
        capsuleCollider.enabled = false;
        circleCollider.enabled = false;
        RigidBody2d.velocity = new Vector2(0f, jumpForce);
        Invoke("Respawn", 1f);
    }

    public void GameOver()
    {
        SetAnimation("Sonic_Dead");
        gameOver.enabled = true;
        //boxCollider.enabled = false;
        capsuleCollider.enabled = false;
        circleCollider.enabled = false;
        RigidBody2d.velocity = new Vector2(0f, jumpForce);
        Invoke("EndMenu", 1f);
    }

    private void Respawn()
    {
        SonicRespawn.RestartLevel();
    }

    private void EndMenu()
    {
        SonicRespawn.LoadNextLevel();
    }

    void DisplayLives(int livesToDisplay)
    {
        currentLives.SetText(livesToDisplay.ToString());
    }

    void DisplayRings(int ringsToDisplay)
    {
        currentRings.SetText(ringsToDisplay.ToString());
    }
}
