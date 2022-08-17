using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySource : MonoBehaviour
{
    public Wire wire;

    // Update is called once per frame
    void FixedUpdate()
    {
        wire.hasEnergy = true;
    }
}
