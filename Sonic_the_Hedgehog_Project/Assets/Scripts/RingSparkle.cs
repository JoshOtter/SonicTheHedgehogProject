using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSparkle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Vanish());
    }

    private IEnumerator Vanish()
    {
        yield return new WaitForSeconds(0.35f);
        Destroy(gameObject);
    }
}
