using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering.Universal;

public class ColorSwitch : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Light2D _light;
    
    [SerializeField] private List<KeyCodeColorPair> keyCodeColorPairs;
    [SerializeField] private float gracePeriod = 0.2f;
    private float _timeSinceChange;

    private bool _isInGracePeriod = false;
    private readonly List<Color> _colorsDuringGracePeriod = new();

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _light = GetComponent<Light2D>();
    }

    private void Update()
    {
        if (!_isInGracePeriod)
        {
            // Start the grace period when the first key is pressed
            foreach (var pair in keyCodeColorPairs)
            {
                if (!Input.GetKeyDown(pair.key)) continue;
                
                _isInGracePeriod = true;
                _timeSinceChange = 0;
                _colorsDuringGracePeriod.Clear();
                AddColor(pair.color);
            }
        }
        else
        {
            _timeSinceChange += Time.deltaTime;

            foreach (var pair in keyCodeColorPairs.Where(pair => Input.GetKey(pair.key)))
            {
                AddColor(pair.color);
            }

            if (!(_timeSinceChange >= gracePeriod)) return;
            
            ApplyCollectedColors();
            _isInGracePeriod = false;
        }
    }

    private void AddColor(Color color)
    {
        if (!_colorsDuringGracePeriod.Contains(color))
        {
            _colorsDuringGracePeriod.Add(color);
        }
    }

    private void ApplyCollectedColors()
    {
        if (_colorsDuringGracePeriod.Count == 0) return;

        var finalColor = new Color(0, 0, 0, 0);
        finalColor = _colorsDuringGracePeriod.Aggregate(finalColor, (current, color) => current + color);

        if (finalColor == Color.blue) finalColor = new Color(0, 0.25f, 1); // Workaround because you can't see blue

        if (_sprite) _sprite.color = finalColor;
        if (_light) _light.color = finalColor;
    }
}
