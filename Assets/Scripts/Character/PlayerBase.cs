using UnityEngine;

public abstract class PlayerBase :MonoBehaviour
{
    public abstract PlayerType GetPlayerType();
    public abstract void SetBuff(ItemType itemType , bool isOnOrOff , object data);
    public abstract bool IsOwnBuff(ItemType itemType);
}