using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStrikeEffect : MonoBehaviour
{
    public float hideTimes;

    private void OnEnable()
    {
        StartCoroutine(Cor_AutoHide());
    }

    private IEnumerator Cor_AutoHide()
    {
        hideTimes = 1;
        yield return new WaitForSeconds(hideTimes);
        
        gameObject.SetActive(false);
    }
}
