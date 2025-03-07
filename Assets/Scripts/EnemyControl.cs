using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private RunnerControl runner;
    private ParticlesManager particlesManager;
    [SerializeField]
    private BoxCollider attackTrigger;
    private Vector3 normalSize = new Vector3(1.5f, 1, 1.5f);
    private Vector3 attackSize = new Vector3(1.5f, 1, 5f);
    public float speed = 10f;
    private float timerAttack = 0.0f;
    private float attackDuration = 1f;
    private float attackRange = 30f;
    private bool isAttacking = false;
    public bool isAlive;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isAlive)
        {
            if (!runner.isAttacking)
            {
                if (runner.Health > 0 && runner.Invinsible == false)
                {
                    SoundsManager.instance.PlaySFX("SwordImpact", 0.75f);
                    runner.TakeDmg();
                    runner.speed = 5f;
                    if (particlesManager != null)
                    {
                        particlesManager.PlayEffect("BloodHit", new Vector3(runner.transform.position.x, runner.transform.position.y + 1.5f, runner.transform.position.z));
                    }
                }
            }
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        runner = FindObjectOfType<RunnerControl>();
        particlesManager = FindObjectOfType<ParticlesManager>();
        isAlive = true;
    }

    void Update()
    {
        if (runner.isAlive)
        {


            if (Vector3.Distance(transform.position, runner.transform.position) <= attackRange && Vector3.Distance(transform.position, runner.transform.position) >= 3f)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(runner.transform.position.x, 0.1f, runner.transform.position.z), speed * Time.deltaTime);
                //pour qu'il regarde vers le joueur
                Vector3 direction = (runner.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                //pour bloquer la rotation x et z
                Vector3 lookRotationEuler = lookRotation.eulerAngles;
                lookRotationEuler.x = 0;
                lookRotationEuler.z = 0;
                //on applique la rotation Y
                Quaternion lookRotationY = Quaternion.Euler(lookRotationEuler);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotationY, 100f * Time.deltaTime);
            }
            if (Vector3.Distance(transform.position, runner.transform.position) <= 10f)
            {
                timerAttack += 4 * Time.deltaTime;
                if (timerAttack >= 1)
                {
                    isAttacking = true;
                    timerAttack = 0;
                }
            }
        }
        if (isAttacking)
        {
            StartCoroutine(Attack());
        }
        if (isAlive == false)
        {
            StartCoroutine(Die());
        }
    }
    IEnumerator Attack()
    {
        animator.SetBool("Attack", true);
        attackTrigger.size = attackSize; // on augmente et déplace le collider 
        attackTrigger.center = new Vector3(0, 0.5f, 1);

        yield return new WaitForSeconds(attackDuration);

        attackTrigger.size = normalSize;
        attackTrigger.center = new Vector3(0, 0.5f, 0);
        animator.SetBool("Attack", false);
        isAttacking = false;
    }
    IEnumerator Die()
    {
        animator.SetBool("isAlive", false);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
