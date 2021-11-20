using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickpad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // On collision enter, if colliding object has Capturable tag, set its Rigidbody to kinematic, but only if GameObject.GetComponent<Capturable>().beingPlaced is false
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Capturable")
        {
            if (!collision.gameObject.GetComponent<Capturable>().beingPlaced)
            {
                collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
