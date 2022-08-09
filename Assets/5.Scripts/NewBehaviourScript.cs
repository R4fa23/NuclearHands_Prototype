using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInput playerInput;
    PlayerStats playerStats;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    public float currentInput;
    float coyoteTimeCounter;

    float smoothInputVelocity;
    float smoothInputSpeed;
    public bool jumped;
    bool grounded;
    bool overrideGravity;


    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        playerInput = playerManager.playerInput;
        playerStats = playerManager.playerStats;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    } 

    void Start()
    {
        overrideGravity = false;
    }

    void Update()
    {        
        smoothInputSpeed = playerStats.smoothInputSpeed;
        Debug.Log(Grounded());        
    }

    private void FixedUpdate()
    {
        if (playerManager.canMove) Movement();
        if (jumped && Grounded()) Jump();
        else jumped = false;


        GravityControll();
    }

    void Movement()
    {
        float input = playerInput.actions["Movement"].ReadValue<float>();    
        currentInput = Mathf.SmoothDamp(currentInput, input, ref smoothInputVelocity, smoothInputSpeed);
        rb.velocity = new Vector2(currentInput * playerStats.speed, rb.velocity.y);
    }

    void Jump()
    {
        float jumpForce = playerStats.jumpHeight;
        rb.gravityScale = rb.gravityScale = playerStats.normalGravity;
        overrideGravity = true;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        Debug.Log(rb.gravityScale);
    }
    bool Grounded()
    {
        float rangeY = .05f;
        float offSet = 0.1f;
        bool ground;
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x - offSet, boxCollider.bounds.size.y);
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxSize, 0f, Vector2.down, rangeY, playerStats.groundLayerMask);

        if (raycastHit.collider != null) ground = true;
        else ground = false;

        return ground;
    }

    void GravityControll()
    {      
        if (rb.velocity.y >= 0) rb.gravityScale = playerStats.normalGravity;
        else if (rb.velocity.y < 0) rb.gravityScale = playerStats.fallingGravity;

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

    public void JumpListener()
    {
        jumped = true;
    }
    private void OnEnable()
    {
        playerStats.JumpEvent.AddListener(JumpListener);
    }
    private void OnDisable()
    {
        playerStats.JumpEvent.RemoveListener(JumpListener);
    }
}
