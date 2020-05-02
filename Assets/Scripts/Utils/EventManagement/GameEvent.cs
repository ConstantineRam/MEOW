using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public abstract class GameEvent
{
  private readonly List<EventHandlerData> eventHandlers = new List<EventHandlerData>();

  public bool HasListeners { get { return eventHandlers.Count > 0; } }


  protected void InvokeInternal(params object[] args)
  {
    for (var i = 0; i < eventHandlers.Count; i++)
    {
      EventHandlerData eventHandlerData = eventHandlers[i];
      MethodInfo method = eventHandlerData.method;
      object handle = eventHandlerData.handle;

      if (handle == null || handle.Equals(null))
      {
        eventHandlers.Remove(eventHandlers[i]);
        i--; //так как при удалении все элементы сместятся
        continue;
      }

      if (handle is MonoBehaviour && !((MonoBehaviour) handle).gameObject.activeInHierarchy)
      {
        continue;
      }

      method.Invoke(eventHandlerData.handle, args);
    }
  }

  protected void ListenInternal(MethodInfo method, object handler)
  {
    // не регаем уже зареганные методы
    if (eventHandlers.Exists(e => e.method == method && e.handle == handler)) return;

    eventHandlers.Add(new EventHandlerData(handler, method));
  }




  private class EventHandlerData
  {
    public object handle;
    public MethodInfo method;

    public EventHandlerData(object handle, MethodInfo method)
    {
      this.handle = handle;
      this.method = method;
    }
  }
}
