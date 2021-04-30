using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    private Victim _victim;
    private Image _image;

    private void Awake()
    {
        _victim = GetComponentInParent<Victim>();
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _victim.Dangered += OnDangered;
    }

    private void OnDisable()
    {
        _victim.Dangered -= OnDangered;
    }

    private void OnDangered(bool isDangered)
    {
        if (isDangered)
            _image.enabled = true;
        else
            _image.enabled = false;
    }
}
