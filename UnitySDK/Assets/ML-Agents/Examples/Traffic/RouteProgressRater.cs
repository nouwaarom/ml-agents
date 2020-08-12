﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteProgressRater : MonoBehaviour
{
    private GameObject goal;
    private float initialDistance;

    public float progress;
    
    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindWithTag("Finish");     
        initialDistance = Vector3.Distance(transform.position, goal.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, goal.transform.position);
        progress = (initialDistance - distance) / initialDistance;
    }
}
