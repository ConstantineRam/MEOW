using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootBase : MonoBehaviour
{
  private Signal onRootUnloaded;

  private GameObject[] roots;

  protected void Awake()
  {
    onRootUnloaded = new Signal();

    roots = SceneManager.GetActiveScene().GetRootGameObjects();
  }

  private void Activate()
  {
    if (GetType() == typeof(RootBase))
    {
      gameObject.SetActive(true);
    }
    if (roots != null)
      foreach (var root in roots)
        root.SetActive(true);
  }
  private void Deactivate()
  {
    if (GetType() == typeof(RootBase))
    {
      gameObject.SetActive(false);
    }
    if (roots != null)
      foreach (var root in roots)
        root.SetActive(false);
  }

  public void Load()
  {
    Activate();
  }

  public virtual void Unload()
  {
    Deactivate();

    onRootUnloaded.Invoke();

    SceneManager.UnloadSceneAsync(gameObject.scene);
  }

  public void ListenRootUnloaded(Action action)
  {
    onRootUnloaded.Listen(action);
  }
}
