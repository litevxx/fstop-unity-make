using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capturable : MonoBehaviour
{
    // This script is put on gameobjects that are capturable
    // It can set the following settings:
    // - Whether the object can be captured at the time
    // - Whether the object can be scaled at the time
    // - Whether the object should be aligned to the wall if it is placed there
    // These should be controlled by public inspector variables with descriptions
    [Tooltip("Whether the object can be captured at the time")]
    public bool canBeCaptured = true;
    [Tooltip("Whether the object can be scaled at the time")]
    public bool canBeScaled = true;
    [Tooltip("Whether the object should be aligned to the wall if it is placed there")]
    public bool alignToWall = false;
    [Tooltip("Whether the object should only be scaled by x and y and not z")]
    public bool onlyScaleXY = false;

    // This bool will be set by copilot.cs when we are in the process of placing the object
    // It is used internally for fans to stop the fan while the object is being placed
    [Tooltip("Whether the object is currently being placed")]
    public bool beingPlaced = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set the "Capturable" tag on the GameObject it is added to
        gameObject.tag = "Capturable";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
