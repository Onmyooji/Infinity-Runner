using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public static MapControl Instance;
    public SkyManager Sky;
    public GameObject Fog;
    [SerializeField]
    private GameObject GroundStart;
    private GameObject FirstGround;
    [SerializeField]
    private GameObject DecorStartL;
    private GameObject FirstDecorL;
    [SerializeField]
    private GameObject DecorStartR;
    private GameObject FirstDecorR;
    private GameObject[] GroundOnStage;
    private GameObject[] DecorLOnStage;
    private GameObject[] DecorROnStage;
    private GameObject Runner;
    private float GroundSize;
    public GameObject[] groundsLevel1;
    public GameObject[] groundsLevel2;
    public GameObject[] groundsLevel3;
    public GameObject[] leftDecor;
    public GameObject[] rightDecor;
    private List<GameObject[]> groundLevels;
    private float distLevel2 = 1000f;
    private float distLevel3 = 2000f;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    void Start()
    {
        Runner = GameObject.Find("Character");
        SoundsManager.instance.PlayMusic("Music_Level1");
        FirstGround = Instantiate(GroundStart, transform.position, Quaternion.identity);
        FirstDecorL = Instantiate(DecorStartL, DecorStartL.transform.position, Quaternion.identity);
        FirstDecorR = Instantiate(DecorStartR, DecorStartR.transform.position, Quaternion.identity);
        Fog.SetActive(false);
        groundLevels = new List<GameObject[]>() { groundsLevel1, groundsLevel2, groundsLevel3 };
        GroundOnStage = new GameObject[groundsLevel1.Length];
        for (int i = 0; i < groundsLevel1.Length; i++)
        {
            int n = Random.Range(0, groundsLevel1.Length);
            GroundOnStage[i] = Instantiate(groundsLevel1[n]);
        }
        DecorLOnStage = new GameObject[leftDecor.Length];
        for (int i = 0; i < leftDecor.Length; i++)
        {
            int n = Random.Range(0, leftDecor.Length);
            DecorLOnStage[i] = Instantiate(leftDecor[n]);
        }
        DecorROnStage = new GameObject[rightDecor.Length];
        for (int i = 0; i < rightDecor.Length; i++)
        {
            int n = Random.Range(0, rightDecor.Length);
            DecorROnStage[i] = Instantiate(rightDecor[n]);
        }
        GroundSize = GroundOnStage[0].GetComponentInChildren<Transform>().Find("Road").localScale.z;

        float pos = Runner.transform.position.z;
        foreach (var road in GroundOnStage)
        {
            pos += GroundSize;
            road.transform.position = new Vector3(0, 0, pos);
        }
        pos = Runner.transform.position.z;
        foreach (var decor in DecorLOnStage)
        {
            pos += GroundSize;
            decor.transform.position = new Vector3(decor.transform.position.x, decor.transform.position.y, pos);
        }
        pos = Runner.transform.position.z;
        foreach (var decor in DecorROnStage)
        {
            pos += GroundSize;
            decor.transform.position = new Vector3(decor.transform.position.x, decor.transform.position.y, pos);
        }
    }

    void Update()
    {
        float runnerPosZ = Runner.transform.position.z;
        if (GroundStart.transform.position.z + GroundSize / 2 < runnerPosZ - 6f)
        {
            Destroy(FirstGround);
            Destroy(FirstDecorL);
            Destroy(FirstDecorR);
        }
        for (int i = GroundOnStage.Length - 1; i >= 0; i--)
        {
            GameObject road = GroundOnStage[i];
            if (road.transform.position.z + GroundSize/2 < runnerPosZ - 6f)
            {
                float roadZ = road.transform.position.z;
                Destroy(road);
                GameObject[] currentGrounds;
                if (runnerPosZ >= distLevel3)
                {
                    currentGrounds = groundsLevel3;
                    if (Sky.currentSkyIndex != 2)
                    {
                        Sky.StartTransition();
                    }
                    Fog.SetActive(true);
        
                    if (SoundsManager.instance.currentlyMusicName != "Music_Level3")
                    {
                        SoundsManager.instance.PlayMusic("Music_Level3");
                    }
                }
                else if(runnerPosZ >= distLevel2)
                {
                    currentGrounds = groundsLevel2;
                    if (Sky.currentSkyIndex != 1)
                    {
                        Sky.StartTransition();
                    }
                    if (SoundsManager.instance.currentlyMusicName != "Music_Level2")
                    {
                        SoundsManager.instance.PlayMusic("Music_Level2");
                    }
                }
                else
                {
                    currentGrounds = groundsLevel1;
                }
                int n = Random.Range(0, currentGrounds.Length);
                road = Instantiate(currentGrounds[n]);
                road.transform.position = new Vector3(0, 0, roadZ + GroundSize * currentGrounds.Length);
                GroundOnStage [i] = road;
            }
        }
        //Pour le décor
        for (int i = DecorLOnStage.Length - 1; i >= 0; i--)
        {
            GameObject decor = DecorLOnStage[i];
            if (decor.transform.position.z + GroundSize / 2 < runnerPosZ - 6f)
            {
                float decorZ = decor.transform.position.z;
                Destroy(decor);

                int n = Random.Range(0, leftDecor.Length);
                decor = Instantiate(leftDecor[n]);
                decor.transform.position = new Vector3(decor.transform.position.x, decor.transform.position.y, decorZ + GroundSize * leftDecor.Length);
                DecorLOnStage[i] = decor;
            }
        }
        for (int i = DecorROnStage.Length - 1; i >= 0; i--)
        {
            GameObject decor = DecorROnStage[i];
            if (decor.transform.position.z + GroundSize / 2 < runnerPosZ - 6f)
            {
                float decorZ = decor.transform.position.z;
                Destroy(decor);

                int n = Random.Range(0, rightDecor.Length);
                decor = Instantiate(rightDecor[n]);
                decor.transform.position = new Vector3(decor.transform.position.x, decor.transform.position.y, decorZ + GroundSize * rightDecor.Length);
                DecorROnStage[i] = decor;
            }
        }
    }
}
