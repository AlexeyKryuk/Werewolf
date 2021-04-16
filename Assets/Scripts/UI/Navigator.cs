using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    private List<Transform> _targets = new List<Transform>();
    private Transform _currentTarget;

    private void OnEnable()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            _targets.Add(enemy.transform);
            enemy.Died += RemoveTarget;
        }

        _currentTarget = _targets[0];
    }

    private void Start()
    {
        StartCoroutine(SelectTarget());
    }

    private void Update()
    {
        transform.LookAt(_currentTarget);
    }

    private IEnumerator SelectTarget()
    {
        while (_targets.Count > 0)
        {
            float minDistance = Vector3.Distance(transform.position, _targets[0].position);
            int nearestTargetIndex = 0;

            for (int i = 1; i < _targets.Count; i++)
            {
                if (minDistance > Vector3.Distance(transform.position, _targets[i].position))
                {
                    minDistance = Vector3.Distance(transform.position, _targets[i].position);
                    nearestTargetIndex = i;
                }
            }

            _currentTarget = _targets[nearestTargetIndex];

            yield return new WaitForSeconds(1f);
        }
    }

    private void RemoveTarget(Enemy target)
    {
        if (_currentTarget == target.transform)
        {
            _targets.Remove(target.transform);
            _currentTarget = _targets[0];
        }
    }
}
