using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class EnemyMove : MonoBehaviour
{
    Transform Player;
    NavMeshAgent nav;
    Animator animator;
    EnemyHealth enemyHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        Player = GameManager.instance.Player.transform;
    }

    void Update()
    {
        if (!GameManager.instance.GameOver && enemyHealth.IsAlive)
        {
            nav.SetDestination(Player.position);
        }
        else if ((!GameManager.instance.GameOver || GameManager.instance.GameOver) && !enemyHealth.IsAlive)
        {
            nav.enabled = false;
        }
        else
        {
            nav.enabled = false;
            animator.Play("Idle");
        }
    }
}
