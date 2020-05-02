using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardStorage : MonoBehaviour
{
  [SerializeField]
  // private ACardData[] cardStorage;
  private ACAtCardData[] cardStorage;

  public SpoiledCardsImages SpoiledCardsImages { get; private set; }
  public CardModsSpriteStorage CardModsSpriteStorage { get; private set; }
  public CardTYpeImageStorage CardTYpeImageStorage { get; private set; }

  //---------------------------------------------------------------------------------------------------------------
  void Start ()
  {
    this.SpoiledCardsImages = this.GetComponentInChildren<SpoiledCardsImages>();
    this.CardModsSpriteStorage = this.GetComponentInChildren<CardModsSpriteStorage>();
    this.CardTYpeImageStorage = this.GetComponentInChildren<CardTYpeImageStorage>();
    if (cardStorage.Length < 1)
    {
      Debug.Log("No Cards in card storage.");
    }
	}

  //---------------------------------------------------------------------------------------------------------------
  public List<ACAtCardData> GetDataForMagicCatLevel(int CardLevel)
  {





    List<ACAtCardData> found = new List<ACAtCardData>();

    foreach (ACardData cardTypeCounter in cardStorage)
    {

        ACAtCardData catC = (ACAtCardData) cardTypeCounter;
        if (catC.UnlocksAt == CardLevel)
        {
          found.Add(catC);
        }
    }


    if (found.Count < 1)
    {
      Debug.LogError("Card storage has no cards unlocked at " + CardLevel);
    }


    return found;
  }

  //---------------------------------------------------------------------------------------------------------------
  public ACAtCardData GetAvaliableDataByType(CardTypes cardType)
  {
    if (cardType == CardTypes.NoType || cardType == CardTypes.Black)
    {
      Debug.LogError("Card storage has no cards of type " + cardType + ".");
      return null;
    }


    int UnlockedCats = Game.ActionRoot.UniqueKotanStorage.GetTopUnlockedKotanPosition();

    List<ACAtCardData> found = new List<ACAtCardData>();

    foreach (ACardData cardTypeCounter in cardStorage)
    {
      if (cardTypeCounter.GetCardType() == cardType)
      {
        ACAtCardData catC = (ACAtCardData) cardTypeCounter;
        if (catC.UnlocksAt <= UnlockedCats)
        {
          found.Add(catC);
        }
      }
    }


    if (found.Count < 1)
    {
      Debug.LogError("Card storage has no cards of type " + cardType + " unlocked at " + UnlockedCats);
      return null;
    }


    return found.ElementAt(UnityEngine.Random.Range(0, found.Count));
  }

    //---------------------------------------------------------------------------------------------------------------
    public ACAtCardData GetCardDataByType(CardTypes cardType, int points)
  {
    if (cardType == CardTypes.NoType || cardType == CardTypes.Black)
    {
      Debug.LogError("Card storage has no cards of type " + cardType +".");
      return null;
    }


    List<ACAtCardData> found = new List<ACAtCardData>();

    foreach (ACardData cardTypeCounter in cardStorage)
    {
      if (cardTypeCounter.GetCardType() == cardType)
      {
        ACAtCardData catC = (ACAtCardData) cardTypeCounter;
        if (catC.Points == points)
        {
          found.Add(catC);
        }
      }
    }


    if (found.Count < 1)
    {
      Debug.LogError("Card storage has no cards of type " + cardType + " with points " + points);
      return null;
    }


    return found.ElementAt(UnityEngine.Random.Range(0, found.Count));
  }

  //---------------------------------------------------------------------------------------------------------------
  public ACardData GetCardDataByType(CardTypes cardType)
  {
    foreach (ACardData cardTypeCounter in cardStorage)
    {
      if (cardTypeCounter.GetCardType() == cardType)
      {
        return cardTypeCounter;
      }
    }

    return null;
  }
}
