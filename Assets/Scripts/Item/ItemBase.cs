using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public abstract ItemType ItemType { get; }

    public abstract void SetBuff(IPlayer player);

}

public class ItemSpeed : ItemBase
{
    public override ItemType ItemType => ItemType.EnemySpeed;

    public override void SetBuff(IPlayer player)
    {
    }
}