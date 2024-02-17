using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public enum Direction
{
    Inwards,
    Outwards
}

public class RingPulse : MonoBehaviour, IBeatTimeSubject
{
    public Ring[] rings;
    public int offset = 0;
    public Direction direction = Direction.Outwards;

    public bool switchOnOff = true;

    public Color[] colorPattern;
    public bool colorTravel;
    
    public float chanceToGlow = 0f;
    [SerializeField] private Light2D relativeGlow;

    private Dictionary<Ring, List<Organism>> _allOrganisms;
    private Dictionary<Ring, List<Organism>> _selectedOrganisms;
    
    private void Start()
    {
        BeatTimer.Instance.Register(this);
    }

    private void OnEnable()
    {
        _allOrganisms = SelectOrganisms(1);
        if (chanceToGlow > 0) _selectedOrganisms = SelectOrganisms(chanceToGlow);
    }

    private void OnDisable()
    {
        foreach (var organism in _allOrganisms.SelectMany(pair => pair.Value))
        {
            organism.Switch(true);
            organism.Glow(Color.clear, 0);
        }
    }

    private Dictionary<Ring, List<Organism>> SelectOrganisms(float chance)
    {
        var selected = new Dictionary<Ring, List<Organism>>
        {
            { rings[0], new List<Organism>() },
            { rings[1], new List<Organism>() },
            { rings[2], new List<Organism>() },
            { rings[3], new List<Organism>() },
        };
        
        foreach (var ring in rings)
        {
            var organisms = ring.GetComponentsInRing<Organism>();
            selected[ring] = chance >= 0.99f ? organisms : organisms.Where(_ => Random.value <= chanceToGlow).ToList();
        }

        return selected;
    }

    public void OnBeat(int beatCount)
    {
        var current = (beatCount - 1 + offset) % 4;
        if (direction == Direction.Inwards) current = 3 - current;

        var colors = SelectColors(colorTravel ? 0 : (current + 1 % 4), beatCount);

        for (int i = 0; i < rings.Length; i++)
        {
            var ring = rings[i];
            
            SwitchOrganisms(ring, i == current);
            GlowOrganisms(ring, colors[i]);
        }
    }

    private Color[] SelectColors(int index, int beatCount)
    {
        var colors = new Color[4];
        if (chanceToGlow == 0 || colorPattern.Length < 4) return colors;

        colors[index] = GetPatternColor(beatCount - 1);
        colors[NextRingIndex(index, 3)] = GetPatternColor(beatCount);
        colors[NextRingIndex(index, 2)] = GetPatternColor(beatCount + 1);
        colors[NextRingIndex(index, 1)] = GetPatternColor(beatCount + 2);

        return colors;
    }

    private int NextRingIndex(int index, int steps)
    {
        return (index + (direction == Direction.Inwards ? 4 - steps : steps)) % 4;
    }

    private void SwitchOrganisms(Ring ring, bool on)
    {
        if (!switchOnOff) return;
        
        foreach (var organism in _allOrganisms[ring])
        {
            organism.Switch(on);
        }
    }

    private void GlowOrganisms(Ring ring, Color color)
    {
        if (chanceToGlow <= 0) return;

        var intensity = relativeGlow ? Mathf.Clamp(1 - relativeGlow.intensity, 0, 1) : 1;
        
        foreach (var organism in _selectedOrganisms[ring])
        {
            organism.Glow(color, intensity);
        }
    }

    private Color GetPatternColor(int pos)
    {
        while (pos < 0) pos += colorPattern.Length;
        while (pos >= colorPattern.Length) pos -= colorPattern.Length;
        
        var color = colorPattern[pos];

        if (color == Color.blue) color = new Color(0, 0.25f, 1); // Workaround because you can't see blue
        
        return color;
    }
}
