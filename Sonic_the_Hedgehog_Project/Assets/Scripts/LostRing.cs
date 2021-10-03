using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostRing : MonoBehaviour
{
    public GameObject ringSparkle;

    public Rigidbody2D ringRb2d;

    public float speed = 10f;

    void Start()
    {
        //Retrieves and instantiates the Rigidbody2D component.
        ringRb2d = GetComponent<Rigidbody2D>();
        //Immediately sends the lost ring shooting forward from its location (the rotation is controlled in the SonicController_FSM script).
        ringRb2d.AddForce(transform.up * speed, ForceMode2D.Impulse);
        StartCoroutine(RingVanish());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SonicPlayer")
        {
            Instantiate(ringSparkle, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }

    private IEnumerator RingVanish()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
