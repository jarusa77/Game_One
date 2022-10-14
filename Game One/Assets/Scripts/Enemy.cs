using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [Header("Attributes")]
    public Rigidbody2D rb;
    public int MaxHealth;
    public int CurrentHealth;
    public int attackDamage=5;
    public Transform sight;


    //Private Attributes
    private bool enemyDied;
    private Animator EnemyAnimator;

    //Patrol
    [Header("Patrol Movement")]
    public float speed;
    public float distanceDown = 2f;
    public float pauseTime = 2f;
    public LayerMask floorLayerMask;
    public LayerMask playerLayers;
    public float sightRange = 5f;

    private int floorLayer;
    private float direction = -1;//determines direction based on facing 1=right , -1 = left
    private bool isFacingRight = false;
    private float pauseTimer = Mathf.Infinity;
    private bool paused;

    private float AttackCoolDownTimer = Mathf.Infinity;
    public float AttackCoolDown = 2f;
    public Transform attackPoint;
    public float attackRange;
    public float attackDash=2f;

    


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        EnemyAnimator = GetComponent<Animator>();
        enemyDied = false;
        floorLayer = LayerMask.NameToLayer("Floor");
        AttackCoolDownTimer = 0;


    }


    void Update()
    {
        AttackCoolDownTimer += Time.deltaTime;
        pauseTimer += Time.deltaTime;

        if (PlayerInRange())
            Attack();
        else
            patrol();
    }



    private bool PlayerInRange()
    {
        bool inRange = false;

        RaycastHit2D Sight;

        if (isFacingRight)
            Sight = Physics2D.Raycast(sight.position, Vector2.right, sightRange);
        else
            Sight = Physics2D.Raycast(sight.position, Vector2.left, sightRange);
        

        if (Sight.collider)
        {
            Transform SightObject = Sight.transform;

            if (SightObject.gameObject.CompareTag("Player"))
                inRange= true;

        }

        return inRange;


    }

    void Attack()
    {

        //Debug.Log("Attack");

        if (AttackCoolDownTimer <= AttackCoolDown)
            return;

        EnemyAnimator.SetTrigger("Attack");
        

        AttackCoolDownTimer = 0;
    }

    void AttackDash()
    {
        rb.velocity = new Vector2(direction * speed * attackDash, rb.velocity.y);
    }

    private void DamagePlayer()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        Debug.Log("Damage Called");

        foreach (Collider2D hitPlayer in hitPlayers)
        {
            hitPlayer.GetComponent<PlayerAction>().TakeDamage(attackDamage);
            Debug.Log(hitPlayer.name + " was damaged");
        }
        
    }

    void patrol()
    {

        if (AttackCoolDownTimer <= AttackCoolDown)
            return;

        if (pauseTimer <= pauseTime)
            return;

        Debug.Log(" pause");

        RaycastHit2D groundInfo = Physics2D.Raycast(sight.position, Vector2.down, distanceDown);

        Transform groundObject = groundInfo.transform;

        if (!groundInfo.collider && !paused)
        {
            pauseTimer = 0;
            paused = true;
            EnemyAnimator.SetBool("Idle",true);
               
        }
        else if (!groundInfo)
        {
            Flip();
            paused = false;
        }
        else if (groundObject.gameObject.layer == floorLayer) //prevents walking on  player
        {
            EnemyAnimator.SetBool("Idle", false);
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            
        }

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        if (isFacingRight)
            transform.eulerAngles = new Vector3(0, -180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
        
        direction *= -1f;
        EnemyAnimator.SetBool("Idle", false);
    }



    public void TakeDamage(int damage)
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

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);


    }

}
