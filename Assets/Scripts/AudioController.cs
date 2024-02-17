using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private int startBeatOffset; // For testing purposes

    public float bpm = 120f;
    public float BeatInterval { get; private set; }
    
    private float _loopStartTime;
    private float _loopEndTime = float.MaxValue;
    public bool IsLooping => _loopEndTime < source.clip.length;

    public bool IsPlaying => source.isPlaying;
    
    public float Time => source.time;
    public int Beat => Mathf.FloorToInt(source.time / BeatInterval) + 1;
    public float BeatDeltaTime => source.time % BeatInterval;

    [SerializeField] private Text DisplayCurrentBeat;

    private void Awake()
    {
        BeatInterval = 60f / bpm;
    }
    
    private void Start() {
        if (startBeatOffset > 0) source.time = startBeatOffset * BeatInterval;
        source.Play();
    }

    private void Update()
    {
        if (source.time >= _loopEndTime - UnityEngine.Time.deltaTime && (_loopStartTime + (source.time - _loopEndTime) > 0))
        {
            source.time = _loopStartTime + (source.time - _loopEndTime);
        }

        if (DisplayCurrentBeat) DisplayCurrentBeat.text = Beat.ToString();
    }

    public void EnableLoop(int startBeat, int beatCount)
    {
        _loopStartTime = BeatInterval * (startBeat - 1);
        _loopEndTime = beatCount > 0 ? _loopStartTime + BeatInterval * beatCount : float.MaxValue;
    }

    public void DisableLoop()
    {
        _loopStartTime = 0;
        _loopEndTime = float.MaxValue;
    }
}
