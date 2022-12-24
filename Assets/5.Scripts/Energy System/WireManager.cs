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
    public List<bool> linksOutput;  //Links que são saidas de energia (É Chamado no Shape)
    public List<Link> enabledLinks; //Links que estão ligados (É Chamado no Shape)
    [SerializeField] List<Link> connectedLinks; //Mudar para wire managers

    [Header("Raycast")]
    [SerializeField] float offSet;
    [SerializeField] float rayLength;
    [SerializeField] bool debug;
    Vector3 castPoint;
    Vector3 direction;

    [Header("References")]
    [SerializeField] Shape shape;
    public List<Link> allLinks;
    SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (updateShape)
        {
            shape.UpdateShape();

            updateShape = false;
        }
    }

    private void OnValidate()
    {
        if (updateShape)
        {
            shape.UpdateShape();
            linksOutput = new List<bool>();          
            updateShape = false;
        }
    }

    private void OnEnable()
    {
        shape.UpdateShape();
        StartCoroutine(CheckForOtherWires());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }   

    IEnumerator CheckForOtherWires()
    {
        CheckSurroudings();
        CheckForEnergy();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CheckForOtherWires());      
    }

    public void CheckForEnergy()
    {
        if (!first)
        {
            if (connectedLinks.Count > 0) hasEnergy = true;
            else hasEnergy = false;
        }

        if (hasEnergy) spriteRenderer.color = Color.yellow;
        else spriteRenderer.color = Color.grey;
    }

    public void CheckSurroudings()
    {
        //Passa por todos os links da lista, começando pelo que superior central, com o direction
        //apontado para cima. Após a iteração, vai para o próximo e gira a
        //direção do Raycast para o posicção correta do Link

        foreach (var link in allLinks)
        {
            if (link.gameObject.activeSelf)
            {               
                switch (allLinks.IndexOf(link))
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
                CastRays();

                if (debug) Debug.DrawRay(castPoint, direction * rayLength, Color.cyan, 1f);                
            }
        }
    }

    public void CastRays()
    {
        RaycastHit2D hit = Physics2D.Raycast(castPoint, direction, rayLength);

        if (hit.collider != null && hit.collider.GetComponent<Link>())
        {
            if (hit.collider.GetComponent<Link>().hasEnergy)
            {                
                if (!connectedLinks.Contains(hit.collider.GetComponent<Link>())) connectedLinks.Add(hit.collider.GetComponent<Link>());
            }
            else connectedLinks.Remove(hit.collider.GetComponent<Link>());
        }
        else connectedLinks.Clear();
    }
}
