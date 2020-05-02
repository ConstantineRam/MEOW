// This manager allow you to start a timer that will make a callback once done and destroy itself.
// The more important feature, you can assign timer to a timelayer (check TimeScaleProvider for more info), so you can start and stop specific timers simulteneously.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimerManager
{

  private List<Timer> activeTimers = new List<Timer>();

  //---------------------------------------------------------------------------------------------------------------
  public TimerManager(TimeScaleProvider timeScaleProvider)
  {
    List<TimeScaleLayer> layersList = Enum.GetValues(typeof(TimeScaleLayer)).OfType<TimeScaleLayer>().ToList();
    foreach (TimeScaleLayer layer in layersList)
    {
      var localLayer = layer;
      timeScaleProvider.ListenUpdate(layer, scale => Update(localLayer, scale));
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Update(TimeScaleLayer layer, float timeScale)
  {
    activeTimers
      .FindAll(t => t.Layer == layer)
      .ForEach(t => t.Update(timeScale));

    activeTimers.RemoveAll(t => !t.IsWorking);
  }

  //---------------------------------------------------------------------------------------------------------------
  public Timer Start(float time, Action callback, TimeScaleLayer layer = TimeScaleLayer.Common)
  {
    if (time < 0) time = 0;
    Timer timer = new Timer(time, callback, layer);
    activeTimers.Add(timer);

    return timer;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Stop(Timer timer)
  {
    if (timer == null) return;
    activeTimers.FindAll(t => t == timer)
      .ForEach(t => t.Stop());
  }

  //---------------------------------------------------------------------------------------------------------------
  public void HaltAll()
  {
    foreach(Timer t in activeTimers)
    {
      t.Halt();
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void StopAll(TimeScaleLayer layer)
  {
    activeTimers.FindAll(t => t.Layer == layer)
      .ForEach(t => t.Stop());
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Stop(Action callback)
  {
    if (callback == null) return;
    activeTimers.FindAll(t => t.Callback == callback)
      .ForEach(t => t.Stop());
  }

  //---------------------------------------------------------------------------------------------------------------
  public void PauseAll(TimeScaleLayer layer)
  {
    activeTimers.FindAll(t => t.Layer == layer)
      .ForEach(t => t.Pause());
  }

  public void Pause(Timer timer)
  {
    if (timer == null) return;
    activeTimers.FindAll(t => t == timer)
      .ForEach(t => t.Pause());
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Resume(Timer timer)
  {
    if (timer == null) return;
    activeTimers.FindAll(t => t == timer)
      .ForEach(t => t.Resume());
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ResumeAll(TimeScaleLayer layer)
  {
    activeTimers.FindAll(t => t.Layer == layer)
      .ForEach(t => t.Resume());
  }

  //cat's poo
  ////---------------------------------------------------------------------------------------------------------------
  ///// <summary>
  ///// Removes timer from the list of active times, allowing GC to destroy it, once any other instances of it would be removed.
  ///// This method called by Timer itself and you shouldn't call it manually.
  ///// </summary>
  //public void RemoveTimer(Timer timerToRemove)
  //{
  //  if (timerToRemove == null)
  //  {
  //    Debug.LogError("Time Manager got null at timerToRemove.");
  //    return;
  //  }


  //  if (!activeTimers.Contains(timerToRemove))
  //  {
  //    Debug.LogError("Time Manager got timer to remove, but this timer never was/already removed from active timers.");
  //    return;
  //  }

  //  activeTimers.Remove(timerToRemove);
  //}

  //***************************************************************************************************************
  public class Timer
  {
    public Action Callback { get; private set; }
    public bool IsWorking { get; private set; }
    public bool IsPaused { get; private set; }
    public TimeScaleLayer Layer { get; private set; }

    private float initTimeNeeded;
    private float timeNeeded;
    private float timePassed;
    private bool isRepeated;

    public Timer(float time, Action callback, TimeScaleLayer layer)
    {
      initTimeNeeded = timeNeeded = time;
      Callback = callback;

      timePassed = 0;

      IsWorking = true;
      IsPaused = false;
      Layer = layer;
    }

    public Timer AddTime(float time)
    {
      timeNeeded += time;
      return this;
    }

    public Timer SetTime(float time)
    {
      if (time < 0) time = 0;
      timeNeeded = time;
      return this;
    }

    public Timer SetCallBack(Action callback)
    {
      this.Callback = callback;
      return this;
    }

    public Timer SetRepeated(bool value)
    {
      isRepeated = value;
      return this;
    }

    public Timer SetLayer(TimeScaleLayer newLayer)
    {
      Layer = newLayer;
      return this;
    }

    public void Update(float timeScale)
    {
      if (IsPaused || !IsWorking)
      {
        return;
      }

      timePassed += Time.deltaTime * timeScale;

      if (timePassed > timeNeeded)
      {
        if (isRepeated) timeNeeded += initTimeNeeded;
        else Stop();

        if (Callback != null)
          Callback.Invoke();
      }
    }

    public float GetTimeLeft()
    {
      return Mathf.Max(0, timeNeeded - timePassed);
    }

    public void Complete()
    {
      timePassed = timeNeeded;
      Update(1);
    }

    public void Pause()
    {
      IsPaused = true;
    }

    public void Resume()
    {
      IsPaused = false;
    }

    public void Stop()
    {
      if (!IsWorking) return;
      IsWorking = false;
    }

    public void Halt()
    {
      Callback = null;
      this.Stop();
    }

    public void StopAndExecute()
    {
      if (!IsWorking) return;
      if (Callback != null)
      {
        Callback.Invoke();
      }
        
      this.Stop();
    }
    private void RemoveSelf()
    {

    }
  }
}


