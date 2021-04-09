using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // UI Elements
    public GameObject score;
    public GameObject time;

    private float totalScore = 0.0f;
    private float elapsedTime = 0.0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime - minutes * 60);

        score.GetComponentInChildren<Text>().text = totalScore.ToString();
        time.GetComponentInChildren<Text>().text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
