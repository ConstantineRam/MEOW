using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils.ExtensionMethods;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadedRadioButton : MonoBehaviour
{
  [SerializeField] private Image activeStateImage;
  //[SerializeField] private float 

  private ScrollRect scrollRect;
  private RectTransform relatedItem;

  // норм. значение (0-1) при котором relatedItem находится по центру вьюПорта
  private float normalizedCenter;

  private float relatedItemWidth;


  public void Init(ScrollRect scrollRect, RectTransform relatedItem)
  {
    gameObject.SetActive(true);

    this.scrollRect = scrollRect;
    this.relatedItem = relatedItem;

    relatedItemWidth = relatedItem.rect.width;

    Canvas.ForceUpdateCanvases();

    scrollRect.content.anchoredPosition =
      (Vector2) scrollRect.transform.InverseTransformPoint(scrollRect.content.position)
      - (Vector2) scrollRect.transform.InverseTransformPoint(relatedItem.position);

    normalizedCenter = scrollRect.normalizedPosition.x;
  }

  public void UpdateFadeValue()
  {
    float value = 0;
    float scrollRectNormPos = Mathf.Clamp01(scrollRect.normalizedPosition.x);

    float normPosDelta = Mathf.Abs(normalizedCenter - scrollRectNormPos);

    normPosDelta = normPosDelta.Remap01(0.2f, 0);
    normPosDelta = Mathf.Clamp01(normPosDelta);
    value = normPosDelta;

    activeStateImage.color = activeStateImage.color.WithAlpha(value);
  }

  public void OnClick()
  {
    scrollRect.DOKill();

    Vector2 valueToMove = (Vector2) scrollRect.transform.InverseTransformPoint(scrollRect.content.position)
                        - (Vector2) scrollRect.transform.InverseTransformPoint(relatedItem.position);
    scrollRect.content.DOAnchorPos(valueToMove, 0.3f);
  }

}
