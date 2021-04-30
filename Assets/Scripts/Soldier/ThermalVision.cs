using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalVision : MonoBehaviour
{
    [SerializeField] private Transformation _target;
    [SerializeField] private SkinnedMeshRenderer[] _renderers;
    [SerializeField] private Material _thermalMaterial;

    private Material[] _defaultMaterials;

    private void Awake()
    {
        _defaultMaterials = new Material[_renderers.Length];

        for (int i = 0; i < _defaultMaterials.Length; i++)
        {
            _defaultMaterials[i] = _renderers[i].material;
        }

        ChangeMaterial(true);
    }

    private void OnEnable()
    {
        _target.Transformed += ChangeMaterial;
    }
    private void OnDisable()
    {
        _target.Transformed -= ChangeMaterial;
    }

    private void ChangeMaterial(bool isTransformed)
    {
        if (isTransformed)
        {
            StartCoroutine(ChangeMaterial(_thermalMaterial));
        }
        else
        {
            StartCoroutine(ChangeMaterial(_defaultMaterials));
        }
    }

    private IEnumerator ChangeMaterial(Material target)
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material = target;
        }
    }

    private IEnumerator ChangeMaterial(Material[] targets)
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material = targets[i];
        }
    }
}
