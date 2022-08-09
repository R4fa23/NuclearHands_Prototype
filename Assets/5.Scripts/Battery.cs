using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : HandInteractible
{
    public Transform dock;

    [SerializeField] float timer;
    [SerializeField] Image wheelSprite;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Transform batteryFollowPos;
    float currentTime;
    bool docked;
    bool carrying;

    private void Start()
    {
        currentTime = timer;
        docked = true;
    }

    private void Update()
    {
        if (batteryFollowPos) carrying = true;
        else carrying = false;

        if (!docked && !carrying) ReturningToDock();
        if (carrying) CarryingBattery();       
    }

    public void CarryingBattery()
    {
        WheelReset();
        docked = false;
        transform.position = batteryFollowPos.position;
    }

    public void ReturningToDock()
    {
        currentTime += Time.deltaTime;

        float normalizedValue = Mathf.Clamp(currentTime / timer, 0.0f, 1.0f);
        wheelSprite.fillAmount = normalizedValue;

        if(currentTime >= timer && dock)
        {
            wheelSprite.fillAmount = 0;
            transform.position = dock.position;
            docked = true;
            currentTime = 0;
        }
    }

    void WheelReset()
    {
        currentTime = 0;
        wheelSprite.fillAmount = 0;
    }

    public override void ReachedAction()
    {
        base.ReachedAction();
        batteryFollowPos = playerHand;
    }

    public override void HoldingAction()
    {
        base.HoldingAction();
        batteryFollowPos = playerHand;
    }

    public override void EndedHookAction()
    {
        base.EndedHookAction();
        inBackpack = true;
        PlayerManager.objectInBackpack = this;
        batteryFollowPos = playerBackpack;
        Debug.Log(PlayerManager.objectInBackpack.name);
    }

    public override void DropedAction()
    {
        base.DropedAction();
        batteryFollowPos = null;
    }

}
