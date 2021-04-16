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
    private Coroutine coroutine;

    public bool IsTransformed  => _isTransformed;

    public UnityAction<bool> Transformed;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch[] touches = Input.touches;
            for (int i = 0; i < touches.Length; i++)
            {
                if (Input.GetTouch(i).tapCount == 2)
                {
                    if (coroutine == null)
                        coroutine = StartCoroutine(Transform());
                }
            }
        }
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
