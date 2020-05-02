using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Script.Utility.Editor
{
  public class ProjectScenes : EditorWindow
  {
    static ProjectScenes()
    {
      EditorApplication.playmodeStateChanged += PlymodeStateChanged;
    }

    [MenuItem("Tools/Project Scenes")]
    private static void Init()
    {
      var window = GetWindow(typeof(ProjectScenes));
      window.titleContent = new GUIContent("Project Scenes");
    }

    private static SortedDictionary<String, List<String>> _gameScenePaths;
    private static SortedDictionary<String, List<String>> _otherScenePaths;
    private static ProjectScenes _ref;
    [SerializeField]
    private List<GameObject> _roots = new List<GameObject>(20);
    private static ProjectScenesSettings _settings;

    private Vector2 _scrollPosition;

    public ProjectScenesSettings Settings
    {
      get
      {
        if (_settings != null)
          return _settings;

        var path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)) + ".user.asset";

        if (File.Exists(path))
          _settings = (ProjectScenesSettings) AssetDatabase.LoadAssetAtPath(path, typeof(ProjectScenesSettings));

        else
          AssetDatabase.CreateAsset(_settings = new ProjectScenesSettings(), path);

        return _settings;
      }
    }

    private static void PlymodeStateChanged()
    {
      if (_ref != null)
        _ref.ClearAdditionalRoots();
    }

    private void OnEnable()
    {
      _ref = this;

      ReloadScenes(false);
    }
    private void OnGUI()
    {
      GUILayout.BeginHorizontal(EditorStyles.toolbar);
      {
        GUILayout.Space(2);

        if (GUILayout.Button("Reload Scenes", EditorStyles.toolbarButton))
          ReloadScenes(true);

        GUILayout.FlexibleSpace();
        GUILayout.Space(2);
      }
      GUILayout.EndHorizontal();

      if (_roots.Count > 0)
      {
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Clear Additinal Scenes"))
        {
          ClearAdditionalRoots();
        }
        GUI.backgroundColor = Color.white;
        GUILayout.Space(5);
      }

      DrawQuickScenes();

      GUILayout.BeginHorizontal();
      {
        GUILayout.Space(3);
        GUILayout.BeginVertical();
        {
          _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
          {
            if (Settings.ShowProjectScenes = EditorGUILayout.Foldout(Settings.ShowProjectScenes, "Project"))
              DrawSceneButtons(_gameScenePaths);

            if (Settings.ShowOtherScenes = EditorGUILayout.Foldout(Settings.ShowOtherScenes, "Thrid Party"))
              DrawSceneButtons(_otherScenePaths);
          }
          GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
        GUILayout.Space(1);
      }
      GUILayout.EndHorizontal();
      GUILayout.FlexibleSpace();
    }

    private void DrawQuickScenes()
    {
      GUILayout.Space(5);
      if (Settings.QuickAccessScenes.Count > 0)
      {

        GUILayout.BeginVertical("Quick Scenes", GUI.skin.window, GUILayout.MinHeight(20));
        {
          for (var i = 0; i < Settings.QuickAccessScenes.Count; i++)
          {
            var path = Settings.QuickAccessScenes[i];
            GUILayout.BeginHorizontal();
            {
              if (GUILayout.Button("S", "buttonLeft", GUILayout.Width(20)))
              {
                var obj = AssetDatabase.LoadAssetAtPath("Assets/" + path, typeof(Object));
                Selection.activeObject = obj;
              }

              if (GUILayout.Button(@"\/", "buttonRight", GUILayout.Width(20)))
              {
                Settings.QuickAccessScenes.Remove(path);
                EditorUtility.SetDirty(Settings);
                i--;
              }

              var text = path.Replace(".unity", "");

              if (GUILayout.Button(text, "buttonLeft"))
              {
                Debug.Log("[ProjectScenes] - " + text);

                if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
                  EditorApplication.OpenScene("Assets/" + path);
              }
              GUI.backgroundColor = Color.green;

              if (GUILayout.Button("Add", "buttonRight", GUILayout.Width(40)))
              {
                var oldRoots = FindObjectsOfType(typeof(Transform)).Cast<Transform>().Where(t => t.parent == null)
                  .ToList();

                EditorApplication.OpenSceneAdditive("Assets/" + path);

                var newRoots = FindObjectsOfType(typeof(Transform)).Cast<Transform>().Where(t => t.parent == null)
                  .ToList();

                foreach (var root in oldRoots)
                  newRoots.Remove(root);

                foreach (var root in newRoots)
                {
                  SetDoNotSaveFlagToHierarchy(root);
                  _roots.Add(root.gameObject);
                }
              }
              GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
          }
        }
        GUILayout.EndVertical();
        GUILayout.Space(5);
      }
    }
    private void DrawSceneButtons(IDictionary<String, List<String>> scenePaths)
    {
      if (scenePaths.Count <= 0)
        return;

      foreach (var scenePathPair in scenePaths)
      {
        GUILayout.Space(5);
        GUILayout.BeginVertical(scenePathPair.Key.Replace("/", ">"), GUI.skin.window);
        {
          foreach (var path in scenePathPair.Value)
          {
            GUILayout.BeginHorizontal();
            {
              if (GUILayout.Button("S", "buttonLeft", GUILayout.Width(20)))
              {
                var obj = AssetDatabase.LoadAssetAtPath("Assets/" + scenePathPair.Key + path, typeof(Object));
                Selection.activeObject = obj;
              }

              if (Settings.QuickAccessScenes.Contains(scenePathPair.Key + path))
              {
                if (GUILayout.Button(@"\/", "buttonRight", GUILayout.Width(20)))
                {
                  Settings.QuickAccessScenes.Remove(scenePathPair.Key + path);
                  EditorUtility.SetDirty(Settings);
                }
              }
              else
              {
                if (GUILayout.Button(@"^", "buttonRight", GUILayout.Width(20)))
                {
                  Settings.QuickAccessScenes.Add(scenePathPair.Key + path);
                  EditorUtility.SetDirty(Settings);
                }
              }

              var text = path.Replace(".unity", "");

              if (GUILayout.Button(text, "buttonLeft"))
              {
                if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
                  EditorApplication.OpenScene("Assets/" + scenePathPair.Key + path);
              }
              GUI.backgroundColor = Color.green;

              if (GUILayout.Button("Add", "buttonRight", GUILayout.Width(40)))
              {
                var oldRoots = FindObjectsOfType(typeof(Transform)).Cast<Transform>().Where(t => t.parent == null).ToList();

                EditorApplication.OpenSceneAdditive("Assets/" + scenePathPair.Key + path);

                var newRoots = FindObjectsOfType(typeof(Transform)).Cast<Transform>().Where(t => t.parent == null).ToList();

                foreach (var root in oldRoots)
                  newRoots.Remove(root);

                foreach (var root in newRoots)
                {
                  SetDoNotSaveFlagToHierarchy(root);
                  _roots.Add(root.gameObject);
                }
              }
              GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
          }
        }

        GUILayout.EndVertical();
      }
    }

    private static void ReloadScenes(Boolean repaint)
    {
      _gameScenePaths = new SortedDictionary<String, List<String>>();

      foreach (var path in AssetDatabase.GetAllAssetPaths().Where(p => p.EndsWith(".unity") && p.StartsWith("Assets/Scenes/")))
      {
        var pathToScene = path.Substring("Assets/".Length);
        var index = pathToScene.LastIndexOf('/');
        var sceneName = pathToScene.Substring(index + 1);
        pathToScene = pathToScene.Substring(0, index + 1);

        List<String> scenes;
        if (_gameScenePaths.TryGetValue(pathToScene, out scenes))
          scenes.Add(sceneName);

        else
          _gameScenePaths.Add(pathToScene, new List<String> { sceneName });
      }

      _otherScenePaths = new SortedDictionary<String, List<String>>();

      foreach (var path in AssetDatabase.GetAllAssetPaths().Where(p => p.EndsWith(".unity") && !p.StartsWith("Assets/Scenes/")))
      {
        var pathToScene = path.Substring("Assets/".Length);
        var index = pathToScene.LastIndexOf('/');
        var sceneName = pathToScene.Substring(index + 1);
        pathToScene = pathToScene.Substring(0, index + 1);

        List<String> scenes;
        if (_otherScenePaths.TryGetValue(pathToScene, out scenes))
          scenes.Add(sceneName);

        else
          _otherScenePaths.Add(pathToScene, new List<String> { sceneName });
      }

      if (!repaint)
        return;

      var window = GetWindow(typeof(ProjectScenes));
      window.Repaint();
    }

    private void SetDoNotSaveFlagToHierarchy(Transform root)
    {
      root.gameObject.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
      foreach (Transform child in root)
        SetDoNotSaveFlagToHierarchy(child);
    }
    private void ClearAdditionalRoots()
    {
      foreach (var root in _roots)
        DestroyImmediate(root);

      _roots.Clear();
    }
  }
}