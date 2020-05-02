using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RotationEffect : MonoBehaviour
{
  private Image glow;
  private Tween glowTween;

  private void Awake()
  {
    this.glow = this.GetComponent<Image>();


  }

  void Start ()
  {
    Vector3 rotation = new Vector3(0, 0, 180);
    glowTween = glow.transform.DORotate(rotation, 180f, RotateMode.Fast).SetLoops(-1);
  }

  private void OnDestroy()
  {
    this.glowTween.Kill();
  }
}
