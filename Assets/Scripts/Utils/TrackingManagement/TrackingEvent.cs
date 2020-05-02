using System.Collections.Generic;
using UnityEngine.Analytics;

public abstract class TrackingEvent
{

  public string Name { get; private set; }

  public TrackingEvent(string name)
  {
    Name = name;
  }

  protected void SendInternal(params object[] args)
  {
    
  }
}
