using UnityEditor;
using UnityEngine;

public class Grid : EditorWindow
{
    public float stepValue = 1;
    private Transform root;
    Vector3 lastPosition = Vector3.zero;
    private bool x = true, y = true, z = true;
    // Use this for initialization
    void OnEnable()
    {

    }

    [MenuItem("Tools/Grid")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(Grid));
    }

    void Update()
    {
        if (Application.isPlaying) return;

        Transform[] selection = Selection.transforms;



        foreach (var select in selection)
        {
            var curPosition = select.position;
            if (x) curPosition.x = GetNearest(curPosition.x, stepValue);
            if (y) curPosition.y = GetNearest(curPosition.y, stepValue);
            if (z) curPosition.z = GetNearest(curPosition.z, stepValue);

            select.position = curPosition;
        }

        if (root == null || lastPosition == root.position) return;
        lastPosition = root.position;
    }

    private float GetNearest(float value, float step)
    {
        value = value - value % step; // остаток от деления
        var checker = step / 2f + value;
        return value >= checker ? value + step : value;
    }

    void OnSelectionChange()
    {
        Transform[] selection = Selection.transforms;
        if (selection.Length == 0)
        {
            root = null;
            return;
        }

        root = selection[0];
        lastPosition = root.position;
    }

    public void OnGUI()
    {
        stepValue = EditorGUILayout.FloatField("Value", stepValue);

        x = EditorGUILayout.Toggle("X", x);
        y = EditorGUILayout.Toggle("Y", y);
        z = EditorGUILayout.Toggle("Z", z);
    }
}
