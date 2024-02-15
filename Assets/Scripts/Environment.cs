using System.Linq;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public float inSync = 0;
    public Ring[] Rings { get; private set; }
    public float[] RingRadii { get; private set; }
    public float boundary;
    
    public static Environment Instance { get; private set; }
    
    private void Awake()
    {
        Rings = GetComponentsInChildren<Ring>();
        RingRadii = Rings.Select(ring => ring.radius).ToArray();
        
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    
    private void OnDrawGizmos()
    {
        var radii = GetComponents<Ring>().Select(ring => ring.radius).ToArray();
        
        var position = new Vector3(0, 0, 0);
        
        foreach (var radius in radii)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(position, radius);
        }

        if (boundary > 0)
        {
            var color = Color.red;
            color.a = 0.2f;
            Gizmos.color = color;
            Gizmos.DrawWireSphere(position, boundary);
        }
    }    
}
