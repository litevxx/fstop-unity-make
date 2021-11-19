using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialExtensions
{
    public static void ToOpaqueMode(this Material material)
    {
        material.SetOverrideTag("RenderType", "");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }

    public static void ToFadeMode(this Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
}

public class capture : MonoBehaviour
{
    public Material ghost;
    public GameObject captured;
    public bool captureMode = false;
    public AudioSource soundSource;
    public AudioClip captureSound;
    public AudioClip releaseSound;
    // Start is called before the first frame update

    void ChangeAlpha(Material mat, float alphaVal)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
        mat.SetColor("_Color", newColor);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

            if (!captureMode)
            {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 fwd = transform.TransformDirection(Vector3.forward);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, fwd, out hit, 10))
                {
                    if (hit.transform.tag == "Capturable")
                    {
                        print("we hit " + hit.transform.gameObject.name);
                        // Now capture the object
                        GameObject capObject = hit.transform.gameObject;
                        // Make it transparent
                        capObject.GetComponent<MeshRenderer>().material.ToFadeMode();
                        ChangeAlpha(capObject.GetComponent<MeshRenderer>().material, 0.4f);
                        // Make the rigidbody kinematic and disable raycasts
                        capObject.GetComponent<Rigidbody>().isKinematic = true;
                        capObject.layer = 2;
                        // Now we can actually start placement mode
                        soundSource.clip = captureSound;
                        soundSource.Play();
                        captured = capObject;
                        captureMode = true;

                    }
                }
            }
            }
            else
            {
                // Placement mode!
                Vector3 fwd = transform.TransformDirection(Vector3.forward);
                RaycastHit hit;

            // Scale mechanic
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forwards
            {
                captured.transform.localScale = captured.transform.localScale + new Vector3(0.1f,0.1f,0.1f);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                captured.transform.localScale = captured.transform.localScale - new Vector3(0.1f, 0.1f, 0.1f);
            }

            if (Physics.Raycast(transform.position, fwd, out hit, 10))
                {
                
                if(captured.activeSelf == false)
                {
                    captured.SetActive(true);
                }
                captured.GetComponent<Rigidbody>().MovePosition(new Vector3(hit.point.x, hit.point.y + captured.GetComponent<MeshRenderer>().bounds.extents.y, hit.point.z));

            }
            else
                {
                captured.SetActive(false);
                }
                if(Input.GetMouseButtonUp(0))
                {
                // Make it opaque
                captured.GetComponent<MeshRenderer>().material.ToOpaqueMode();
                ChangeAlpha(captured.GetComponent<MeshRenderer>().material, 1f);
                // Unmake the rigidbody kinematic and enable raycasts
                captured.GetComponent<Rigidbody>().isKinematic = false;
                captured.layer = 0;
                // Now we can actually disable placement mode
                soundSource.clip = releaseSound;
                soundSource.Play();
                captured = null;
                captureMode = false;

            }
            }
    }
}
