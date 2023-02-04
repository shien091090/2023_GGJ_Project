using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField]
    private Transform _itemParent_tran;

    [SerializeField]
    private bool _isRandomCreateITem = false;

    [SerializeField]
    private float _randomCreateItemTime = 10;

    [SerializeField]
    private List<ItemBase> _itemBasePrefabs = null;

    private bool _isNeedCreateItem = false;
    private bool _isNeedRemoveItem => _waiteRemoveItemBases.Count > 0;

    private float _elpasedRandomCreateItem = 0;
    private List<ItemBase> _processItemBases = new List<ItemBase>();
    private List<ItemBase> _waiteRemoveItemBases = new List<ItemBase>();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        if (_isRandomCreateITem && !_isNeedCreateItem)
        {
            _elpasedRandomCreateItem += delta;

            if (_elpasedRandomCreateItem > _randomCreateItemTime)
                _isNeedCreateItem = true;
        }

        foreach (var itemBase in _processItemBases)
        {
            itemBase.Tick(delta);
        }
    }

    private void LateUpdate()
    {
        if (_isNeedRemoveItem)
        {
            foreach (var itemBase in _waiteRemoveItemBases)
                _processItemBases.Remove(itemBase);

            _waiteRemoveItemBases.Clear();
        }

        if (_isNeedCreateItem)
        {
            RandomCreateItem(_itemBasePrefabs);

            _elpasedRandomCreateItem = 0;
            _isNeedCreateItem = false;
        }
    }

    public void CreateItem(ItemType itemType = ItemType.None)
    {
        var selectItemBase = _itemBasePrefabs.Where(s => s.ItemType == itemType).ToList();

        if (selectItemBase.Count > 0)
            RandomCreateItem(selectItemBase);
    }

    private void RandomCreateItem(List<ItemBase> selectData)
    {
        var randIndex = Random.Range(0 , selectData.Count - 1);

        CreateItem(selectData[randIndex] , new Vector2(Random.Range(0 , 100) , Random.Range(0 , 100)));
    }

    public void CreateItem(ItemBase itemBase , Vector2 pos)
    {
        ItemBase item = GameObject.Instantiate(itemBase , pos , Quaternion.identity , _itemParent_tran);
        item.InitBuff(ReleaseItem);
        _processItemBases.Add(item);
    }

    private void ReleaseItem(ItemBase itemBase)
    {
        if(!_waiteRemoveItemBases.Contains(itemBase))
            _waiteRemoveItemBases.Add(itemBase);
    }

    public void OnCollider_Item(IPlayer player , ItemBase itemBase)
    {
        if (itemBase.GetTriggerType == TriggerTarget.Self)
            itemBase.TriggerBuff(player);
        else
        {
            var anotherPlayer = PlayerManager.Instacne.GetAnotherPlayer(player.GetPlayerType());
            itemBase.TriggerBuff(anotherPlayer);
        }
    }
}