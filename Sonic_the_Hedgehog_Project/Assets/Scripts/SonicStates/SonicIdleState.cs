using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicIdleState : SonicBaseState
{
    public override void EnterState(SonicController_FSM player)
    {
        //Enables the capsule and box colliders and disables the circle collider.
        player.CircleCollider2d.enabled = false;
        player.CapsuleCollider2d.enabled = true;
        //player.BoxCollider2d.enabled = true;

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
