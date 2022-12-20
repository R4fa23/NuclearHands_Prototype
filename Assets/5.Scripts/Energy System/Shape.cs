using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] WireManager wireManager;

    [Header("Ediçao")]    
    public bool oneInput = true;
    public bool[] booleanos = new bool[4];

    [Header("Leitura")]
    [SerializeField] Sprite[] sprites;

    int wichSprite;
    SpriteRenderer spriteRender;

    private void OnValidate()
    {        
        //if(oneInput) ChooseInput();
        //UpdateInputsSpritesAndBools();
    }

    private void OnEnable()
    {
        UpdateShape();
        //UpdateInputsSpritesAndBools();        
    }

    public void UpdateShape()
    {
        //inputsPositions = new bool[7];

        wireManager.enabledInputs.Clear();
        spriteRender = GetComponent<SpriteRenderer>();        
       
        for (int i = 0; i < wireManager.allInputs.Count; i++)
        {
            wireManager.allInputs[i].gameObject.SetActive(false);
        }

        InputsWire[] allInputs = wireManager.allInputs.ToArray();

        switch (wireManager.myShape)
        {
            case WireManager.WireShapes.Line:
                wichSprite = 0;
                for (int i = 0; i < allInputs.Length; i++)
                {
                    if (i == 0)                         
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                    if (i == 4)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                }
                break;
            case WireManager.WireShapes.Cross:
                wichSprite = 1;
                for (int i = 0; i < allInputs.Length; i++)
                {
                    if (i == 0)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                    if (i == 2)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                    if (i == 4)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                    if (i == 6)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
    
                    }
                }
                break;
            case WireManager.WireShapes.T:
                wichSprite = 2;
                for (int i = 0; i < allInputs.Length; i++)
                {
                    if (i == 0)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                    if (i == 2)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                    if (i == 4)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                }
                break;
            case WireManager.WireShapes.L:
                wichSprite = 3;
                for (int i = 0; i < allInputs.Length; i++)
                {
                    if (i == 0)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                    if (i == 2)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                }
                break;
            case WireManager.WireShapes.Diagonal:
                wichSprite = 4;
                for (int i = 0; i < allInputs.Length; i++)
                {
                    if (i == 1)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                    if (i == 5)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                }
                break;
            case WireManager.WireShapes.LDiagonal:
                wichSprite = 5;
                for (int i = 0; i < allInputs.Length; i++)
                {
                    if (i == 0)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
                    if (i == 3)
                    {
                        allInputs[i].gameObject.SetActive(true);
                        wireManager.enabledInputs.Add(allInputs[i]);
                    }
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

    /*public void SetInputSprite()
    {
        for (int i = 0; i < inputsPositions.Length; i++)
        {
            if (inputsPositions[i] == true) wireInputsPadrao[i].gameObject.SetActive(true);
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
            item.SwitchSprite();
        }
    }*/
}
