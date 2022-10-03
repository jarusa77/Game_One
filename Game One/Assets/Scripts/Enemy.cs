using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int MaxHealth;
    int CurrentHealth;

    private Animator EnemyAnimator;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        EnemyAnimator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        //toDo hurt animation
        if(EnemyAnimator != null)
            EnemyAnimator.SetBool("damage", true);

        if (CurrentHealth < 0)
        {
            Die();
        }

    }

    private void Die()
    {
        //disable enemy
        Debug.Log("Enemy has Died!");

        Destroy(gameObject);
    }

}
