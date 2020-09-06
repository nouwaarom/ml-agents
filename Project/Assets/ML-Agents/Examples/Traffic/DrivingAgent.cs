using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class DrivingAgent : Agent
{
    private RouteProgressRater _progressRater;
    private float previousProgress;
    private VisionRays _vision;
    CarController _car;

    public float timeBetweenDecisions = 0.2f;
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
        _vision.penalty = 0.0f;
        previousProgress = 0.0f;
        
        SetResetParameters();
    }

    // Check if we collide with something.
    // Check progress towards the goal.
    public override void CollectObservations(VectorSensor sensor)
    {
        for (int i = 0; i < _vision.distances.Length; i++)
        {
            sensor.AddObservation(_vision.distances[i] / 25.0f);
        }

        sensor.AddObservation(_car.velocity / 15.0f);
        sensor.AddObservation((_car.steer_angle + 0.7f * Mathf.PI) / (1.4f * Mathf.PI));
    }

    private void FixedUpdate()
    {
        if (m_TimeSinceDecision >= timeBetweenDecisions) {
            m_TimeSinceDecision = 0f;
            RequestDecision();
        } else {
            m_TimeSinceDecision += Time.fixedDeltaTime;
        }
    }

    public override void OnActionReceived(float[] vectorAction)
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
                _car.brakePedalPressed = true; // TODO, make true so that is learns to go forward as well.
                break;
        }
        
        var steering = (int)vectorAction[1];
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
        
        AddReward(_progressRater.progress-previousProgress);
        previousProgress = _progressRater.progress;

        AddReward(-0.001f);
        if (_vision.penalty > 0.0f) {
            AddReward(-0.001f);
        }

        if (_progressRater.progress > 0.95f) {
            AddReward(2.0f);
            EndEpisode(); // Finished.
        } else if (_progressRater.progress < -0.1f) { // Hard fail, if we drive back.
            AddReward(-10.0f);
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
