using System.Collections;
using UnityEngine;

public class Link : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    public bool output;
    public bool energized;

    public void SetLinkOutput(bool output)
    {
        this.output = output;
        if (output) spriteRenderer.color = Color.cyan;
        else spriteRenderer.color = Color.red;
    }
}
