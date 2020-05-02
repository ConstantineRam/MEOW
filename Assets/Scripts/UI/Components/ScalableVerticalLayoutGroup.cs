using System.Collections;
using Assets.Scripts.Utils.ExtensionMethods;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/Extensions/Scalable Vertical Layout Group")]
public class ScalableVerticalLayoutGroup : LayoutGroup
{
  [SerializeField]
  protected float spacing = 0;
  [SerializeField]
  protected Vector2 childPivot = new Vector2(0.5f, 0.5f);

  private float _aspectRatio;
  private Coroutine _coroutine;

  protected ScalableVerticalLayoutGroup() { }
  public override void SetLayoutHorizontal() { }

  public override void CalculateLayoutInputVertical()
  {
    float size = padding.vertical;

    foreach (RectTransform child in rectChildren)
    {
      var childSize = child.GetSize().y * child.localScale.y;
      size += childSize;
      size += childSize > 0 ? spacing : 0;
    }

    size -= spacing;

    SetLayoutInputForAxis(size, size, -1, 1);
  }


  public override void SetLayoutVertical()
  {
    SetCellsVertical();
  }

  private void SetCellsVertical()
  {
    for (int i = 0; i < rectChildren.Count; i++)
    {
      RectTransform child = rectChildren[i];
      RectTransform prevChild = rectChildren.HasElement(i - 1) ? rectChildren[i - 1] : null;

      float sizeX = rectTransform.rect.width - padding.horizontal;
      float sizeY = child.GetBoundsWithChildren().size.y;

      child.pivot = childPivot;

      float scaleAndPivotInfluence = child.GetSize().y * (child.localScale.y - 1) * Math2.Remap01(child.pivot.y, 1, 0);

      float positionX = padding.left;
      float positionY = prevChild == null
        ? padding.top + scaleAndPivotInfluence
        : -prevChild.GetBottom() + spacing + scaleAndPivotInfluence;


      SetChildAlongAxis(child, 0, positionX, sizeX);
      SetChildAlongAxis(child, 1, positionY, sizeY);
    }
  }

  public void UpdateGroup()
  {
    LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
  }

  protected override void OnRectTransformDimensionsChange()
  {
    base.OnRectTransformDimensionsChange();

    if (gameObject.activeInHierarchy && _coroutine == null)
    {
      _coroutine = StartCoroutine(RebuildLayoutLazy());
    }
  }

  private IEnumerator RebuildLayoutLazy()
  {
    yield return null;
    _coroutine = null;
    RebuildLayout();
  }

  private void RebuildLayout()
  {
    float newAspectRatio = (float) Screen.width / Screen.height;
    if (_aspectRatio == 0 || _aspectRatio != newAspectRatio)
    {
      LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
      _aspectRatio = newAspectRatio;
    }
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    if (_coroutine != null)
    {
      StopCoroutine(_coroutine);
      _coroutine = null;
    }
  }
}