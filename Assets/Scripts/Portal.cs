using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Portal : MonoBehaviour
{
    //scroll speed of textures
    [Range(0, 10)]
    float rotationSpeed;
    [Range(0, 10)]
    public float parallaxStrength;
 [HideInInspector]  public GameObject portal;
    float rotation;
    public bool startPortal;
    Quaternion portalRotation;  
    private void Start()
    {
        portal = this.gameObject;
        //_ParallaxMap may not be easy to see as the standard shader script sets it super low.
        GetComponent<Renderer>().material.SetFloat("_Parallax", parallaxStrength);
       
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        rotation += Time.deltaTime*20;
        if (rotation >= 360)
        {
            rotation = 0;
        }
        //change the rotation of the gameobject on X
        if (startPortal) { portal.transform.rotation = Quaternion.Euler(rotation, 90, 90); }
        //change the rotation of the gameobject on Y                   
        else { portal.transform.rotation = Quaternion.Euler(0, rotation, 0); }                          
    }
}
