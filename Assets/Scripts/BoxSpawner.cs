using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int amount;
    public float width = 1;
    public float height = 1;

    public float randomArrangement = 0;
    public bool randomRotation = false;

    public float clearRadius = 0;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 1));

        if (clearRadius > 0) Gizmos.DrawWireSphere(transform.position, clearRadius);
    }

    private void Start()
    {
        // Calculate rows and columns
        var rows = Mathf.CeilToInt(Mathf.Sqrt(amount));
        var cols = Mathf.CeilToInt((float)amount / rows);

        // Calculate spacing
        var xSpacing = width / cols;
        var ySpacing = height / rows;

        var center = transform.position;
        
        // Instantiate objects
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                // Calculate position
                var x = center.x + (j * xSpacing + xSpacing / 2) - width / 2 + Random.Range(-randomArrangement, randomArrangement) * xSpacing;
                var y = center.y + (i * ySpacing + ySpacing / 2) - height / 2 + Random.Range(-randomArrangement, randomArrangement) * ySpacing;

                var position = new Vector3(x, y, center.z);

                if ((position - center).magnitude < clearRadius) continue;
                
                var rotation = randomRotation ? Quaternion.Euler(0, 0, Random.Range(0f, 360f)) : Quaternion.identity;
                Instantiate(prefab, position, rotation);
            }
        }
    }
}
