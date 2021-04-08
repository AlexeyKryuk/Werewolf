using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _hitParticlesLeft;
    [SerializeField] private ParticleSystem[] _hitParticlesRight;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transformation _transformation;

    private List<Enemy> _enemies = new List<Enemy>();

    public UnityAction EnemyKilled;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _transformation.IsTransformed)
        {
            _animator.SetTrigger("Attack");

            StopCoroutine(PlayHitEffects());
            StartCoroutine(PlayHitEffects());

            if (_enemies.Count > 0)
            {
                for (int i = 0; i < _enemies.Count; i++)
                {
                    _enemies[i].Kill();
                    EnemyKilled?.Invoke();
                }
                _enemies.Clear();
            }
        }
    }

    private IEnumerator PlayHitEffects()
    {
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < _hitParticlesRight.Length; i++)
        {
            _hitParticlesRight[i].Play();
        }

        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < _hitParticlesLeft.Length; i++)
        {
            _hitParticlesLeft[i].Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _enemies.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {            
            _enemies.Remove(enemy);
        }
    }
}
