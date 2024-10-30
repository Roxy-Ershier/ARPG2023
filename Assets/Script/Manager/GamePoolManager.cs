using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGG.Tool.Singleton;

public class GamePoolManager : Singleton<GamePoolManager>
{
    [System.Serializable]

    private class PoolItem
    {
        public string ItemName;
        public GameObject Item;
        public int InitMaxCout;
    }
    [SerializeField] private List<PoolItem> _configPoolItem = new List<PoolItem>();


    private Dictionary<string, Queue<GameObject>> _poolCentre = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, Transform> _poolTransfrom = new Dictionary<string, Transform>();

    private void Start()
    {
        InitGamePool();
    }

    private void InitGamePool()
    {
        if (_configPoolItem.Count <= 0) return;

        for(int i=0;i< _configPoolItem.Count; i++)
        {
            if (!_poolCentre.ContainsKey(_configPoolItem[i].ItemName))
            {
                _poolCentre.Add(_configPoolItem[i].ItemName, new Queue<GameObject>());

                var objT = new GameObject(_configPoolItem[i].ItemName);
                objT.transform.SetParent(transform);
                _poolTransfrom.Add(_configPoolItem[i].ItemName, objT.transform);

            }
            for(int j = 0; j < _configPoolItem[i].InitMaxCout; j++)
            {
                var obj = Instantiate(_configPoolItem[i].Item);
                _poolCentre[_configPoolItem[i].ItemName].Enqueue(obj);
                obj.SetActive(false);
                obj.transform.SetParent(_poolTransfrom[_configPoolItem[i].ItemName]);
            }
        }
    }

    /// <summary>
    /// 从对象池中唤醒一个对象
    /// </summary>
    /// <param name="name"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void TryAwakeOneItemFromPool(string name,Vector3 position,Quaternion rotation)
    {
        if (!_poolCentre.ContainsKey(name))
        {
            Debug.Log($"对象池中不存在名为[{name}]的对象池");
            return;
        }

        var item = _poolCentre[name].Dequeue();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.SetActive(true);
        _poolCentre[name].Enqueue(item);

    }

    /// <summary>
    /// 从对象中获取一个对象数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject TryGetOneItemFromPool(string name)
    {
        if (!_poolCentre.ContainsKey(name))
        {
            Debug.Log($"对象池中不存在名为[{name}]的对象池");
            return null;
        }

        var item = _poolCentre[name].Dequeue();
        item.SetActive(true);
        _poolCentre[name].Enqueue(item);
        return item;

    }

}
