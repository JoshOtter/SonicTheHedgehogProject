using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicJumpingState : SonicBaseState
{
    public override void EnterState(SonicController_FSM player)
    {
        //Disables Sonic's box and capsule colliders and enables the circle collider.
        //player.BoxCollider2d.enabled = false;
        player.CapsuleCollider2d.enabled = false;
        player.CircleCollider2d.enabled = true;

        //Activates the spinning animation.
        player.SetAnimation("Sonic_Spinning");

        player.isDeadly = true;
    }

    public override void OnCollisionEnter2D(SonicController_FSM player)
    {
        //When Sonic collides with the ground, it gives control back to the IdleState script.
        player.TransitionToState(player.IdleState);
    }

    public override void Update(SonicController_FSM player)
    {
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            //When the player presses a button to move right, Sonic is given a new x velocity while his current y velocity stays the same.
            player.RigidBody2d.velocity = new Vector2(player.initialSpeed, player.RigidBody2d.velocity.y);
            //Gives control to the JumpingRightState script.
            player.TransitionToState(player.JumpingRightState);
        }

        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            //When the player presses a button to move right, Sonic is given a new x velocity while his current y velocity stays the same.
            player.RigidBody2d.velocity = new Vector2(-player.initialSpeed, player.RigidBody2d.velocity.y);
            //Gives control to the JumpingRightState script.
            player.TransitionToState(player.JumpingLeftState);
        }
    }
}
