using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerInput playerInput;
    static public HandInteractible handTarget;
    static public HandInteractible objectInBackpack;
    public bool canMove;

    void Awake()
    {
        canMove = true;
        handTarget = null;
        //playerStats.canMove = true;
    }

    //Link
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started) playerStats.JumpTrigger();
    }

    public void Hook(InputAction.CallbackContext context)
    {
        if (context.started) playerStats.HookTrigger();        
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Drop Action");
            if (objectInBackpack) objectInBackpack.DropedAction();
        }
    }

}
