using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
  private List<ObjectPoolData> poolsInfo;
  private List<ObjectPool> pools;

  //---------------------------------------------------------------------------------------------------------------
  private void Awake()
  {
    poolsInfo = new List<ObjectPoolData>();
    pools = new List<ObjectPool>();

    foreach (ObjectPoolData data in poolsInfo)
    {
      AddPool(data, null);
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public ObjectPool GetPool(ObjectPoolName name)
  {
    return pools.Find(p => p.data.name == name);
  }

  //---------------------------------------------------------------------------------------------------------------
  public APoolable Pop(ObjectPoolName name, Transform NewParent)
  {
    APoolable result = this.Pop(name);
    result.gameObject.SetActive(true);
    result.transform.SetParent(NewParent);

    return result;

  }
  /// <summary>
  /// Does NOT set object active.
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  //---------------------------------------------------------------------------------------------------------------
  public APoolable Pop(ObjectPoolName name)
  {
    return GetPool(name).Pop();
  }

  //---------------------------------------------------------------------------------------------------------------
  public T Pop<T>(ObjectPoolName name) where T : APoolable
  {
    return (T) GetPool(name).Pop();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void AddPool(ObjectPoolData data, RootBase root = null, Transform container = null)
  {
    if (GetPool(data.name) != null)
    {
      Debug.Log("PoolManager => pool " + data.name + " already exists. Removing old instance.");
      this.DestroyPool(data.name);
    }

    if (root != null)
      root.ListenRootUnloaded(() => DestroyPool(data.name));

    GameObject parent;
    if (container != null)
      parent = container.gameObject;
    else
    {
      parent = new GameObject(data.name + "Pool");
      parent.transform.SetParent(this.transform);
    }

    pools.Add(new ObjectPool(data, parent));
  }

  //---------------------------------------------------------------------------------------------------------------
  private void DestroyPool(ObjectPoolName name)
  {
    pools.FindAll(p => p.data.name == name).ForEach(p => Destroy(p.parent));
    pools.RemoveAll(p => p.data.name == name);
  }
}
