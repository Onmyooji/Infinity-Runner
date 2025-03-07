using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    private int countdown = 3;
    private RunnerControl runnerControl;
    void Start()
    {
        runnerControl = FindObjectOfType<RunnerControl>();
        StartCoroutine(Countdown());
        SoundsManager.instance.PlaySFX("Countdown", 1f);
    }
    private void Update()
    {
        scoreText.fontSize--;
        if (scoreText.fontSize < 0)
        {
            scoreText.fontSize = 0;
        }
        if (scoreText.text == "GO!")
        {
            scoreText.fontSize = 100;
        }
    }
    IEnumerator Countdown()
    {
        do
        {
            scoreText.fontSize = 100;
            scoreText.text = countdown.ToString();
            yield return new WaitForSeconds(1);
            countdown--;
        } while (countdown > 0);
        if (countdown == 0)
        {
            scoreText.text = "GO!";
            runnerControl.isRunning = true;
            yield return new WaitForSeconds(1);
            scoreText.text = "";
        }
    }
}
