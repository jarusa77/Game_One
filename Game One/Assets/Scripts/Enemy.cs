using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [Header("Attributes")]
    public Rigidbody2D rb;
    public int MaxHealth;
    public int CurrentHealth;

    public float speed;
    public LayerMask floorLayerMask;
    public LayerMask playerLayers;
    public int attackDamage = 5;

    //Protected Attributes
    protected bool enemyDied;
    protected Animator EnemyAnimator;
    protected bool isFacingRight = false;
    protected int floorLayer;
    protected float direction = -1;//determines direction based on facing 1=right , -1 = left

    protected void Flip()
    {
        isFacingRight = !isFacingRight;
        if (isFacingRight)
            transform.eulerAngles = new Vector3(0, -180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
        
        direction *= -1f;
        
    }

    public virtual void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if(EnemyAnimator != null)
            EnemyAnimator.SetBool("Damage", true);

        if (CurrentHealth < 0)
        {
            EnemyAnimator.SetBool("Die", true);
            enemyDied = true;
        }
    }

    protected void Die()
    {
        //disable enemy
        Debug.Log("Enemy has Died!");
        Destroy(gameObject);
    }


    protected void OnCollisionEnter2D(Collision2D player)
    {
        if (player.gameObject.CompareTag("Player") && !enemyDied)
        {
            player.gameObject.GetComponent<PlayerAction>().TakeDamage(attackDamage);
            Debug.Log("Player was damaged");
        }
    }


}
