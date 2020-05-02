using System;

[Serializable]
public class ObjectPoolData
{
  public ObjectPoolName name;
  public APoolable prefab;
  public int initialCapacity;

  public ObjectPoolData()
  {
  }

  public ObjectPoolData(ObjectPoolName name, APoolable prefab, int initialCapacity)
  {
    this.name = name;
    this.prefab = prefab;
    this.initialCapacity = initialCapacity;
  }
}

