using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float velocity = 0.0f;
    public float steer_angle = 0.0f;

    private float vehicle_length;

    // Start is called before the first frame update
    void Start()
    {
        vehicle_length = GetComponent<Collider>().bounds.size.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (velocity > 15) {
            velocity = 15;
        } else if (velocity < -5) {
            velocity = -5;
        }
        
        if (steer_angle > 0.7f * Mathf.PI) {
            steer_angle = 0.7f * Mathf.PI;
        }
        if (steer_angle < -0.7f * Mathf.PI) {
            steer_angle = -0.7f * Mathf.PI;
        }

        Vector3 forward = (Quaternion.Euler(0, 90, 0) * transform.forward).normalized;

        // Update the forward position.
        float forward_velocity = Time.deltaTime * velocity * Mathf.Cos(steer_angle);
        transform.position += forward * forward_velocity;

        // Update the sidewards position.
        Vector3 x_direction = (Quaternion.Euler(0, 90, 0) * forward.normalized).normalized;
        transform.position +=  x_direction * (Time.deltaTime * 0.5f * velocity * Mathf.Sin(steer_angle));

        // Update the rotation.
        float y_rotation = Mathf.Atan((Time.deltaTime * velocity * Mathf.Sin(steer_angle) / vehicle_length));
        transform.Rotate(0, Mathf.Rad2Deg * y_rotation, 0);
    }
}
