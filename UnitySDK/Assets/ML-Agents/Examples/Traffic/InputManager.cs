using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    CarController car;

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            car.velocity += 3.0f * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.S)) {
            if (car.velocity > 0.0f) {
                car.velocity -= 20.0f * Time.deltaTime;
            } else {
                car.velocity -= 2.0f * Time.deltaTime;
            }
        } else {
            if (car.velocity > 0.0f) {
                car.velocity = Mathf.Max(0.0f, car.velocity - 1.0f * Time.deltaTime);
            } else if (car.velocity < 0.0f) {
                car.velocity = Mathf.Min(0.0f, car.velocity + 1.0f * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.A)) {
            car.steer_angle -= 0.5f * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D)) {
            car.steer_angle += 0.5f * Time.deltaTime;
        } else {
            if (car.steer_angle > 0.1f) {
                car.steer_angle -= 1.2f * Time.deltaTime;
            } else if (car.steer_angle < -0.1f) {
                car.steer_angle += 1.2f * Time.deltaTime;
            } else {
                car.steer_angle = 0.0f;
            }
        }
    }
}
