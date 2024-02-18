using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSheet : MonoBehaviour
{
    private float[] _array;
    private int _start = 0;

    public int size;
    public float weightFactor = 1.0f;

    [SerializeField] private float score;
    
    private void Awake()
    {
        _array = new float[size];
    }

    public void Push(float number)
    {
        _array[_start] = number;
        _start = (_start + 1) % size;
    }

    public float WeightedAvg
    {
        get
        {
            var total = 0f;
            var weightSum = 0f;
            var currentWeight = 1f;

            var currentIndex = _start;
            do
            {
                total += _array[currentIndex] * currentWeight;
                weightSum += currentWeight;
                currentWeight *= weightFactor;

                currentIndex = (currentIndex + 1) % size;
            }
            while (currentIndex != _start);

            return total / weightSum;
        }
    }

    public void FixedUpdate()
    {
        score = WeightedAvg;
    }
}
