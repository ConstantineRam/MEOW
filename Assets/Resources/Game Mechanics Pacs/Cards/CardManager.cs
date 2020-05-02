//---------------------------------------------------------------------------------------------------------------
// This is card controlling script
// Attach it to the action root that will use cards or remove if not needed.
//---------------------------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
  [Header("Default values are used for all cards that don't have specified Sprites.")]
  [SerializeField]
  private Sprite DefaultCover;
  [SerializeField]
  private Sprite DefaultBorder;

  

  [SerializeField]
  private float cardFlyTime = 0.3f;
  public float CardFlyTime { get { return cardFlyTime; } }
  [SerializeField]
  private float flipZoom = 1.2f;
  public float GetFlipZoom { get { return flipZoom; } }
  [SerializeField]
  private float flipTime = 0.8f;
  public float GetFlipTimeFull { get { return flipTime; } }
  private float flipTimeHalf;
  public float GetFlipTimeHalf { get { return flipTimeHalf; } }
  [SerializeField]
  private Canvas canvas;
  public float GetScaleFactor { get { return canvas.scaleFactor; } }
  public bool IsCanvasOverlay { get { if (this.canvas.renderMode == RenderMode.ScreenSpaceOverlay) { return true; } return false;  } }

  [Header("If true places cards in a bit innacurate manner.")]
  [SerializeField]
  private bool allowArtisticRotation = false;
  public bool GetAllowArtisticRotation { get { return allowArtisticRotation; } }

  [Tooltip("Set an Object that will handle cards dispose.")]
  [SerializeField]
  private CardDisposer cardDisposer;
//  [SerializeField]
 // private GameObject cardPrefab;
//  [SerializeField]
  private CardStorage cardStorage;
  public CardStorage CardStorage { get { return cardStorage; } }


  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
    this.cardStorage = GameData.CardStorage;
  }



  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {
    this.flipTimeHalf = this.flipTime / 2;

    if (this.cardDisposer == null)
    {
      Debug.LogError("Card disposer was't set!");
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void DisposeCard(ACard disposableCard)
  {
    this.cardDisposer.Dispose(disposableCard);
  }

  //---------------------------------------------------------------------------------------------------------------
  public ACardData GetCardDataByType(CardTypes cardType)
  {
    return this.cardStorage.GetCardDataByType(cardType);
  }

  //---------------------------------------------------------------------------------------------------------------
  public ACard CreateACard(CardTypes cardType, CardHolderController cardHolder, CardState initialState = CardState.FaceUp)
  {
    return this.CreateACard(this.GetCardDataByType(cardType), cardHolder, initialState);
  }

  //---------------------------------------------------------------------------------------------------------------
  public ACard CreateACard(ACardData cardType, CardHolderController cardHolder, CardState initialState = CardState.FaceUp)
  {
    if (cardHolder == null)
    {
      Debug.LogError("Card Manager. CreateACard got null at card holder; Card can't be created without a card holder.");
      return null;
    }

    if (cardType == null)
    {
      Debug.LogError("Card Manager. CReateACard got null at cardTyper;");
      return null;
    }

    ACard newCard = Game.PoolManager.Pop(ObjectPoolName.ACard) as ACard;
    if (newCard == null)
    {
      Debug.LogError("Ceate ACard can't pop card prefab from a pool.");
      return null;
    }
    newCard.gameObject.SetActive(true);
    newCard.EnableGrab(cardHolder.AllowGrabCards);
    newCard.Intiate(cardType, initialState);


    cardHolder.PushCard(newCard, true);
    return newCard;
  }

  //---------------------------------------------------------------------------------------------------------------
  public ACard CreateACard(CardTypes cardType, Deck deck)
  {
    return this.CreateACard(this.GetCardDataByType(cardType), deck);
  }

  //---------------------------------------------------------------------------------------------------------------
  public ACard CreateACard(ACardData cardType, Deck deck)
  {
    if (deck == null)
    {
      Debug.LogError("Card Manager. CreateACard got null at deck; Card can't be created without a deck.");
      return null;
    }

    if (cardType == null)
    {
      Debug.LogError("Card Manager. CReateACard got null at cardTyper;");
      return null;
    }

    ACard newCard = Game.PoolManager.Pop(ObjectPoolName.ACard) as ACard;
    if (newCard == null)
    {
      Debug.LogError("Ceate ACard can't pop card prefab from pool.");
      return null;
    }
    newCard.gameObject.SetActive(true);
    newCard.EnableGrab(false);
    newCard.Intiate(cardType, CardState.CoverUp);
    deck.PushCard(newCard, true);

    return newCard;
  }
  //---------------------------------------------------------------------------------------------------------------
  public void RefreshCardholders()
  {
    //foreach (CardHolderController holder in this.cardHolders)
    //{
    //  holder.Refresh();
    //}
            
  }

  //---------------------------------------------------------------------------------------------------------------
  public Sprite GetDefaultCover()
  {
    return this.DefaultCover;
  }

  public Sprite GetDefaultBorder()
  {
    return this.DefaultBorder;
  }


}
