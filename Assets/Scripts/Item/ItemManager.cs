using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public void OnCollider_Item(IPlayer player, ItemBase itemBase)
    {
        switch (itemBase.ItemType)
        {
            case ItemType.SelfSpeed:
            case ItemType.SelfControl:
                itemBase.SetBuff(player);
                break;
            case ItemType.EnemySpeed:
            case ItemType.EnemyControl:
                var anotherPlayer = PlayerManager.Instacne.GetAnotherPlayer(player.GetPlayerType());
                itemBase.SetBuff(anotherPlayer);
                break;
        }
    }
}