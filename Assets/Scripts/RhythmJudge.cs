using System;
using UnityEngine;
using UnityEngine.UI;

public class RhythmJudge : MonoBehaviour, IBeatTimeSubject
{
    private ScoreSheet _scoreSheet;
    
    private float _beatInterval;
    
    private bool _isKeyPressed = false;
    private float _timer;
    private int _timerBeatCount;
    private int _judgeBeatCount;
    
    public float perfectTiming = 0.05f;
    public float goodTiming = 0.15f;

    public float perfectScore = 1f;
    public float goodScore = 0.7f;
    
    [SerializeField] private KeyCode[] keys;
    [SerializeField] private Text DisplayHit;
    
    private void Start()
    {
        BeatTimer.Instance.Register(this);
        _beatInterval = BeatTimer.Instance.BeatInterval;

        _scoreSheet = GetComponent<ScoreSheet>();
    }

    public void OnBeat(int beatCount)
    {
        _timer = _beatInterval;
        _timerBeatCount = beatCount;
    }
    
    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= _beatInterval / 2 && _judgeBeatCount != _timerBeatCount)
        {
            NextBeat();
            _isKeyPressed = false;
        }
        
        CheckKeyPress();
    }

    private void NextBeat()
    {
        _judgeBeatCount = _timerBeatCount;
        if (!_isKeyPressed) HandleSkip();
    }

    private KeyCode KeyPressed()
    {
        if (keys.Length == 0) return KeyCode.None;
        
        var key = keys[(_timerBeatCount + keys.Length - 1) % keys.Length];
        return Input.GetKeyDown(key) ? key : KeyCode.None;
    }
    
    private void CheckKeyPress()
    {
        if (_isKeyPressed) return;

        var keyPressed = KeyPressed();
        if (keyPressed == KeyCode.None) return;
        
        _isKeyPressed = true;
        
        if (Math.Abs(_timer - _beatInterval) <= perfectTiming || Math.Abs(_timer) <= perfectTiming)
        {
            HandlePerfectHit();
        }
        else if (Math.Abs(_timer - _beatInterval) <= goodTiming || Math.Abs(_timer) <= goodTiming)
        {
            HandleHit();
        }
        else
        {
            HandleMiss();
        }
    }
    
    private void HandlePerfectHit()
    {
        if (_scoreSheet) _scoreSheet.Push(perfectScore);
        if (DisplayHit) DisplayHit.text = "Perfect!";
    }

    private void HandleHit()
    {
        if (_scoreSheet) _scoreSheet.Push(goodScore);
        if (DisplayHit) DisplayHit.text = "Good";
    }

    private void HandleMiss()
    {
        if (_scoreSheet) _scoreSheet.Push(0f);
        if (DisplayHit) DisplayHit.text = "Miss";
    }
    
    private void HandleSkip()
    {
        if (_scoreSheet) _scoreSheet.Push(0f);
        if (DisplayHit) DisplayHit.text = "zzz";
    }
}
