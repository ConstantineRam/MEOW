// this UI script should be attached to 2D UI element with RectTransform.
// The purpose of this element is act like a standard button for RPG or roguelike interface with targeting functionalty (could be disabled).
// Once player touches that button and start dragging it shows activation animation, but instead of following player drag it then start drawing a particle line from 
// itself to current position of player drag. Once Player releases drag it calles OnDrop from assigned Object.
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ATargetingButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler //, IPointerClickHandler
{
  [SerializeField]
  private float FloatUpTime = 0.15f;
  [Tooltip("Time that takes this object to return to its initial position.")]
  [SerializeField]
  private float ReturnTime = 0.12f;

  private float YMove  = 0.9f;
  private float UpYmove;
  private float BackYmove;

  private RectTransform myRectTransform;
  private RectTransform RectTransform { get { return this.myRectTransform; } }

  private Vector3 myInitialPosition;
  private Quaternion myInitialRotation;
  private Vector3 myInitialScale;
  private Vector3 myUpRotation;
  private Image myClickImage;
  private List<Tween> tweens;
  //---------------------------------------------------------------------------------------------------------------
  void Awake ()
  {
    this.myRectTransform = this.GetComponent<RectTransform>();
    this.myInitialPosition = this.RectTransform.localPosition;
    this.myInitialRotation = this.RectTransform.localRotation;
    this.myInitialScale = this.RectTransform.localScale;
    this.myUpRotation = new Vector3 (0, 0, 0);
    tweens = new List<Tween>();
    this.UpYmove =  YMove;
    this.BackYmove = this.RectTransform.localPosition.y;

    this.myClickImage = this.GetComponent<Image>();
    if (this.myClickImage == null)
    {
      Debug.LogError("ATargetingButton got null for ClickImage.");
    }
  }
  //---------------------------------------------------------------------------------------------------------------
  private void EnableGrab(bool enable)
  {
    this.myClickImage.raycastTarget = enable;
  }

  #region Drag&Drop
  //---------------------------------------------------------------------------------------------------------------
  public virtual void OnBeginDrag(PointerEventData eventData)
  {
    //  Game.Events.DragStarted.Invoke();
    this.EnableGrab(false);
    tweens.Clear();
    tweens.Add(this.RectTransform.DOLocalMoveY(this.UpYmove, FloatUpTime));
    tweens.Add(this.RectTransform.DORotate(this.myUpRotation, FloatUpTime));
    tweens.Add(this.RectTransform.DOScale (1.2f, FloatUpTime));

  }

  //---------------------------------------------------------------------------------------------------------------
  public virtual void OnDrag(PointerEventData data)
  {
     
  }

  //---------------------------------------------------------------------------------------------------------------
  public virtual void OnEndDrag(PointerEventData eventData)
  {
    Debug.Log("I did drop! Meow!");
    tweens.ForEach(t => t.Kill());
    tweens.Clear();

    this.RectTransform.localRotation = this.myInitialRotation;
    this.RectTransform.localScale = this.myInitialScale;
    tweens.Add(this.RectTransform.DOLocalMoveY(this.BackYmove, this.ReturnTime));
    tweens.Add(this.RectTransform.DOScale(this.myInitialScale, this.ReturnTime));
    Game.TimerManager.Start(this.ReturnTime + 0.2f, () => 
            {
              tweens.ForEach(t => t.Kill());
             
              this.RectTransform.localPosition = this.myInitialPosition;
              this.EnableGrab(true);
            });

  }
  #endregion
}
