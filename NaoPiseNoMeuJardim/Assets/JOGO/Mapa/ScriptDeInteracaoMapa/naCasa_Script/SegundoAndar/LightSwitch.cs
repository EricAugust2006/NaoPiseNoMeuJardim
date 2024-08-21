using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitch : MonoBehaviour
{
    [Header("Light2D")]
    public Light2D lightSource;

    [Header("Booleana")]
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
