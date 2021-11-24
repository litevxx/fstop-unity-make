using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanButton : MonoBehaviour
{
    // This is like the DoorButton, but instead of hiding the object on exit it disables the Fan component, then loops over all of its children and disables their FanBlade components.
    // When the collider leaves again, it re-enables the Fan component and re-enables the FanBlade components.

    public GameObject fan;
    public Material btnInactiveMat;
    public Material btnActiveMat;
    // Start is called before the first frame update
    void Start()
    {
        // set inactive mat
        gameObject.GetComponent<Renderer>().material = btnInactiveMat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
            fan.GetComponent<Fan>().enabled = true;
            foreach (Transform child in fan.transform)
            {
                child.gameObject.GetComponent<FanBlade>().enabled = true;
            }
            // set active mat
            gameObject.GetComponent<Renderer>().material = btnActiveMat;

    }
    void OnCollisionExit(Collision collision)
    {

            fan.GetComponent<Fan>().enabled = false;
            foreach (Transform child in fan.transform)
            {
                child.gameObject.GetComponent<FanBlade>().enabled = false;
            }
            // set inactive mat again
            gameObject.GetComponent<Renderer>().material = btnInactiveMat;
    }

}
