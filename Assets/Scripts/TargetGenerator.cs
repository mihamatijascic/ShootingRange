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
    [SerializeField] private int targetNumberInRow = 4;
    [SerializeField] private float levelDuration = 30f;
    [SerializeField] private float showLevelTime = 10f;
    [SerializeField] private float pauseTime = 10f;
    [SerializeField] private ScoreCounter scoreCounter;

    private Coroutine routine;
    

    [SerializeField] private List<Level> levels;
    private int nextLevelIndex;
    private float SpawnTargetsTime;
    private float nextLevelTime;

    private List<TargetBehaviour>[] targetsInRows;
    private Desk[] desks;

    private Coroutine rutina = null;

    private float xStep;
    private float xStart;
    private float zStep;
    private float zStart;
    
    void Start()
    {
        targetsInRows = new List<TargetBehaviour>[rows];

        desks = new Desk[rows];
        xStep = (max.transform.position.x - min.transform.position.x) / (rows + 1);
        xStart = min.transform.position.x + xStep;
        zStep = (max.transform.position.z - min.transform.position.z) / (targetNumberInRow + 2);
        zStart = min.transform.position.z + zStep;

        for (int row = 0; row < rows; row++)
        {
            targetsInRows[row] = new List<TargetBehaviour>();
            desks[row] = CreateDesk(xStart + row*xStep);
            CreateRow(targetsInRows[row], xStart + row * xStep, levels[nextLevelIndex]);
        }

        nextLevelTime = 0f;
    }

    void Update()
    {
        if (Time.time > nextLevelTime)
        {
            if (nextLevelIndex < levels.Count)
            {

                BetweenLevelPauseCorutine(pauseTime);
                scoreCounter.nextLevel();
                scoreCounter.ShowLevelCorutine(showLevelTime);
                BetweenLevelPauseCorutine(showLevelTime);

                Debug.Log("new level");
                nextLevelTime = Time.time + levelDuration;
                SpawnTargetsTime = Time.time + levels[nextLevelIndex].bringUpTargetTime;
                SetTargetRows(levels[nextLevelIndex]);
                nextLevelIndex++;
                BringUpTargets();
                return;
            }
            else
            {
                //Debug.Log("Game over");
            }
        }

        if (Time.time > SpawnTargetsTime)
        {
            SpawnTargetsTime = Time.time + levels[nextLevelIndex - 1].bringUpTargetTime;
            BringUpTargets();
        }

    }

    private void BetweenLevelPauseCorutine(float delay)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(BetweenLevelPause(delay));
    }

    private IEnumerator BetweenLevelPause(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void BringUpTargets()
    {
        for(int target = 0; target < levels[nextLevelIndex-1].spawnTargetsNumber; target++)
        {
            bool isOneUp = false;
            while (!isOneUp)
            {
                int randomRow = UnityEngine.Random.Range(0, rows);
                int randomIndex = UnityEngine.Random.Range(0, targetsInRows[randomRow].Count);
                if (targetsInRows[randomRow][randomIndex].IsDown)
                {
                    isOneUp = true;
                    targetsInRows[randomRow][randomIndex].StartTargetUpTime(levels[nextLevelIndex-1].timeUpForTarget);
                }
            }
        }
    }

    //private void SetTargetRowsCorutine(Level level)
    //{
    //    if(rutina != null) StopCoroutine(rutina);
    //    StartCoroutine(SetTargetRows(level));
    //}

    private void SetTargetRows(Level level)
    {
        for(int row = 0; row < rows; row++)
        {
            ResetTargetsPositions(targetsInRows[row], xStart + row * xStep, level);
        }
    }

    private void ResetTargetsPositions(List<TargetBehaviour> targetBehaviours, float startOfRow, Level level)
    {
        for (int i = 0; i < targetNumberInRow; i++)
        {
            Vector3 position = new Vector3(startOfRow, min.transform.position.y, zStart + i * zStep);
            var target = targetBehaviours[i];
            target.transform.position = position;
            SetTargetParameters(target, level);
        }
    }

    private void DeleteTargets(List<TargetBehaviour> targets)
    {
        if (targets.Count == 0) return;
        for(int i = targets.Count-1; i >= 0; i--)
        {
            var target = targets[i].gameObject;
            targets.RemoveAt(i);
            Destroy(target);
        }
    }

    private void CreateRow(List<TargetBehaviour> targets, float startOfRow, Level level)
    {
        for(int i = 0; i < targetNumberInRow; i++)
        {
            Vector3 position = new Vector3(startOfRow, min.transform.position.y, zStart + i * zStep);
            targets.Add(CreateTarget(position, level));
        }
    }

    private TargetBehaviour CreateTarget(Vector3 position, Level level)
    {
        var target = Instantiate(targetPrefab, position, Quaternion.identity).GetComponent<TargetBehaviour>();
        target.generatedTarget = true;
        SetTargetParameters(target, level);
        return target;
    }

    private void SetTargetParameters(TargetBehaviour target, Level level)
    {
        target.scoreCounter = this.scoreCounter;
        target.isMoving = UnityEngine.Random.Range(0f, 1f) < level.posibilityOfMoving;
        target.movementSpeed = UnityEngine.Random.Range(level.minSpeed, level.maxSpeed);
        target.stepZ = UnityEngine.Random.Range(0f, 1f) > 0.5f ? -1 : 1;
        float step34 = zStep * (3f / 4f);
        float step14 = zStep / 4f;
        target.trajectoryMin = UnityEngine.Random.Range(target.transform.position.z - step34, target.transform.position.z - step14);
        target.trajectoryMax = UnityEngine.Random.Range(target.transform.position.z + step14, target.transform.position.z + step34);
    }

    private Desk CreateDesk(float startOfRow)
    {
        float frontTranslation = xStep * (1f / 5f);
        Vector3 position = new Vector3(startOfRow + frontTranslation, min.position.y, (min.position.z + max.position.z) / 2f);
        Desk desk = Instantiate(deskPrefab, position, Quaternion.identity).GetComponent<Desk>();
        return desk;
    } 
    
}
