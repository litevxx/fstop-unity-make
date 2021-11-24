using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject devConsole;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.X)){
        if(!isPaused){
        GetComponent<FirstPersonController>().enabled = false;
        Camera.main.GetComponent<copilot2>().enabled = false;
        // unlock and unhide cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // pause time
        // Time.timeScale = 0;
        // enable the DeveloperConsole component on devconsole
        devConsole.GetComponent<Console.DeveloperConsole>().enabled = true;
        isPaused = true;
        }else{
            // undo everything
            GetComponent<FirstPersonController>().enabled = true;
            Camera.main.GetComponent<copilot2>().enabled = true;
            // lock and hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // unpause time
            //Time.timeScale = 1;
            // disable the DeveloperConsole component on devconsole
            devConsole.GetComponent<Console.DeveloperConsole>().enabled = false;
            isPaused = false;
        }
        }
    }
}
