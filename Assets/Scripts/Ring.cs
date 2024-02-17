using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private CircleCollider2D outer;

    public float radius;
    public float thickness = 1;

    private float _innerRadius;

    private void Awake()
    {
        AdjustColliderSizes();
    }

    private void OnValidate()
    {
        AdjustColliderSizes();
    }

    private void AdjustColliderSizes()
    {
        if (radius == 0) return;

        _innerRadius = radius - 0.5f * thickness;
        if (outer) outer.radius = radius + 0.5f * thickness;
    }

    public List<T> GetComponentsInRing<T>() where T : Component
    {
        var componentsInRing = new List<T>();

        var hits = Physics2D.OverlapCircleAll(transform.position, outer.radius);
        
        foreach (var hit in hits)
        {
            if (!hit.gameObject.activeInHierarchy) continue;

            var distanceToCenter = Vector2.Distance(transform.position, hit.transform.position);
            if (!(distanceToCenter > _innerRadius) || !(distanceToCenter <= outer.radius)) continue;
            
            var component = hit.GetComponent<T>();
            if (component) componentsInRing.Add(component);
        }

        return componentsInRing;
    }
}
