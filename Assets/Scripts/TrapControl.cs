using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapControl : MonoBehaviour
{
    RunnerControl runner;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (runner.Health > 0 && runner.Invinsible == false)
            {
                runner.TakeDmg();
                if (gameObject.tag != "Lava")
                {
                    runner.speed = 5f;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (runner.Health > 0 && runner.Invinsible == false)
            {
                runner.TakeDmg();
            }
        }
    }
    void Start()
    {
        runner = FindObjectOfType<RunnerControl>();
    }
}
