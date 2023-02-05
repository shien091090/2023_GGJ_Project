using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField]
    private Transform _itemParent_tran;

    [SerializeField]
    private bool _isRandomCreateItem = false;

    [SerializeField]
    private float _randomCreateItemTime = 10;

    [SerializeField]
    private List<ItemBase> _itemBasePrefabs = null;

    [SerializeField]
    private ItemTip _itemTip = null;

    private bool _isNeedCreateItem = false;
    private bool _isNeedRemoveItem => _waiteRemoveItemBases.Count > 0;

    private float _elpasedRandomCreateItem = 0;
    private List<ItemBase> _processItemBases = new List<ItemBase>();
    private List<ItemBase> _waiteRemoveItemBases = new List<ItemBase>();

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        ItemManager.Instance = null;
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        if (_isRandomCreateItem && !_isNeedCreateItem)
        {
            _elpasedRandomCreateItem += delta;

            if (_elpasedRandomCreateItem > _randomCreateItemTime)
                _isNeedCreateItem = true;
        }

        foreach (var itemBase in _processItemBases)
            itemBase.Tick(delta);
    }

    private void LateUpdate()
    {
        if (_isNeedRemoveItem)
        {
            foreach (var itemBase in _waiteRemoveItemBases)
            {
                _processItemBases.Remove(itemBase);
                Destroy(itemBase.gameObject);
            }

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
        if (TerrainManager.Instances.GetTerrain(out (int id, Vector2 pos) data))
        {
            var randIndex = Random.Range(0 , selectData.Count);
            //var values = System.Enum.GetValues(typeof(TriggerTarget));
            //var randTrigger = (TriggerTarget)Random.Range(0 , values.Length);

            CreateItem(selectData[randIndex] , TriggerTarget.Another , data.id , data.pos);
        }
    }

    public void CreateItem(ItemBase itemBase , TriggerTarget triggerTarget , int terrainId , Vector2 pos)
    {
        var newTipPos = pos;
        newTipPos.y += _itemTip.SpriteSize.y * 0.5f;

        var itemTip = GameObject.Instantiate(_itemTip , newTipPos , Quaternion.identity , _itemParent_tran);
        itemTip.Show(() =>
        {
            var newItemPos = pos;
            newItemPos.y += itemBase.SpriteSize.y * 0.5f;

            ItemBase item = GameObject.Instantiate(itemBase , newItemPos , Quaternion.identity , _itemParent_tran);
            item.InitBuff(triggerTarget , terrainId , ReleaseItem);
            _processItemBases.Add(item);
        });
    }

    private void ReleaseItem(ItemBase itemBase)
    {
        if (!_waiteRemoveItemBases.Contains(itemBase))
        {
            TerrainManager.Instances.ReleaseTerrain(itemBase.TerrainId);
            _waiteRemoveItemBases.Add(itemBase);
        }
    }

    public void OnCollider_Item(PlayerBase playerBase , ItemBase itemBase)
    {
        if (playerBase.IsOwnBuff(itemBase.ItemType))
        {
            ReleaseItem(itemBase);
            return;
        }

        string msg = "";
        if (itemBase.TriggerType == TriggerTarget.Self)
        {
            itemBase.TriggerBuff(playerBase);
            msg += playerBase.GetPlayerType().ToString();
        }
        else
        {
            var anotherPlayer = PlayerManager.Instance.GetAnotherPlayer(playerBase.GetPlayerType());
            itemBase.TriggerBuff(anotherPlayer);
            msg += anotherPlayer.GetPlayerType().ToString();
        }

        msg += $" ¿Ú±o {itemBase.GetDirections}";

        ToastManager.Instance.ShowToast(msg);
    }
}