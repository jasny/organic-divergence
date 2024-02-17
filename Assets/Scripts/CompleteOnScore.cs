using UnityEngine;

public class CompleteOnScore : MonoBehaviour, IBeatTimeSubject
{
    private Challenge _challenge;
    [SerializeField] private ScoreSheet scoreSheet;
    [SerializeField] private float requiredScore;
    [SerializeField] private int beatDuration;

    public bool setInSync;

    private int _positiveBeatCount = 0;

    private void Start()
    {
        _challenge = GetComponent<Challenge>();
        
        BeatTimer.Instance.Register(this);
    }

    public void OnBeat(int beatCount)
    {
        if (!_challenge.IsActive) return;
        
        if (scoreSheet.WeightedAvg >= requiredScore)
        {
            _positiveBeatCount++;
        }
        else
        {
            _positiveBeatCount = 0;
        }

        if (_positiveBeatCount > beatDuration)
        {
            _challenge.isComplete = true;
        }

        if (setInSync) Environment.Instance.inSync = _challenge.isComplete ? 0.9f : Mathf.Clamp(scoreSheet.WeightedAvg, 0, 0.9f);
    }
}
