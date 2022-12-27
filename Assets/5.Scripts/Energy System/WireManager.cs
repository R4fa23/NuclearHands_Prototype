using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WireManager : MonoBehaviour
{
    public enum WireShapes { Line, Cross, T, L, Diagonal, LDiagonal }
    public WireShapes myShape;

    [Header("Wire Energy")]
    [SerializeField] bool font;
    public bool hasEnergy;
    bool fontTurnOnOutputs;

    [Header("Update")]
    public bool updateShape;
    public bool updateLinksOutputs;

    [Header("Wire Lists")]
    public bool[] linksOutput;  //Links que são saidas de energia (É Chamado no Shape)
    public List<Link> enabledLinks; //Links que estão ligados (É Chamado no Shape)
    [SerializeField] List<Link> connectedLinks;

    [Header("Raycast")]
    [SerializeField] float offSet;
    [SerializeField] float rayLength;
    [SerializeField] bool debug;
    Vector3 castPoint;
    Vector3 direction;

    [Header("References")]
    [SerializeField] Shape shape;
    [SerializeField] SpriteRenderer spriteRenderer;
    public List<Link> allLinks;

    private void OnEnable()
    {
        shape.UpdateShape();
        for (int i = 0; i < linksOutput.Length; i++)
        {
            if (linksOutput[i] == true) enabledLinks[i].SetLinkOutput(true);
            else enabledLinks[i].SetLinkOutput(false);
        }

        StartCoroutine(TimerToCheck());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Awake()
    {
        if (font) SetUpFont();
    }

    private void OnValidate()
    {
        if (updateShape)
        {
            shape.UpdateShape();
            linksOutput = new bool[enabledLinks.Count];
            updateShape = false;
        }

        if (updateLinksOutputs)
        {
            for (int i = 0; i < linksOutput.Length; i++)
            {
                if (linksOutput[i] == true) enabledLinks[i].SetLinkOutput(true);
                else enabledLinks[i].SetLinkOutput(false);
            }
            updateLinksOutputs = false;
        }
    }

    private void Update()
    {
        if (updateShape)
        {
            shape.UpdateShape();
            updateShape = false;
        }

        if (updateLinksOutputs)
        {
            for (int i = 0; i < linksOutput.Length; i++)
            {
                if (linksOutput[i] == true) enabledLinks[i].SetLinkOutput(true);
                else enabledLinks[i].SetLinkOutput(false);
            }
            updateLinksOutputs = false;
        }
    }
    
    IEnumerator TimerToCheck()
    {
        SearchForOtherWires();
        //if (!font) CheckForEnergy();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(TimerToCheck());      
    }

    public void SearchForOtherWires()
    {
        //Passa por todos os links da lista, começando pelo que superior central, com o direction
        //apontado para cima. Após a iteração, vai para o próximo e gira a
        //direção do Raycast para o posicção correta do Link

        foreach (var link in allLinks)
        {
            if (link.gameObject.activeSelf && !link.output)
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
                AddNearbyOutputLinks(allLinks.IndexOf(link));
            }
        }
    }

    public void AddNearbyOutputLinks(int indexPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(castPoint, direction, rayLength, LayerMask.GetMask("Link"));
        Color color;
        if (hit.collider)
        {
            Link nearbyLink = hit.collider.GetComponent<Link>();

            if (!nearbyLink.output) connectedLinks[indexPosition] = nearbyLink;
            color = Color.yellow;
        }
        else
        { 
            color = Color.black;
            connectedLinks[indexPosition] = null;
        }

        if (debug) Debug.DrawRay(castPoint, direction * rayLength, color);
    }

    public void CheckForEnergy()
    {

        foreach (var item in enabledLinks)
        {
            if (hasEnergy && item.output) item.energized = true;
            else item.energized = false;
        }

        foreach (var link in allLinks)
        {
            if(link.gameObject.activeSelf && link.energized)
            {

            }
        }
        /*foreach (var link in connectedLinks)
        {
            if (link) if (link.energized) hasEnergy = true;
        }*/

        /*else
        {
            foreach (var link in enabledLinks)
            {
                if (link) link.energized = false;
            }
            hasEnergy = false;
        }
        */
        if (hasEnergy) spriteRenderer.color = Color.yellow;
        else spriteRenderer.color = Color.grey;
    }

    public void SetUpFont()
    {
        for (int i = 0; i < linksOutput.Length; i++)
        {
            linksOutput[i] = true;
        }

        for (int i = 0; i < linksOutput.Length; i++)
        {
            if (linksOutput[i] == true)
            {
                enabledLinks[i].SetLinkOutput(true);
                enabledLinks[i].energized = true;
            }
            else
            {
                enabledLinks[i].SetLinkOutput(false);
                enabledLinks[i].energized = false;
            }
        }

        hasEnergy = true;
        spriteRenderer.color = Color.yellow;
    }
}
