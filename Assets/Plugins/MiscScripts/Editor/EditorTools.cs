using UnityEditor;
using UnityEngine;

public static class EditorTools
{
    //% (ctrl), # (shift), & (alt)
    public const string TOOLS = "Tools/";
    public const string TOOLS_OTHER = TOOLS + "Hotkeys/";
    public const string PLAYER_PREFS = TOOLS + "PlayerPrefs/";

    [MenuItem(TOOLS_OTHER + "ResetTransform &z")]
    private static void ResetTransform()
    {
        foreach (Transform t in Selection.transforms)
        {
            t.localPosition = Vector3.zero;
            t.rotation = Quaternion.identity;
            t.localScale = Vector3.one;
        }
    }

    [MenuItem(TOOLS_OTHER + "Toggle active &d")]
    private static void ToggleActive()
    {
        foreach (GameObject go in Selection.gameObjects)
            go.SetActive(!go.activeSelf);
    }

    [MenuItem(TOOLS_OTHER + "Apply prefab &a")]
    private static void ApplyPrefab()
    {
        var sel = Selection.activeGameObject;

        if (sel != null)
        {
            var parent = PrefabUtility.GetCorrespondingObjectFromSource(sel);
            if (parent != null)
            {
                PrefabUtility.ReplacePrefab(PrefabUtility.FindRootGameObjectWithSameParentPrefab(sel), parent, ReplacePrefabOptions.ConnectToPrefab);
            }
        }
    }

    [MenuItem(PLAYER_PREFS + "Clear PlayerPrefs")]
    private static void ClearPrefs()
    {
      PlayerPrefs.DeleteAll();
    }
}
