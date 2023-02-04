using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create ScriptableObject", menuName ="Create QteInfo",order =1)]
public class QteInfo : ScriptableObject
{
    public const float FULL_ANGLE = 360;

    [SerializeField]
    [Header("最小打擊框長度")]
    [Range(0, 1)]
    public float hitBoxMinRange;
    [SerializeField]
    [Header("最大打擊框長度")]
    [Range(0, 1)]
    public float hitBoxMaxRnage;
    [SerializeField]
    [Header("打擊框最近距離")]
    [Range(0, 1)]
    public float hitBoxRotateMin;

    [SerializeField]
    [Header("成功顯示延遲時間")]
    public float hitterSuccessDelayTime;
    [SerializeField]
    [Header("失敗震動時間")]
    public float failShakeTime;
    [SerializeField]
    [Header("失敗震動強度")]
    public float failShakeStrength;

    [SerializeField]
    [Header("QTE 反應時間")]
    public float qteTime;
}
