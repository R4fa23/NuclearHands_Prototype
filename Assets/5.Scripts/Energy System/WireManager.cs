using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireManager : MonoBehaviour
{
    [Header("Wire Shape")]
    public bool updateShape;
    public enum WireShapes { Line, Cross, T, L, Diagonal, LDiagonal }
    public WireShapes myShape;

    [Header("Wire propriedades")]
    public bool hasEnergy;
    [SerializeField] bool first;
    public List<InputsWire> enabledInputs;
    [SerializeField] List<InputsWire> connectedWires; //Mudar para wire managers
    InputsWire[] inputsPositions;

    [Header("Raycast")]
    [SerializeField] float offSet;
    [SerializeField] float rayLength = 0.8f;
    [SerializeField] bool debug;

    [Header("References")]
    [SerializeField] Shape shape;
    SpriteRenderer spriteRenderer;
    public List<InputsWire> allInputs;

    [Header("Local Variables")]
    Vector3 castPoint;
    Vector3 direction;
    Color color;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnValidate()
    {
        if (updateShape)
        {
            shape.UpdateShape();
            updateShape = false;
        }
    }

    private void OnEnable()
    {
        shape.UpdateShape();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }   

    IEnumerator CheckForOtherWires()
    {
        CheckSurroudings();
        CheckForEnergy();
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CheckForOtherWires());      
    }

    public void CheckForEnergy()
    {
        if (!first)
        {
            if (connectedWires.Count > 0) hasEnergy = true;
            else hasEnergy = false;
        }

        if (hasEnergy) spriteRenderer.color = Color.yellow;
        else spriteRenderer.color = Color.grey;
    }

    public void CheckSurroudings()
    {
        inputsPositions = allInputs.ToArray();

        for (int i = 0; i < inputsPositions.Length; i++)
        {
            if (inputsPositions[i].gameObject.activeInHierarchy)
            {
                //adicionar direçao em 45 a cada iteraçao
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

                castPoint = transform.position + direction * offSet;

                if (enabledInputs[i] == true)
                {
                    CastRays();
                    color = Color.cyan;
                }
                else
                {
                    color = Color.red;
                }               

                if(debug)Debug.DrawRay(castPoint, direction * rayLength, color);
            }
        }
    }

    public void CastRays()
    {
        RaycastHit2D hit = Physics2D.Raycast(castPoint, direction, rayLength);

        if (hit.collider != null && hit.collider.GetComponent<InputsWire>())
        {
            if (hit.collider.GetComponent<InputsWire>().hasEnergy)
            {                
                if (!connectedWires.Contains(hit.collider.GetComponent<InputsWire>())) connectedWires.Add(hit.collider.GetComponent<InputsWire>());
            }
            else connectedWires.Remove(hit.collider.GetComponent<InputsWire>());
        }
        else connectedWires.Clear();
    }
}
