using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicRollingRightState : SonicBaseState
{
    public override void EnterState(SonicController_FSM player)
    {
        //Disables Sonic's box and capsule colliders and enables his circle collider.
        //player.BoxCollider2d.enabled = false;
        player.CapsuleCollider2d.enabled = false;
        player.CircleCollider2d.enabled = true;

        //Activates the spinning animation.
        player.SetAnimation("Sonic_Spinning");

        //Reverses the sprite for leftward movement.
        player.ReverseSprite(false);

        player.isDeadly = true;
    }

    public override void OnCollisionEnter2D(SonicController_FSM player)
    {
        
    }

    public override void Update(SonicController_FSM player)
    {
        //Checks if the input button is being pressed and if Sonic is standing still.
        if ((Input.GetKey("s") || Input.GetKey("down")) && player.RigidBody2d.velocity.x == 0)
        {
            //Gives control to the DuckingState script.
            player.TransitionToState(player.DuckingState);
        }

        //Checks if Sonic is Grounded and if the spacebar is being pressed.
        if (player.IsGroundedSpin() && Input.GetButtonDown("Jump"))
        {
            //resets updatedSpeed to initialSpeed.
            player.updatedSpeed = player.initialSpeed;
            //Gives sonic a new vertical velocity while reseting his horizontal velocity.
            player.RigidBody2d.velocity = new Vector2(player.initialSpeed, player.jumpForce);
            //Gives control to the JumpingRightState script.
            player.TransitionToState(player.JumpingRightState);
        }

        //Checks if the player is hitting the opposite movement key.
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            //Gives control to RollingLeftState script.
            player.TransitionToState(player.RollingLeftState);
        }

        //Uses raycast to check if sonic is not in a tunnel and checks to see if he is standing still
        if (player.IsNotInTunnel() && player.RigidBody2d.velocity.x == 0)
        {
            //resets updatedSpeed to initialSpeed.
            player.updatedSpeed = player.initialSpeed;
            //Gives control to the IdleState script.
            player.TransitionToState(player.IdleState);
        }
    }
}
