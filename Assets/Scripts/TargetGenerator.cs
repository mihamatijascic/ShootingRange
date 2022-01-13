using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGenerator : MonoBehaviour
{
    [SerializeField] private TargetBehaviour targetPrefab;
    [SerializeField] private Transform min;
    [SerializeField] private Transform max;
    [SerializeField] int rows = 3;
    [SerializeField] int targetsInRow = 4;
    [SerializeField] float minSpeed = 0.001f;
    [SerializeField] float maxSpeed = 0.002f;
    private List<TargetBehaviour>[] targetRows;
    
    void Start()
    {
        targetRows = new List<TargetBehaviour>[rows];
        float xStep = (max.transform.position.x - min.transform.position.x) / (rows + 1);
        float xStart = min.transform.position.x + xStep;
        float zStep = (max.transform.position.z - min.transform.position.z) / (targetsInRow + 2);
        float zStart = min.transform.position.z + zStep;

        for(int row = 0; row < rows; row++)
        {
            List<TargetBehaviour> targets = new List<TargetBehaviour>();
            CreateRow(targets, xStart + row*xStep, xStep, zStart, zStep);
            targetRows[row] = targets;
        }
    }

    private void CreateRow(List<TargetBehaviour> targets, float xStart, float xStep, float zStart, float zStep)
    {
        for(int i = 0; i < targetsInRow; i++)
        {
            Vector3 position = new Vector3(xStart, min.transform.position.y, zStart + i * zStep);
            var target = Instantiate(targetPrefab, position, Quaternion.identity).GetComponent<TargetBehaviour>();
            target.isMoving = true;
            target.movementSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
            target.stepZ = UnityEngine.Random.Range(0f, 1f) > 0.5f ? -1 : 1;
            float step34 = zStep * (3f / 4f);
            float step14 = zStep / 4f; 
            target.trajectoryMin = UnityEngine.Random.Range(target.transform.position.z - step34, target.transform.position.z - step14);
            target.trajectoryMax = UnityEngine.Random.Range(target.transform.position.z + step14, target.transform.position.z + step34);
            targets.Add(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
