using System;
using System.Collections;
using UnityEngine;

public class CoroutineProvider
{
  private readonly MonoBehaviour monoBehaviour;

  public CoroutineProvider(MonoBehaviour monoBehaviour)
  {
    this.monoBehaviour = monoBehaviour;
  }

  public Coroutine StartCoroutine(IEnumerator enumerator)
  {
    var result = monoBehaviour.StartCoroutine(enumerator);
    return result;
  }

  public void StopCoroutine(IEnumerator coroutine)
  {
    monoBehaviour.StopCoroutine(coroutine);
  }

  public void RunInEndOfFrame(Action action)
  {
    StartCoroutine(RunInEndOfFrameInternal(action));
  }

  private IEnumerator RunInEndOfFrameInternal(Action action)
  {
    yield return new WaitForEndOfFrame();
    action.Invoke();
  }
}
