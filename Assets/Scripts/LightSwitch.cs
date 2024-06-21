using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LightSwitch : MonoBehaviour
{
    public KeyCode key;
    [SerializeField] private GenericSwitch switchButton;

    private Light2D _light;

    [SerializeField] private Slider powerSlider;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }

    private void Update()
    {
        if (key != KeyCode.None && Input.GetKeyDown(key)) _light.enabled = !_light.enabled;
        
        if (switchButton && _light.enabled != switchButton.IsOn)
        {
            var on = switchButton.IsOn;
            
            _light.enabled = on;
            if (powerSlider) powerSlider.value = on ? 0.01f : 1f;
        }
    }
}
