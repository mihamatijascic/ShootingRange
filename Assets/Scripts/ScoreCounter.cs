using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text levelText;
    private float score = 0f;
    private int level = 0;
    private Coroutine routine;
    // Start is called before the first frame update
    void Start()
    {
        scoreLabel.text = $"{score}";
        levelText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        scoreLabel.text = $"level: {level}| score: {score}";
    }

    public void AddToScore(float points)
    {
        score += points;
    }

    public void nextLevel()
    {
        level++;
    }

    public void ShowLevelCorutine(float delay)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShowLevel(delay));
    }

    private IEnumerator ShowLevel(float delay)
    {
        levelText.text = $"Level {level} coming...";
        levelText.enabled = true;
        yield return new WaitForSeconds(delay);
        levelText.enabled = false;
    }
}
