using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicDuckingState : SonicBaseState
{
    public override void EnterState(SonicController_FSM player)
    {
        //Disables Sonic's box and capsule colliders and enables his circle collider.
        //player.BoxCollider2d.enabled = false;
        player.CapsuleCollider2d.enabled = false;
        player.CircleCollider2d.enabled = true;

        //This ensure's sonic is not moving when looking up.
        player.RigidBody2d.velocity = new Vector2(0f, 0f);

        //This activates the ducking animation.
        player.SetAnimation("Sonic_Ducking");

        player.isDeadly = false;
    }

    public override void OnCollisionEnter2D(SonicController_FSM player)
    {
        
    }

    public override void Update(SonicController_FSM player)
    {
        if (Input.GetKeyUp("s") || Input.GetKeyUp("down"))
        {
            //This switches control back to the IdleState when the up key is released.
            player.TransitionToState(player.IdleState);
        }
    }
}
