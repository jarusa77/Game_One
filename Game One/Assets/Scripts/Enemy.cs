using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int MaxHealth;
    int CurrentHealth;
    public int attackDamage=5;

    private bool enemyDied;

    private Animator EnemyAnimator;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        EnemyAnimator = GetComponent<Animator>();
        enemyDied = false;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if(EnemyAnimator != null)
            EnemyAnimator.SetBool("Damage", true);

        if (CurrentHealth < 0)
        {
            EnemyAnimator.SetBool("Die", true);
            //Die();
            enemyDied = true;
        }

    }

    private void Die()
    {
        //disable enemy
        Debug.Log("Enemy has Died!");



        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D player)
    {
        if (player.gameObject.CompareTag("Player") && !enemyDied)
        {
            player.gameObject.GetComponent<PlayerAction>().TakeDamage(attackDamage);
            Debug.Log("Player was damaged");
        }
    }

}
