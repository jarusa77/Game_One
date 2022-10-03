using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [Header("Interaction")]
    public Rigidbody2D rb;
    public Transform floorCheck;
    public LayerMask floorLayer;


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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnGround = IsGrounded();

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        /*
        PlayerState = (int)Status.walk;
        PlayerAnimator.SetInteger("Player State", PlayerState);
        */
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
}
