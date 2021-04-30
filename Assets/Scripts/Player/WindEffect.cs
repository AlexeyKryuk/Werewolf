using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    [SerializeField] private TrailRenderer[] _trails;

    public void OnEnable()
    {
        foreach (var line in _trails)
        {
            line.enabled = true;
        }
    }

    public void OnDisable()
    {
        foreach (var line in _trails)
        {
            line.enabled = false;
        }
    }
}
