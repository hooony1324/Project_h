using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class CoroutineManager : MonoBehaviour
{
    private Dictionary<IEnumerator, Coroutine> _activatedCoroutines = new Dictionary<IEnumerator, Coroutine>();

    public void Register(IEnumerator coroutine)
    {
        if (!_activatedCoroutines.ContainsKey(coroutine))
        {
            Coroutine newCoroutine = StartCoroutine(coroutine);
            _activatedCoroutines.Add(coroutine, newCoroutine);
        }
    }

    public void UnRegister(IEnumerator coroutine)
    {
        if (_activatedCoroutines.ContainsKey(coroutine))
        {
            StopCoroutine(_activatedCoroutines[coroutine]);
            _activatedCoroutines.Remove(coroutine);
        }
    }

    public void StopAllRegisteredCoroutines()
    {
        foreach (var coroutine in _activatedCoroutines.Values)
        {
            StopCoroutine(coroutine); // 모든 코루틴 중지
        }
        _activatedCoroutines.Clear();     // 딕셔너리 초기화
    }
}