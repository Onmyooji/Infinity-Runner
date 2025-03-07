using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BtnBreak : MonoBehaviour
{
    public bool isPause = false;
    private RunnerControl runner;

    private void Start()
    {
        runner = FindObjectOfType<RunnerControl>();
    }
    public void OnClickBreak()
    {
        if (runner.isRunning)
        {
            isPause = !isPause;

            if (isPause)
            {
                Time.timeScale = 0f;
                SoundsManager.instance.PauseAllAudio();
                runner.runningSound.enabled = false;
            }
            else
            {
                Time.timeScale = 1f;
                SoundsManager.instance.ResumeAllAudio();
                runner.runningSound.enabled = true;
            }
        }
    }
}
