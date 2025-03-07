using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField]
    private TMP_Text scoreText;
    int score = 0;

    private void Awake()
    {
        Instance = this;
    }
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = Mathf.Round(score) + " POINTS";
    }
    public int GetScore() 
    {
        return score; 
    }
    void Start()
    {
        scoreText.text = Mathf.Round(score) + " POINTS";
    }
}
