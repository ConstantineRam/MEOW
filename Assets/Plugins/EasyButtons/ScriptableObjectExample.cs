using UnityEngine;
using EasyButtons;

[CreateAssetMenu(fileName = "Example.asset", menuName = "New Example ScriptableObject")]
public class ScriptableObjectExample : ScriptableObject
{
    [InspectorButton]
    public void SayHello()
    {
        Debug.Log("Hello");
    }

    [InspectorButton(ButtonMode.DisabledInPlayMode)]
    public void SayHelloEditor()
    {
        Debug.Log("Hello from edit mode");
    }

    [InspectorButton(ButtonMode.EnabledInPlayMode)]
    public void SayHelloPlayMode()
    {
        Debug.Log("Hello from play mode");
    }
}
