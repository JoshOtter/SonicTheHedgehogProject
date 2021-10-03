using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzBomberTrigger : MonoBehaviour
{
    //Creates a field in the inspector to place the related Buzz Bomber enemy object.
    public BuzzBomber buzzBomber;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks for a collision with Sonic.
        if (collision.gameObject.tag == "SonicPlayer")
        {
            //Activates the related BuzzBomber's Move() function by changing its speed from 0 to 5.
            buzzBomber.speed = 5f;
            //Destroys this game object after activation, so it does not interrupt the BuzzBomber's movements.
            Destroy(gameObject);
        }
    }
}
