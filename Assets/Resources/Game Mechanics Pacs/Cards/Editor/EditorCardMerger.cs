using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardMerger))]
public class EditorCardMerger : Editor
{

  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    EditorGUILayout.LabelField("This holder merge card and invoke 'OnCardMerge' event.");
    
    EditorGUILayout.Space();
    SerializedProperty WaitForCommand = serializedObject.FindProperty("WaitForCommand");
    EditorGUILayout.PropertyField(WaitForCommand, true);

    EditorGUILayout.Space();
    SerializedProperty MyHolders = serializedObject.FindProperty("MyHolders");
    EditorGUILayout.PropertyField(MyHolders, true);

   EditorGUILayout.Space();
    SerializedProperty CardRotationTime = serializedObject.FindProperty("CardRotationTime");
    EditorGUILayout.PropertyField(CardRotationTime, true);

    EditorGUILayout.Space();
    SerializedProperty CheckForMergeDelay = serializedObject.FindProperty("CheckForMergeDelay");
    EditorGUILayout.PropertyField(CheckForMergeDelay, true);

    EditorGUILayout.Space();
    SerializedProperty CardRotationDelay = serializedObject.FindProperty("CardRotationDelay");
    EditorGUILayout.PropertyField(CardRotationDelay, true);

    EditorGUILayout.Space();
    SerializedProperty CardRotation = serializedObject.FindProperty("CardRotation");
    EditorGUILayout.PropertyField(CardRotation, true);

    EditorGUILayout.Space();
    SerializedProperty Poof = serializedObject.FindProperty("Poof");
    EditorGUILayout.PropertyField(Poof, true);
    serializedObject.ApplyModifiedProperties();

  }

}
