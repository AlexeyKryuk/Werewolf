using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class WindEffect : MonoBehaviour
{
    [SerializeField] private Transformation _playerTransform;

    private TrailRenderer _trail;

    private void Awake()
    {
        _trail = GetComponent<TrailRenderer>();
        _trail.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _playerTransform.IsTransformed)
        {
            _trail.enabled = true;
        }
        else
        {
            _trail.enabled = false;
        }
    }
}
