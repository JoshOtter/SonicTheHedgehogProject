using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingNewtronTrigger : MonoBehaviour
{
    //Creates a field in the inspector to place the related newtron enemy object.
    public ShootingNewtron shootingNewtron;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks for a collision with Sonic.
        if (collision.gameObject.tag == "SonicPlayer")
        {
            //Activates the related newtron's Move() function by changing its speed from 0 to 5.
            shootingNewtron.activate = true;
            //Destroys this game object after activation, so it does cause any interruptsions with the newtron's behaviors.
            Destroy(gameObject);
        }
    }
}
