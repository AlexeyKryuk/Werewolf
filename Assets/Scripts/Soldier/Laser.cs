using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _smoothing;

    private LineRenderer _lazerBeam;

    private void Awake()
    {
        _lazerBeam = GetComponent<LineRenderer>();
        Stop();
    }

    private void LateUpdate()
    {
        if (_lazerBeam.enabled)
        {
            _lazerBeam.SetPosition(0, _startPoint.position);

            _lazerBeam.SetPosition(1, Vector3.Lerp(_lazerBeam.GetPosition(1), _target.position + Vector3.up,
                _smoothing));
        }
    }

    public void Start()
    {
        _lazerBeam.gameObject.SetActive(true);
    }

    public void Stop()
    {
        _lazerBeam.gameObject.SetActive(false);
    }
}
