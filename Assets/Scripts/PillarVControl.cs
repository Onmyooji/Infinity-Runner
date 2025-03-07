using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    RunnerControl runner;
    public Transform pointA;
    public Transform pointB;
    private float speed = 2.0f;
    private Vector3 targetPoint;
    
    void Start()
    {
        runner = FindObjectOfType<RunnerControl>();
        targetPoint = pointA.position;
    }

    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint) < 0.01f)
        {
            if (targetPoint == pointA.position)
            {
                targetPoint = pointB.position;
            }
            else
            {
                targetPoint = pointA.position;
            }
        }
    }
}
