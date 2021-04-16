using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(Animator))]
public class DangerBar : MonoBehaviour
{
    [SerializeField] private Player _player;

    private Slider _slider;
    private Animator _animator;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _slider.value = _player.Danger;

        _animator.SetFloat("Value", _slider.value);
    }
}
