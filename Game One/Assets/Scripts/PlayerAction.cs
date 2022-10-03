using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [Header("Interaction")]
    public Rigidbody2D rb;
    public Transform floorCheck;
    public LayerMask floorLayer;
    public Transform attackPoint;
    public LayerMask enemyLayers;


    [Header("Attributes")]
    public float speed;
    public float jumpingPower;
    public int attackDamage = 10;
    public float attackRange;

    [Header("States")]
    public bool OnGround;
    public int PlayerState;

    private float horizontal;
    private bool isFacingRight = true;
    private enum Status { idle, walk, jump, attack, jumpAttack }

    private Animator PlayerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        OnGround = IsGrounded();
        PlayerAnimator = GetComponent<Animator>();
        PlayerState = (int)Status.idle;
        PlayerAnimator.SetInteger("Player State", PlayerState);
    }

    // Update is called once per frame
    void Update()
    {
        OnGround = IsGrounded();

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if ((!isFacingRight && horizontal > 0f) ||
           (isFacingRight && horizontal < 0f))
        {
            Flip();
        }

        //Animation  States
        if (OnGround)
        {
            PlayerAnimator.SetBool("JumpAttack", false);

            if (horizontal != 0f)
            {
                PlayerState = (int)Status.walk;
                PlayerAnimator.SetInteger("Player State", PlayerState);

            }
            else
            {
                PlayerState = (int)Status.idle;
                PlayerAnimator.SetInteger("Player State", PlayerState);
            }
        }
        else
        {
            PlayerState = (int)Status.jump;
            PlayerAnimator.SetInteger("Player State", PlayerState);
        }

    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        
        //PlayerState = (int)Status.walk;
       // PlayerAnimator.SetInteger("Player State", PlayerState);
        
        //UnityEngine.Debug.Log("Move");

    }

    public void Jump(InputAction.CallbackContext context)
    {

        UnityEngine.Debug.Log("Jump");



        //UnityEngine.Debug.Log("jump)");
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            

        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(floorCheck.position, 0.2f, floorLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Attack(InputAction.CallbackContext context)
    {

        string stateName;


        //Animation
        if (IsGrounded())
        {
            stateName = "Attack";

            if (context.performed)
            {
                PlayerAnimator.SetTrigger("Attack");
                horizontal = horizontal * 0.5f;
                Debug.Log(stateName + " was performed");
            }
            else if (context.canceled)
                horizontal = context.ReadValue<Vector2>().x;
        }
        else
        {
            stateName = "jumpAttack";
            if (context.performed)
            {
                PlayerAnimator.SetBool("JumpAttack", true);
                horizontal = horizontal * 0.5f;
                Debug.Log(stateName + " was performed");
            }
            else if (context.canceled)
                horizontal = context.ReadValue<Vector2>().x;
        }

    }
    
    private void DamageEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
          enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            Debug.Log(enemy.name + " was damaged");
        }

    }

    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);


    }
}
