using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //Set's the movement speed of the projectile.
    public float speed = 5f;

    void Update()
    {
        //Moves the projectile forward based on its beginning position and rotation.
        transform.position += transform.right * -speed * Time.deltaTime;
        //Invokes the SelfDestruct() function after 10 seconds.
        Invoke("SelfDestruct", 10f);
    }

    //The projectile object is destroyed so as to limit the number of projectiles in the scene.
    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
