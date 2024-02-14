using System;
using System.Collections.Generic;
using UnityEngine;

public class BeatTimer : MonoBehaviour
{
    public static BeatTimer Instance { get; private set; }

    private HashSet<IBeatTimeSubject> _subjects;

    [SerializeField] private float bpm = 120f;

    public float BeatInterval { get; private set; }
    private float _timeSinceLastCheck = 0f;
    

    [Tooltip("4 * Number of bars in the song")]
    public int totalBeats = 64;
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
        _timeSinceLastCheck += Time.deltaTime;
        if (!(_timeSinceLastCheck >= BeatInterval)) return;
        
        OnBeat();
        _timeSinceLastCheck -= BeatInterval;
    }

    private void OnBeat()
    {
        _beatCount = (_beatCount % totalBeats) + 1;
        
        foreach (var subject in _subjects)
        {
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
