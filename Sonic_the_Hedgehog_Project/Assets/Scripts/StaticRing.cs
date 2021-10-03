using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticRing : MonoBehaviour
{
    public GameObject ringSparkle;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SonicPlayer")
        {
            Instantiate(ringSparkle, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }
}
