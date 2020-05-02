using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePopupsCanvas : MonoBehaviour
{
  public Transform Container;

  private void Awake()
  {
    gameObject.SetActive(false);
  }
}
