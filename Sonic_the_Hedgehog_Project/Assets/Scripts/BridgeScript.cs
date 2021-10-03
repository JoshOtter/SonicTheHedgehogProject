using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This Script will recieve more attention at a later time.
public class BridgeScript : MonoBehaviour
{
    public SpriteRenderer[] bridgeNodes;

    // Start is called before the first frame update
    void Start()
    {
        bridgeNodes = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
