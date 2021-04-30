using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Victim : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _safeDistance;
    [SerializeField] private Animator _animator;

    private Coroutine _coroutine;

    public bool IsDangered { get; private set; }

    public UnityAction<bool> Dangered;
    public UnityAction<Victim> Died;

    private void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < _safeDistance)
        {
            IsDangered = true;
            Dangered?.Invoke(IsDangered);
        }
        else
        {
            IsDangered = false;
            Dangered?.Invoke(IsDangered);
        }
    }

    public void Kill()
    {
        if (_coroutine == null)
            _coroutine = StartCoroutine(KillWithDelay());
    }

    public IEnumerator KillWithDelay()
    {
        _animator.SetTrigger("death");

        yield return new WaitForSeconds(2f);
        Died?.Invoke(this);
        gameObject.SetActive(false);
    }
}
