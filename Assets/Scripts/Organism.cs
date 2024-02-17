using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Organism : MonoBehaviour
{
    private SpriteRenderer _sprite;
    [SerializeField] private Light2D spriteLight;

    [SerializeField] private Color onColor = Color.white;
    [SerializeField] private Color offColor = Color.black;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void Switch(bool on)
    {
        if (_sprite) _sprite.color = on ? onColor : offColor;
    }
    
    public void Glow(Color color, float intensity = 1f)
    {
        if (spriteLight)
        {
            spriteLight.color = color;
            spriteLight.intensity = intensity;
        }
        else if (_sprite && Mathf.Approximately(intensity, 1f))
        {
            var originalAlpha = _sprite.color.a;
            _sprite.color = color == Color.clear ? onColor : new Color(color.r, color.g, color.b, originalAlpha);
        }
    }
}
