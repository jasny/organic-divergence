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
        _scoreSheet = GetComponent<ScoreSheet>();
    }

    private void OnEnable()
    {
        _judgeBeatCount = 0;
        _timerBeatCount = 0;
        
        var beatTimer = BeatTimer.Instance;
        _beatInterval = beatTimer.BeatInterval / (beatTimer.Syncopation ? 2 : 1);
    }

    public void OnBeat(int beatCount)
    {
        _timer = _beatInterval;
        _timerBeatCount = beatCount;
    }
    
    private void Update()
    {
        _timer -= Time.deltaTime;
        CheckKeyPress();

        if (_timer > _beatInterval / 2 || _judgeBeatCount == _timerBeatCount) return;
        
        NextBeat();
        _isKeyPressed = false;
    }

    private void NextBeat()
    {
        if (!_isKeyPressed) HandleSkip();
        _judgeBeatCount = _timerBeatCount;
    }

    private KeyCode KeyPressed()
    {
        if (keys.Length == 0) return KeyCode.None;
        
        var key = keys[(_judgeBeatCount + keys.Length - 1) % keys.Length];
        return Input.GetKeyDown(key) ? key : KeyCode.None;
    }
    
    private void CheckKeyPress()
    {
        if (_isKeyPressed) return;

        var keyPressed = KeyPressed();
        if (keyPressed == KeyCode.None) return;
        
        _isKeyPressed = true;
        
        Debug.Log(Math.Min(Math.Abs(_timer - _beatInterval), Math.Abs(_timer)));
        
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

        if (DisplayHit)
        {
            var key = keys[(_judgeBeatCount + keys.Length - 1) % keys.Length];
            DisplayHit.text = $"Missed {key.ToString()}";
        }
    }
}
