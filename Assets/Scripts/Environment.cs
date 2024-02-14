using UnityEngine;

public class Environment : MonoBehaviour
{
    public float inSync = 0;
    public float[] rings;
    public float boundary;
    
    public static Environment Instance { get; private set; }
    
    // Singleton
    private void Awake()
    {
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
        if (rings == null) return;
        
        var position = new Vector3(0, 0, 0);
        
        foreach (var radius in rings)
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