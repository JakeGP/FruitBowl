using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // UI Elements
    private GameObject level = null;
    private GameObject difficulty = null;
    private GameObject score = null;
    private GameObject time = null;

    // Score Parameters
    private float bonusScore = 0.0f;
    private float cellScore = 0.0f;
    private float elapsedTime = 0.0f;

    public void InitialiseScore()
    {
        level = 
        score = GameObject.FindGameObjectWithTag("Score");
        time = GameObject.FindGameObjectWithTag("Time");
    }

    void Update()
    {
        UpdateTime();
    }

    void UpdateTime()
    {
        if(time == null) { return; }
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime - minutes * 60);
        time.GetComponentInChildren<Text>().text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void ResetTime()
    { 
        if(time == null) { return; }
        elapsedTime = 0.0f;
        UpdateTime();
    }

    public void UpdateScore(GridManager gridManager)
    {
        if (score == null) { return; }
        cellScore = gridManager.GetAllCellComponents().Sum(cell => cell.cellValue) * 100;
        score.GetComponentInChildren<Text>().text = (cellScore + bonusScore).ToString("n2").Split('.')[0];
    }

    public void AddToScore(int amount, GridManager gridManager)
    {
        if (score == null) { return; }
        bonusScore += amount;
        UpdateScore(gridManager);
    }
}
