using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Every frame, rotate this gameobject slightly by a tiny bit around the y-axis
        transform.Rotate(0, 0.4f, 0);
    }
}
