using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnContinue : MonoBehaviour
{
    private SceneChanger sceneChanger;

    private void Start()
    {
        SoundsManager.instance.PlayMusic("Music_Tuto");
        sceneChanger = FindObjectOfType<SceneChanger>();
    }
    public void OnClickContinue()
    {
        SoundsManager.instance.PlaySFX("Button", 0.75f);
        SoundsManager.instance.StopMusic("Music_Tuto");
        sceneChanger.FadeToScene("SceneGame");
    }
}
