using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public static class SpineEx
{
  public static void SetProgress(this SkeletonGraphic @this, float progress)
  {
    var animation = @this.Skeleton.Data.FindAnimation(@this.startingAnimation);
    if (animation != null)
    {
      @this.AnimationState.GetCurrent(0).TrackTime = animation.Duration * progress;
    }
  }
}
