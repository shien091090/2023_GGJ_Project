using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance { get; private set; }

    [SerializeField]
    private RectTransform _msgRoot = null;

    [SerializeField]
    private Text _test = null;

    [SerializeField]
    private Vector3 _targetScale = Vector3.one;

    private TweenerCore<Vector3, Vector3, VectorOptions> _tweener;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void ShowToast(string msg)
    {
        if (_tweener != null)
            _tweener.Kill(false);

        if (_msgRoot.gameObject.activeSelf)
            _msgRoot.gameObject.SetActive(false);

        _test.text = msg;

        _msgRoot.transform.localScale = Vector3.zero;
        _msgRoot.gameObject.SetActive(true);
        _tweener = _msgRoot.DOScale(_targetScale, 0.2f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            _tweener = _msgRoot.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBounce).SetDelay(1).Play();
        }).Play();
    }
}
