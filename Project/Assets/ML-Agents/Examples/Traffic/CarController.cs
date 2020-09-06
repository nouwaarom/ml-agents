using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float velocity = 0.0f;
    public float steer_angle = 0.0f;

    private float vehicle_length;

    public bool gasPedalPressed = false;
    public bool brakePedalPressed = false;
    public bool steerLeft = false; 
    public bool steerRight = false;

    // Start is called before the first frame update
    void Start()
    {
        vehicle_length = GetComponent<Collider>().bounds.size.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Update velocity and steer angle based on input.
        if (gasPedalPressed) {
            velocity += 3.0f * Time.deltaTime;
        } else if (brakePedalPressed) {
            if (velocity > 0.0f) {
                velocity -= 20.0f * Time.deltaTime;
            } else {
                velocity -= 2.0f * Time.deltaTime;
            }
        } else {
            if (velocity > 0.0f) {
                velocity = Mathf.Max(0.0f, velocity - 1.0f * Time.deltaTime);
            } else if (velocity < 0.0f) {
                velocity = Mathf.Min(0.0f, velocity + 1.0f * Time.deltaTime);
            }
        }
        if (velocity > 15) {
            velocity = 15;
        } else if (velocity < -5) {
            velocity = -5;
        }

        if (steerLeft) {
            steer_angle -= 0.5f * Time.deltaTime;
        } else if (steerRight) {
            steer_angle += 0.5f * Time.deltaTime;
        } else {
            if (steer_angle > 0.1f) {
                steer_angle -= 1.2f * Time.deltaTime;
            } else if (steer_angle < -0.1f) {
                steer_angle += 1.2f * Time.deltaTime;
            } else {
                steer_angle = 0.0f;
            }
        }
        if (steer_angle > 0.7f * Mathf.PI) {
            steer_angle = 0.7f * Mathf.PI;
        }
        if (steer_angle < -0.7f * Mathf.PI) {
            steer_angle = -0.7f * Mathf.PI;
        }

        // Update position based on velocity and steer angle.
        Vector3 forward = (Quaternion.Euler(0, 90, 0) * transform.forward).normalized;

        // Update the forward position.
        float forward_velocity = Time.deltaTime * velocity * Mathf.Cos(steer_angle);
        Vector3 newPosition = transform.position + forward * forward_velocity;

        // Update the sidewards position.
        Vector3 x_direction = (Quaternion.Euler(0, 90, 0) * forward.normalized).normalized;
        newPosition += x_direction * (Time.deltaTime * 0.5f * velocity * Mathf.Sin(steer_angle));
        transform.position = newPosition;
        
        // Update the rotation.
        float y_rotation = Mathf.Atan((Time.deltaTime * velocity * Mathf.Sin(steer_angle) / vehicle_length));
        transform.Rotate(0, Mathf.Rad2Deg * y_rotation, 0);
    }

    public void OnCollisionEnter(Collision other)
    {
        velocity = Math.Min(velocity, 0.0f);
    }
}
