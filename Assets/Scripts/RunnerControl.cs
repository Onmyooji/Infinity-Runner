using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RunnerControl : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private ParticlesManager particlesManager;
    private SceneChanger sceneChanger;
    private BtnBreak btnBreak;
    [SerializeField]
    private float jumpForce = 12f;
    private int jumpCount = 0;
    [SerializeField]
    private bool isGrounded;
    public float speed = 1f;
    public bool isAttacking = false;
    public bool isAlive = true;
    private bool isDead;
    private float attackDuration = 0.5f;
    [SerializeField]
    private BoxCollider attackTrigger;
    private Vector3 normalSize = new Vector3(1f, 1.1f, 0.6f);
    private Vector3 attackSize = new Vector3(1.5f, 1.1f, 2f);
    private Rigidbody rb;
    [SerializeField]
    public int Health;
    private Renderer[] renderers;
    private Color blinkColor = Color.red;
    private Color[] originalColors;
    private int blinkNb = 4;
    private float blinkDuration = 0.1f;
    [SerializeField]
    private float timerInvinsible = 0.0f;
    public bool Invinsible;
    [SerializeField]
    public Image healthBarGreen;
    public bool isRunning;
    [SerializeField]
    public AudioSource runningSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isJump", false);
            animator.SetBool("DoubleJump", false);
            isGrounded = true;
            jumpCount = 0;
        }
        if (collision.gameObject.CompareTag("Object"))
        {
            speed = 0.0f;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 4f);
            SoundsManager.instance.PlaySFX("GroundImpact", 0.5f);
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            runningSound.enabled = false;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (isRunning)
            {
                runningSound.enabled = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                EnemyControl enemy = other.GetComponent<EnemyControl>();
                if (enemy.isAlive)
                {
                    SoundsManager.instance.PlaySFX("SwordImpact", 0.75f);
                    SoundsManager.instance.PlaySFX("TurtleDeath", 0.75f);
                    enemy.isAlive = false;
                    if (particlesManager != null)
                    {
                        particlesManager.PlayEffect("BloodHit", new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1.5f, enemy.transform.position.z - 3));
                    }
                    ScoreManager.Instance.AddScore(200);
                }
            }
        }
    }
    void Start()
    {
        isRunning = false;
        btnBreak = FindObjectOfType<BtnBreak>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        particlesManager = FindObjectOfType<ParticlesManager>();
        sceneChanger = FindObjectOfType<SceneChanger>();
        transform.position = new Vector3(0f, 0f, 0f);
        Health = 100;
        //on gère les couleurs pour le blink
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];
        for(int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }
    void Update()
    {
        if (isRunning)
        {
            if (isAlive)
            {
                if (Health <= 0)
                {
                    isAlive = false;
                }
                speed += 9f * Time.deltaTime;
                if (speed >= 25f)
                {
                    speed = 25f;
                }
                animator.SetFloat("Speed", speed);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);

                if (btnBreak.isPause == false)
                {
                    Move();
                    if (Input.GetKeyDown(KeyCode.UpArrow) && isAttacking == false)
                    {
                        if (isGrounded)
                        {
                            SoundsManager.instance.PlaySFX("SwordAttack", 0.5f);
                        }
                        else
                        {
                            SoundsManager.instance.PlaySFX("JumpAttack", 0.5f);
                        }
                        StartCoroutine(Attack());
                    }
                }

                if (Invinsible)
                {
                    timerInvinsible += 2 * Time.deltaTime;
                    if (timerInvinsible >= 2)
                    {
                        Invinsible = false;
                        timerInvinsible = 0;
                    }
                }

                // Pour éviter les bugs

                //pour eviter que le perso sorte des "lignes" de déplacement
                if (transform.position.x != 0f && transform.position.x <= 0.75f && transform.position.x >= -0.75f)
                {
                    transform.position = new Vector3(0f, transform.position.y, transform.position.z);
                }
                if (transform.position.x != 1.5f && transform.position.x <= 2.25f && transform.position.x > 0.75f)
                {
                    transform.position = new Vector3(1.5f, transform.position.y, transform.position.z);
                }
                if (transform.position.x != 3 && transform.position.x > 2.25f)
                {
                    transform.position = new Vector3(3, transform.position.y, transform.position.z);
                }
                if (transform.position.x != -1.5f && transform.position.x >= -2.25f && transform.position.x < -0.75f)
                {
                    transform.position = new Vector3(-1.5f, transform.position.y, transform.position.z);
                }
                if (transform.position.x != -3 && transform.position.x < -2.25f)
                {
                    transform.position = new Vector3(-3, transform.position.y, transform.position.z);
                }

                //si le personnage passe sous la map
                if (transform.position.y <= -5f)
                {
                    transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
                }
            }
            else if (!isDead)
            {
                runningSound.enabled = false;
                StartCoroutine(Die());
                isDead = true;
                Debug.Log("You are dead");
            }
        }
    }
    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < 2.5f && isGrounded)
        {
            transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > -2.5f && isGrounded)
        {
            transform.position = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            if (isGrounded)
            {
                animator.SetBool("isJump", true);
                SoundsManager.instance.PlaySFX("Jump", 0.75f);
                Jump();
            }
            else
            {
                animator.SetBool("DoubleJump", true);
                animator.SetBool("isJump", true);
                SoundsManager.instance.PlaySFX("DoubleJump", 0.75f);
                Jump();
            }
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); //remettre à 0 la vélocité avant le saut
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpCount++;

    }
    public void TakeDmg()
    {
        Health -= 10;
        healthBarGreen.rectTransform.sizeDelta = new Vector2(healthBarGreen.rectTransform.sizeDelta.x - 20, healthBarGreen.rectTransform.sizeDelta.y);
        Invinsible = true;
        animator.SetTrigger("GetHit");
        SoundsManager.instance.PlaySFX("HurtMale", 0.75f);
        StartCoroutine(Blink());
    }
    IEnumerator Attack()
    {
        animator.SetBool("isAttacking", true);
        isAttacking = true;
        attackTrigger.size = attackSize; // on augmente et déplace le collider 
        attackTrigger.center = new Vector3(0, 0.55f, 0.5f);

        yield return new WaitForSeconds(attackDuration);

        animator.SetBool("isAttacking", false);
        isAttacking = false;
        attackTrigger.size = normalSize;
        attackTrigger.center = new Vector3(0, 0.55f, 0.1f);
    }
    IEnumerator Blink()
    {
        if (Invinsible)
        {
            for (int i = 0; i < blinkNb; i++)
            {
                foreach (var renderer in renderers)
                {
                    renderer.material.color = blinkColor;
                }
                yield return new WaitForSeconds(blinkDuration);
                for (int j = 0; j < renderers.Length; j++)
                {
                    renderers[j].material.color = originalColors[j];
                }
                yield return new WaitForSeconds(blinkDuration);
            }
        }
    }

    IEnumerator Die()
    {
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(3f);
        if (SoundsManager.instance.currentlyMusicName == "Music_Level1")
        {
            SoundsManager.instance.StopMusic("Music_Level1");
        }
        if (SoundsManager.instance.currentlyMusicName == "Music_Level2")
        {
            SoundsManager.instance.StopMusic("Music_Level2");
        }
        if (SoundsManager.instance.currentlyMusicName == "Music_Level3")
        {
            SoundsManager.instance.StopMusic("Music_Level3");
        }
        sceneChanger.FadeToScene("SceneScore");
    }
}
