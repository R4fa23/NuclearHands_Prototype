using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookHand : MonoBehaviour
{
    [SerializeField] Transform hand;
    [SerializeField] Transform backpack;
    HandInteractible target;
    CircleCollider2D handCollider;
    PlayerManager playerManager;
    Vector3 startPosition;
    bool canHook = true;
    bool startHook;
    bool stoped;
    bool returning;
    float timer;

    float handSpeed;
    float interactiblesRange;
    float stopedTimer;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        handCollider = hand.GetComponent<CircleCollider2D>();

        handSpeed = playerManager.playerStats.handSpeed;
        interactiblesRange = playerManager.playerStats.InteractiblesRange;
        stopedTimer = playerManager.playerStats.stopedTimer;
    }

    void Start()
    {
        startPosition = hand.localPosition;
    }

    void Update()
    {
        target = PlayerManager.handTarget;
        Collided();
        if (startHook) HandGoing();
        if (stoped) HandStoped();
        if (returning) HandReturning();
    }
   

    public void HandGoing()
    {
        canHook = false;
        float speed = handSpeed * Time.deltaTime;
        float actualDistance = Vector2.Distance(startPosition, hand.localPosition);
        bool reached;

        if (target)
        {
            Vector3 aim = transform.InverseTransformPoint(target.transform.position);
            hand.localPosition = Vector2.MoveTowards(hand.localPosition, aim, speed);
            if (hand.localPosition == aim) reached = true;
            else reached = false;
        }
        else
        {
            hand.localPosition += hand.right * speed;
            if (actualDistance >= interactiblesRange) 
            {
                hand.localPosition = hand.localPosition;
                reached = true;
            }
            else reached = false;
        }

        if (reached)
        {
            if(target) target.ReachedAction();
            stoped = true;
            startHook = false;
        }
    }

    public void HandStoped()
    {
        timer += Time.deltaTime;

        if (target) target.SettingVariables(hand, backpack);

        if (timer >= stopedTimer)
        {
            timer = 0;
            returning = true;
            stoped = false;
        }
    }    

    public void HandReturning()
    {
        float actualDistance = Vector2.Distance(startPosition, hand.localPosition);
        hand.localPosition = Vector3.MoveTowards(hand.localPosition, startPosition, Time.deltaTime * handSpeed / 2);

        if (target) target.HoldingAction();

        if (actualDistance <= 0.5f)
        {
            if (target) target.EndedHookAction();
            hand.transform.localPosition = startPosition;
            playerManager.canMove = true;
            canHook = true;
            returning = false;
        }      
    }

    bool Collided()
    {
        Vector2 boxSize = new Vector2(handCollider.bounds.size.x, handCollider.bounds.size.y);
        float boxRange = 0.5f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(handCollider.bounds.center, boxSize, 0, hand.right, boxRange, playerManager.playerStats.groundLayerMask);

        return raycastHit.collider != null;
    }

    public void Hook()
    {
        if (canHook)
        {
            playerManager.canMove = false;
            startHook = true;
        }
    }

    public void OnEnable()
    {
        playerManager.playerStats.HookEvent.AddListener(Hook);
    }

    public void OnDisable()
    {
        playerManager.playerStats.HookEvent.RemoveListener(Hook);
    }
}
