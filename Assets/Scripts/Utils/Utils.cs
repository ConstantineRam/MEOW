using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public static class Utils
{
#if UNITY_EDITOR
  public static GameObject LoadAssetByName(string name)
  {
    return AssetDatabase.LoadAssetAtPath<GameObject>(GetAssetPath(name));
  }

  public static string GetAssetPath(string name)
  {
    string guid = AssetDatabase.FindAssets(name).First();
    return AssetDatabase.GUIDToAssetPath(guid);
  }

  public static void AddSceneToBuild(string fullPath, int index = -1)
  {
    List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes.ToList();
    var sceneToAdd = new EditorBuildSettingsScene(fullPath, true);

    if (index < 0)
      scenes.Add(sceneToAdd);
    else
      scenes.Insert(index, sceneToAdd);

    EditorBuildSettings.scenes = scenes.ToArray();
  }

  public static bool IsSceneInBuild(string name)
  {
    bool result = false;

    foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
    {
      string sceneName = Path.GetFileNameWithoutExtension(scene.path);
      if (sceneName != null && sceneName.Equals(name))
      {
        result = true;
        break;
      }
    }

    return result;
  }
#endif

  public static Type GetType(string TypeName)
  {
    var type = Type.GetType(TypeName);

    if (type != null)
      return type;

    var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));

    var assembly = Assembly.LoadWithPartialName(assemblyName);
    if (assembly == null)
      return null;

    return assembly.GetType(TypeName);
  }
}
