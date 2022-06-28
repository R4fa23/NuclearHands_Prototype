using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookHand : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D hand;

    [SerializeField]
    PlayerStats playerStats;
    Vector2 startPosition;

    bool canHook = true;
    bool startHook;
    bool stoped;
    bool returning;

    void Start()
    {
        startPosition = hand.transform.localPosition;
    }

    void FixedUpdate()
    {
        if (startHook) StartCoroutine(HandGoing());
        else StopCoroutine(HandGoing());

        if(stoped) StartCoroutine(HandStoped());
        else StopCoroutine(HandStoped());

        if(returning) StartCoroutine(HandReturning());
        else StopCoroutine(HandReturning());
    }
   
    IEnumerator HandGoing()
    {
        hand.velocity = playerStats.handSpeed * hand.transform.right;
        float actualDistance = Vector2.Distance(startPosition, hand.transform.localPosition);

        while (actualDistance <= playerStats.range) yield return null;

        stoped = true;
        startHook = false;
    }

    IEnumerator HandStoped()
    {
        hand.velocity = Vector2.zero;

        yield return new WaitForSeconds(playerStats.stopedTimer);
                
        returning = true;
        stoped = false;
    }    

    IEnumerator HandReturning()
    {
        hand.velocity = -playerStats.handSpeed * hand.transform.right;

        float actualDistance = Vector2.Distance(startPosition, hand.transform.localPosition);        
        
        yield return new WaitUntil(() => actualDistance <= .1f);

        hand.velocity = Vector2.zero;
        hand.transform.localPosition = startPosition;
        canHook = true;
        playerStats.StopPlayer(false);
        returning = false;

    }


    public void Hook(InputAction.CallbackContext context)
    {
        if (context.started && canHook)
        {
            playerStats.StopPlayer(true);
            startHook = true;
            canHook = false;
        }
    }
}
