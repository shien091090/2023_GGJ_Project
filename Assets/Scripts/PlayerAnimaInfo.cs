using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create ScriptableObject", menuName ="Create PlayereAnimaInfo",order =1)]
public class PlayerAnimaInfo : ScriptableObject
{
    public string idleAnimaName;
    public string runAnimaName;
    public string pull01AnimaName;
    public string pull02AnimaName;
    public string pull03AnimaName;

    public string GetPullLevelAnimaName(int leftHp)
    {
        if (leftHp >= 3)
            return pull01AnimaName;
        if (leftHp >= 2)
            return pull02AnimaName;
            return pull03AnimaName;
    }
}
