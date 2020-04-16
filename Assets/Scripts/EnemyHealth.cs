using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int startingHealth = 20;
    [SerializeField] float timeSinceLastHit = 0.5f;
    [SerializeField] float dissapearSpeed = 2f;
    AudioSource audio;
    float timer = 0f;
    Animator animator;
    NavMeshAgent nav;
    bool isAlive;
    Rigidbody rigidbody;
    CapsuleCollider capsuleCollider;
    bool dissapearEnemy = false;
    int currentHealth;
    ParticleSystem Blood;

    public bool IsAlive
    {
        get { return isAlive; }
    }

    
    void Start()
    {
        GameManager.instance.RegisterEnemy(this);
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        isAlive = true;
        currentHealth = startingHealth;
        Blood = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (dissapearEnemy)
        {
            transform.Translate(Vector3.down * dissapearSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(timer > timeSinceLastHit && !GameManager.instance.GameOver)
        {
            if(other.tag == "PlayerWeapon")
            {
                takeHit();
                timer = 0f;
            }
        }
    }
    void takeHit()
    {
        if (currentHealth > 0)
        {
            audio.PlayOneShot(audio.clip);
            animator.Play("Hurt");
            currentHealth -= 10;
            Blood.Play();
        }
        if (currentHealth <= 0)
        {
            isAlive = false;
            KillEnemy();
        }
    }
    void KillEnemy()
    {
        GameManager.instance.KilledEnemy(this);
        capsuleCollider.enabled = false;
        nav.enabled = false;
        animator.SetTrigger("Die");
        rigidbody.isKinematic = true;
        Blood.Play();
        StartCoroutine(removeEnemy());
    }
    IEnumerator removeEnemy()
    {
        yield return new WaitForSeconds(5f);
        dissapearEnemy = true;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
