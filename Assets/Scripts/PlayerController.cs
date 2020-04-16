using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] LayerMask layerMask;
    CharacterController characterController;
    Vector3 currentLookTarget = Vector3.zero;
    Animator animator;
    BoxCollider[] swords;
    GameObject fireTrail;
    ParticleSystem fireTrailParticles;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        swords = GetComponentsInChildren<BoxCollider>();
        fireTrail = GameObject.FindGameObjectWithTag("Fire") as GameObject;
        fireTrail.SetActive(false);
    }

    void Update()
    {
        if (!GameManager.instance.GameOver)
        {
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            characterController.SimpleMove(moveDirection * moveSpeed);
            if (moveDirection == Vector3.zero)
            {
                animator.SetBool("IsWalking", false);
            }
            else
            {
                animator.SetBool("IsWalking", true);

            }
            if (Input.GetMouseButtonDown(0))
            {
                animator.Play("DoubleChop");
            }
            if (Input.GetMouseButtonDown(1))
            {
                animator.Play("SpinAttack");
            }
        }
    }
    private void FixedUpdate()
    {
        if (!GameManager.instance.GameOver)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 500, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.point != currentLookTarget)
                {
                    currentLookTarget = hit.point;
                }
                Vector3 targetPosiion = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                Quaternion rotation = Quaternion.LookRotation(targetPosiion - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10f);

            }
        }
    }
    public void BAttack()
    {
        foreach(var weapon in swords)
        {
            weapon.enabled = true;
        }
    }
    public void EAttack()
    {
        foreach (var weapon in swords)
        {
            weapon.enabled = false;
        }
    }
    public void SpeedPowerUp()
    {
        StartCoroutine(fireTrailRoutine());
    }
    IEnumerator fireTrailRoutine()
    {
        fireTrail.SetActive(true);
        moveSpeed = 8f;
        yield return new WaitForSeconds(10f);
        moveSpeed = 5f;
        fireTrailParticles = fireTrail.GetComponentInChildren<ParticleSystem>();
        var em = fireTrailParticles.emission;
        em.enabled = false;
        yield return new WaitForSeconds(3f);
        em.enabled = true;
        fireTrail.SetActive(false);
    }
}
