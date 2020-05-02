using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SweepAnimationComponent : MonoBehaviour
{
  [SerializeField] private bool StartOnAwake = false;
  [SerializeField] private Image image;
  [SerializeField] [Range(0, 1)] private float imageOpacity = 1;
  [SerializeField] private float progressFrom = -3;
  [SerializeField] private float progressTo = 7;
  [SerializeField] private float progressSpeed = 2;
  [SerializeField] private bool randomDelay = true;

  private float progress;
  private bool Active = false;

  void Awake()
  {

    if (this.StartOnAwake)
    {
      this.Activate();
    }
  }

  public void Activate()
  {
    if (this.Active)
    {
      return;
    }
    this.Active = true;

      if (image == null)
        image = GetComponentInChildren<Image>();

      if (image == null) return;

      progress = progressFrom;
      image.material = new Material(image.material);
      image.material.SetFloat("_ImageOpacity", imageOpacity);

      DOTween.To(x => progress = x, progressFrom, progressTo, progressSpeed)
        .SetDelay(randomDelay ? Random.Range(0f, progressSpeed) : 0)
        .SetSpeedBased(true)
        .SetLoops(-1, LoopType.Restart)
        .SetEase(Ease.Linear)
        .OnUpdate(() => image.material.SetFloat("_Progress", progress));


  
  }
}
