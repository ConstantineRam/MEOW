using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ACatCard : ACard
{

  [SerializeField]
  private Text cardPowerText;
  protected Text CardPowerText { get { return cardPowerText; } }

  private CardTypes[] Mods;

  [SerializeField]
  private Image[] modImages;

  [SerializeField]
  private Image TypeImage;

 // [SerializeField]
 // private CardTYpeImageStorage cardTypeImageStorage;

  private Color EmptyColor;
  private Color FullColor;

  private const int MAX_BLACK_CARD = -10;

  //---------------------------------------------------------------------------------------------------------------
  private CardTypes GetCardTypeForMod(CardFeatureRequirment BlackCardRule)
  {
    CardTypes LastPossibleMod = CardTypes.LastBlack;
    if (BlackCardRule == CardFeatureRequirment.disabled)
    {
      LastPossibleMod = CardTypes.LastType;
    }

    if (BlackCardRule == CardFeatureRequirment.forced)
    {
      return CardTypes.Black;
    }

    return (CardTypes) UnityEngine.Random.Range((int) CardTypes.FirstType, (int) LastPossibleMod + 1);
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// If Paramentres enforce the bonus, it would be added regardless of amount requirments.
  /// If Black cards are fobidded bonuses will not have black card bonuses.
  /// If Black cards are enforces, they will have only black card bonuses.
  /// Set amount to zero if you are enforcing stuff.
  /// </summary>
  /// <param name="amount"></param>
  /// <param name="SideBonusRule"></param>
  /// <param name="JokerRule"></param>
  /// <param name="BlackCardRule"></param>
  public void AddCardMods(int amount, CardFeatureRequirment SideBonusRule = CardFeatureRequirment.enabled, CardFeatureRequirment JokerRule = CardFeatureRequirment.enabled, CardFeatureRequirment BlackCardRule = CardFeatureRequirment.enabled)
  {
    if (JokerRule == CardFeatureRequirment.disabled && SideBonusRule == CardFeatureRequirment.disabled)
    {
      return;
    }

    // some mods could be obligatory due to tutorial requirments.
    if (SideBonusRule == CardFeatureRequirment.forced)
    {
      CardTypes CardType = this.GetCardTypeForMod(BlackCardRule);
      CardMod cardMod = CardMod.left;
      if (UnityEngine.Random.Range(0, 100) > 50)
      {
        cardMod = CardMod.right;
      }

      this.AddMod(CardType, cardMod);
    }

    if (JokerRule == CardFeatureRequirment.forced)
    {
      CardTypes CardType = this.GetCardTypeForMod(BlackCardRule);
      CardMod cardMod = CardMod.joker;

      this.AddMod(CardType, cardMod);
    }

    if (amount < 0)
    {
      return;
    }

    for (int i = 0; i < amount; i++)
    {
      CardMod newMod = this.GetRandomModCardHasNot(SideBonusRule, JokerRule);
      if (newMod == CardMod.None)
      {
        return;
      }
      CardTypes CardType = this.GetCardTypeForMod(BlackCardRule);
      this.AddMod(CardType, newMod);
    }

  }
  //---------------------------------------------------------------------------------------------------------------
  private CardMod GetRandomModCardHasNot(CardFeatureRequirment SideBonusRule, CardFeatureRequirment JokerRule )
  {
    List<CardMod> freeSlots = new List<CardMod>();
    for (int i = 0; i < (int) CardMod.last+1; i++)
    {
      if (JokerRule == CardFeatureRequirment.disabled)
      {
        if ((CardMod) i == CardMod.joker)
        {
          continue;
        }
      }

        if (SideBonusRule == CardFeatureRequirment.disabled)
      {
        if ((CardMod) i == CardMod.left)
        {
          continue;
        }

        if ((CardMod) i == CardMod.right)
        {
          continue;
        }
      }

      if (this.Mods[i] == CardTypes.NoType)
      {
        freeSlots.Add((CardMod) i);
      }
    }

    if (freeSlots.Count < 1)
    {
      return CardMod.None;
    }

    return freeSlots[UnityEngine.Random.Range(0, freeSlots.Count)];
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasMod(CardMod type, CardTypes CardColor)

  {
    if (CardColor == CardTypes.NoType)
    {
      Debug.LogError("No type was requested");
      return false;
    }

    if (this.Mods[(int) type] == CardColor)
    {
      return true;
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasMod(CardMod type)

  {

    if (this.Mods[(int) type] == CardTypes.NoType)
    {
      return false;
    }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasMod()
  {
    for (int i = 0; i < (int) CardMod.last + 1; i++)
    {
      if (this.Mods[i] != CardTypes.NoType)
      {
        return true;
      }
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void AddMod(CardTypes type, CardMod mod)
  {
    if (this.GetCardType() == CardTypes.Black)
    {
      Debug.Log("attempt to add mod to a black card.");
      return;
    }


    if (type == CardTypes.NoType)
    {
      Debug.LogError("attempt to set NoType as bonus.");
      return;
    }

    this.Mods[(int) mod] = type;
    this.modImages[(int) mod].sprite = GameData.CardStorage.CardModsSpriteStorage.GetSprite(type, mod);

    if (this.GetCardState == CardState.FaceUp)
    {
      this.modImages[(int) mod].color = this.FullColor;
    }
    else
    {
      this.modImages[(int) mod].color = this.EmptyColor;
    }
    
    
  }


  //---------------------------------------------------------------------------------------------------------------
  public void PushPoints(int NewPoints)
  {
    if (NewPoints <= 0)
    {
      Debug.LogError("Attempt to push negative amount af points!");
      return;
    }
    if (this.GetCardType() == CardTypes.Black)
    {
      if (this.GetPoints() == 0)
      {
        return;
      }

      if (this.GetPoints() + NewPoints > 0) // we don't want black cards to go positive.
      {
        this.SetPoints(0);
        return;
      }
    }
    this.SetPoints(this.GetPoints() + NewPoints);



    FloatingMsg newMsg = Game.PoolManager.Pop(ObjectPoolName.AFlotingMsg) as FloatingMsg;
    newMsg.Show("+"+ NewPoints.ToString(), Color.white, this.gameObject);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ScaleBump()
  {
    this.GetCardRectTransform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack).SetAutoKill();
  }


  //---------------------------------------------------------------------------------------------------------------
  public CardTypes GetMod(CardMod type)

  {

    return this.Mods[(int) type];
    
  }

  //---------------------------------------------------------------------------------------------------------------
  public int GetPoints()
  {
    return this.GetCatCardData().Points;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void SetPoints(int newPoints)
  {
    ACAtCardData data = this.GetCatCardData();
    data.SetPoints = newPoints;

    if (this.GetCardState == CardState.FaceUp)
    {
      this.cardPowerText.text = newPoints.ToString();
    }
    
  }

  //---------------------------------------------------------------------------------------------------------------
  protected ACAtCardData GetCatCardData()
  {
    return (ACAtCardData) this.GetCardData;
  }

  //---------------------------------------------------------------------------------------------------------------
  protected sealed override void OnFaceUpState()
  {


    this.CardPowerText.text = this.GetCatCardData().Points.ToString();
    for (int i = 0; i < (int) CardMod.last + 1; i++)
    {
      if (this.Mods[i] == CardTypes.NoType)
      {
        continue;
      }

      this.modImages[i].color = this.FullColor; 
    }
    this.TypeImage.color = this.FullColor;
  }
  //---------------------------------------------------------------------------------------------------------------
  protected sealed override void OnCoverUpState()
  {
    this.CardPowerText.text = "";
    for (int i = 0; i < (int) CardMod.last + 1; i++)
    {
      this.modImages[i].color = this.EmptyColor;

    }
    this.TypeImage.color = this.EmptyColor;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void SetTypeImage()
  {

    this.TypeImage.sprite = GameData.CardStorage.CardTYpeImageStorage.GetSprite(this.GetCardType());
  }

  //---------------------------------------------------------------------------------------------------------------
  protected sealed override void OnInitiate()
  {
    this.Clone((ACAtCardData) this.GetCardData);
    this.EmptyColor = new Color(0,0,0,0);
    this.FullColor = new Color(1, 1, 1, 1);

    this.Mods = new CardTypes[(int) CardMod.last+1];
    for (int i = 0; i < (int) CardMod.last + 1; i++)
    {
      this.Mods[i] = CardTypes.NoType;
    }

    for (int i = 0; i < (int) CardMod.last + 1; i++)
    {
      this.modImages[i].sprite = null;
      this.modImages[i].color = this.EmptyColor;
    }

    this.SetTypeImage();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Clone(ACAtCardData dataToCLone)
  {
    if (dataToCLone.GetGraphicData() == null)
    {
      Debug.LogError("Unexpected Error, graphic data to clone == null");
      return;
    }

    ACAtCardData newCardData = new ACAtCardData();
    CardGraphics graph = new CardGraphics();
    newCardData.SetCardGraphics = graph;

    newCardData.SetPoints = dataToCLone.Points;

    graph.BackImage = dataToCLone.GetGraphicData().BackImage;
    graph.Border = dataToCLone.GetGraphicData().Border;
    graph.Cover = dataToCLone.GetGraphicData().Cover;
    graph.FrontImage = dataToCLone.GetGraphicData().FrontImage;


    newCardData.SetCardName = dataToCLone.GetCardName;
    newCardData.SetcCardType = dataToCLone.GetCardType();
    newCardData.SetPoints = dataToCLone.Points;

    this.SetCardData = newCardData;


  }
  //---------------------------------------------------------------------------------------------------------------
  public void Spoil()
  {
    if (this.GetPoints() == 0)
    {
      this.SetPoints(1);
    }

    CardGraphics catCardGraph = this.CardGraphics;
    SpoiledCardsImages.SpoilResult result = GameData.CardStorage.SpoiledCardsImages.GetRandom();
    catCardGraph.FrontImage = result.bitmap;
    this.SetCardType(CardTypes.Black);
    this.SetPoints(this.GetPoints() * -1);
    //this.SetPoints( System.Math.Max( this.GetPoints() * -1, MAX_BLACK_CARD) );
    this.SetTypeImage();
  }
}

public enum CardMod
{
  None = -1,
  first = 0,
  left = 0,
  right = 1,
  joker = 2,
  last = 2
}
