using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class KeyCodeColorPair
{
    public KeyCode key;
    public Color color;
}

public class ColorSwitch : MonoBehaviour
{
    private Light2D _light;
    
    [SerializeField] private List<KeyCodeColorPair> keyCodeColorPairs;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }

    private void Update()
    {
        Color tempColor = Color.black; // Start with black (no color)
        int colorCount = 0;

        foreach (var pair in keyCodeColorPairs)
        {
            if (Input.GetKey(pair.key)) // Use GetKey to check if the key is held down
            {
                tempColor += pair.color;
                colorCount++;
            }
        }

        if (colorCount == 0)
        {
            tempColor = Color.white;
        } else
        {
            //tempColor /= colorCount; // Average the color by the number of keys pressed
        }
        
        _light.color = tempColor;
    }
}
