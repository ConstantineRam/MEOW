using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TaskController : MonoBehaviour
{
  [SerializeField]
  private Image myBack;

  [SerializeField]
  private Image myMain;

  [SerializeField]
  private Image mySecondary;

  [SerializeField]
  private RectTransform myHintObject;

  [SerializeField]
  private Image myHint;

  [SerializeField]
  private CanvasGroup MainTaskCanvas;
  [SerializeField]
  private CanvasGroup SecondaryTaskCanvas;
  [SerializeField]
  private CanvasGroup HintCanvas;

  [SerializeField]
  private Vector3 MainPunch;

  private bool Reset;
  CardTypes currentTask;
  public CardTypes GetTask { get { return currentTask; } }
  public CardTypes SetTask { set { currentTask = value; } }
  [SerializeField]
  Sprite[] TaskSprites;

  private RectTransform myRect;
  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
    this.myRect = this.GetComponent<RectTransform>();
    this.myRect.localScale = Vector3.zero;
    this.myHintObject.localScale = Vector3.zero;
    this.Reset = true;
  }
  //---------------------------------------------------------------------------------------------------------------
    void Start ()
  {
    this.myMain.rectTransform.DOPunchPosition(MainPunch, 3f).SetLoops(-1);
	}

  //---------------------------------------------------------------------------------------------------------------
  public void Show(CardTypes prime, CardTypes second, CardTypes hint)
  {
    if (prime == CardTypes.NoType)
    {
      Debug.LogError("Wrong type to show at hint.");
      return;
    }

    if (second == CardTypes.NoType)
    {
      Debug.LogError("Wrong type to show at hint.");
      return;
    }

    this.Reset = true;

    this.currentTask = prime;
    this.myMain.sprite = this.TaskSprites[(int) prime];
    this.mySecondary.sprite = this.TaskSprites[(int) second];

    if (hint != CardTypes.NoType)
    {
      this.myHint.sprite = this.TaskSprites[(int) hint];
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ResetAlpha()
  {
    this.SecondaryTaskCanvas.alpha = 1;
    this.HintCanvas.alpha = 1;
    this.MainTaskCanvas.alpha = 1;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void HideSecondaryAndNextHint()
  {
    this.SecondaryTaskCanvas.alpha = 0;
    this.HintCanvas.alpha = 0;
    this.myHintObject.DOScale(Vector3.zero, 0f).SetAutoKill();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ShowSecondary()
  {
    this.SecondaryTaskCanvas.alpha = 1;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void CheckGoal(CardTypes newCard)
  {
    if (newCard != this.currentTask)
    {
      return;
    }

    if (!this.Reset)
    {
      return;
    }

  //  this.MainTaskCanvas.DOFade(0, 1f);
  }


    //---------------------------------------------------------------------------------------------------------------
    public void Hide(float time = 0.0001f)
  {
    this.ResetAlpha();
    this.transform.DOScale(Vector3.zero, time).SetAutoKill();
    this.myHintObject.DOScale(Vector3.zero, time).SetAutoKill();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void UnHide(float time = 0.001f, bool showHint = true)
  {
    this.transform.DOScale(Vector3.one, time).SetAutoKill();
    if (showHint)
    {
      this.myHintObject.DOScale(Vector3.one, time).SetAutoKill();
    }
    
  }
}
