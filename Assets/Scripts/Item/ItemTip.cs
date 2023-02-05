using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemTip : MonoBehaviour
{
    [SerializeField]
    private int _showCount = 3;

    [SerializeField]
    private SpriteRenderer _sr;

    [SerializeField]
    private Color _defaultColor = Color.white;

    [SerializeField]
    private Color _targetColor = Color.white;

    private Action _callback = null;

    public void Show(Action callback)
    {
        _callback = callback;

        _sr.color = _defaultColor;

        _sr.DOColor(_targetColor, 0.2f).SetLoops(_showCount, LoopType.Yoyo).OnComplete(() =>
        {
            callback?.Invoke();

            DestroyImmediate(gameObject);
        }).Play();
    }
}
