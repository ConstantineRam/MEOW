using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HolderInsideBlock))]
public class EditorHolderInsideBlock : Editor
{

  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    EditorGUILayout.LabelField("This type of holder can only accept cards.");
    EditorGUILayout.LabelField("Also it needs Image to accept raycast.");

    serializedObject.ApplyModifiedProperties();

  }

}
