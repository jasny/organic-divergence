using UnityEngine;

public class ScaleToBeat : MonoBehaviour, IBeatTimeSubject
{
    private Vector3 _defaultScale;
    [SerializeField] private Vector3 beatScale = new Vector3(1.1f, 1.1f, 1.1f);
    
    [SerializeField] private float scaleDuration = 0.5f;
    private float scaleTime = 0;
    
    private void Start()
    {
        BeatTimer.Instance.Register(this);
        _defaultScale = transform.localScale;
    }

    private void OnDestroy()
    {
        BeatTimer.Instance.Unregister(this);
    }

    public void OnBeat(int beatCount)
    {
        transform.localScale = Vector3.Scale(_defaultScale, beatScale);
        scaleTime = 0;
    }

    private void Update()
    {
        if (transform.localScale == _defaultScale) return;
        
        scaleTime += Time.deltaTime;
        var fraction = scaleTime / scaleDuration;

        transform.localScale = Vector3.Lerp(transform.localScale, _defaultScale, fraction);

        if (!(fraction >= 1)) return;
        
        transform.localScale = _defaultScale;
        scaleTime = 0;
    }
}
