using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Attack : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private AttackBehaviour _attackBehaviour;

    public void OnPointerDown(PointerEventData eventData)
    {
        _attackBehaviour.Attack();
    }
}
