using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatTimer : MonoBehaviour
{
    public static BeatTimer Instance { get; private set; }

    private HashSet<IBeatTimeSubject> _subjects;

    public float bpm = 120f;
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
                _beatCount *= 2 - (_timeSinceLastCheck < BeatInterval / 2 ? 1 : 0);
            }
            else
            {
                _beatCount = Mathf.FloorToInt((float)_beatCount / 2);
            }
        }
    }

    public float BeatInterval { get; private set; }
    private float _timeSinceLastCheck = 0f;
    
    [SerializeField] private Text DisplayCurrentBeat;

    public int totalBeats = 16;
    private int _beatCount = 0;

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
        
        BeatInterval = 60f / bpm;
    }

    private void Update()
    {
        var interval = BeatInterval / (_syncopation ? 2 : 1);
        
        _timeSinceLastCheck += Time.deltaTime;
        if (_timeSinceLastCheck < interval) return;
        
        OnBeat();
        _timeSinceLastCheck -= interval;
    }

    private void OnBeat()
    {
        _beatCount = (_beatCount % totalBeats) + 1;

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
}
