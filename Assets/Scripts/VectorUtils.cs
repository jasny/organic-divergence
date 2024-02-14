using UnityEngine;

public static class VectorUtils
{
    public static Vector2 Vector2FromDegrees(float distance, float angleInDegrees)
    {
        var angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        var x = distance * Mathf.Cos(angleInRadians);
        var y = distance * Mathf.Sin(angleInRadians);

        return new Vector2(x, y);
    }

    public static Vector2 CenterBetween(Vector2 positionA, Vector2 positionB)
    {
        return (positionA + positionB) / 2;
    }

    public static Vector3 CenterBetween(Vector3 positionA, Vector3 positionB)
    {
        return (positionA + positionB) / 2;
    }

    public static float Distance2D(Vector3 positionA, Vector3 positionB)
    {
        return Vector2.Distance(new Vector2(positionA.x, positionA.z), new Vector2(positionB.x, positionB.z));
    }
}
