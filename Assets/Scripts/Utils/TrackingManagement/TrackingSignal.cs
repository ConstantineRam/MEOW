public class TrackingSignal : TrackingEvent
{
  public TrackingSignal(string name) : base(name) { }
  public void Send() { SendInternal(); }
}

public class TrackingSignal<T1> : TrackingEvent
{
  public TrackingSignal(string name) : base(name) { }
  public void Send(T1 t) { SendInternal(t); }
}

public class TrackingSignal<T1, T2> : TrackingEvent
{
  public TrackingSignal(string name) : base(name) { }
  public void Send(T1 t1, T2 t2) { SendInternal(t1, t2); }
}