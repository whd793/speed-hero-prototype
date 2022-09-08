using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {

    public float metalSphereVelocity;

    private Rigidbody rb;

    public GameObject metalSphere;
   
    private int i;
    private float z;

    void Start ()
    {      
        i = 1;
        //Starts with z = -2 so that the elastic line starts at the size of the elastic.
        z = -2;       
	}

    void Update()
    {
        //If you press the left mouse button, the elastic starts to stretch.
        if (Input.GetMouseButton(0))
        {
            z -= 0.1f;
            if (i == 1)
            {
                //if the i(increment) is equal to one, it means that there is no metal sphere in the slingshot, then the sphere is created to be thrown next.
                metalSphere = Instantiate(metalSphere, new Vector3(metalSphere.transform.position.x, metalSphere.transform.position.y, -3), Quaternion.identity);
                rb = metalSphere.GetComponent<Rigidbody>();
                //The metal sphere is parented to the slingshot, so that it can move with the slingshot.
                metalSphere.transform.parent = this.transform;
                i = 0;                           
            }

           

            if (z >= -7)
            {
              
                metalSphere.transform.localPosition = new Vector3(-1.42f, 2.286f, z + 1.7f);
               
            }
            else
            {
                //If the z axis value reaches the maximum -7, that value will remain and the slingshot elastic will be completely stretched.
               
                metalSphere.transform.localPosition = new Vector3(-1.42f, 2.286f, -5.3f);
           
            }

        }
        //When the left mouse button is released, the metallic sphere will be thrown and the elastic will be decompressed with the movement.
        if (Input.GetMouseButtonUp(0))
        {
            //i receives the value of one so that a new metallic sphere is created when the mouse is pressed again.
            i = 1;

            //Activates the sphere's gravity as soon as it is thrown.
            rb.useGravity = true;

            //Adds a forward force on the rigidbody of the metallic sphere, depending on the amount that the elastic is stretched.
            if (z >= -7)
            {
                //For the elastic to stretch the value of the z axis is increased to - 7, maximum of the stretch.
                //The force that the metallic sphere will be thrown, will be multiplied by the amount of stretch of the elastic.
                rb.AddForce(transform.forward * metalSphereVelocity * -z, ForceMode.Impulse);
            }else
            {
                rb.AddForce(transform.forward * metalSphereVelocity * 15, ForceMode.Impulse);
            }
            //The metal sphere is taken (parent = null) in the slingshot, so that the sphere stops moving with the slingshot and the camera.
            metalSphere.transform.parent = null;
            z = -2;                      
        }
    }
}
