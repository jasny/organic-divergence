using System;
using UnityEngine;
using UnityEngine.UI;

public class RhythmJudge : MonoBehaviour, IBeatTimeSubject
{
    private ScoreSheet _scoreSheet;

    [SerializeField] private float _beatInterval;
    [SerializeField] private int _totalBeats;
    
    private bool _isKeyPressed = false;
    private float _timer;
    [SerializeField] private int _timerBeatCount;
    [SerializeField] private int _judgeBeatCount;

    public float perfectTiming = 0.05f;
    public float goodTiming = 0.15f;

    public float perfectScore = 1f;
    public float goodScore = 0.7f;
    
    [SerializeField] private KeyCode[] keys;
    [SerializeField] private Text DisplayHit;
    [SerializeField] private string beatPattern;
    
    private void Start()
    {
        BeatTimer.Instance.Register(this);
        _scoreSheet = GetComponent<ScoreSheet>();
    }

    private void OnEnable()
    {
        _judgeBeatCount = 0;
        _timerBeatCount = 0;
    }

    private void OnDisable()
    {
        if (DisplayHit) DisplayHit.text = "";
    }

    public void OnBeat(int beatCount)
    {
        _timer = _beatInterval;
        _timerBeatCount = beatCount;

        if (_judgeBeatCount == 0) InitJudge();
    }

    private void InitJudge()
    {
        _judgeBeatCount = _timerBeatCount;
            
        var beatTimer = BeatTimer.Instance;
        _beatInterval = beatTimer.BeatInterval / (beatTimer.Syncopation ? 2 : 1);
        _totalBeats = beatTimer.totalBeats;
    }
    
    private void Update()
    {
        if (_judgeBeatCount == 0) return; // Before first beat

        CheckKeyPress();
        
        _timer -= Time.deltaTime;
        
        if (_judgeBeatCount % _totalBeats == _timerBeatCount % _totalBeats && _timer < _beatInterval / 2)
        {
            NextBeat();
        }
    }

    private void NextBeat()
    {
        if (!_isKeyPressed && ShouldHit()) HandleSkip();
        _isKeyPressed = false;

        _judgeBeatCount = (_timerBeatCount % _totalBeats) + 1;

        if (DisplayHit) DisplayHit.text = "";
    }

    private KeyCode KeyPressed()
    {
        if (keys.Length == 0) return KeyCode.None;
        
        var key = keys[(_judgeBeatCount - 1) % keys.Length];
        return Input.GetKeyDown(key) ? key : KeyCode.None;
    }
    
    private void CheckKeyPress()
    {
        if (_isKeyPressed) return;

        var keyPressed = KeyPressed();
        if (keyPressed == KeyCode.None) return;
        
        _isKeyPressed = true;

        if (!ShouldHit())
        {
            HandleMiss();
            return;
        }

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

    private bool ShouldHit()
    {
        if (beatPattern == "") return true;

        return beatPattern[_judgeBeatCount] == '+';
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

        /*if (DisplayHit)
        {
            var key = keys[(_judgeBeatCount + keys.Length - 1) % keys.Length];
            DisplayHit.text = $"Missed {key.ToString()}";
        }*/
    }
}
