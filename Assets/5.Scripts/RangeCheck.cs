using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeCheck : MonoBehaviour
{
    public HandInteractible bestTarget;

    [SerializeField] Image highlight;
    [SerializeField] Transform playerCenter;

    PlayerManager playerManager;
    HandInteractible[] interactibles;
    float range;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        range = playerManager.playerStats.InteractiblesRange;
        PlayerManager.handTarget = SerachingForInteractibles();
    }

    HandInteractible SerachingForInteractibles()
    {
        float closestDistance = Mathf.Infinity;
        bestTarget = null;
        interactibles = FindObjectsOfType<HandInteractible>();
        
        foreach (var interact in interactibles)
        {
            float distanceToInteratible = (interact.transform.position - playerCenter.position).sqrMagnitude;
            
            if (distanceToInteratible < closestDistance && distanceToInteratible <= range*range && !interact.inBackpack)
            {                
                closestDistance = distanceToInteratible;
                bestTarget = interact;
            }
        }

        if (bestTarget)
        {
            Vector3 targetPos = bestTarget.transform.position;
            Debug.DrawLine(playerCenter.position, targetPos);
            HighlightItem(targetPos, true);
        }
        else HighlightItem(transform.position, false);

        return bestTarget;
    }

    void HighlightItem(Vector3 targetPos, bool On)
    {
        if (On)
        {
            highlight.gameObject.SetActive(true);
            highlight.transform.position = targetPos;
        } 
        else highlight.gameObject.SetActive(false);
    }
}
