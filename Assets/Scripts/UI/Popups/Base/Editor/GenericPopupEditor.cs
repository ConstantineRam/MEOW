using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenericPopup), true)]
public class GenericPopupEditor : Editor
{
  public override void OnInspectorGUI()
  {
    GenericPopup popup = (GenericPopup) target;
    if (GUILayout.Button("Edit"))
    {
      popup.OpenForEdit();
    }

    DrawDefaultInspector();
  }
}