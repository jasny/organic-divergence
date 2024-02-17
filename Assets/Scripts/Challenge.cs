using UnityEngine;
using UnityEngine.UI;

public class Challenge : MonoBehaviour
{
    [SerializeField] private GameObject challengeElement;
    [SerializeField] private AudioController audioController;
    [SerializeField] private Text displayCurrentChallenge;

    public bool IsActive { get; private set; }
    public bool isComplete = false;

    public int startBeat;
    public int beats;
    public bool syncopation;

    private bool _wasActive = false;
    
    private void LateUpdate()
    {
        if (!audioController.IsPlaying) return;
        if (audioController.BeatDeltaTime < 0.2f) return; // Grace period
        
        var currentBeat = audioController.CurrentBeat;
        IsActive = currentBeat >= startBeat && currentBeat < startBeat + beats;

        if (!_wasActive && IsActive) Activate();
        if (_wasActive && !IsActive) Deactivate();
        
        if (IsActive && isComplete) audioController.DisableLoop();

        _wasActive = IsActive;
    }

    private void Activate()
    {
        if (challengeElement) challengeElement.SetActive(true);

        audioController.EnableLoop(startBeat, beats);
        
        BeatTimer.Instance.totalBeats = beats;
        BeatTimer.Instance.Syncopation = syncopation;
        
        if (displayCurrentChallenge) displayCurrentChallenge.text = name;
    }
    
    // Beware, the other challenge might have already been activated
    private void Deactivate()
    {
        if (challengeElement) challengeElement.SetActive(false);
        if (displayCurrentChallenge && displayCurrentChallenge.text == name) displayCurrentChallenge.text = "";;
    }
}
