using UnityEngine;
// Don't use Awake/Start or OnEnable for this object. It will not work. Use OnPop()
public abstract class APoolable : MonoBehaviour
{
  public ObjectPool Pool  { get; set; }
  private bool isPooled;
  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Return true if this object is not in game, but inside it's pool contaier and disabled.
  /// </summary>
  public bool IsPooled { get { return isPooled; }}

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Works as "Instantiate" for standard prefab, but instead moves poolable Object from the container or creates new if there is no more poolable objects left.
  /// You need to clean the object for future use in OnReturnedToPool OR OnPop.
  /// </summary>
  public virtual void OnPop()
  {
    isPooled = false;
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Called by automatically when ReturnToPool is called. You don't need to run it manually, but you have to inplement any requred cleaning routines there.
  /// </summary>
  public abstract void OnReturnedToPool();


  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Works as "Destroy" for standard prefab, but instead moves poolable Object to its container.
  /// You need to clean the object for future use in OnReturnedToPool OR OnPop.
  /// </summary>
  public void ReturnToPool()
  {
    isPooled = true;
    if (Pool != null)
      Pool.Push(this);
    else
    {
      Destroy(gameObject);
    }
      

    OnReturnedToPool();
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Called once this object was create.
  /// NOTE: Onject is created once, but could be pooled and many times. Standard Awake will not fire.
  /// </summary>
  public virtual void OnCreate()
  {

  }

  //---------------------------------------------------------------------------------------------------------------
  private void OnDestroy()
  {
    if(!isPooled)
      ReturnToPool();
  }
}
