using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int StartingHealth = 100;
    [SerializeField] float timeSinceLastHit = 2f;
    float timer = 0f;
    CharacterController characterController;
    Animator animator;
    int currentHealth;
    AudioSource audio;
    [SerializeField] Slider healthSlider;
    ParticleSystem Blood;

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if(value < 0)
            {
                currentHealth = 0;
            }
            else
            {
                currentHealth = value;
            }
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(healthSlider);
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        currentHealth = StartingHealth;
        audio = GetComponent<AudioSource>();
        Blood = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(timer>=timeSinceLastHit && !GameManager.instance.GameOver)
        {
            if(other.tag == "Weapon")
            {
                takeHit();
                timer = 0;
            }
        }
    }
    void takeHit()
    {
        if(currentHealth > 0)
        {
            GameManager.instance.PlayerHit(currentHealth);
            animator.Play("Hurt");
            currentHealth -= 10;
            healthSlider.value = currentHealth;
            audio.PlayOneShot(audio.clip);
            Blood.Play();
        }
        else
        {
            KillPlayer();
        }
    }
    void KillPlayer()
    {
        GameManager.instance.PlayerHit(currentHealth);
        animator.SetTrigger("Die");
        characterController.enabled = false;
        audio.PlayOneShot(audio.clip);
        Blood.Play();
    }
    public void PowerUpHealth()
    {
        if(currentHealth < 70)
        {
            CurrentHealth += 30;
        }
        else if(currentHealth < StartingHealth)
        {
            CurrentHealth = StartingHealth;
        }
        healthSlider.value = currentHealth;
    }
}
