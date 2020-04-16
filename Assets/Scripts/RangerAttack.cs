using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttack : MonoBehaviour
{
    [SerializeField] float range = 3f;
    [SerializeField] float timeBAttack = 1f;
    Animator animator;
    GameObject Player;
    bool PlayerInRange;
    EnemyHealth enemyHealth;
    private GameObject arrow;
    [SerializeField] Transform fireLoc;
    void Start()
    {
        arrow = GameManager.instance.Arrow;
        Player = GameManager.instance.Player;
        animator = GetComponent<Animator>();
        StartCoroutine(attack());
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < range && enemyHealth.IsAlive)
        {
            PlayerInRange = true;
            animator.SetBool("PlayerInRange", true);
            RotateTowards(Player.transform);
        }
        else
        {
            PlayerInRange = false;
            animator.SetBool("PlayerInRange", false);
        }
    }
    IEnumerator attack()
    {
        if (PlayerInRange && !GameManager.instance.GameOver)
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
    public void FireArrow()
    {
        GameObject newArrow = Instantiate(arrow) as GameObject;
        newArrow.transform.position = fireLoc.position;
        newArrow.transform.rotation = transform.rotation;
        newArrow.GetComponent<Rigidbody>().velocity = transform.forward * 25f;
    }
}