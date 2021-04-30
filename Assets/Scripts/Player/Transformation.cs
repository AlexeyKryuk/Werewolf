using MeshTransformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Transformation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _cameraAnimator;

    private bool _isTransformed = true;
    private Coroutine coroutine;

    public bool IsTransformed  => _isTransformed;

    public UnityAction<bool> Transformed;

    private void OnClick()
    {
        if (coroutine == null)
            coroutine = StartCoroutine(Transform());
    }

    private IEnumerator Transform()
    {
        _isTransformed = !_isTransformed;
        Transformed?.Invoke(_isTransformed);

        Animate();

        yield return new WaitForSeconds(2f);
        coroutine = null;
    }

    private void Animate()
    {

        _animator.SetBool("IsTransformed", _isTransformed);
        _animator.SetTrigger("Transform");
        _cameraAnimator.SetTrigger("Switch");
    }
}
