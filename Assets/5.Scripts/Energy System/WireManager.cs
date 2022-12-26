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
    [SerializeField] List<Link> connectedLinks; //Mudar para wire managers

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
        //shape.UpdateShape();
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
        SearchForOtherWires(true);
        if (!font) CheckForEnergy();
        SearchForOtherWires(false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(TimerToCheck());      
    }

    public void SearchForOtherWires(bool addLinks)
    {
        //Passa por todos os links da lista, começando pelo que superior central, com o direction
        //apontado para cima. Após a iteração, vai para o próximo e gira a
        //direção do Raycast para o posicção correta do Link

        foreach (var link in allLinks)
        {
            if (link.gameObject.activeSelf && link.output)
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
                if (addLinks) AddNearbyOutputLinks();
                //else RemoveAwayOutputLinks();
                castPoint = transform.position + direction * offSet;
                if (debug) Debug.DrawRay(castPoint, direction * rayLength, Color.cyan);                
            }
        }
    }

    public void AddNearbyOutputLinks()
    {
        RaycastHit2D hit = Physics2D.Raycast(castPoint, direction, rayLength, LayerMask.GetMask("Link"));

        if (hit)
        {           
            Link nearbyLink = hit.collider.GetComponent<Link>();

            if (!nearbyLink.output)
            {
                if (debug) Debug.Log(hit);
                if (!connectedLinks.Contains(nearbyLink)) connectedLinks.Add(nearbyLink);
            }
        }                            
    }

    public void RemoveAwayOutputLinks()
    {
        RaycastHit2D hit = Physics2D.Raycast(castPoint, direction, rayLength, LayerMask.GetMask("Link"));
  
        for (int i = 0; i < connectedLinks.Count; i++)
        {
            if (connectedLinks[i] != hit || !hit) connectedLinks.Remove(connectedLinks[i]);
        }
        
    }

    public void ClearConnectedLinksList()
    {
        connectedLinks.Clear();
    }

    public void CheckForEnergy()
    {
        if (connectedLinks.Count > 0)
        {
            for (int i = 0; i < connectedLinks.Count; i++)
            {
                if (connectedLinks[i].energized)
                {
                    hasEnergy = true;
                    break;
                }
            }
        }
        else
        {
            foreach (var link in enabledLinks)
            {
                if (link) link.energized = false;
            }
            hasEnergy = false;
        }

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
