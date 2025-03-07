using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarHControl : MonoBehaviour
{
    public Transform pointC;
    public Transform pointD;
    private float speed = 2.0f;
    private Vector3 targetPoint;

    void Start()
    {
        targetPoint = pointC.position;
    }

    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint) < 0.01f)
        {
            if (targetPoint == pointC.position)
            {
                targetPoint = pointD.position;
            }
            else
            {
                targetPoint = pointC.position;
            }
        }
    }
}
