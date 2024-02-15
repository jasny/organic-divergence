using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmJudge : MonoBehaviour, IBeatTimeSubject
{
    public ScoreSheet scoreSheet;
    
    public float InSync => Mathf.Clamp(scoreSheet.WeightedAvg, 0, 1f);

    private float _beatInterval;
    
    private bool _isKeyPressed = false;
    private float _timer;
    private int _timerBeatCount;
    private int _judgeBeatCount;
    
    public float perfectTiming = 0.05f;
    public float goodTiming = 0.15f;
    
    private void Start()
    {
        BeatTimer.Instance.Register(this);
        _beatInterval = BeatTimer.Instance.BeatInterval;
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

        Environment.Instance.inSync = InSync;
    }

    private static KeyCode KeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.A)) return KeyCode.A;
        if (Input.GetKeyDown(KeyCode.S)) return KeyCode.S;
        if (Input.GetKeyDown(KeyCode.D)) return KeyCode.D;
        if (Input.GetKeyDown(KeyCode.F)) return KeyCode.F;
        if (Input.GetKeyDown(KeyCode.J)) return KeyCode.J;
        if (Input.GetKeyDown(KeyCode.K)) return KeyCode.K;
        if (Input.GetKeyDown(KeyCode.L)) return KeyCode.L;
        if (Input.GetKeyDown(KeyCode.Semicolon)) return KeyCode.Semicolon;
        if (Input.GetKeyDown(KeyCode.Space)) return KeyCode.Space;

        return KeyCode.None;
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
        scoreSheet.Push(0.9f);
        Debug.Log("perfect");
    }

    private void HandleHit()
    {
        scoreSheet.Push(0.9f);
        Debug.Log("good");
    }

    private void HandleMiss()
    {
        scoreSheet.Push(0f);
        Debug.Log("miss");
    }
    
    private void HandleSkip()
    {
        scoreSheet.Push(0f);
    }
}
