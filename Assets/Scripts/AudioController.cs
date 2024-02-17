using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioController : MonoBehaviour, IBeatTimeSubject
{
    [SerializeField] private AudioSource source;
    [SerializeField] private int startBeatOffset = 0; // For testing purposes
    
    private float _loopStartTime;
    private float _loopEndTime = float.MaxValue;

    public bool IsPlaying => source.isPlaying;
    
    public int CurrentBeat => Mathf.FloorToInt(source.time / BeatTimer.Instance.BeatInterval) + 1;
    public float BeatDeltaTime => source.time % BeatTimer.Instance.BeatInterval;

    [SerializeField] private Text DisplayCurrentBeat;
    
    public void Start()
    {
        BeatTimer.Instance.Register(this);
    }

    public void OnBeat(int beatCount)
    {
        if (source.isPlaying) return;

        var startBeat = (BeatTimer.Instance.Syncopation ? Mathf.FloorToInt(((float)beatCount - 1) / 2) : beatCount - 1) + startBeatOffset;
        source.time = _loopStartTime + BeatTimer.Instance.BeatInterval * startBeat;
        source.Play();
    }

    private void Update()
    {
        if (source.time >= _loopEndTime)
        {
            source.time = _loopStartTime + (source.time - _loopEndTime);
        }

        if (DisplayCurrentBeat) DisplayCurrentBeat.text = CurrentBeat.ToString();
    }

    public void EnableLoop(int startBeat, int beatCount)
    {
        _loopStartTime = BeatTimer.Instance.BeatInterval * (startBeat - 1);
        _loopEndTime = beatCount > 0 ? _loopStartTime + BeatTimer.Instance.BeatInterval * beatCount : float.MaxValue;
    }

    public void DisableLoop()
    {
        _loopStartTime = 0;
        _loopEndTime = float.MaxValue;
    }
}
