using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    // The purpose of this script is to call OpenDoor() in the "Door" component of another gameobject when an object triggers the collider of the button, and then call CloseDoor() in the "Door" component of the same gameobject when the object leaves the collider
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Capturable" || other.gameObject.tag == "Player")
        {
            door.GetComponent<Door>().OpenDoor();
            // set this gameobjects material color to be green using SetColor
            GetComponent<Renderer>().material.SetColor("_Color", Color.green);

        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Capturable" || other.gameObject.tag == "Player")
        {
            door.GetComponent<Door>().CloseDoor();
            // now set it back to red
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }
}