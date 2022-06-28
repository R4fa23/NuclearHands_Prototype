using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerSO/PlayerStats") ]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public bool canMove;
    public float speed;
    public float jumpHeight;
    public float normalGravity;
    public float fallingGravity;
    public LayerMask groundLayerMask;

    [Header("Hook Hand")]

    public float range;
    public float handSpeed;
    public float stopedTimer;

    [System.NonSerialized]
    public UnityEvent stopPlayerEvent;

    
    public void OnEnable()
    {
        canMove = true;
        if (stopPlayerEvent == null)
            stopPlayerEvent = new UnityEvent();
        Debug.Log("OnEnable");
    }


    public void StopPlayer(bool stop)
    {
        if (stop) canMove = false;        
        else canMove = true;        
        stopPlayerEvent.Invoke();
    }

    public void OnDisable()
    {
        Debug.Log("OnDisable");
    }

    public void OnDestroy()
    {
        Debug.Log("OnDestroy");
    }
    
}
