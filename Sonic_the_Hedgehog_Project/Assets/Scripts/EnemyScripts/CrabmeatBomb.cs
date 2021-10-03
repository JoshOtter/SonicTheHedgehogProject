using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabmeatBomb : MonoBehaviour
{
    //Declares the Rigidbody2D variable for the crabmeat bomb.
    public Rigidbody2D crabmeatBombRb2d;

    //Sets the speed of the crabmeat bomb.
    public float speed = 5f;

    void Start()
    {
        //Retrieves and instantiates the Rigidbody2D component.
        crabmeatBombRb2d = GetComponent<Rigidbody2D>();
        //Immediately sends the crabmeat bomb shooting forward from its location (the rotation is controlled in the crabmeat script.
        crabmeatBombRb2d.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    void Update()
    {
        //Destroys itself after 2 seconds when it is has fallen off the screen, so as to limit the number of bombs in the scene.
        Invoke("SelfDestruct", 2f);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
