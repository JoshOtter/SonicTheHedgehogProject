using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //Checks to see if the player is standing next to a collider and if one of the left movement keys is being pressed.
        if (player.IsPushing() && (Input.GetKey("d") || Input.GetKey("right")))
        {
            //Gives control to the PushingState script.
            player.TransitionToState(player.PushingState);
        }

        //Checks to see if the player continues to press the movement key related to this state.
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            //When the updatedSpeed is equal to the initialSpeed variable, it is increased by the acceleration value multiplied by Time.detlaTime.
            if (player.updatedSpeed == player.initialSpeed)
            {
                player.updatedSpeed = player.initialSpeed + player.acceleration * Time.deltaTime;
            }
            //When the updatedSpeed is above the initialSpeed variable, it is increased by the acceleration value multiplied by Time.deltaTime.
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
        //When the player is not holding down the movement key, sonic slows down, ensuring his animations change with his speed.
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
            //When sonic slows down, the updatedSpeed variable is reset, so he doesn't take off at full speed next time he starts running.
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
