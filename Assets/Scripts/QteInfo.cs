using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create ScriptableObject", menuName ="Create QteInfo",order =1)]
public class QteInfo : ScriptableObject
{
    public const float FULL_ANGLE = 360;

    [SerializeField]
    [Header("�̤p�����ت���")]
    [Range(0, 1)]
    public float hitBoxMinRange;
    [SerializeField]
    [Header("�̤j�����ت���")]
    [Range(0, 1)]
    public float hitBoxMaxRnage;
    [SerializeField]
    [Header("�����س̪�Z��")]
    [Range(0, 1)]
    public float hitBoxRotateMin;

    [SerializeField]
    [Header("���\��ܩ���ɶ�")]
    public float hitterSuccessDelayTime;
    [SerializeField]
    [Header("���Ѿ_�ʮɶ�")]
    public float failShakeTime;
    [SerializeField]
    [Header("���Ѿ_�ʱj��")]
    public float failShakeStrength;

    [SerializeField]
    [Header("QTE �����ɶ�")]
    public float qteTime;
}
