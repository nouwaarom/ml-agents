﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRays : MonoBehaviour
{
    private const int numViewDirections = 3;
    private Vector3[] rays;

    private float collisionDistance = 1.1f;

    public float penalty = 0.0f;
    public float[] distances; 
    
    // Start is called before the first frame update
    void Start()
    {
        distances = new float[numViewDirections+2];
        // Generate rays in every direction. 
        rays = new Vector3[numViewDirections];
        float angleIncrement = (Mathf.PI) / (numViewDirections+1);

        for (int i = 0; i < numViewDirections; i++) {
            float x = Mathf.Sin((1+i) * angleIncrement);
            float z = Mathf.Cos ((1+i) * angleIncrement);
            rays[i] = new Vector3 (x, 0, z);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float sphereRadius = 0.1f;
        penalty = 0.0f;

        int layerMask = ~(1 << 8);
        
       // Detect collisions. 
       RaycastHit hit;
       for (int i = 0; i < rays.Length; i++) {
           Vector3 dir = transform.TransformDirection (rays[i]);

           bool isForward = i <= rays.Length / 2;
           Vector3 forward = (Quaternion.Euler(0, 90, 0) * transform.forward).normalized;
           Vector3 origin = transform.position + 2.5f * forward;
           float maxDistance = isForward ? 50.0f : 25.0f;
           if (Physics.SphereCast (origin, sphereRadius, dir, out hit, maxDistance, layerMask)) {
               distances[i] = hit.distance;
               if (hit.distance > collisionDistance) {
                   Debug.DrawLine(origin, origin + (dir * hit.distance), Color.blue);
               } else {
                   Debug.DrawLine(origin, origin + (dir * hit.distance), Color.red);
                   penalty = 1.0f;
               }
           } else {
               distances[i] = maxDistance;
           }
       }
       
       if (Physics.SphereCast (transform.position, sphereRadius, transform.forward, out hit, 25.0f, layerMask)) {
           distances[numViewDirections] = hit.distance;
           if (hit.distance > collisionDistance) {
               Debug.DrawLine(transform.position, transform.position + (transform.forward * hit.distance), Color.green);
           } else {
               Debug.DrawLine(transform.position, transform.position + (transform.forward * hit.distance), Color.red);
               penalty = 1.0f;
           }
       } else {
           distances[numViewDirections] = 25.0f;
       }
       
       if (Physics.SphereCast (transform.position, sphereRadius, -transform.forward, out hit, 25.0f, layerMask)) {
           distances[numViewDirections+1] = hit.distance;
           if (hit.distance > collisionDistance) {
               Debug.DrawLine(transform.position, transform.position + (-transform.forward * hit.distance), Color.green);
           } else {
               Debug.DrawLine(transform.position, transform.position + (-transform.forward * hit.distance), Color.red);
               penalty = 1.0f;
           }
       } else {
           distances[numViewDirections+1] = 25.0f;
       }
       
       // Check which direction is best to go to.
    }
}
