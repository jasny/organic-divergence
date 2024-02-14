using UnityEngine;

public class Wander : MonoBehaviour
{
    public float wanderlust = 0;
    
    public float minSpeed = 0.5f;
    public float maxSpeed = 1f;
    
    private Vector3 _originalPosition;
    private Vector3 _destination;
    private float _speed;

    private void Start()
    {
        _originalPosition = transform.position;
        _destination = _originalPosition;
    }

    private void NewDestination()
    {
        var distance = Random.Range(0.5f, wanderlust);
        var directionDegrees = Random.value * 360;
        var direction = VectorUtils.Vector2FromDegrees(distance, directionDegrees);
        var newPosition = VectorUtils.CenterBetween(transform.position, _originalPosition) + (Vector3)direction;

        _speed = Random.Range(minSpeed, maxSpeed);
        
        // Check if within boundary
        if (!Environment.Instance || Vector3.Distance(transform.position, Vector3.zero) > Environment.Instance.boundary)
        {
            _destination = newPosition;
            return;
        }

        float closestRingDistance = ClosestRing();

        // Calculate the influence of the closest ring based on inSync
        float inSync = Environment.Instance.inSync;
        
        // Calculate the nearest position on the closest ring
        Vector3 directionToCenter = (transform.position - Vector3.zero).normalized;

        // Assuming 'closestRingDistance' is the radius of the closest ring
        Vector3 attractedPosition = Vector3.zero + directionToCenter * closestRingDistance;

        // Pick a position between `newPosition` and `attractedPosition` based on `inSync`
        _destination = Vector3.Lerp(newPosition, attractedPosition, inSync);

        
        // Pick a position between `newPosition` and `attractedPosition` based on `inSync`.
        // The lower `inSync`, the closer it should be to `newPosition`
        // The higher `inSync`, the closer it should be to `attractedPosition`
        _destination = Vector3.Lerp(newPosition, attractedPosition, inSync);
    }

    private float ClosestRing()
    {
        float distanceFromCenter = Vector3.Distance(transform.position, Vector3.zero);

        float closestRingRadius = float.MaxValue;
        float smallestDifference = float.MaxValue;

        foreach (var ring in Environment.Instance.rings)
        {
            float difference = Mathf.Abs(ring - distanceFromCenter);
            if (difference < smallestDifference)
            {
                closestRingRadius = ring;
                smallestDifference = difference;
            }
        }

        return closestRingRadius;
    }

    private void Update()
    {
        if (wanderlust == 0) return;

        if (Vector3.Distance(transform.position, _destination) < 0.01f)
        {
            NewDestination();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
        }
    }
}
