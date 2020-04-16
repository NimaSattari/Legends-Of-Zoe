using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float range = 3f;
    [SerializeField] float timeBAttack = 1f;
    Animator animator;
    GameObject Player;
    bool playerInRange;
    BoxCollider[] weaponColliders;
    EnemyHealth enemyHealth;
    void Start()
    {
        weaponColliders = GetComponentsInChildren<BoxCollider>();
        Player = GameManager.instance.Player;
        animator = GetComponent<Animator>();
        StartCoroutine(attack());
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < range && enemyHealth.IsAlive)
        {
            playerInRange = true;
            RotateTowards(Player.transform);
        }
        else
        {
            playerInRange = false;
        }
    }
    IEnumerator attack()
    {
        if(playerInRange && !GameManager.instance.GameOver)
        {
            animator.Play("Attack");
            yield return new WaitForSeconds(timeBAttack);
        }
        yield return null;
        StartCoroutine(attack());
    }
    private void RotateTowards(Transform player)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }
    public void BAttack()
    {
        foreach(var weapon in weaponColliders)
        {
            weapon.enabled = true;
        }
    }
    public void EAttack()
    {
        foreach (var weapon in weaponColliders)
        {
            weapon.enabled = false;
        }
    }
}
