using EasyButtons;
using UnityEngine;

public class ButtonsExample : MonoBehaviour
{
    // Example use of the ButtonAttribute
    [InspectorButton]
    public void SayMyName()
    {
        Debug.Log(name);
    }

    // Example use of the ButtonAttribute that is not shown in play mode
    [InspectorButton(ButtonMode.DisabledInPlayMode)]
    public void SayHelloEditor()
    {
        Debug.Log("Hello from edit mode");
    }

    // Example use of the ButtonAttribute that is only shown in play mode
    [InspectorButton(ButtonMode.EnabledInPlayMode)]
    public void SayHelloInRuntime()
    {
        Debug.Log("Hello from play mode");
    }

}
