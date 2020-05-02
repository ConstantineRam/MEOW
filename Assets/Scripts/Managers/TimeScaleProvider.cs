using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class TimeScaleProvider
{
  private List<TimeScaleData> datas;

  public TimeScaleProvider()
  {
    datas = new List<TimeScaleData>();

    List <TimeScaleLayer> layersList = Enum.GetValues(typeof(TimeScaleLayer)).OfType<TimeScaleLayer>().ToList();
    foreach (TimeScaleLayer layer in layersList)
    {
      TimeScaleData data = new TimeScaleData(layer);
      datas.Add(data);
    }
  }

  public void Update()
  {
    datas.ForEach(d => d.Invoke());
  }



  public void ListenUpdate(TimeScaleLayer layer, Action<float> action)
  {
    datas.Find(d => d.Layer == layer).Listen(action);
  }

  public void SetScale(TimeScaleLayer layer, float scale, float time = 0)
  {
    datas.Find(d => d.Layer == layer).SetScale(scale, time);
  }

  public float GetScale(TimeScaleLayer layer)
  {
    return datas.Find(d => d.Layer == layer).GetScale();
  }







  private class TimeScaleData
  {
    public TimeScaleLayer Layer { get; private set; }
    public Signal<float> Signal { get; private set; }

    private float scale;
    private Tween tween;


    public TimeScaleData(TimeScaleLayer layer)
    {
      Layer = layer;
      Signal = new Signal<float>();
      scale = 1;
    }

    public void Listen(Action<float> action)
    {
      Signal.Listen(action);
    }

    public void Invoke()
    {
      Signal.Invoke(scale);
    }

    public void SetScale(float newScale, float time)
    {
      tween.Kill();
      if (time == 0)
      {
        scale = newScale;
        return;
      }

      tween = DOTween.To(() => scale, x => scale = x, newScale, time);
    }

    public float GetScale()
    {
      return scale;
    }
  }
}
