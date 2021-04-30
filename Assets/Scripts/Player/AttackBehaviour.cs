using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(MoveBehaviour))]
public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _hitParticlesLeft;
    [SerializeField] private ParticleSystem[] _hitParticlesRight;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transformation _transformation;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector3 _targetOffset;
    [SerializeField] private BasicBehaviour _basicControl;
    [SerializeField] private MoveBehaviour _moveControl;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Navigator _navigator;
    [SerializeField] private Button _attackButton;

    private Sequence _sequence;


    public UnityAction EnemyKilled;
    public UnityAction Jump;

    private void OnEnable()
    {
        _attackButton.onClick.AddListener(FindVictim);
    }

    private void OnDisable()
    {
        _attackButton.onClick.RemoveListener(FindVictim);
    }

    private void FindVictim()
    {
        Victim victim = _navigator.CurrentTarget;

        if (victim != null && victim.IsDangered)
        {
            Attack(victim);
        }

    }

    private void Update()
    {
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(Input.touchCount - 1);

        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        Ray ray = _camera.ScreenPointToRay(touch.position);
        //        RaycastHit hit;

        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            if (hit.collider.TryGetComponent<Victim>(out Victim victim))
        //            {
        //                if (victim.IsDangered)
        //                    Attack(victim);
        //            }
        //        }
        //    }
        //}
    }

    public void Attack(Victim victim)
    {
        if (_transformation.IsTransformed)
        {
            if (_sequence == null)
            {
                _animator.SetTrigger("Jump");

                _sequence = DOTween.Sequence();

                _sequence.AppendCallback(DisableControl);
                _sequence.Append(_rigidbody.DOJump(victim.transform.position + _targetOffset, 3f, 1, 1f));
                transform.DOLookAt(victim.transform.position, 0.5f);
                _sequence.AppendCallback(AnimateAttack);
                _sequence.AppendCallback(() => victim.Kill());
                _sequence.AppendInterval(2f);
                _sequence.AppendCallback(EnableControl);
                _sequence.AppendCallback(() => _sequence = null);
            }
        }
    }

    private IEnumerator PlayHitEffects()
    {
        for (int i = 0; i < _hitParticlesRight.Length; i++)
        {
            _hitParticlesRight[i].Play();
        }

        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < _hitParticlesLeft.Length; i++)
        {
            _hitParticlesLeft[i].Play();
        }
    }

    private void AnimateAttack()
    {
        _animator.SetTrigger("Attack");

        StopCoroutine(PlayHitEffects());
        StartCoroutine(PlayHitEffects());
    }

    private void DisableControl()
    {
        _joystick.enabled = false;
        //_basicControl.enabled = false;
        //_moveControl.enabled = false;
    }

    private void EnableControl()
    {
        //_basicControl.enabled = true;
        //_moveControl.enabled = true;
        _joystick.enabled = true;
    }
}
