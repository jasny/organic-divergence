using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatTimer : MonoBehaviour
{
    public static BeatTimer Instance { get; private set; }

    private HashSet<IBeatTimeSubject> _subjects;

    public float bpm = 120f;
    public float BeatInterval { get; private set; }

    [SerializeField] private Slider offsetSlider;
    [SerializeField] private float offset;
    
    private bool _syncopation;
    public bool Syncopation
    {
        get => _syncopation;
        set
        {
            if (_syncopation == value) return;
            
            _syncopation = value;

            if (value)
            {
                _beatCount *= 2 - (_beatDeltaTime < BeatInterval / 2 ? 1 : 0);
            }
            else
            {
                _beatCount = Mathf.FloorToInt((float)_beatCount / 2);
            }
        }
    }

    private AudioController _audioController;
    private int _audioCurrentBeat = -1;
    private float _beatDeltaTime = 0f;
    
    [SerializeField] private Text DisplayCurrentBeat;

    public int totalBeats = 16;
    private int _beatCount = 0;

    private void OnValidate()
    {
        var controller = GetComponent<AudioController>();
        if (controller) bpm = controller.bpm;
    }

    // Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        _subjects = new HashSet<IBeatTimeSubject>();

        _audioController = GetComponent<AudioController>();
        BeatInterval = 60f / bpm;
        
        if (offsetSlider) offsetSlider.value = offset;
    }

    private void Update()
    {
        if (_audioController)
        {
            TimeOnAudio();
        }
        else
        {
            TimeStandalone();
        }
    }

    private void TimeOnAudio()
    {
        var interval = BeatInterval / (_syncopation ? 2 : 1);
        var beat = Mathf.FloorToInt((_audioController.Time - offset) / interval) + 1;
        
        if (_audioCurrentBeat == beat || beat < 0) return;
        
        _audioCurrentBeat = beat;
        OnBeat();
    }

    private void TimeStandalone()
    {
        var interval = BeatInterval / (_syncopation ? 2 : 1);

        _beatDeltaTime += Time.deltaTime;
        if (_beatDeltaTime < interval) return;

        _beatDeltaTime -= interval;
        OnBeat();
    }

    private void OnBeat()
    {
        _beatCount = _audioController ? (_audioCurrentBeat - 1) % totalBeats + 1 : (_beatCount % totalBeats) + 1;

        if (DisplayCurrentBeat) DisplayCurrentBeat.text = _beatCount.ToString();
        
        foreach (var subject in _subjects)
        {
            if (subject is Behaviour behaviour && (!behaviour.enabled || !behaviour.gameObject.activeInHierarchy)) continue;
            subject.OnBeat(_beatCount);
        }
    }

    public void Register(IBeatTimeSubject subject)
    {
        _subjects.Add(subject);
    }

    public void Unregister(IBeatTimeSubject subject)
    {
        _subjects.Remove(subject);
    }

    public void SetOffset(float value)
    {
        offset = value;
    }
}
