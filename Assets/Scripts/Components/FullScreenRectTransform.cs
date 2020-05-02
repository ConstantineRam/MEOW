using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenRectTransform : MonoBehaviour
{

  void Start()
  {
    Canvas canvas = GetComponentInParent<Canvas>();
    if (canvas == null) return;
    RectTransform rectTransform = GetComponent<RectTransform>();
    if (rectTransform == null) return;

    Vector3 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

    rectTransform.position = Vector3.zero;
    rectTransform.sizeDelta = canvasSize;
  }
}