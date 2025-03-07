using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
    public Material[] skyMaterials;
    public int currentSkyIndex;
    private float transition = 0.0f;
    private float transitionDuration = 4.0f;
    private bool isTransitioning = false;
    private Material initialSky;
    private Material targetSky;
    void Start()
    {
        currentSkyIndex = 0;
        RenderSettings.skybox = skyMaterials[currentSkyIndex];
    }
    void Update()
    {
        if (isTransitioning)
        {
            Transition();
        }
    }
    public void StartTransition()
    {
        if (isTransitioning == false)
        {
            initialSky = RenderSettings.skybox;
            currentSkyIndex = (currentSkyIndex + 1) % skyMaterials.Length;
            targetSky = skyMaterials[currentSkyIndex];
            transition = 0.0f;
            isTransitioning = true;
        }
    }
    void Transition()
    {
        transition += Time.deltaTime / transitionDuration;
        if (transition >= 1.0f)
        {
            transition = 1.0f;
            isTransitioning = false;
        }
        RenderSettings.skybox.Lerp(initialSky, targetSky, transition);
    }
}
