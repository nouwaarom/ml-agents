using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRays : MonoBehaviour
{
    private const int numViewDirections = 16;
    private Vector3[] rays;

    private float collisionDistance = 1.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        // Generate rays in every direction. 
        rays = new Vector3[numViewDirections];
        float angleIncrement = (Mathf.PI * 2) / numViewDirections;

        for (int i = 0; i < numViewDirections; i++) {
            float x = Mathf.Sin(i * angleIncrement);
            float z = Mathf.Cos (i * angleIncrement);
            rays[i] = new Vector3 (x, 0, z);
        }
        
       // Split rays in rays looking to the front and rays going to the side.
    }

    // Update is called once per frame
    void Update()
    {
        float sphereRadius = 0.1f;
       // Detect collisions. 
       RaycastHit hit;
       for (int i = 0; i < rays.Length; i++) {
           Vector3 dir = transform.TransformDirection (rays[i]);

           Vector3 forward = (Quaternion.Euler(0, 90, 0) * transform.forward).normalized;
           Vector3 origin = transform.position + (i > rays.Length/2 ? 2.5f * -forward : 2.5f * forward);
           if (Physics.SphereCast (origin, sphereRadius, dir, out hit, 25.0f)) {
               if (hit.distance > collisionDistance) {
                   if (i <= rays.Length / 2) {
                       Debug.DrawLine(origin, origin + (dir * hit.distance), Color.blue);
                   } else {
                       Debug.DrawLine(origin, origin + (dir * hit.distance), Color.green);
                   }
               } else {
                   Debug.DrawLine(origin, origin + (dir * hit.distance), Color.red);
               }
           }
       }
       
       if (Physics.SphereCast (transform.position, sphereRadius, transform.forward, out hit, 25.0f)) {
           if (hit.distance > collisionDistance) {
               Debug.DrawLine(transform.position, transform.position + (transform.forward * hit.distance), Color.green);
           } else {
               Debug.DrawLine(transform.position, transform.position + (transform.forward * hit.distance), Color.red);
           }
       }
       
       if (Physics.SphereCast (transform.position, sphereRadius, -transform.forward, out hit, 25.0f)) {
           if (hit.distance > collisionDistance) {
               Debug.DrawLine(transform.position, transform.position + (-transform.forward * hit.distance), Color.green);
           } else {
               Debug.DrawLine(transform.position, transform.position + (-transform.forward * hit.distance), Color.red);
           }
       }
       
       // Check which direction is best to go to.
    }
}
