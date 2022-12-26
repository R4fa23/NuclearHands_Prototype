using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestApagar : MonoBehaviour
{

    private void Update()
    {
        CastRays();
    }
    public void CastRays()
    {
        Debug.DrawRay(transform.position, Vector2.right * 1f, Color.cyan);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1f, LayerMask.GetMask("Link"));

        if (hit)
        {
            Debug.Log("Hit");
        }

    }
}
