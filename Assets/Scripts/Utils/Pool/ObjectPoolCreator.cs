using UnityEngine;

public class ObjectPoolCreator : MonoBehaviour
{
  [SerializeField]
  private APoolable prefab;
  [SerializeField]
  private ObjectPoolName name;
  [Header("Can be empty")]
  [SerializeField]
  private int initCapacity;
  [SerializeField]
  private RootBase root;
  [SerializeField]
  private Transform container;

  private void Awake()
  {
    prefab.gameObject.SetActive(false);

    ObjectPoolData data = new ObjectPoolData(name, prefab, initCapacity);
    Game.PoolManager.AddPool(data, root, container);
  }
}
