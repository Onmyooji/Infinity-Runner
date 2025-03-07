using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    void Start()
    {
        int score = ScoreManager.Instance.GetScore();
        scoreText.text = score + " POINTS";
    }

}
