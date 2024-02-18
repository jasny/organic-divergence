using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D.Animation;

public class Organism : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private SpriteSelector _spriteSelector;
    private CircleCollider2D _collider;
    private Wander _wander;
    private Drift _drift;
    [SerializeField] private Light2D spriteLight;
    [SerializeField] private GameObject evolveEffect;

    [SerializeField] private Color onColor = Color.white;
    [SerializeField] private Color offColor = Color.black;

    [SerializeField] private SpriteLibraryAsset evolveSprites;

    private bool _on;
    
    public string SpriteLabel => _spriteSelector ? _spriteSelector.spriteResolvers[0].GetLabel() : "";

    public Color Color
    {
        get => _sprite.color;
        set => _sprite.color = value;
    }

    public bool Free
    {
        get => _wander.free;
        set
        {
            _wander.free = value;
            _wander.wanderlust = 20f;
            _wander.rotate = true;

            if (_collider) _collider.enabled = !value;
            if (_drift) _drift.enabled = !value;
        }
    }

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _spriteSelector = GetComponent<SpriteSelector>();
        _collider = GetComponent<CircleCollider2D>();
        _wander = GetComponent<Wander>();
        _drift = GetComponent<Drift>();
    }

    public void Switch(bool on)
    {
        _on = on;
        if (_sprite) _sprite.color = on ? onColor : offColor;
    }
    
    public void Glow(Color color, float intensity = 1f)
    {
        if (spriteLight)
        {
            spriteLight.color = color;
            spriteLight.intensity = intensity;
        }
        else if (_sprite)
        {
            var originalAlpha = _sprite.color.a;
            _sprite.color = color == Color.clear || intensity <= 0.01f
                ? (_on ? onColor : offColor)
                : new Color(color.r, color.g, color.b, originalAlpha);
        }
    }

    public bool Evolve()
    {
        if (!_spriteSelector || !evolveSprites) return false;

        if (evolveEffect) evolveEffect.SetActive(true);
        _spriteSelector.ChangeLibrary(evolveSprites);
        evolveSprites = null;
        
        return true;
    }
}
