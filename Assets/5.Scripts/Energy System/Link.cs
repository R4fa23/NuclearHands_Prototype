using System.Collections;
using UnityEngine;

public class Link : MonoBehaviour
{
    public bool hasEnergy;
    public bool input;
    [SerializeField] WireManager wireManager;
    [SerializeField] SpriteRenderer spriteRenderer;


    private void OnValidate()
    {
        SwitchSprite();
    }

    public void SwitchSprite()
    {
        if (input) spriteRenderer.color = Color.cyan;
        else spriteRenderer.color = Color.red;
    }

    IEnumerator HasEnergy()
    {
        hasEnergy = wireManager.hasEnergy;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(HasEnergy());
    }

    private void OnEnable()
    {
        //StartCoroutine(HasEnergy());
    }

    private void OnDisable()
    {
        //StopAllCoroutines();
    }
}
