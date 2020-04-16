using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject Hero;
    [SerializeField] GameObject Tank;
    [SerializeField] GameObject Sold;
    [SerializeField] GameObject Range;

    Animator heroAnim;
    Animator TankAnim;
    Animator SoldAnim;
    Animator RangeAnim;

    void Start()
    {
        heroAnim = Hero.GetComponent<Animator>();
        TankAnim = Tank.GetComponent<Animator>();
        SoldAnim = Sold.GetComponent<Animator>();
        RangeAnim = Range.GetComponent<Animator>();
        StartCoroutine(ShowCase());
    }

    void Update()
    {
        
    }
    IEnumerator ShowCase()
    {
        yield return new WaitForSeconds(1f);
        heroAnim.Play("SpinAttack");
        yield return new WaitForSeconds(1f);
        TankAnim.Play("Attack");
        yield return new WaitForSeconds(1f);
        SoldAnim.Play("Attack");
        yield return new WaitForSeconds(1f);
        RangeAnim.Play("Attack");
        StartCoroutine(ShowCase());
    }
    public void Battle()
    {
        SceneManager.LoadScene("Level");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
