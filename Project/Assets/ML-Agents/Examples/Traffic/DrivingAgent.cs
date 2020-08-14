using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class DrivingAgent : Agent
{
    private RouteProgressRater _progressRater;
    private VisionRays _vision;
    CarController _car;

    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;
    
    public override void Initialize()
    {
        Debug.Log("Agent initializing.");
        _progressRater = GetComponent<RouteProgressRater>();
        _vision = GetComponent<VisionRays>();
        _car = GetComponent<CarController>();
    }
    
    public override void OnEpisodeBegin()
    {
        Debug.Log("Starting episode.");
        // We start back at the spawn.
        transform.position = _progressRater.start.transform.position + new Vector3(0, 2, 0);
        transform.rotation = _progressRater.start.transform.rotation;
        _car.velocity = 0.0f;
        _car.steer_angle = 0.0f;
        
        SetResetParameters();
    }

    // Check if we collide with something.
    // Check progress towards the goal.
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_vision.distances);
        sensor.AddObservation(_car.velocity);
        sensor.AddObservation(_car.steer_angle);
    }

    private void FixedUpdate()
    {
        if (Academy.Instance.IsCommunicatorOn)
        {
            RequestDecision();
        }
        else
        {
            if (m_TimeSinceDecision >= timeBetweenDecisionsAtInference)
            {
                m_TimeSinceDecision = 0f;
                RequestDecision();
            }
            else
            {
                m_TimeSinceDecision += Time.fixedDeltaTime;
            }
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var gas = (int)vectorAction[0];
        switch (gas) {
            case 1:
                _car.gasPedalPressed = false;
                _car.brakePedalPressed = false;
                break;
            case 0:
                _car.gasPedalPressed = true;
                _car.brakePedalPressed = false;
                break;
            case 2:
                _car.gasPedalPressed = false;
                _car.brakePedalPressed = false; // TODO, make true so that is learns to go forward as well.
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
        AddReward(5.0f * _progressRater.progress);
        AddReward(-_vision.penalty); // Collisions have a negative reward.

        if (_progressRater.progress > 0.98f) {
            EndEpisode(); // Finished.
        } else if (_progressRater.progress < -0.1f) { // Hard fail, if we drive back.
            AddReward(-1000.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
    }

    public void SetResetParameters()
    {
    }
}
