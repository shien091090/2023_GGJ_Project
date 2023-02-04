using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QTE : MonoBehaviour
{
    [SerializeField]
    private GameObject hitter;
    [SerializeField]
    private Image hitBox;

    [SerializeField]
    [Range(0,1)]
    private float hitBoxMinRange;
    [SerializeField]
    [Range(0, 1)]
    private float hitBoxMaxRnage;
    [SerializeField]
    [Range(0, 1)]
    private float hitBoxRotateMin;

    private float hitBoxStart;
    private float hitBoxEnd;

    private bool hitterRotateSwitch;
    private float hitterRotateSpeed;
    private float hitterRotateY;

    public void StartQte()
    {
        SetHitBox();
        PlayHitter();
    }

    public bool TrigQte()
    {
        if (hitBoxEnd <= hitter.transform.rotation.y && hitter.transform.rotation.y >= hitBoxStart)
            return true;
        else
        {
            QteFail();
            return false;
        }
    }

    [ContextMenu("Yep")]
    public void PlayHitter()
    {
        hitter.gameObject.SetActive(true);
        hitterRotateSwitch = true;
        //hitter.transform.DOLocalRotate(new Vector3(0,0,-180),2).SetEase(Ease.Linear);
    }
    private void QteFail()
    {
        hitter.gameObject.SetActive(false);
        //hit ®Ì°Ê
        hitterRotateSwitch = false;

    }

    private void SetHitBox()
    {
        hitBox.fillAmount = Random.Range(hitBoxMinRange,hitBoxMaxRnage);
        var rotate = Random.Range(hitBox.fillAmount,1 - hitBoxRotateMin) * 360;
        hitBox.rectTransform.localRotation = Quaternion.Euler(new Vector3(0,0, rotate));
        hitBoxStart = rotate + hitBox.fillAmount * 360;
        hitBoxEnd = rotate;
    }


    private void Update()
    {
        if (hitterRotateSwitch)
        { 
            hitterRotateY += Time.deltaTime * hitterRotateSpeed;
            hitter.transform.localRotation = Quaternion.Euler(0,0, hitterRotateY);
        }
    }
}
