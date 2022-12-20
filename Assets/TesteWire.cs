using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteWire : MonoBehaviour
{
    [SerializeField]Shape wireShape;

    [SerializeField] List<WireManager> wiresList;
    public bool[] connectionPoints;
    Vector3 castPoint;

    Vector3 direction;
    float rayLength = 0.8f;
    Color color;    

    void Start()
    {
        
    }

    
}

