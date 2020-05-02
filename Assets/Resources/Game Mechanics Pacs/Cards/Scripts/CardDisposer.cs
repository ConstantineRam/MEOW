// this class provide you with to tool to dispose of a cards you may not want outright destroy.
// For example cards that we droped at maginal location, can't fit deck or holder. Were replaced by other cards etc
// by default Disposer will destroy these cards, but you may want to write custom tailored process for these cards.
// NOTE: Game expects that disposed card will be removed from its current parent.
using UnityEngine;

public class CardDisposer : MonoBehaviour
{

	public virtual void Dispose (ACard CardToDispose)
  {
    if (CardToDispose == null)
    {
      Debug.LogError("CardDisposer got null at card to Dispose.");
      return;
    }

    CardToDispose.ReturnToPool();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
