using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using UnityEngine;

public class GraphicsTimeScaler : MonoBehaviour
{
  [SerializeField]
  private TimeScaleLayer layer = TimeScaleLayer.Common;

  private List<ParticleSystem> particles = new List<ParticleSystem>();
  private List<SkeletonTimeScalePair> skeletons = new List<SkeletonTimeScalePair>();
  private List<TrailTimeScalePair> trails = new List<TrailTimeScalePair>();


  void Start() { }

  void Awake()
  {
    particles = GetComponentsInChildren<ParticleSystem>().ToList();
    particles.Add(GetComponent<ParticleSystem>());
    particles.RemoveAll(p => p == null);

    var skeletonsTemp = GetComponentsInChildren<SkeletonAnimation>().ToList();
    skeletonsTemp.Add(GetComponent<SkeletonAnimation>());
    skeletonsTemp.RemoveAll(s => s == null);
    skeletonsTemp.ForEach(s => skeletons.Add(new SkeletonTimeScalePair(s, s.timeScale)));

    var trailsTemp = GetComponentsInChildren<TrailRenderer>().ToList();
    trailsTemp.Add(GetComponent<TrailRenderer>());
    trailsTemp.RemoveAll(t => t == null);
    trailsTemp.ForEach(t => trails.Add(new TrailTimeScalePair(t, t.time)));


    Game.TimeScale.ListenUpdate(layer, UpdateAll);
  }

  void UpdateAll(float timeScale)
  {
    particles.ForEach(p => p.Simulate(Time.deltaTime * timeScale, true, false));
    skeletons.ForEach(s => s.skeleton.timeScale = s.initTimeScale * timeScale);
    trails.ForEach(s => s.trail.time = s.initTime * Math2.Remap(timeScale, 0, 1, 100, 1));
  }



  private class SkeletonTimeScalePair
  {
    public SkeletonAnimation skeleton;
    public float initTimeScale;

    public SkeletonTimeScalePair(SkeletonAnimation skeleton, float time)
    {
      this.skeleton = skeleton;
      initTimeScale = time;
    }
  }

  private class TrailTimeScalePair
  {
    public TrailRenderer trail;
    public float initTime;

    public TrailTimeScalePair(TrailRenderer trail, float time)
    {
      this.trail = trail;
      initTime = time;
    }
  }
}
