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

    private void Update()
    {
        if (!audioController.IsPlaying || !IsActive) return;
        
        if (isComplete && audioController.IsLooping) audioController.DisableLoop();
        if (ShouldDeactivate()) Deactivate();
    }
    
    private void LateUpdate()
    {
        if (!audioController.IsPlaying || IsActive) return;
        if (ShouldActivate()) Activate();
    }
    
    private bool ShouldActivate()
    {
        if (Environment.Instance.activeChallenge) return false;
        
        var currentBeat = audioController.Beat;
        return currentBeat >= startBeat && currentBeat < startBeat + beats;
    }

    private bool ShouldDeactivate()
    {
        if (!isComplete) return false;
        
        var currentBeat = audioController.Beat;
        return currentBeat < startBeat || currentBeat > startBeat + beats;
    }
    

    private void Activate()
    {
        IsActive = true;
        Environment.Instance.activeChallenge = this;
        
        if (challengeElement) challengeElement.SetActive(true);
        if (!isComplete) audioController.EnableLoop(startBeat, beats);
        
        BeatTimer.Instance.totalBeats = beats;
        BeatTimer.Instance.Syncopation = syncopation;
        
        if (displayCurrentChallenge) displayCurrentChallenge.text = name;
    }
    
    private void Deactivate()
    {
        IsActive = false;
        Environment.Instance.activeChallenge = null;

        if (challengeElement) challengeElement.SetActive(false);
        if (displayCurrentChallenge && displayCurrentChallenge.text == name) displayCurrentChallenge.text = "";
    }
}
