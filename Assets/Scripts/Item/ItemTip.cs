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
    private float _shinDurTime = 0.2f;

    [SerializeField]
    private SpriteRenderer _sr;

    [SerializeField]
    private GameObject _particle_obj = null;

    [SerializeField]
    private Color _defaultColor = Color.white;

    [SerializeField]
    private Color _targetColor = Color.white;

    public Vector2 SpriteSize => _sr.bounds.size;

    public void Show(Action callback)
    {
        _sr.color = _defaultColor;

        var sequence = DOTween.Sequence();
        sequence.Append(_sr.DOColor(_targetColor, _shinDurTime).SetLoops(_showCount, LoopType.Yoyo));
        //sequence.AppendInterval(_shinDurTime * _showCount);
        sequence.AppendCallback(() =>
        {
            _particle_obj.SetActive(true);
            _sr.enabled = false;
        });
        sequence.AppendInterval(0.2f);
        sequence.OnComplete(() =>
        {
            callback?.Invoke();
            DestroyImmediate(gameObject);
        });
    }
}
