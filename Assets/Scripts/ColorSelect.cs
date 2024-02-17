using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class ColorSelect : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Light2D _light;

    private Color _defaultSpriteColor;
    private Color _defaultLightColor;
    
    [SerializeField] private List<KeyCodeColorPair> keyCodeColorPairs;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _light = GetComponent<Light2D>();

        if (_sprite) _defaultSpriteColor = _sprite.color;
        if (_light) _defaultLightColor = _light.color;
    }

    private void Update()
    {
        var pair = keyCodeColorPairs.Find(pair => Input.GetKey(pair.key));
        
        if (pair != null) {
            if (_sprite) _sprite.color = pair.color;
            if (_light) _light.color = pair.color;
        }
        else
        {
            if (_sprite) _sprite.color = _defaultSpriteColor;
            if (_light) _light.color = _defaultLightColor;
        }
    }
}
