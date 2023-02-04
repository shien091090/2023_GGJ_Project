using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class QTE : MonoBehaviour
{
    [SerializeField]
    private GameObject hitter;
    [SerializeField]
    private Image hitBox;

    private bool isPlaying;
    private float hitBoxStart;
    private float hitBoxEnd;

    private Tween hitterRotate;

    [SerializeField]
    private QteInfo info;

    [SerializeField]
    private CharacterKeySetting keySetting;

    private Action<bool> callback;


    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void StartQte(Action<bool> _callBack)
    {
        isPlaying = true;
        SetHitBox();
        PlayHitter();
        callback = _callBack;
    }

    public void TrigQte()
    {
        hitterRotate.Kill();
        var rotation = hitter.transform.rotation.eulerAngles.z;
        if (hitBoxEnd >= rotation && rotation >= hitBoxStart)
            DOVirtual.DelayedCall(info.hitterSuccessDelayTime, QteSuccess);
        else
            QteFail();
    }

    public void QteUnexceptedStop()
    {
        QteFail();
    }

    private void SetHitBox()
    {
        hitBox.fillAmount = UnityEngine.Random.Range(info.hitBoxMinRange, info.hitBoxMaxRnage);
        var rotate = UnityEngine.Random.Range(hitBox.fillAmount, 1 - info.hitBoxRotateMin) * QteInfo.FULL_ANGLE;
        hitBox.rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotate));
        hitBoxStart = rotate - hitBox.fillAmount * QteInfo.FULL_ANGLE;
        hitBoxEnd = rotate;
    }
    private void PlayHitter()
    {
        hitter.gameObject.SetActive(true);
        hitterRotate = hitter.transform.DOLocalRotate(new Vector3(0,0,-QteInfo.FULL_ANGLE), info.qteTime, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnComplete(()=> {
            QteFail();
        });
    }
    private void QteSuccess()
    {
        callback(true);
        hitter.gameObject.SetActive(false);
        hitter.transform.rotation = Quaternion.Euler(0, 0, 0);
        isPlaying = false;
    }

    private void QteFail()
    {
        callback(false);
        hitBox.transform.DOShakePosition(info.failShakeTime, info.failShakeStrength);
        hitter.transform.DOShakePosition(info.failShakeTime, info.failShakeStrength).OnComplete(()=> {
            hitter.gameObject.SetActive(false);
            hitter.transform.rotation = Quaternion.Euler(0, 0, 0);
        });
        isPlaying = false;
    }

    private void Update()
    {
        if (isPlaying && Input.GetKeyUp(keySetting.qteKey))
            TrigQte();     
    }
}
