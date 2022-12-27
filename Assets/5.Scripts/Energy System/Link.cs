using System.Collections;
using UnityEngine;

public class Link : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    public WireManager wireManager;
    public bool input;
    //public bool energized;

    public void SetLinkInput(bool input)
    {
        this.input = input;
        if (input) spriteRenderer.color = Color.green;
        else spriteRenderer.color = Color.red;
    }
}
