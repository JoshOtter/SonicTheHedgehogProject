using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicJumpingRightState : SonicBaseState
{
    public override void EnterState(SonicController_FSM player)
    {
        //Disables Sonic's box and capsule colliders and enables his circle collider.
        //player.BoxCollider2d.enabled = false;
        player.CapsuleCollider2d.enabled = false;
        player.CircleCollider2d.enabled = true;

        //Activates the spinning animation.
        player.SetAnimation("Sonic_Spinning");

        //Reverts the sprite to normal for rightward movement.
        player.ReverseSprite(false);

        player.isDeadly = true;
    }

    public override void OnCollisionEnter2D(SonicController_FSM player)
    {
        //Checks if the down input keys are being pressed when colliding with a ground surface.
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            //maintains the same horizontal and vertical velocity.
            player.RigidBody2d.velocity = new Vector2(player.RigidBody2d.velocity.x, player.RigidBody2d.velocity.y);
            //Gives control to the RollingRightState script.
            player.TransitionToState(player.RollingRightState);
        }
        else
        {
            //Gives control to the RunningRightState script.
            player.TransitionToState(player.RunningRightState);
        }
        
    }

    public override void Update(SonicController_FSM player)
    {
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

        //Checks if the player is entering the opposite movement key.
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            //Set's sonic's horizontal velocity to a negative value of initialSpeed while maintaining the current vertical velocity.
            player.RigidBody2d.velocity = new Vector2(-player.initialSpeed, player.RigidBody2d.velocity.y);
            //Gives control to the JumpingLeftState script.
            player.TransitionToState(player.JumpingLeftState);
        }
    }
}
