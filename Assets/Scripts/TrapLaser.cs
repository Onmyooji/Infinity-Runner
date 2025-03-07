using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLaser : MonoBehaviour
{
    [SerializeField]
    private Transform PointA;
    [SerializeField]
    private Transform PointB;
    private float speed = 5.0f;
    private Vector3 targetPoint;
    void Start()
    {
        if (transform.localPosition.z < 30)
        {
            targetPoint = PointB.position;
        }
        else
        {
            targetPoint = PointA.position;
        }
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint) < 0.01f)
        {
            if (targetPoint == PointA.position)
            {
                targetPoint = PointB.position;
            }
            else
            {
                targetPoint = PointA.position;
            }
        }
    }
}
