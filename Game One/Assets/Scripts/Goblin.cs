using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    [Header("Attributes")]
    public Transform sight;
    public Transform player;


    //Patrol
    [Header("Patrol Movement")]
    
    public float distanceDown = 2f;
    public float pauseTime = 2f;
    public float sightRange = 5f;



    private float pauseTimer = Mathf.Infinity;
    private bool paused;

    private float AttackCoolDownTimer = Mathf.Infinity;
    public float AttackCoolDown = 2f;
    public Transform attackPoint;
    public float attackRange;
    public float attackDash = 2f;
    private float slowDown;


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
                inRange = true;

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
        rb.velocity = new Vector2(direction * speed * attackDash, attackDash / 2);

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

        Debug.Log(" edge found");

        RaycastHit2D groundInfo = Physics2D.Raycast(sight.position, Vector2.down, distanceDown);
        Transform groundObject = groundInfo.transform;

        if (!groundInfo.collider && !paused)
        {
            Debug.Log(" edge found");
            slowDown -= 0.20F;

            if (slowDown < 0)
            {
                pauseTimer = 0;
                paused = true;
                EnemyAnimator.SetBool("Idle", true);
            }
            else
                rb.velocity = new Vector2(direction * speed * slowDown, rb.velocity.y);

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
            slowDown = 1f;



        }

    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(sight.position, distanceDown);


    }




    public override void TakeDamage(int damage)
    {

        if (CurrentHealth < 0)
            return;

        CurrentHealth -= damage;

        if (EnemyAnimator != null)
            EnemyAnimator.SetBool("Damage", true);


        if ((transform.position.x > player.position.x && isFacingRight) || (transform.position.x < player.position.x && !isFacingRight))
            Flip();



        if (CurrentHealth < 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            EnemyAnimator.SetBool("Die", true);
            enemyDied = true;
        }
    }
}
