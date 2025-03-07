using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnMenu : MonoBehaviour
{
    private SceneChanger sceneChanger;
    private void Start()
    {
        SoundsManager.instance.PlayMusic("Music_Menu");
        sceneChanger = FindObjectOfType<SceneChanger>();
    }
    public void OnClickPlay()
    {
        SoundsManager.instance.PlaySFX("Button", 0.75f);
        SoundsManager.instance.StopMusic("Music_Menu");
        sceneChanger.FadeToScene("SceneTuto");
    }
}
