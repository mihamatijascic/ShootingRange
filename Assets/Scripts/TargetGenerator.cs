using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TargetGenerator : MonoBehaviour
{
    [SerializeField] private TargetBehaviour targetPrefab;
    [SerializeField] private TargetBehaviour civilianPrefab;
    [SerializeField] private Desk deskPrefab;
    [SerializeField] private Transform min;
    [SerializeField] private Transform max;
    [SerializeField] private int rows = 3;
    [SerializeField] private int targetNumberInRow = 4;
    [SerializeField] private float levelDuration = 30f;
    [SerializeField] private float showLevelTime = 10f;
    [SerializeField] private float pauseTime = 10f;
    [SerializeField] private ScoreCounter scoreCounter;
    [SerializeField] private List<Level> levels;

    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Canvas endCanvas;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button exitButton;

    private List<TargetBehaviour>[] targetsInRows;
    private Desk[] desks;

    private float xStep;
    private float xStart;
    private float zStep;
    private float zStart;

    void Start()
    {
        gameCanvas.enabled = true;
        endCanvas.enabled = false;
        playAgainButton.onClick.AddListener(() => SceneManager.LoadScene("ShootingRange"));
        exitButton.onClick.AddListener(Application.Quit);

        targetsInRows = new List<TargetBehaviour>[rows];

        desks = new Desk[rows];
        xStep = (max.transform.position.x - min.transform.position.x) / (rows + 1);
        xStart = min.transform.position.x + xStep;
        zStep = (max.transform.position.z - min.transform.position.z) / (targetNumberInRow + 2);
        zStart = min.transform.position.z + zStep;

        for (int row = 0; row < rows; row++)
        {
            targetsInRows[row] = new List<TargetBehaviour>();
            desks[row] = CreateDesk(xStart + row * xStep);
            CreateRow(targetsInRows[row], xStart + row * xStep, levels[0]);
        }

        StartCoroutine(StartNextLevel());
    }

    private IEnumerator StartNextLevel()
    {
        foreach (var level in levels)
        {
            yield return new WaitForSeconds(pauseTime);

            this.scoreCounter.NextLevel();
            yield return StartCoroutine(scoreCounter.ShowLevel(showLevelTime));

            SetTargetRows(level);

            for (int i = 0; i < level.upNumber; i++)
            {
                BringUpTargets(level);
                yield return new WaitForSeconds(level.targetUpTime);
            }
        }
        Debug.Log("Game over");

        scoreCounter.SetStatsText();
        gameCanvas.enabled = false;
        endCanvas.enabled = true;
    }

    public void BringUpTargets(Level level)
    {
        for (int target = 0; target < level.spawnTargetsNumber; target++)
        {
            bool isOneUp = false;
            while (!isOneUp)
            {
                int randomRow = UnityEngine.Random.Range(0, rows);
                int randomIndex = UnityEngine.Random.Range(0, targetsInRows[randomRow].Count);
                if (targetsInRows[randomRow][randomIndex].IsDown)
                {
                    isOneUp = true;
                    targetsInRows[randomRow][randomIndex].StartTargetUpTime(level.timeUpForTarget);
                }
            }
        }
    }

    private void SetTargetRows(Level level)
    {
        for (int row = 0; row < rows; row++)
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
            target = CreateTarget(position, level);
        }
    }

    private void DeleteTargets(List<TargetBehaviour> targets)
    {
        if (targets.Count == 0) return;
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            var target = targets[i].gameObject;
            targets.RemoveAt(i);
            Destroy(target);
        }
    }

    private void CreateRow(List<TargetBehaviour> targets, float startOfRow, Level level)
    {
        for (int i = 0; i < targetNumberInRow; i++)
        {
            Vector3 position = new Vector3(startOfRow, min.transform.position.y, zStart + i * zStep);
            targets.Add(CreateTarget(position, level));
        }
    }

    private TargetBehaviour CreateTarget(Vector3 position, Level level)
    {
        TargetBehaviour.TargetType type = UnityEngine.Random.Range(0f, 1f) > level.posibilityOfCreatingCivilian
            ? TargetBehaviour.TargetType.Target : TargetBehaviour.TargetType.Civilian;
        var prefab = type == TargetBehaviour.TargetType.Target ? targetPrefab : civilianPrefab;
        var target = Instantiate(prefab, position, Quaternion.identity).GetComponent<TargetBehaviour>();
        target.generatedTarget = true;
        target.type = type;
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
        target.hitScore = target.type == TargetBehaviour.TargetType.Civilian
            ? level.negativePoints : level.positivePoints;
    }

    private Desk CreateDesk(float startOfRow)
    {
        float frontTranslation = xStep * (1f / 5f);
        Vector3 position = new Vector3(startOfRow + frontTranslation, min.position.y, (min.position.z + max.position.z) / 2f);
        Desk desk = Instantiate(deskPrefab, position, Quaternion.identity).GetComponent<Desk>();
        return desk;
    }

}
