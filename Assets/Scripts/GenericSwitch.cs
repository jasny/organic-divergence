using System;
using UnityEngine;

public class GenericSwitch : MonoBehaviour
{
    [SerializeField] private GameObject offComponent;
    [SerializeField] private GameObject onComponent;
    
    public bool IsOn
    {
        get => onComponent.activeSelf;
        set
        {
            offComponent.SetActive(!value);
            onComponent.SetActive(value);
        }
    }

    public void Toggle()
    {
        IsOn = !IsOn;
    }
}
