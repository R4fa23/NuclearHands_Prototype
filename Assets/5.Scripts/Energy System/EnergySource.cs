using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySource : MonoBehaviour
{
    public WireManager wire;

    // Update is called once per frame
    void FixedUpdate()
    {
        wire.hasEnergy = true;
    }
}
