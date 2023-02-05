using System.Collections.Generic;
using UnityEngine;

public class CharacterStrikeEffectManager : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    private List<PlayerType> contactPlayerList = new List<PlayerType>();
    private List<GameObject> effectPool = new List<GameObject>();
    public static CharacterStrikeEffectManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void PlayStrikeEffect(PlayerType playerType, Vector2 contactPointPoint)
    {
        if (contactPlayerList.Contains(playerType))
            return;
        else
            contactPlayerList.Add(playerType);

        if (!contactPlayerList.Contains(PlayerType.Player1) || !contactPlayerList.Contains(PlayerType.Player2))
            return;

        contactPlayerList = new List<PlayerType>();
        PutEffectObject(contactPointPoint);
    }

    private void PutEffectObject(Vector2 effectPos)
    {
        GameObject targetObj = null;
        foreach (GameObject effectObj in effectPool)
        {
            if (effectObj.activeSelf == false)
            {
                targetObj = effectObj;
                break;
            }
        }

        if (targetObj == null)
            targetObj = Instantiate(effectPrefab, this.transform);

        targetObj.transform.position = effectPos;
        targetObj.SetActive(true);
        
        effectPool.Add(targetObj);
    }
}