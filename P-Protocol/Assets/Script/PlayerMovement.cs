using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private PlayerControls controls;
    private Vector2 moveInput;
    private Rigidbody rb;
    private bool isGrounded;

    //Dash
    public float dashSpeed = 25f;
    public int dashFrames = 10;            
    public int cooldownFrames = 20;        
    public int invincibleFrames = 8;

    private int dashCounter = 0;
    private int cooldownCounter = 0;
    private bool isInvincible = false;

    private Vector3 dashDirection;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

       
        controls.Player.Jump.performed += ctx => Jump();

        controls.Player.Dash.performed += ctx => Dash();

    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x * moveSpeed, rb.velocity.y, 0);
        rb.velocity = movement;
        RotateTowardsMovement();
        if (dashCounter > 0)
        {
            rb.velocity = dashDirection * dashSpeed;

            // Invincibility window
            isInvincible = dashCounter > (dashFrames - invincibleFrames);

            dashCounter--;

            if (dashCounter == 0)
            {
                rb.velocity = Vector3.zero;
                isInvincible = false;
                cooldownCounter = cooldownFrames;
            }
        }
        else
        {
            if (cooldownCounter > 0)
                cooldownCounter--;
        }
    }

    void RotateTowardsMovement()
    {
        if(moveInput.x!=0)
        {
            float targetYrotation = (moveInput.x>0) ? 90f : -90f;
            Quaternion targetRotation = Quaternion.Euler(0,targetYrotation,0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 50f);
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        }
    }

    private void Dash()
    {
        if (dashCounter > 0 || cooldownCounter > 0)
            return;

        dashDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (dashDirection == Vector3.zero)
            dashDirection = transform.forward;

        dashCounter = dashFrames;
    }



    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
