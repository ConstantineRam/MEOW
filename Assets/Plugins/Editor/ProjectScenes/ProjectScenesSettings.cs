using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Utility.Editor
{
  public class ProjectScenesSettings : ScriptableObject
  {
    [SerializeField]
    private List<String> _quickAccessScenes = new List<String>();
    [SerializeField]
    private Boolean _showOtherScenes;
    [SerializeField]
    private Boolean _showProjectScenes = true;

    public List<String> QuickAccessScenes
    {
      get
      {
        return _quickAccessScenes;
      }
    }
    public Boolean ShowOtherScenes
    {
      get
      {
        return _showOtherScenes;
      }
      set
      {
        if (_showOtherScenes != value)
          EditorUtility.SetDirty(this);

        _showOtherScenes = value;
      }
    }
    public Boolean ShowProjectScenes
    {
      get
      {
        return _showProjectScenes;
      }
      set
      {
        if (_showProjectScenes != value)
          EditorUtility.SetDirty(this);

        _showProjectScenes = value;
      }
    }
  }
}