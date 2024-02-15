using System;
using UnityEngine;

public class Challenge : MonoBehaviour, IBeatTimeSubject
{
    [SerializeField] private Behaviour challengeComponent;
    [SerializeField] private AudioController audioController;

    public bool IsActive { get; private set; }
    public bool isComplete = false;

    public int audioPhrase;

    private void Start()
    {
        BeatTimer.Instance.Register(this);
    }

    public void OnBeat(int beatCount)
    {
        IsActive = audioController.CurrentPhrase == audioPhrase;
        challengeComponent.enabled = IsActive;
        
        if (!IsActive) return;
        
        audioController.LoopPhrase = isComplete ? 0 : audioPhrase;
    }
}
