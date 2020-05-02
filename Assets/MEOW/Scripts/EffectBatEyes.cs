using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EffectBatEyes : MonoBehaviour
{
  private float myY;
  private float myYMove;
  private float time = 4f;
  List<Tween> move;
  private RectTransform MyRect;

  void Awake()
  {
    this.MyRect = this.GetComponent<RectTransform>();
    this.myY = this.MyRect.localPosition.y;
    this.myYMove = this.myY + 120;
    move = new List<Tween>();
  }

  void Start()
  {
    Delay();
  }

  public void MoveUP()
  {
    move.Add(this.MyRect.DOLocalMoveY(this.myYMove, this.time).OnComplete(MoveDown));
  }

  public void MoveDown()
  {

    move.Add(this.MyRect.DOLocalMoveY(this.myY, this.time).OnComplete(Delay));

  }

  public void Delay()
  {
    foreach (Tween t in move)
    {
      t.Kill();
    }
    move.Clear();

    Game.TimerManager.Start(UnityEngine.Random.Range(8f, 18f),MoveUP);
  }
}
