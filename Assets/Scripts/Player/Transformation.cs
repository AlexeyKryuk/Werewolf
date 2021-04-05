using MeshTransformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _cameraAnimator;

    private bool _isTransformed = false;

    public bool IsTransformed  => _isTransformed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isTransformed = !_isTransformed;

            _animator.SetBool("IsTransformed", _isTransformed);
            _animator.SetTrigger("Transform");
            _cameraAnimator.SetTrigger("Switch");
        }
    }
}
