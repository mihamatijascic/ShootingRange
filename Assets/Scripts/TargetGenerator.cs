using Assets.Scripts; 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGenerator : MonoBehaviour
{
    [SerializeField] private TargetBehaviour targetPrefab;
    [SerializeField] private Desk deskPrefab;
    [SerializeField] private Transform min;
    [SerializeField] private Transform max;
    [SerializeField] private int rows = 3;
    [SerializeField] private int targetsInRow = 4;
    [SerializeField] private float levelDuration = 30f;
    [SerializeField] private float pauseTime = 15f;


    private float nextLevelTime;

    [SerializeField] private List<Level> levels;
    private int currentLevelIndex;

    [SerializeField] float score = 0f; 
    private List<TargetBehaviour>[] targetRows;
    private Desk[] desks;

    private float xStep;
    private float xStart;
    private float zStep;
    private float zStart;
    
    void Start()
    {
        targetRows = new List<TargetBehaviour>[rows];

        desks = new Desk[rows];
        xStep = (max.transform.position.x - min.transform.position.x) / (rows + 1);
        xStart = min.transform.position.x + xStep;
        zStep = (max.transform.position.z - min.transform.position.z) / (targetsInRow + 2);
        zStart = min.transform.position.z + zStep;

        for (int row = 0; row < rows; row++)
        {
            targetRows[row] = new List<TargetBehaviour>();
            desks[row] = CreateDesk(xStart + row*xStep);
        }

        nextLevelTime = 0f;
    }

    void Update()
    {
        if (Time.time > nextLevelTime)
        {
            if (currentLevelIndex >= levels.Count)
            {
                // game over
                Debug.Log("Game over");
            }
            nextLevelTime = Time.time + levelDuration;
            SetTargetRows(levels[currentLevelIndex++]);
            Debug.Log("new level");
        }
    }

    private void SetTargetRows(Level level)
    {
        for(int row =0; row < rows; row++)
        {
            if (targetRows[row].Count == 0) continue;
            DeleteTargets(targetRows[row]);
        }

        for(int row = 0; row <= rows; row++)
        {
            CreateRow(targetRows[row], xStart + row * xStep, level);
        }
    }

    private void DeleteTargets(List<TargetBehaviour> targets)
    {
        for(int i = targets.Count-1; i >= 0; i--)
        {
            var target = targets[i].gameObject;
            targets.RemoveAt(i);
            Destroy(target);
        }
    }

    private void CreateRow(List<TargetBehaviour> targets, float startOfRow, Level level)
    {
        for(int i = 0; i < targetsInRow; i++)
        {
            Vector3 position = new Vector3(startOfRow, min.transform.position.y, zStart + i * zStep);
            targets.Add(CreateTarget(position, zStep, level));
        }
    }

    private TargetBehaviour CreateTarget(Vector3 position, float zStep, Level level)
    {
        var target = Instantiate(targetPrefab, position, Quaternion.identity).GetComponent<TargetBehaviour>();
        target.generatedTarget = true;
        target.isMoving = UnityEngine.Random.Range(0f,1f) < level.posibilityOfMoving;
        target.movementSpeed = UnityEngine.Random.Range(level.minSpeed, level.maxSpeed);
        target.stepZ = UnityEngine.Random.Range(0f, 1f) > 0.5f ? -1 : 1;
        float step34 = zStep * (3f / 4f);
        float step14 = zStep / 4f;
        target.trajectoryMin = UnityEngine.Random.Range(target.transform.position.z - step34, target.transform.position.z - step14);
        target.trajectoryMax = UnityEngine.Random.Range(target.transform.position.z + step14, target.transform.position.z + step34);
        return target;
    }

    private Desk CreateDesk(float startOfRow)
    {
        float frontTranslation = xStep * (1f / 5f);
        Vector3 position = new Vector3(startOfRow + frontTranslation, min.position.y, (min.position.z + max.position.z) / 2f);
        Desk desk = Instantiate(deskPrefab, position, Quaternion.identity).GetComponent<Desk>();
        return desk;
    } 
    
}
