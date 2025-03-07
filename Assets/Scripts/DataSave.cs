using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class DataSave : MonoBehaviour 
{
    public HighScoreJson highScore;
    [SerializeField]
    private TMP_Text HighScoreText;
    void Start()
    {
        LoadScores();

        if (ScoreManager.Instance.GetScore() > highScore.highScore)
        {
            highScore.highScore = ScoreManager.Instance.GetScore();
        }
        else
        {
            Debug.Log("Le score est inférieur au HighScore actuel");
        }

        SaveScores();

        HighScoreText.text = highScore.highScore + " POINTS";
    }
    public void SaveScores()
    {
        string dir = Application.persistentDataPath + "/Saves";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(highScore);
        File.WriteAllText(dir + "/highScoreSave.txt", json);
    }

    private void LoadScores()
    {
        string filePath = Application.persistentDataPath + "/Saves/highScoreSave.txt";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            highScore = JsonUtility.FromJson<HighScoreJson>(json);
        }
        else
        {
            Debug.Log("Le fichier n'existe pas");
        }
    }
}