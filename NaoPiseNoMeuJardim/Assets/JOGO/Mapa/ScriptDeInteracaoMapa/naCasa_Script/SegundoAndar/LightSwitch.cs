using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitch : MonoBehaviour
{
    public Light2D lightSource;
    public bool isOn = true;

    private void Start()
    {
        if (lightSource)
        {
            lightSource.enabled = isOn;
        }
    }

    public void ToggleLight()
    {
        if (lightSource)
        {
            isOn = !isOn;
            lightSource.enabled = isOn;
        }
    }
}
