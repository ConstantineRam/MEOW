using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SessionResultButton : MonoBehaviour, IPointerClickHandler
{
  [SerializeField]
  private bool AlwaysVisible = false;
  [SerializeField]
  private Image myButtonCover;
  [SerializeField]
  private Kotan myKotan;


  [SerializeField]
  private Text scoreText;

  [SerializeField]
  private Sprite NoKotanSprite;


  private Vector3 MyCoords;
  private RectTransform myRect;
  public RectTransform RectTransform { get { return myRect; } }
  private const float CloseTime = 0.003f;
  private const float OpenTime = 0.7f;
  private Vector3 CurrentSize;

  private CustomKotamData customKotamData;

  private PopupId MyParentPopUp = PopupId.NoPopUp;

  private int totalScore = 0;
  public int TotalScore { get { return this.totalScore; } }
  private Vector3 pulsing;

  private bool isOpen = false;
  private bool StopAnimation = false;

  private TimerManager.Timer MoveTimer;
  private TimerManager.Timer PulseTimer;
  //---------------------------------------------------------------------------------------------------------------
  public bool HasKotanData ()

  {
    if (this.TotalScore == 0)
    {
      return false;
        
        } 

    //if (this.myKotan.GetKotanData() == null)
    //{
//   }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  void Awake ()
  {
    this.myRect = this.GetComponent<RectTransform>();
  

  }

  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {
    this.MyCoords = new Vector3(this.myRect.localPosition.x, this.myRect.localPosition.y, this.myRect.localPosition.z);
    this.CurrentSize = this.myRect.localScale;
    this.pulsing = new Vector3(this.CurrentSize.x + 0.1f, this.CurrentSize.x + 0.1f, 0);
    this.ClearAndClose();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void pulse()
  {
    if (this.StopAnimation)
    {
      return;
    }

    if (!this.isOpen)
    {
      if (!this.AlwaysVisible)
      {
        return;
      }
    }

    float newPulseTime = UnityEngine.Random.Range(2f, 3f);
    this.transform.DOScale(this.pulsing, newPulseTime).SetAutoKill();

    PulseTimer =  Game.TimerManager.Start(newPulseTime+0.2f, pulseBack);
  }
  //---------------------------------------------------------------------------------------------------------------
  private void pulseBack()
  {
    if (this.StopAnimation)
    {
      return;
    }

    this.transform.DOScale(this.CurrentSize, 2.1f).SetAutoKill();
    if (!this.isOpen)
    {
      if (!this.AlwaysVisible)
      {
        return;
      }
    }

    float newPulseTime = UnityEngine.Random.Range(4f, 8f);
    PulseTimer = Game.TimerManager.Start(newPulseTime, pulse);

  }

  //---------------------------------------------------------------------------------------------------------------
  private void move()
  {
    if (this.StopAnimation)
    {
      return;
    }

    if (!this.isOpen)
    {
      if (!this.AlwaysVisible)
      {
        return;
      }
    }

    float newMoveTime = UnityEngine.Random.Range(2f, 3f);
    Vector3 moving = new Vector3(this.MyCoords.x + UnityEngine.Random.Range(-12f, 12f), this.MyCoords.y + UnityEngine.Random.Range(-9f,9f), this.MyCoords.z);
    this.transform.DOLocalMove(moving, newMoveTime).SetAutoKill();

    this.MoveTimer = Game.TimerManager.Start(newMoveTime+0.3f, moveBack);
  }
  //---------------------------------------------------------------------------------------------------------------
  private void moveBack()
  {
    if (this.StopAnimation)
    {
      return;
    }

    float newMoveTime = UnityEngine.Random.Range(3f, 5f);
    this.transform.DOLocalMove(this.MyCoords, newMoveTime).SetAutoKill();



    if (!this.isOpen)
    {
      if (!this.AlwaysVisible)
      {
        return;
      }
    }


    this.MoveTimer = Game.TimerManager.Start(newMoveTime+0.3f, move);

  }

  //---------------------------------------------------------------------------------------------------------------
  public void StopMoveAnimation()
  {
    this.StopAnimation = true;
    if (this.MoveTimer != null)
    {
      this.MoveTimer.Halt();
    }

    if (this.PulseTimer != null)
    {
      this.PulseTimer.Halt();
    }


  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnPointerClick(PointerEventData pointerEventData)
  {
    if (!this.HasKotanData())
    {
      return;
    }

    PopUpKotanCard.KotanPopupData data = new PopUpKotanCard.KotanPopupData(this.GetKotanData());
    data.SetPopUpCallback = this.MyParentPopUp;
    Game.UiManager.Open(PopupId.CatCard, data);
  }


  //---------------------------------------------------------------------------------------------------------------
  private void RaycastOff()
  {
    this.myButtonCover.raycastTarget = false;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void RaycastOn()
  {
    this.myButtonCover.raycastTarget = true;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void SetScore(int Score)
  {
    this.totalScore = Score;
    if (this.TotalScore == 0)
    {
      this.scoreText.text = "";
      return;
    }

    this.scoreText.text = Score.ToString();
  }

  //---------------------------------------------------------------------------------------------------------------
  public CustomKotamData GetKotanData()
  {
    return this.customKotamData;
  }
  //---------------------------------------------------------------------------------------------------------------
  public void InitAndOpen(KotanData newKotanData, int Score = 0)
  {
    this.customKotamData = null;

    if (newKotanData != null)
    {
      this.customKotamData = new CustomKotamData();

      for (int i = 0; i < (int) KPart.last + 1; i++)
      {
        customKotamData.addData(newKotanData.GetData((KPart) i), (KPart) i);
      }

      customKotamData.SetName = CustomKotamData.NO_NAME;
      customKotamData.SetQuote = CustomKotamData.NO_NAME;
    }
    

    this.InitAndOpen(customKotamData, Score);
  }
    //---------------------------------------------------------------------------------------------------------------
    public void InitAndOpen(CustomKotamData newKotanData, int Score = 0, PopupId ParentPopUp = PopupId.NoPopUp, bool SaveKittyPopUp = false)
  {
    //this.Start();

    if (this.myKotan.HasKotanData)
    {
      Debug.LogError("Kotan data was pushed, however ResultBtn already has it.");
    }

    this.MyParentPopUp = ParentPopUp;

    if (Score > 0)
    {
      this.SetScore(Score);
    }


    this.customKotamData = newKotanData;

    if (this.customKotamData == null)
    {
      this.RaycastOff();
      this.myKotan.TurnOff();
      //this.myButtonCover.sprite = this.NoKotanSprite;
    }
    else
    {
      this.myKotan.ActivateKotan(this.customKotamData);
      this.RaycastOn();
      this.myKotan.TurnOn();
    }
    
    this.transform.DOScale(this.CurrentSize, OpenTime).SetAutoKill();
    this.isOpen = true;


    if (!SaveKittyPopUp && !this.AlwaysVisible)
    {
      Game.TimerManager.Start(UnityEngine.Random.Range(1f, 2f), pulse);
      Game.TimerManager.Start(UnityEngine.Random.Range(0.2f, 0.6f), move);
    }

    Game.ActionRoot.GameSessionManager.ShowTask();

    if (Game.Settings.IsTutorialActive)
    {
      Game.ActionRoot.ContinueTutorial();
      return;
    }

    Game.ActionRoot.GameSessionManager.CheckPotentialLose();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ClearAndClose ()
  {
    this.SetScore(0);
    this.RaycastOff();
    this.myKotan.TurnOff();

    if (!this.AlwaysVisible)
    {
      this.transform.DOScale(Vector3.zero, CloseTime).SetAutoKill();
    }
    else
    {
      Game.TimerManager.Start(UnityEngine.Random.Range(1f, 2f), pulse);
      Game.TimerManager.Start(UnityEngine.Random.Range(0.2f, 0.6f), move);
    }
    

    this.isOpen = false;
	}

}
