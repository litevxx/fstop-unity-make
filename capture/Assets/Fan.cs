using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    // The fan raycasts in the direction it is facing and checks for every object with the "Capturable" tag. It then loops over them, and applies a force to them in the direction of the fan.
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Fan code
        // Only push objects when the fan is not being placed, check Capturable beingPlaced
        if(gameObject.GetComponent<Capturable>().beingPlaced == false)
        {
        Vector3 fanDirection = transform.up;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, fanDirection, 100);
        foreach (RaycastHit hit in hits)
        {
            // print(hit.transform.name);
            if (hit.collider.tag == "Capturable")
            {
                // if has rigidbody
                if (hit.collider.GetComponent<Rigidbody>() != null)
                {
                hit.rigidbody.AddForce(fanDirection * 0.3f, ForceMode.VelocityChange);
                }
            }
            // if its the player, and the fan is rotated to be sideways (the x axis) we need to move really fast
            if (hit.collider.tag == "Player" && transform.rotation.x != 0 && transform.rotation.x != 180)
            {
                print("time to become racist");
                hit.rigidbody.AddForce(fanDirection * 3f, ForceMode.VelocityChange);
            }
            // if its the player but we are not sideways, move at normal speed
            if (hit.collider.tag == "Player" && transform.rotation.x == 0 || transform.rotation.x == 180)
            {
                hit.rigidbody.AddForce(fanDirection * 0.3f, ForceMode.VelocityChange);
            }
        }
        }
    }

}