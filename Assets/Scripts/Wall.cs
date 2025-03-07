using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private RunnerControl runner;
    private ParticlesManager particlesManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (runner.isAttacking && tag == "Weak")
            {
                Destroy(gameObject);
                SoundsManager.instance.PlaySFX("SwordImpactWall", 0.75f);
                if (particlesManager != null)
                {
                    particlesManager.PlayEffect("WoodEffect", transform.position);
                }
                else
                {
                    Debug.LogWarning("particlesManager est null");
                }
            }
            else
            {
                if (runner.Health > 0 && runner.Invinsible == false)
                {
                    runner.TakeDmg();
                    runner.speed = 5f;
                }
                if (tag == "Weak")
                {
                    Destroy (gameObject);
                    SoundsManager.instance.PlaySFX("WallImpact", 0.75f);
                    if (particlesManager != null)
                    {
                        particlesManager.PlayEffect("WoodEffect", transform.position);
                    }
                }
                if (tag == "IceWall")
                {
                    SoundsManager.instance.PlaySFX("BreakingGlass", 0.15f);
                    Destroy(gameObject);
                    if (particlesManager != null)
                    {
                        particlesManager.PlayEffect("IceHit", transform.position);
                    }
                }
            }
        }
    }
    void Start()
    {
        runner = FindObjectOfType<RunnerControl>();
        particlesManager = FindObjectOfType<ParticlesManager>();
        if (particlesManager == null)
        {
            Debug.LogWarning("particlesManager est null");
        }
    }
}
