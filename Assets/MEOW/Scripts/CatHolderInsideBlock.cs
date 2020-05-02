using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatHolderInsideBlock : HolderInsideBlock
{
  public new ACatCard GetCard { get { return (ACatCard) base.GetCard; } }


}
