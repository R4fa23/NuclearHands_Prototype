using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WireManager wireManager;
    [SerializeField] SpriteRenderer spriteRender;
    [SerializeField] Sprite[] sprites;

    public void UpdateShape()
    {
        int wichSprite = 0;
        wireManager.enabledLinks.Clear();      
       
        for (int i = 0; i < wireManager.allLinks.Count; i++)
        {
            wireManager.allLinks[i].gameObject.SetActive(false);
        }

        switch (wireManager.myShape)
        {
            case WireManager.WireShapes.Line:

                wichSprite = 0;

                wireManager.allLinks[0].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[0]);

                wireManager.allLinks[4].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[4]);

                break;
            case WireManager.WireShapes.Cross:

                wichSprite = 1;

                wireManager.allLinks[0].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[0]);

                wireManager.allLinks[2].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[2]);

                wireManager.allLinks[4].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[4]);
                
                wireManager.allLinks[6].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[6]);

                break;
            case WireManager.WireShapes.T:

                wichSprite = 2;

                wireManager.allLinks[0].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[0]);

                wireManager.allLinks[2].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[2]);

                wireManager.allLinks[4].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[4]);

                break;
            case WireManager.WireShapes.L:
 
                wichSprite = 3;

                wireManager.allLinks[0].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[0]);

                wireManager.allLinks[2].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[2]);

                break;
            case WireManager.WireShapes.Diagonal:

                wichSprite = 4;

                wireManager.allLinks[1].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[1]);

                wireManager.allLinks[5].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[5]);

                break;
            case WireManager.WireShapes.LDiagonal:

                wichSprite = 5;

                wireManager.allLinks[0].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[0]);

                wireManager.allLinks[3].gameObject.SetActive(true);
                wireManager.enabledLinks.Add(wireManager.allLinks[3]);

                break;
            default:
                break;
        }
        
        spriteRender.sprite = sprites[wichSprite];
    }

    /*public void SetInputSprite()
    {
        for (int i = 0; i < linksPosition.Length; i++)
        {
            if (linksPosition[i] == true) wireInputsPadrao[i].gameObject.SetActive(true);
            else wireInputsPadrao[i].gameObject.SetActive(false);
        }
        UpdateInputsSpritesAndBools();
    }*/

    /*public void UpdatadeEnergyInputsList()
    {
        for (int i = 0; i < wireInputsPadrao.ToArray().Length; i++)
        {
            if (wireInputsPadrao[i].input) plugsLigados[i] = true;
            else plugsLigados[i] = false;
        }
    }

    public void ChooseInput()
    {
        for (int i = 0; i < booleanos.Length; i++)
        {
            if(booleanos[i] == true) wireInputsLigados[i].input = true;
            if(booleanos[i] == false) wireInputsLigados[i].input = false;            
        }

        foreach (var item in wireInputsLigados)
        {
            item.SetLinkInput();
        }
    }*/
}
