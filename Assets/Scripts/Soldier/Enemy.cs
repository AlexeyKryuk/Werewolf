using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Detection _detection;

    private Coroutine coroutine;

    public UnityAction<Enemy> Died;

    public void Kill()
    {
        if (coroutine == null)
            coroutine = StartCoroutine(KillWithDelay());
    }

    public IEnumerator KillWithDelay()
    {
        _animator.SetTrigger("death");

        if (_detection != null)
            _detection.enabled = false;

        yield return new WaitForSeconds(2f);
        Died?.Invoke(this);
        gameObject.SetActive(false);
    }
}
