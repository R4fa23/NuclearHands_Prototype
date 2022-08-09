using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInput playerInput;
    PlayerStats playerStats;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    bool jumped;
    bool gravity;
    float jumpForce;
    public float coyoteTimeCounter;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        playerStats = playerManager.playerStats;
        playerInput = playerManager.playerInput;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        playerStats.JumpEvent.AddListener(JumpListener);
    }
    private void OnDisable()
    {
        playerStats.JumpEvent.RemoveListener(JumpListener);
    }

    void Start()
    {
        gravity = true;
        rb.gravityScale = playerStats.normalGravity;
        //isso pode bugar o pulo por causa da gravidade mudando constantemente
    }

    void FixedUpdate()
    {
        Movement();
        if (jumped) Jump();

    }

    void Update()
    {
        Grounded();
    }

    void Movement()
    {
        float inputX;

        if (playerManager.canMove)
        {
            inputX = playerInput.actions["Movement"].ReadValue<float>();
            if (rb.velocity.y >= 0) rb.gravityScale = playerStats.normalGravity;
            if (rb.velocity.y < 0) rb.gravityScale = playerStats.fallingGravity;
        }
        else
        {
            inputX = 0;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }

        rb.velocity = new Vector2(inputX * playerStats.speed, rb.velocity.y);
    }

    /*void Gravity()
    {
        if (gravity)
        {
            if (rb.velocity.y >= 0) rb.gravityScale = playerStats.normalGravity;
            if (rb.velocity.y < 0) rb.gravityScale = playerStats.fallingGravity;
        }
        else rb.gravityScale = playerStats.normalGravity;
    }*/

    bool Grounded()
    {
        float rangeY = .05f;
        float offSet = 0.1f;
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x - offSet, boxCollider.bounds.size.y);
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxSize, 0f, Vector2.down, rangeY, playerStats.groundLayerMask);

        if (raycastHit.collider != null) coyoteTimeCounter = playerStats.coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;

        if (coyoteTimeCounter > 0) return true;
        else return false;
    }

    void CoyoteTimeCounter()
    {
        if (Grounded()) coyoteTimeCounter = playerStats.coyoteTime;
        else coyoteTimeCounter -= Time.deltaTime;
    }
      
    void Jump()
    {
        gravity = false;
        jumped = false;
        jumpForce = Mathf.Sqrt(playerStats.jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        gravity = true;
        Debug.Log(jumpForce);
    }

    public void JumpListener()
    {
        if (Grounded()) jumped = true;
    }

    void DebubGroundCollider(RaycastHit2D raycastHit, float offSet, float rangeY)
    {
        Color rayColor;
        if (raycastHit.collider != null) rayColor = Color.green;
        else rayColor = Color.red;
        Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x - offSet / 2, 0), Vector2.down * (boxCollider.bounds.extents.y + rangeY), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x - offSet / 2, 0), Vector2.down * (boxCollider.bounds.extents.y + rangeY), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x - offSet / 2, boxCollider.bounds.extents.y + rangeY), Vector2.right * (boxCollider.bounds.extents.y - offSet), rayColor);
    }
}
