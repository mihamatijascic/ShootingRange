using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text stats;
    private float score = 0f;
    private int level = 0;
    private int totalShots = 0;
    private int totalHits = 0;
    // Start is called before the first frame update
    void Start()
    {
        scoreLabel.text = $"{score}";
        levelText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        var textLevel = level != 0 ? $"level: {level} " : "\n";
        var textScore = $"score: {score}";
        scoreLabel.text = $"{textLevel}{textScore}";
    }

    public void AddToScore(float points)
    {
        score += points;
    }

    public void NextLevel()
    {
        level++;
    }

    public void Shot()
    {
        totalShots++;
    }

    public void Hit()
    {
        totalHits++;
    }

    public void SetStatsText()
    {
        stats.text = $"accuracy: {totalHits}/{totalShots}  final score: {score}";
    }

    public IEnumerator ShowLevel(float delay)
    {
        levelText.text = $"Level {level}";
        levelText.enabled = true;
        yield return new WaitForSeconds(delay);
        levelText.enabled = false;
    }
}
