using System;
using UnityEngine;

public class AudioController : MonoBehaviour, IBeatTimeSubject
{
    [SerializeField] private AudioSource source;

    private int _loopPhrase;
    public int LoopPhrase
    {
        set
        {
            _loopPhrase = value;

            if (value == 0)
            {
                _loopStartTime = 0;
                _loopEndTime = float.MaxValue;
            }
            else
            {
                var duration = BeatTimer.Instance.BeatInterval * 16;
                _loopStartTime = duration * (value - 1);
                _loopEndTime = _loopStartTime + duration;            
            }
        }

        get => _loopPhrase;
    }
    
    private float _loopStartTime;
    private float _loopEndTime;

    public int CurrentBeat => Mathf.FloorToInt(source.time / BeatTimer.Instance.BeatInterval) + 1;
    public int CurrentBar => Mathf.FloorToInt(source.time / (BeatTimer.Instance.BeatInterval * 4)) + 1;
    public int CurrentPhrase => Mathf.FloorToInt(source.time / (BeatTimer.Instance.BeatInterval * 16)) + 1;
    
    public void Start()
    {
        BeatTimer.Instance.Register(this);
    }

    public void OnBeat(int beatCount)
    {
        if (source.isPlaying) return;
        
        source.time = _loopStartTime + BeatTimer.Instance.BeatInterval * (beatCount - 1);
        source.Play();
    }

    private void Update()
    {
        if (_loopEndTime == 0) return;
        
        if (source.time >= _loopEndTime)
        {
            source.time = _loopStartTime + (source.time - _loopEndTime);
        }
    }
}
