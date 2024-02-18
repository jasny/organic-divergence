using UnityEngine;

public class Drift : MonoBehaviour
{
    public float minSpeed = 10f;
    public float maxSpeed = 30f;
    public float chanceToChangeDirection = 0.005f;

    private float _speed;
    private int _direction = 1; // 1 for clockwise, -1 for counterclockwise

    private void Start()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
        _direction = Random.value > 0.5 ? 1 : -1;
    }

    private void FixedUpdate()
    {
        if (!(Random.value < chanceToChangeDirection)) return;
        
        _direction *= -1;
        _speed = Random.Range(minSpeed, maxSpeed);
    }
    
    private void Update()
    {
        transform.Rotate(0, 0, _speed * _direction * Time.fixedDeltaTime, Space.Self);
    }
}
