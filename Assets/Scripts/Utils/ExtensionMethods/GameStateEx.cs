using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utils.ExtensionMethods
{
  public static class GameStateEx
  {
    public static GameState FindFirstState()
    {
      GameState result = GameState.None;

      var states = Enum.GetValues(typeof(GameState)).OfType<GameState>().Where(s => s != GameState.None).ToList();

      if (states.Count < 1)
      {
        Debug.LogError("<color=red>There's no GameState Please add one\n</color>");
        return result;
      }

      result = states.First();

      //if ((result == GameState.Menu) && (Game.IsIgnoreMainMenu()))
      //{
      //  result++;
      //}

      return result;
    }

    public static RootBase ToRootBase(this GameState state)
    {
      switch (state)
      {
        //case GameState.Menu:
        //  return Game.MenuRoot;
        case GameState.Action:
          return Game.ActionRoot;

        default:
          return null;
      }

    }

    public static string ToSceneName(this GameState state)
    {
      string name = state.ToString() + "Scene";
      if (Application.CanStreamedLevelBeLoaded(name))
      {
        return name;
      }

      Debug.LogError("<color=red>Load Scene Failed. " + name + " not found.</color>");
      return String.Empty;
    }
  }
}
