using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class BtnExit : MonoBehaviour
{
    private void Start()
    {
        SoundsManager.instance.PlayMusic("Music_Score");
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}
