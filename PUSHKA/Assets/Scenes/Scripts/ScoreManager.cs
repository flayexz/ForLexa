using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; set; }
    public Text scoreDisplay;

    private void Update()
    {
        scoreDisplay.text = "Score: " + Score;
    }

    public void ResetScore()
    {
        Score = 0;
    }
}