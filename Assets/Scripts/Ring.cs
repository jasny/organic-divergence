using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private CircleCollider2D inner;
    [SerializeField] private CircleCollider2D outer;

    public float radius;
    public float thickness = 1;

    private void OnValidate()
    {
        if (radius == 0) return;

        if (inner) inner.radius = radius - 0.5f * thickness;
        if (outer) outer.radius = radius + 0.5f * thickness;
    }
}
