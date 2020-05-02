using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardTYpeImageStorage : MonoBehaviour
{
  [SerializeField]
  private Sprite[] TypeImageCollection;


  public Sprite GetSprite(CardTypes cardType)
  {
    if (cardType == CardTypes.NoType)
    {
      return null;
    }

    return TypeImageCollection[(int) cardType];
  }
}
