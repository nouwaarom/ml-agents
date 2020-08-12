using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class DrivingAgent : Agent
{
    private RouteProgressRater _progressRater;
    private VisionRays _vision;
    CarController _car;
    
    public override void InitializeAgent()
    {
        _progressRater = GetComponent<RouteProgressRater>();
        _vision = GetComponent<VisionRays>();
        _car = GetComponent<CarController>();
    }
    
    public override void AgentReset()
    {
        // We start back at the spawn.
        transform.position = _progressRater.start.transform.position + new Vector3(0, 2, 0);
        _car.velocity = 0.0f;
        _car.steer_angle = 0.0f;
    }

    // Check if we collide with something.
    // Check progress towards the goal.
    public override void CollectObservations()
    {
        AddVectorObs(_vision.distances);
        AddVectorObs(_car.velocity);
        AddVectorObs(_car.steer_angle);
    }

    public override void AgentAction(float[] vectorAction)
    {
        var gas = (int)vectorAction[0];
        switch (gas) {
            case 0:
                _car.gasPedalPressed = false;
                _car.brakePedalPressed = false;
                break;
            case 1:
                _car.gasPedalPressed = true;
                _car.brakePedalPressed = false;
                break;
            case 2:
                _car.gasPedalPressed = false;
                _car.brakePedalPressed = true;
                break;
        }
        
        var steering = (int)vectorAction[0];
        switch (steering) {
            case 0:
                _car.steerLeft = false;
                _car.steerRight = false;
                break;
            case 1:
                _car.steerLeft = true;
                _car.steerRight = false;
                break;
            case 2:
                _car.steerLeft = false;
                _car.steerRight = true;
                break;
        }
        
        // Add reward and penalties.
        AddReward(_progressRater.progress);
        AddReward(-_vision.penalty); // Collisions have a negative reward.

        if (_progressRater.progress > 0.98f) {
            Done();
        }
    }

    public override float[] Heuristic() {
        return new float[] { 0 };
    }

    public void SetResetParameters()
    {
    }
}
