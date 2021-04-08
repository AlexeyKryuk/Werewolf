using MeshTransformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Transformation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _cameraAnimator;

    private bool _isTransformed = false;

    public bool IsTransformed  => _isTransformed;

    public UnityAction<bool> Transformed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isTransformed = !_isTransformed;
            Transformed?.Invoke(_isTransformed);

            Animate();
        }
    }

    private void Animate()
    {
        _animator.SetBool("IsTransformed", _isTransformed);
        _animator.SetTrigger("Transform");
        _cameraAnimator.SetTrigger("Switch");
    }
}
