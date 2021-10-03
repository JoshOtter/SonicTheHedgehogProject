using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicLookingUpState : SonicBaseState
{
    public override void EnterState(SonicController_FSM player)
    {
        //Enables capsule and box colliders and disables the circle collider.
        player.CircleCollider2d.enabled = false;
        player.CapsuleCollider2d.enabled = true;
        //player.BoxCollider2d.enabled = true;

        //Activates the Looking Up animation.
        player.SetAnimation("Sonic_Looking_Up");

        player.isDeadly = false;
    }

    public override void OnCollisionEnter2D(SonicController_FSM player)
    {
        
    }

    public override void Update(SonicController_FSM player)
    {
        if (Input.GetKeyUp("w") || Input.GetKeyUp("up"))
        {
            //Returns control to the IdleState script when the key is released.
            player.TransitionToState(player.IdleState);
        }

        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            //Sets new positive horizontal velocity.
            player.RigidBody2d.velocity = new Vector2(player.initialSpeed, player.RigidBody2d.velocity.y);
            //Transfers control to the RunningRightState script.
            player.TransitionToState(player.RunningRightState);
        }

        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            //Sets new negative horizontal velocity.
            player.RigidBody2d.velocity = new Vector2(-player.initialSpeed, player.RigidBody2d.velocity.y);
            //Transfers control to the RunningLeftState script.
            player.TransitionToState(player.RunningLeftState);
        }
    }
}
