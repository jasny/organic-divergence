using UnityEngine;

public class Wander : MonoBehaviour
{
    public float wanderlust = 0;
    public bool rotate;
    
    public float minSpeed = 0.5f;
    public float maxSpeed = 1f;
    
    private Vector3 _originalPosition;
    private Vector3 _destination;
    private Quaternion _rotation;
    private float _speed;

    public bool free;

    private void Start()
    {
        _originalPosition = transform.position;
        _destination = _originalPosition;
    }

    private void NewDestination()
    {
        var distance = Random.Range(Mathf.Max(wanderlust / 10, 0.5f), wanderlust);
        var directionDegrees = Random.value * 360;
        var direction = (Vector3)VectorUtils.Vector2FromDegrees(distance, directionDegrees);
        var newPosition = VectorUtils.CenterBetween(transform.position, _originalPosition) + direction;

        _speed = Random.Range(minSpeed, maxSpeed);

        if (rotate)
        {
            var (yRotation, zRotation) = VectorUtils.AngleToFlipAndRotation(directionDegrees);
            transform.rotation = Quaternion.Euler(0, yRotation, transform.rotation.eulerAngles.z);
            _rotation = Quaternion.Euler(0, yRotation, zRotation);
        }

        if (free || !Environment.Instance || Vector3.Distance(transform.position, Vector3.zero) > Environment.Instance.boundary)
        {
            _destination = newPosition;
            return;
        }

        var closestRingDistance = ClosestRing();
        var inSync = Environment.Instance.inSync;
        var directionToCenter = (transform.position - Vector3.zero).normalized;
        var attractedPosition = Vector3.zero + directionToCenter * closestRingDistance;

        _destination = Vector3.Lerp(newPosition, attractedPosition, inSync);
    }

    private float ClosestRing()
    {
        var distanceFromCenter = Vector3.Distance(transform.position, Vector3.zero);

        var closestRingRadius = float.MaxValue;
        var smallestDifference = float.MaxValue;

        foreach (var ring in Environment.Instance.RingRadii)
        {
            var difference = Mathf.Abs(ring - distanceFromCenter);
            if (!(difference < smallestDifference)) continue;
            
            closestRingRadius = ring;
            smallestDifference = difference;
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
            if (rotate) transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotation, 20 * Time.deltaTime);
        }
    }
}
