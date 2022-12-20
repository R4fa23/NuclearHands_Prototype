using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteractible: MonoBehaviour
{
    public Transform playerHand;
    public Transform playerBackpack;
    public bool inBackpack;

    public virtual void ReachedAction()
    {
        Debug.Log("Reached");
    }

    public virtual void HoldingAction()
    {
        Debug.Log("Holding");
    }

    public virtual void EndedHookAction()
    {
        Debug.Log("Released");
    }

    public virtual void DropedAction()
    {
        inBackpack = false;
    }

    public void SettingVariables(Transform hand, Transform backpack)
    {
        playerHand = hand;
        playerBackpack = backpack;

        Debug.Log(playerHand + " " + playerBackpack);
    }
}
