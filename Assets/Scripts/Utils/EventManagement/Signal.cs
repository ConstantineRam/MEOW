using System;


public class Signal : GameEvent
{
  public void Listen(Action action) { ListenInternal(action.Method, action.Target); }
  public void Invoke() { InvokeInternal(); }
}

public class Signal<T1> : GameEvent
{
  public void Listen(Action<T1> action) { ListenInternal(action.Method, action.Target); }
  public void Invoke(T1 t) { InvokeInternal(t); }
}

public class Signal<T1, T2> : GameEvent
{
  public void Listen(Action<T1, T2> action) { ListenInternal(action.Method, action.Target); }
  public void Invoke(T1 t1, T2 t2) { InvokeInternal(t1, t2); }
}
