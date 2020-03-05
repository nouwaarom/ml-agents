using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject block1;
    public GameObject block2;
    public GameObject corner;
    public GameObject edge;

    public int gridSize;

    // Start is called before the first frame update
    void Start()
    {
        var rand = new System.Random();

        // Blocks are 5x5 
        for (int i=1; i< gridSize; i++)
        {
            for (int j=1; j< gridSize; j++)
            {
                if (rand.NextDouble() > 0.5)
                {
                    Instantiate(block1, new Vector3(50*i, 0, 50*j), Quaternion.identity);
                } else
                {
                    Quaternion rotation = Quaternion.identity;
                    if (rand.NextDouble() > 0.5)
                    {
                        rotation = Quaternion.AngleAxis(90, Vector3.up);
                    }

                    Instantiate(block2, new Vector3(50 * i, 0, 50 * j), rotation);
                }

            }
        }

        // Create corners.
        Instantiate(corner, new Vector3(0, 0, 0), Quaternion.AngleAxis(90, Vector3.up));
        Instantiate(corner, new Vector3(0, 0, 50*gridSize), Quaternion.AngleAxis(180, Vector3.up));
        Instantiate(corner, new Vector3(50*gridSize, 0, 0), Quaternion.AngleAxis(0, Vector3.up));
        Instantiate(corner, new Vector3(50*gridSize, 0, 50*gridSize), Quaternion.AngleAxis(270, Vector3.up));

        // Create edges.
        for (int i = 1; i < gridSize; i++)
        {
            Instantiate(edge, new Vector3(50*i, 0, 0), Quaternion.AngleAxis(90, Vector3.up));
            Instantiate(edge, new Vector3(50*i, 0, 50 * gridSize), Quaternion.AngleAxis(270, Vector3.up));
        }
        for (int j = 1; j < gridSize; j++)
        {
            Instantiate(edge, new Vector3(0, 0, 50*j), Quaternion.AngleAxis(180, Vector3.up));
            Instantiate(edge, new Vector3(50*gridSize, 0, 50 * j), Quaternion.AngleAxis(0, Vector3.up));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
