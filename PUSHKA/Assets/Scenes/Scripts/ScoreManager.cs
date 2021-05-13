using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int score;
    public Text scoreDisplay;
    
    private void Update()
    {
        scoreDisplay.text = "Score: " + score;
    }

    public void UpdateScore(int value)
    {
        var newScore = score + value;
        if(newScore >= 0)
            score = newScore;
    }

    public void ResetScore()
    {
        score = 0;
    }
}