using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class copilot2 : MonoBehaviour
{
    // This is a script put on the main camera that has two modes, capture mode and placement mode.
    // In capture mode, I can click my left mouse button on an object and it will be captured and capturedObj will be set to it.
    // In placement mode, this object will be moved to my mouse position. When I click the left mouse button, it will be placed and I will go back to capture mode.

    // When an object is captured, its collider is disabled and rigidbody set to kinematic. Its material is set to fade mode and the material color transparency is set to 0.4f
    // After the object is placed, we reenable the collider and rigidbody and set the material to normal mode and the material color transparency to 1.0f

    // whether we are in capture or placement mode
    public bool captureMode = true;
    // currently captured object
    private GameObject capturedObj;
    // a sound source
    public AudioSource soundSource;
    // a captured sound
    public AudioClip capturedSound;
    // a placed sound
    public AudioClip placedSound;
    // a denied sound
    public AudioClip deniedSound;
    // a textmeshpro object that will be set to "capture" when capture mode starts and "place" when placement mode starts
    public TextMeshProUGUI captureText;

    public Image crosshairUI;
    public Sprite crosshairDefault;
    public Sprite crosshairSelected;

    /* We also need the following functions:
    A function that sets a specified material to fade mode
    A function that sets a specified material to normal mode
    A function that changes the alpha of a material using SetColor
    */

    void SetFadeMode(Material material)
    {
        // Set the material to fade mode
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }

    void SetNormalMode(Material material)
    {
        // Set the material to normal mode
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }

    void SetAlpha(Material material, float alpha)
    {
        // Set the alpha of the material
        Color color = material.color;
        color.a = alpha;
        material.SetColor("_Color", color);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set the captureText to "capture"
        captureText.text = "capture";
    }
    void Update()
    {
        if(captureMode)
        {
            // If we are in capture mode, we can click the left mouse button on an object to capture it
            if(Input.GetMouseButtonDown(0))
            {
                // If we clicked on an object, capture it
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    // If the object is tagged "Capturable" and has a capturable component, capture it
                    if(hit.transform.tag == "Capturable" && hit.transform.GetComponent<Capturable>() != null)
                    {
                        if(hit.transform.GetComponent<Capturable>().canBeCaptured)
                        {
                        // Set the capturedObj to the object we clicked on
                        capturedObj = hit.transform.gameObject;
                        // Set the capturedObj's collider to disabled
                        capturedObj.GetComponent<Collider>().isTrigger = true;
                        // Set the capturedObj's rigidbody to kinematic, if we have a rigidbody
                        if(capturedObj.GetComponent<Rigidbody>() != null)
                        {
                            capturedObj.GetComponent<Rigidbody>().isKinematic = true;
                        }

                        // Set the capturedObj's material to fade mode
                        //SetFadeMode(capturedObj.GetComponent<Renderer>().material);
                        // Set the capturedObj's alpha to 0.4f
                        //SetAlpha(capturedObj.GetComponent<Renderer>().material, 0.4f);

                        // loop over all materials then set fade mode and alpha 0.4f
                        foreach(Material material in capturedObj.GetComponent<Renderer>().materials)
                        {
                            SetFadeMode(material);
                            SetAlpha(material, 0.4f);
                        }
                        // Set the captureObj's layer to ignore raycasts
                        capturedObj.layer = 2;
                        // Play the captured sound
                        soundSource.PlayOneShot(capturedSound);
                        // Set the captureText to "place"
                        captureText.text = "place";
                        // Set the crosshair to the selected crosshair
                        crosshairUI.sprite = crosshairSelected;
                        // Set the captureMode to false
                        captureMode = false;
                    }else{
                        // play deny sound
                        soundSource.PlayOneShot(deniedSound);
                    }
                    }else{
                        // play deny sound
                        soundSource.PlayOneShot(deniedSound);
                    }
                }
            }
        }else{
                // Raycast forward from the camera, then move the gameobject to the point that was hit.
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    Vector3 lastPosition = capturedObj.transform.position;

                    // Now move the object
                    if(hit.normal.y < 0.5)
                    {
                        capturedObj.transform.position = hit.point + hit.normal * capturedObj.GetComponent < Collider > ().bounds.extents.x;
                    }

                    if(hit.normal.z < -0.5 && !(hit.normal.z > 0.5))
                    {
                        capturedObj.transform.position = hit.point + hit.normal * capturedObj.GetComponent < Collider > ().bounds.extents.z;
                    }
                    if(hit.normal.z > 0.5)
                    {
                        capturedObj.transform.position = hit.point + hit.normal * capturedObj.GetComponent < Collider > ().bounds.extents.z;
                    }

                    if(!(hit.normal.y < 0.5) && !(hit.normal.z < -0.5) && !(hit.normal.z > 0.5))
                    {
                        capturedObj.transform.position = hit.point + hit.normal * capturedObj.GetComponent < Collider > ().bounds.extents.y;
                    }
                    if(capturedObj.GetComponent<Capturable>().alignToWall)
                    {
                       // capturedObj.transform.position = hit.point;
                        capturedObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    }

                    // we want to do a physics check if we are inside of more than two objects now
                    Collider[] colliders = Physics.OverlapBox(capturedObj.transform.position, capturedObj.GetComponent<Collider>().bounds.extents, Quaternion.identity, LayerMask.GetMask("levelGeometry"));
                    if(colliders.Length > 1)
                    {
                        // if we are, we need to move the object out of what we are touching
                        capturedObj.transform.position = lastPosition;                        
                    }
                }
                if (capturedObj.GetComponent < Capturable > ().canBeScaled) {
                    Vector3 scaleAmount = new Vector3(0.3f, 0.3f, 0.3f);
                    if(capturedObj.GetComponent<Capturable>().onlyScaleXY) {
                        scaleAmount = new Vector3(0.3f, 0f, 0.3f);
                     }
				    if (Input.GetAxis("Mouse ScrollWheel") > 0) {
					    // increase the size of the object
					    capturedObj.transform.localScale = Vector3.Lerp(capturedObj.transform.localScale, capturedObj.transform.localScale + scaleAmount, Time.deltaTime * 10);
				    }
				    if (Input.GetAxis("Mouse ScrollWheel") < 0) {
					    // decrease the size of the object
					    capturedObj.transform.localScale = Vector3.Lerp(capturedObj.transform.localScale, capturedObj.transform.localScale - scaleAmount, Time.deltaTime * 10);
				    }
			    }

                // When we press left mouse, uncapture the object by changing everything back
                if(Input.GetMouseButtonDown(0))
                {
                    // Set the capturedObj's material to normal mode
                    // SetNormalMode(capturedObj.GetComponent<Renderer>().material);
                    // Set the capturedObj's alpha to 1
                    //SetAlpha(capturedObj.GetComponent<Renderer>().material, 1);

                    // loop over all materials then set normal mode and alpha 1
                    foreach(Material material in capturedObj.GetComponent<Renderer>().materials)
                    {
                        SetNormalMode(material);
                        SetAlpha(material, 1);
                    }
                    
                    // Set the capturedObj's layer to default
                    capturedObj.layer = 0;
                    // Set the capturedObj's collider to enabled
                    capturedObj.GetComponent<Collider>().isTrigger = false;
                    // Set the capturedObj's rigidbody to not kinematic, if we have a rigidbody
                    if(capturedObj.GetComponent<Rigidbody>() != null)
                    {
                        capturedObj.GetComponent<Rigidbody>().isKinematic = false;
                    }
                    // Set the capturedObj to null
                    capturedObj = null;
                    // Set the captureText to "capture"
                    captureText.text = "capture";
                    // play placed sound
                    soundSource.PlayOneShot(placedSound);
                    // Set the crosshair to the default crosshair
                    crosshairUI.sprite = crosshairDefault;
                    // Set the captureMode to true
                    captureMode = true;
                }
        }
    }
}
