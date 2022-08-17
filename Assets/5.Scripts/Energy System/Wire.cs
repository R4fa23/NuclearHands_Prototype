using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    //Identificar qual é o fio que ta vindo da fonte

    public bool debugRays;
    public enum WireShape { Line, Cross, T, L, Diagonal, LDiagonal }
    public WireShape shapes;

    public bool first;
    public bool hasEnergy;
    public bool flipHorizontal;
    public bool flipVertical;

    [SerializeField] Sprite[] sprites;
    [SerializeField] bool[] connectionPoints;
    [SerializeField] SpriteRenderer spriteRender;



    int wichSprite;
    float rayLength = 0.8f;
    int poweredInputs;
    Vector3 castPoint;
    Vector3 direction;
    Color debugColor = Color.cyan;

    private void OnValidate()
    {        
        UpdateSphape();
    }

    private void Awake()
    {
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        UpdateSphape();
        StartCoroutine(CheckForOtherWires());
        Debug.Log("aaaa");
    }

    private void FixedUpdate()
    {
        if (debugRays) Debug.DrawRay(castPoint, direction * rayLength, debugColor);
    }

    IEnumerator CheckForOtherWires()
    {
        if (!first) SettingInputs();
        else hasEnergy = true;
        if (hasEnergy) spriteRender.color = Color.yellow;
        else spriteRender.color = Color.white;
        yield return new WaitForSeconds(1f);
        if (!first) StartCoroutine(CheckForOtherWires());
        Debug.Log(gameObject.name);
    }

    void UpdateSphape()
    {
        connectionPoints = new bool[7];
        for (int i = 0; i < connectionPoints.Length; i++)
        {
            connectionPoints[i] = false;
        }

        switch (shapes)
        {
            case WireShape.Line:
                wichSprite = 0;
                for (int i = 0; i < connectionPoints.Length; i++)
                {
                    if (i == 0) connectionPoints[i] = true;
                    if(i == 4) connectionPoints[i] = true;
                }
                break;
            case WireShape.Cross:
                wichSprite = 1;
                for (int i = 0; i < connectionPoints.Length; i++)
                {
                    if (i == 0) connectionPoints[i] = true;
                    if (i == 2) connectionPoints[i] = true;
                    if (i == 4) connectionPoints[i] = true;
                    if (i == 6) connectionPoints[i] = true;
                }
                break;
            case WireShape.T:
                wichSprite = 2;
                for (int i = 0; i < connectionPoints.Length; i++)
                {
                    if (i == 0) connectionPoints[i] = true;
                    if (i == 2) connectionPoints[i] = true;
                    if (i == 4) connectionPoints[i] = true;
                }                
                break;
            case WireShape.L:
                wichSprite = 3;
                for (int i = 0; i < connectionPoints.Length; i++)
                {
                    if (i == 0) connectionPoints[i] = true;
                    if (i == 2) connectionPoints[i] = true;
                }
                break;
            case WireShape.Diagonal:
                wichSprite = 4;
                for (int i = 0; i < connectionPoints.Length; i++)
                {
                    if (i == 1) connectionPoints[i] = true;
                    if (i == 5) connectionPoints[i] = true;
                }                
                break;
            case WireShape.LDiagonal:                
                wichSprite = 5;
                for (int i = 0; i < connectionPoints.Length; i++)
                {
                    if (i == 0) connectionPoints[i] = true;
                    if (i == 3) connectionPoints[i] = true;
                }
                break;
            default:
                break;                
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            if (i == wichSprite) spriteRender.sprite = sprites[i];
        }
    }

    public void SettingInputs()
    {
        poweredInputs = 0;
        for (int i = 0; i < connectionPoints.Length; i++)
        {
            if(connectionPoints[i] == true)
            {
                switch (i)
                {
                    case 0:
                        direction = transform.up;
                        break;
                    case 1:
                        direction = (transform.up + transform.right);
                        break;
                    case 2:
                        direction = transform.right;
                        break;
                    case 3:
                        direction = (-transform.up + transform.right);
                        break;
                    case 4:
                        direction = -transform.up;
                        break;
                    case 5:
                        direction = (-transform.up + -transform.right);
                        break;
                    case 6:
                        direction = -transform.right;
                        break;
                    default:
                        break;
                }
            }
            CastRays();
        }

        if (poweredInputs >= 1) hasEnergy = true;
        else hasEnergy = false;
    }

    public void CastRays()
    {
        castPoint = transform.position + direction * .3f;

        RaycastHit2D hit = Physics2D.Raycast(castPoint, direction, rayLength);
        if (hit.collider != null && hit.collider.GetComponent<Wire>())
        {
            if (hit.collider.GetComponent<Wire>().hasEnergy)
            {
                poweredInputs++;
                debugColor = Color.green;
            }
            else debugColor = Color.red;
        }
        else debugColor = Color.red;  
    }
}
