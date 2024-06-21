using UnityEngine;

public class EnvironmentSync : MonoBehaviour
{
    public float inSync;

    private void OnEnable()
    {
        if (!Environment.Instance) return;
        Environment.Instance.inSync = inSync;
    }
}
