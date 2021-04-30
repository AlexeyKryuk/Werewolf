using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    private List<Victim> _targets = new List<Victim>();

    public Victim CurrentTarget { get; private set; }

    private void OnEnable()
    {
        foreach (var victim in FindObjectsOfType<Victim>())
        {
            _targets.Add(victim);
            victim.Died += RemoveTarget;
        }

        CurrentTarget = _targets[0];
    }

    private void Start()
    {
        StartCoroutine(SelectTarget());
    }

    private void Update()
    {
        if (CurrentTarget != null)
        {
            transform.LookAt(CurrentTarget.transform);
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        }
    }

    private IEnumerator SelectTarget()
    {
        while (_targets.Count > 0)
        {
            float minDistance = Vector3.Distance(transform.position, _targets[0].transform.position);
            int nearestTargetIndex = 0;

            for (int i = 1; i < _targets.Count; i++)
            {
                if (minDistance > Vector3.Distance(transform.position, _targets[i].transform.position))
                {
                    minDistance = Vector3.Distance(transform.position, _targets[i].transform.position);
                    nearestTargetIndex = i;
                }
            }

            CurrentTarget = _targets[nearestTargetIndex];

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void RemoveTarget(Victim target)
    {
        if (CurrentTarget == target)
        {
            _targets.Remove(target);
        }
    }
}
