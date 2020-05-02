using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyButtons
{
    /// <summary>
    /// Custom inspector for Object including derived classes.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class ObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            // Loop through all methods with no parameters
            foreach (var method in target.GetType().GetMethods()
                .Where(m => m.GetParameters().Length == 0))
            {
                // Get the ButtonAttribute on the method (if any)
                var ba = (InspectorButton) Attribute.GetCustomAttribute(method, typeof(InspectorButton));

                if (ba != null)
                {
                    // Determine whether the button should be enabled based on its mode
                    GUI.enabled = ba.mode == ButtonMode.AlwaysEnabled
                        || (EditorApplication.isPlaying ? ba.mode == ButtonMode.EnabledInPlayMode : ba.mode == ButtonMode.DisabledInPlayMode);

                    // Draw a button which invokes the method
                    if (GUILayout.Button(ObjectNames.NicifyVariableName(method.Name)))
                    {
                        foreach (var m_target in targets)
                        {
                            method.Invoke(m_target, null);
                        }
                    }

                    GUI.enabled = true;
                }
            }

            // Draw the rest of the inspector as usual
            DrawDefaultInspector();
        }
    }
}
