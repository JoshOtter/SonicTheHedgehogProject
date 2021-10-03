using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicPushingState : SonicBaseState
{
    public override void EnterState(SonicController_FSM player)
    {
        player.CircleCollider2d.enabled = false;
        player.CapsuleCollider2d.enabled = true;
        //player.BoxCollider2d.enabled = true;

        player.SetAnimation("Sonic_Pushing");

        player.isDeadly = false;
    }

    public override void OnCollisionEnter2D(SonicController_FSM player)
    {
        
    }

    public override void Update(SonicController_FSM player)
    {
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            player.ReverseSprite(true);
        }
        else if (Input.GetKey("d") || Input.GetKey("right"))
        {
            player.ReverseSprite(false);
        }

        if (Input.GetKeyUp("a") || Input.GetKeyUp("left") || Input.GetKeyUp("d") || Input.GetKeyUp("right"))
        {
            player.TransitionToState(player.IdleState);
        }

        if (player.IsGrounded() && Input.GetKey("space"))
        {
            player.RigidBody2d.velocity = new Vector2(player.RigidBody2d.velocity.x, player.jumpForce);
            player.TransitionToState(player.JumpingState);
        }
    }
}
