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
        car.gasPedalPressed = Input.GetKey(KeyCode.W);
        car.brakePedalPressed = Input.GetKey(KeyCode.S);
            
        car.steerLeft = Input.GetKey(KeyCode.A);
        car.steerRight = Input.GetKey(KeyCode.D);
    }
}
