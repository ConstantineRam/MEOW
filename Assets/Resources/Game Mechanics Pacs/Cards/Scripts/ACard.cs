using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ACard : APoolable, IBeginDragHandler, IDragHandler, IEndDragHandler
{

  //[SerializeField]
  private CardGraphics cardGraphics = new CardGraphics();
  protected CardGraphics CardGraphics { get { return this.cardGraphics; } }

  [Header("-----Don't edit fields below.-----")]
  [SerializeField]
  private GameObject myCoverObject;
  [SerializeField]
  private GameObject myBorderObject;
  [SerializeField]
  private GameObject myFrontImageObject;
  [SerializeField]
  private GameObject myBackImageObject;
  [SerializeField]
  private GameObject myClickImageObject;

  private Image myCover;
  private Image myBorder;
  private Image myFrontImage;
  private Image myBackImage;
  private CanvasGroup myCanvas;
  private Image myClickImage;
  private LayoutElement myLayoutElement;
  

  private bool freshCardInBlock; 
  /// <summary>
  /// Cards inside the Block may be moved around by block, but should be reported only once.
  /// </summary>
  public bool FreshCardInBlock { get { return freshCardInBlock; } set { freshCardInBlock = value; } }

  //-- Internal variables --
  protected RectTransform cardRectTransform;
  public RectTransform GetCardRectTransform { get { return cardRectTransform; } }
  protected ACardData myCardData;
  public ACardData GetCardData { get { return myCardData; } }
  protected ACardData SetCardData { set { myCardData = value; } }

  private CardState cardState = CardState.FaceUp;
  public CardState GetCardState { get { return cardState; } }

  protected CardParent myCurrentParent;
  public bool HasParent { get { if (this.myCurrentParent == null) { return false; } return true; } }

  private bool isInitiated = false;


  //---------------------------------------------------------------------------------------------------------------
  public override void OnCreate()
  {
    this.cardRectTransform = this.GetComponent<RectTransform>();


    // CardGraphics cardGraphics = new CardGraphics();
    this.myBackImage = this.myBackImageObject.GetComponent<Image>();
    this.myFrontImage = this.myFrontImageObject.GetComponent<Image>();
    this.myCover = this.myCoverObject.GetComponent<Image>();
    this.myBorder = this.myBorderObject.GetComponent<Image>();


    this.myCanvas = this.GetComponent<CanvasGroup>();
    if (this.myCanvas == null)
    {
      Debug.LogError("ACard got null for myCanvas.");
    }
    this.myClickImage = this.myClickImageObject.GetComponent<Image>();
    if (this.myClickImage == null)
    {
      Debug.LogError("ACard got null for ClickImage.");
    }
    this.myLayoutElement = this.GetComponent<LayoutElement>();
    if (this.myLayoutElement == null)
    {
      Debug.LogError("ACard got null for myLayoutElement.");
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void OnPop()
  {
    this.gameObject.SetActive(true);

    this.cardRectTransform.DOScale(1f, 0.0f);
    this.myBorder.enabled = true;
    this.myBackImage.enabled = true;
    this.FreshCardInBlock = true;

  }

  //---------------------------------------------------------------------------------------------------------------
  void Awake ()
  {
    // don't use it. This Object is Poolable
   
  }
  
  //---------------------------------------------------------------------------------------------------------------
    protected RectTransform GetParent()
  {
    return this.cardRectTransform.parent as RectTransform;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool IsParent(RectTransform checkTransform)
  {
    if (checkTransform == null)
    {
      return false;
    }

    if (this.cardRectTransform.parent == checkTransform)
    {
      return true;
    }

    return false;
  }

  
  //---------------------------------------------------------------------------------------------------------------
  public override void OnReturnedToPool()
  {
    if (this.HasParent)
    {
      this.myCurrentParent.OnLeftParent(this);
    }
    this.transform.SetParent(null);
    this.myCurrentParent = null;

    this.myCardData = null;


  }

  //---------------------------------------------------------------------------------------------------------------
  public bool GetGrabState()
  {
    return this.myClickImage.raycastTarget;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void MoveToParent(bool RightNow = false)
  {
    bool savedGrab = this.GetGrabState();
   // this.EnableGrab(false);
    if (!this.HasParent)
    {
      return;
    }

    if (RightNow)
    {
      this.ForceParentPosition();
      return;
    }

    Vector3 moveCoords = new Vector3(this.GetParent().position.x, this.GetParent().position.y, this.GetParent().position.z);
    this.cardRectTransform.DOMove(moveCoords, this.OurManager().CardFlyTime);
    Game.TimerManager.Start(this.OurManager().CardFlyTime+0.01f, () =>
    {
     // this.EnableGrab(savedGrab);
      this.ForceParentPosition();
    });
  }
  //---------------------------------------------------------------------------------------------------------------
  // If we need to rework card data to custom one.
  protected virtual void OnInitiate()
  {

  }

  //---------------------------------------------------------------------------------------------------------------
  public void Intiate(ACardData cardData, CardState initialState = CardState.FaceUp)
  {
    if (cardData == null)
    {
      Debug.LogError("Acard; Card initiate was called, but no actual card data was sent.");
      return;
    }

    this.cardRectTransform = this.GetComponent<RectTransform>();

    if (cardData == null)
    {
      Debug.LogError("ABasicCard; Card initiate was called, but no actual card data was sent.");
      return;
    }

    this.myCardData = cardData;

    this.isInitiated = true;

    this.OnInitiate();

    CardGraphics newCardGraphics = cardData.GetGraphicData();

    this.cardGraphics.BackImage = newCardGraphics.BackImage;
    if (this.cardGraphics.BackImage == null)
    {
      this.myBackImage.enabled = false; 
    }

    this.cardGraphics.Border = newCardGraphics.Border;
    this.cardGraphics.Cover = newCardGraphics.Cover;
    this.cardGraphics.FrontImage = newCardGraphics.FrontImage;


    if (this.cardGraphics.Cover == null)
    {
      this.cardGraphics.Cover = Game.ActionRoot.CardManager.GetDefaultCover();
    }

    if (this.cardGraphics.Border == null)
    {
      this.cardGraphics.Border = Game.ActionRoot.CardManager.GetDefaultBorder();
      if (this.cardGraphics.Border == null)
      {
        this.myBorder.enabled = false;
      }
    }

    

    this.SetState(initialState);

    this.ChangeArtisticRotation();
  }

  //---------------------------------------------------------------------------------------------------------------
  protected CardManager OurManager()
  {
    return Game.ActionRoot.CardManager;
  }

  //---------------------------------------------------------------------------------------------------------------
  public CardTypes GetCardType()
  {
    return this.myCardData.GetCardType();
  }


  //---------------------------------------------------------------------------------------------------------------
  protected void SetCardType(CardTypes newType)
  {
    this.myCardData.SetcCardType = newType;
  }


  //---------------------------------------------------------------------------------------------------------------
  private void ApplyGraphics()
  {


    this.myBackImage.sprite = this.cardGraphics.BackImage;
    this.myFrontImage.sprite = this.cardGraphics.FrontImage;
    this.myCover.sprite = this.cardGraphics.Cover;
    this.myBorder.sprite = this.cardGraphics.Border;

  }

  //---------------------------------------------------------------------------------------------------------------
  public void AttachTo(RectTransform newParent)
  {
    this.cardRectTransform.SetParent(newParent);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Flip()
  {
    
    Vector3 rotationAngle = new Vector3(0, 90f, 0);
    this.cardRectTransform.DOScale(this.OurManager().GetFlipZoom, 0.1f);


    this.cardRectTransform.DORotate(rotationAngle, this.OurManager().GetFlipTimeHalf, RotateMode.Fast);

    Game.TimerManager.Start(this.OurManager().GetFlipTimeHalf, () =>
    {
      if (this.cardState == CardState.CoverUp)
      {
        this.SetState(CardState.FaceUp);
      }
      else
      {
        this.SetState(CardState.CoverUp);
      }
      
      Game.TimerManager.Start(0.1f, () =>
      {
        Vector3 rotationAngleBack = new Vector3(0, 0, 0);
        this.cardRectTransform.DORotate(rotationAngleBack, this.OurManager().GetFlipTimeHalf);
        Game.TimerManager.Start(this.OurManager().GetFlipTimeHalf, () =>
        {
          this.cardRectTransform.DOScale(1f, 0.1f);

        });
      });

    });

    this.ChangeArtisticRotation();
  }
  //---------------------------------------------------------------------------------------------------------------
  public void ChangeArtisticRotation()
  {
    if (!this.OurManager().GetAllowArtisticRotation)
    {
      return;
    }

    float ArtisticRotation = UnityEngine.Random.Range(0f, 4.0f);
    if (UnityEngine.Random.Range(0, 100) > 50)
    {
      ArtisticRotation = ArtisticRotation * -1;
    }
    Vector3 newRotation = new Vector3(0, 0, ArtisticRotation);

    this.cardRectTransform.DORotate(newRotation, 0.01f);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ClearArtisticRotation()
  {
    Vector3 newRotation = new Vector3(0, 0, 0);

    this.cardRectTransform.DORotate(newRotation, 0.01f);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ArtisticPilePush(int pushPower)
  {
    this.cardRectTransform.DOLocalMoveY((pushPower * 3.6f), 0.001f);

    float ArtisticX = UnityEngine.Random.Range(0f, 2.6f);
    if (UnityEngine.Random.Range(0, 100) > 50)
    {
      ArtisticX = ArtisticX * -1;
    }
    Vector3 newRotation = new Vector3(0, 0, ArtisticX);
    this.cardRectTransform.DOLocalMoveX(ArtisticX, 0.001f);
  }


  //---------------------------------------------------------------------------------------------------------------
  // obsolette
  //public void SetParentForced(CardParent newParent)
  //{
  //  if (HasParent)
  //  {
  //    this.myCurrentParent.OnLeftParent(this);
  //  }

  //  this.myCurrentParent = newParent;
  //  this.myLayoutElement.ignoreLayout = true;
  //  this.cardRectTransform.SetParent(newParent.GetRectTransform);
  //}

  //---------------------------------------------------------------------------------------------------------------
  public void SetParent(CardParent newParent)
  {
    if (this.cardRectTransform.parent == newParent)
    {
      return;
    }

    if (HasParent)
    {
      this.myCurrentParent.OnLeftParent(this);
    }

    this.myCurrentParent = newParent;
    this.myLayoutElement.ignoreLayout = true;
    this.cardRectTransform.SetParent(newParent.GetRectTransform, true);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ForceParentPosition()
  {
    this.transform.localPosition = new Vector3 (0,0,0);
  }
  //---------------------------------------------------------------------------------------------------------------
  protected virtual void OnFaceUpState()
  {

  }
  //---------------------------------------------------------------------------------------------------------------
  protected virtual void OnCoverUpState()
  {

  }

  //---------------------------------------------------------------------------------------------------------------
  public void SetState(CardState newState)
  {
    this.cardState = newState;

    if (this.cardState == CardState.CoverUp)
    {
      
      this.myCoverObject.SetActive(true);
      this.myBorderObject.SetActive(false);
      this.myFrontImageObject.SetActive(false);
      this.myBackImageObject.SetActive(false);

      this.OnCoverUpState();
      this.ApplyGraphics();
      return;
    }

    this.myCoverObject.SetActive(false);
    this.myBorderObject.SetActive(true);
    this.myFrontImageObject.SetActive(true);
    this.myBackImageObject.SetActive(true);
    this.OnFaceUpState();
    this.ApplyGraphics();
  }


  #region Drag&Drop
  //---------------------------------------------------------------------------------------------------------------
  public void OnBeginDrag(PointerEventData eventData)
  {
    Game.Events.DragStarted.Invoke();
    this.ClearArtisticRotation();
    this.cardRectTransform.SetAsLastSibling();

    if (Game.ActionRoot.CardManager.IsCanvasOverlay)
    {
      this.cardRectTransform.position = Input.mousePosition;
    }
    else
    {
      Vector3 screenPoint = Input.mousePosition;
      screenPoint.z = 100.0f;
      this.cardRectTransform.position = Game.ActionRoot.Camera.ScreenToWorldPoint(screenPoint);
    }
    
    
    this.GetComponent<CanvasGroup>().blocksRaycasts = false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void EnableGrab(bool enable)
  {
    this.myClickImage.raycastTarget = enable;
  }

    //---------------------------------------------------------------------------------------------------------------
    public void OnDrag(PointerEventData data)
  {

    if (Game.ActionRoot.CardManager.IsCanvasOverlay)
    {
      this.cardRectTransform.position = Input.mousePosition;
    }
    else
    {
      Vector3 screenPoint = Input.mousePosition;
      screenPoint.z = 100.0f;
      this.cardRectTransform.position = Game.ActionRoot.Camera.ScreenToWorldPoint(screenPoint);
    }

    Game.Events.DragedOver.Invoke(this.cardRectTransform.position.x, this.cardRectTransform.position.y); //(data.pointerCurrentRaycast.screenPosition.x, data.pointerCurrentRaycast.screenPosition.y);//(data.pointerCurrentRaycast.worldPosition.x, data.pointerCurrentRaycast.worldPosition.y); //(this.cardRectTransform.position.x, this.cardRectTransform.position.y);

  }



  //---------------------------------------------------------------------------------------------------------------
  public void OnEndDrag(PointerEventData eventData)
  {
    Game.Events.DragEnded.Invoke();
    this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    this.ChangeArtisticRotation();
    Game.TimerManager.Start(0.1f, () =>
    {
      this.MoveToParent();
    });

   }
  #endregion
}


public enum CardState
{
  FaceUp = 0,
  CoverUp = 1
}
