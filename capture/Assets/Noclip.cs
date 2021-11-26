using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noclip : MonoBehaviour
{
    public GameObject fpsObject;
    public FirstPersonController fpsController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if the key V is pressed, toggle the fpsController and the Camera main SimpleCameraController component
        if (Input.GetKeyDown(KeyCode.V))
        {
            fpsController.enabled = !fpsController.enabled;
            // toggle the rigidbody and collider on the fpsObject
            fpsObject.GetComponent<Rigidbody>().isKinematic = !fpsObject.GetComponent<Rigidbody>().isKinematic;
            fpsObject.GetComponent<Collider>().enabled = !fpsObject.GetComponent<Collider>().enabled;
            GetComponent<FlyCamera>().enabled = !GetComponent<FlyCamera>().enabled;
        }
    }
}
