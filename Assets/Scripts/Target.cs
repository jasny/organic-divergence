using System;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IBeatTimeSubject
{
    private float _radius;
    private List<Organism> _organisms;

    public int beat;
    public Color color;

    private bool _lightWasOn;
    
    [SerializeField] private GenericSwitch flashLight;
    
    private void Awake()
    {
        _organisms = new List<Organism>();

        var circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider) _radius = circleCollider.radius;
    }

    private void Start()
    {
        BeatTimer.Instance.Register(this);
    }

    private void OnEnable()
    {
        if (flashLight) _lightWasOn = flashLight.IsOn;
    }

    private void OnDisable()
    {
        ClearOrganisms();
    }

    private void SelectOrganisms()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, _radius);
        
        foreach (var hit in hits)
        {
            if (!hit.gameObject.activeInHierarchy) continue;

            var organism = hit.GetComponent<Organism>();
            if (organism) _organisms.Add(organism);
        }
    }

    private void OnMouseEnter()
    {
        if (!flashLight || !flashLight.IsOn) return;
        
        SelectOrganisms();
        
        foreach (var organism in _organisms)
        {
            organism.Switch(true);
            organism.Glow(color);
        }
    }

    private void OnMouseExit()
    {
        foreach (var organism in _organisms)
        {
            organism.Glow(Color.clear);
        }

        _organisms = new List<Organism>();
    }

    public void OnBeat(int beatCount)
    {
        if (flashLight && flashLight.IsOn && _lightWasOn) return;

        if (flashLight && flashLight.IsOn) {
            ClearOrganisms();
            _lightWasOn = true;
            return;
        }

        var on = beatCount % 4 == beat % 4;
        
        if (on) SelectOrganisms();
        
        foreach (var organism in _organisms)
        {
            organism.Switch(on);
            organism.Glow(on ? Color.white : Color.clear);
        }

        _lightWasOn = false;
    }

    private void ClearOrganisms()
    {
        foreach (var organism in _organisms)
        {
            organism.Switch(true);
            organism.Glow(Color.clear);
        }
    }
}
