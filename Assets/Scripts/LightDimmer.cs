using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightDimmer : MonoBehaviour
{
    private Light2D _light;

    public KeyCode down;
    public KeyCode up;

    public float min = 0;
    public float max = 1;
    
    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }

    private void Update()
    {
        if (Input.GetKey(down))
        {
            _light.intensity = Mathf.Clamp(_light.intensity - Time.deltaTime, min, max);
        }
        
        if (Input.GetKey(up))
        {
            _light.intensity = Mathf.Clamp(_light.intensity + Time.deltaTime, min, max);
        }
    }
}
