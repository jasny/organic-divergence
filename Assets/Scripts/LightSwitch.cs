using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitch : MonoBehaviour
{
    public KeyCode key;

    private Light2D _light;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(key)) _light.enabled = !_light.enabled;
    }
}
