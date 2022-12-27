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

    [Header("Update")]
    public bool updateShape;
    public bool updateLinksInputs;

    [Header("Wire Lists")]
    public bool[] linksInputs;  //Links que são saidas de energia (É Chamado no Shape)
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
        for (int i = 0; i < linksInputs.Length; i++)
        {
            if (linksInputs[i] == true) enabledLinks[i].SetLinkInput(true);
            else enabledLinks[i].SetLinkInput(false);
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
            linksInputs = new bool[enabledLinks.Count];
            updateShape = false;
        }

        if (updateLinksInputs)
        {
            for (int i = 0; i < linksInputs.Length; i++)
            {
                if (linksInputs[i] == true) enabledLinks[i].SetLinkInput(true);
                else enabledLinks[i].SetLinkInput(false);
            }
            updateLinksInputs = false;
        }
    }

    private void Update()
    {
        if (updateShape)
        {
            shape.UpdateShape();
            updateShape = false;
        }

        if (updateLinksInputs)
        {
            for (int i = 0; i < linksInputs.Length; i++)
            {
                if (linksInputs[i] == true) enabledLinks[i].SetLinkInput(true);
                else enabledLinks[i].SetLinkInput(false);
            }
            updateLinksInputs = false;
        }
    }
    
    IEnumerator TimerToCheck()
    {
        SearchForOtherWires();
        if (!font) CheckForEnergy();
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
            if (link.gameObject.activeSelf && link.input)
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

            if (!nearbyLink.input)
            {
                connectedLinks[indexPosition] = nearbyLink;
                color = Color.green;
            }
            else color = Color.black;
        }
        else
        { 
            color = Color.black;
            connectedLinks[indexPosition] = null;
        }

        if (debug) Debug.DrawRay(castPoint, direction * rayLength, color, 0.5f);
    }

    public void CheckForEnergy()
    {
        for (int i = 0; i < connectedLinks.Count; i++)
        {
            if (connectedLinks[i] != null)
            {
                if (connectedLinks[i].wireManager.hasEnergy)
                {
                    Debug.Log(i);
                    hasEnergy = true;
                    break;
                }
                else hasEnergy = false;
            }
            else hasEnergy = false;
        }
        
        if (hasEnergy) spriteRenderer.color = Color.yellow;
        else spriteRenderer.color = Color.grey;
    }

    public void SetUpFont()
    {
        for (int i = 0; i < linksInputs.Length; i++)
        {
            linksInputs[i] = false;
        }

        for (int i = 0; i < linksInputs.Length; i++)
        {
            enabledLinks[i].SetLinkInput(false);
        }

        hasEnergy = true;
        spriteRenderer.color = Color.yellow;
    }
}
