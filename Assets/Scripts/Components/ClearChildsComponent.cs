using Assets.Scripts.Utils.ExtensionMethods;
using UnityEngine;

public class ClearChildsComponent : MonoBehaviour
{
  void Awake()
  {
    transform.ClearChilds();
  }
}
