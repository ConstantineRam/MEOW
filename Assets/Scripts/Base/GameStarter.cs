using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1000)]
public class GameStarter : MonoBehaviour
{
  [SerializeField]
  private GameState preferredState;

  private void Awake()
  {
    DestroyImmediate(gameObject);

    if (Game.InitStarted) return;

    DisableSceneObjects();
    Game.InitStarted = true;
    Game.PreferredFirstState = preferredState;
    SceneManager.LoadScene(0);
  }




  private void DisableSceneObjects()
  {
    SceneManager.GetActiveScene()
      .GetRootGameObjects().ToList()
      .ForEach(g => g.SetActive(false));
  }
}