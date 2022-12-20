using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerSO/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float smoothInputSpeed;
    public float speed;
    public float jumpHeight;
    public float normalGravity;
    public float fallingGravity;
    public float coyoteTime;
    public float jumpBuffer;
    public LayerMask groundLayerMask;

    [Header("Hook Hand")]
    public float handSpeed;
    public float stopedTimer;
    public float InteractiblesRange;

    //InputsWire Events
    [System.NonSerialized] public UnityEvent HookEvent;
    [System.NonSerialized] public UnityEvent JumpEvent;

    void OnEnable()
    {
        if (HookEvent == null) HookEvent = new UnityEvent();
        if (JumpEvent == null) JumpEvent = new UnityEvent();
    }

    void OnDisable()
    {
        //Debug.Log("OnDisable");
    }

    void OnDestroy()
    {
        //Debug.Log("OnDestroy");
    }

    public void JumpTrigger()
    {
        JumpEvent.Invoke();
    }
    public void HookTrigger()
    {
        HookEvent.Invoke();
    }

    private void Awake()
    {
    }

}
