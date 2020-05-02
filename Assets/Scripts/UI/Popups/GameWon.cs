//using EasyMobile;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class GameWon : GenericPopup
{
  [SerializeField]
  private Text currentScoreTextField;
  [SerializeField]
  private Text currentScore;

  [SerializeField]
  private Text totalScore;

  [SerializeField]
  private Text totalScoreTextField;

  [SerializeField]
  private Text NextUnlock;

  [SerializeField]
  private Text PromoString;

  [SerializeField]
  private Text NewKotanName;

  [SerializeField]
  private GameObject XCat;

  [SerializeField]
  private Kotan UnlockedKotan;

  [SerializeField]
  private Button ExitBtn;
  [SerializeField]
  private Button SaveCatBtn;
  [SerializeField]
  private RectTransform ExitBtnTarget;

  [SerializeField]
  private Image glow;
  private Tween glowTween;

  [SerializeField]
  private Image SummonPuff;

  [SerializeField]
  private RectTransform AnimationStart;
  [SerializeField]
  private RectTransform AnimationEnd;
  [SerializeField]
  private RectTransform AnimationJump;

  [SerializeField]
  private RectTransform MovingScreenBoundaries;
  [SerializeField]
  private RectTransform ScoreBackImage;
  [SerializeField]
  private RectTransform TextMask;

  [SerializeField]
  private GameObject FirstRandomKittyHint;

  private WinPopupData data = new WinPopupData();
  private Tween ScoreTween;
  private Vector3 PunchBig;
  private Vector3 PunchMinor;
  private bool IsSummonMagicCat = false;

  private TimerManager.Timer SaveKontansTimer;

  private int savedOldTotal;
  private const string NextUnlockAt = "Next Cat unlocks at: <color=#01458C>";

  public class WinPopupData
  {
    public int score;
  }


  //---------------------------------------------------------------------------------------------------------------
  public override void Activate(object data)
  {
    this.FirstRandomKittyHint.SetActive(false);

    if (data != null && data is WinPopupData)
    {
      this.data = (WinPopupData) data;
    }

    if (isNewKotanReadyToUnlock())
    {
      this.ExitBtn.interactable = false;
    }

    NextUnlock.supportRichText = true;
    NewKotanName.supportRichText = true;
    Vector3 rotation = new Vector3(0,0, 180);
    glowTween = glow.transform.DORotate(rotation,180f, RotateMode.Fast).SetEase(Ease.OutBack).SetLoops(-1);


    int NewKotanToUnlock = Game.ActionRoot.UniqueKotanStorage.NextToUnlock();
    if (NewKotanToUnlock == UniqueKotanStorage.AllUnlocked)
    {
      this.NextUnlock.text = "Wow! All Cats unlocked!";
    }
    else
    {
      if (isFreemiumLimitReached())
      {
       
        this.NextUnlock.text = "Buy game to unlock other Magic Cats";
      }
      else
      {
        int NewCatUnlock = Game.ActionRoot.UniqueKotanStorage.Cost(NewKotanToUnlock);
        this.NextUnlock.text = NextUnlockAt + NewCatUnlock.ToString() + "</color>";
      }
      
    }

    this.PromoString.text = "";

    this.PunchBig = new Vector3(1.3f, 1.3f, 0);
    this.PunchMinor = new Vector3(1.1f, 1.1f, 0);

    this.currentScore.text = this.data.score.ToString();
    this.totalScore.text = Game.ActionRoot.UniqueKotanStorage.GetScore().ToString();

    this.savedOldTotal = Game.ActionRoot.UniqueKotanStorage.GetScore();

    Game.ActionRoot.UniqueKotanStorage.SetScore(Game.ActionRoot.UniqueKotanStorage.GetScore() + this.data.score);
    this.UnlockNewKotan();
    //Game.TimerManager.Start(1f, () => { this.pushScore(savedOldTotal, this.data.score); });



  }

  //---------------------------------------------------------------------------------------------------------------
  private bool isFreemiumLimitReached()
  {
    if (!Game.Settings.IsPremium)
    {
      if (Game.Settings.FreeCats < Game.ActionRoot.UniqueKotanStorage.NextToUnlock())
      {
        return true;
      }
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  private bool isNewKotanReadyToUnlock()
  {
    if (this.isFreemiumLimitReached())
    {
      return false;
    }

    int NewKotanToUnlock = Game.ActionRoot.UniqueKotanStorage.NextToUnlock();
    if (NewKotanToUnlock == UniqueKotanStorage.AllUnlocked)
    {
      return false;
    }

    if (Game.ActionRoot.UniqueKotanStorage.GetScore() >= Game.ActionRoot.UniqueKotanStorage.Cost(NewKotanToUnlock))
    {
      return true;
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void UnlockNewKotan()
  {
    //Tween XCatRemover = this.XCat.transform.DOScale(Vector3.zero, 2f).SetAutoKill();
    KotanData newKotan = null;

    if (this.isNewKotanReadyToUnlock())
    {
      newKotan = Game.ActionRoot.UniqueKotanStorage.UnlockNewCat();
      this.IsSummonMagicCat = true;
    }
    else
    {
      newKotan = Game.ActionRoot.KotansStorage.GetRandomKotan();
      this.IsSummonMagicCat = false;

      if (!Game.Settings.FirstKittyHintShown)
      {
        Game.Settings.FirstKittyHintShown = true;
        this.FirstRandomKittyHint.SetActive(true);
      }
    }


    
    if (newKotan == null)
    {
      Debug.LogError("Unlock new Kotan returned null!");
      return;
    }
    this.UnlockedKotan.ActivateKotan(newKotan);
    this.UnlockedKotan.transform.localScale = Vector3.zero;
    this.UnlockedKotan.TurnOn();
    this.UnlockedKotan.transform.DOScale(Vector3.one, 1.2f);

    

    if (this.IsSummonMagicCat)
    {
      int KotanNum = Game.ActionRoot.UniqueKotanStorage.GetTopUnlockedKotanNum();
      this.NewKotanName.text = Game.ActionRoot.UniqueKotanStorage.GetNameNum(KotanNum);
      Vector3 scaleTo = new Vector3(1.2f, 1.2f, 0);
    //  this.NewKotanName.rectTransform.DOLocalMoveY(this.NewKotanName.rectTransform.localPosition.y + 120, 1.0f).SetAutoKill();
    //  this.NewKotanName.rectTransform.DOScale(scaleTo, 0.4f).SetAutoKill();
      this.NextUnlock.text = "unlocked.";
    }
    else
    {
      this.NewKotanName.text = "Custom Kitty came!";
     // this.NextUnlock.text = "";
    }

    Game.TimerManager.Start(0.5f, () => { this.SummonKotanMove(); } );

    //  
  }
  //---------------------------------------------------------------------------------------------------------------
  private void SummonKotanMove()
  {
    this.UnlockedKotan.RectTransform.DOLocalMoveY(this.AnimationJump.localPosition.y, 0.6f).SetAutoKill();

    Game.TimerManager.Start(0.7f, () => { this.SummonedKotanEndMove(); });
  }

  //---------------------------------------------------------------------------------------------------------------
  private void SummonedKotanEndMove()
  {
    this.UnlockedKotan.RectTransform.DOLocalMoveY(this.AnimationEnd.localPosition.y, 0.11f).SetAutoKill();
    Game.TimerManager.Start(.4f, () => { this.ShowScore(); });
  }

  //---------------------------------------------------------------------------------------------------------------
  private void ShowScore()
  {

    this.ScoreBackImage.sizeDelta = new Vector2(this.ScoreBackImage.sizeDelta.x, this.ScoreBackImage.sizeDelta.y + 10f );
    this.ScoreBackImage.anchoredPosition = new Vector2(this.ScoreBackImage.anchoredPosition.x, this.ScoreBackImage.anchoredPosition.y -5);

    this.TextMask.sizeDelta = new Vector2(this.TextMask.sizeDelta.x, this.TextMask.sizeDelta.y + 10f);
    this.TextMask.anchoredPosition = new Vector2(this.TextMask.anchoredPosition.x, this.TextMask.anchoredPosition.y - 5);


 
    if (this.ScoreBackImage.sizeDelta.y < this.MovingScreenBoundaries.sizeDelta.y)
    {
      Game.TimerManager.Start(.01f, () => { this.ShowScore(); });
      return;
    }

    this.pushScore(this.savedOldTotal, this.data.score );

    Game.TimerManager.Start(1.4f, () => {
      this.ExitBtn.interactable = true;
      this.ExitBtn.transform.DOLocalMoveY(this.ExitBtnTarget.localPosition.y, 1.5f);
      if (!this.IsSummonMagicCat)
      {
        this.SaveCatBtn.transform.DOLocalMoveY(this.ExitBtnTarget.localPosition.y, 1.1f);
      }
      
    });
  }

  //---------------------------------------------------------------------------------------------------------------
    private void pushScore(int OldScore, int ExtraScore, float time = 0.09f)
  {
    if (ExtraScore < 1)
    {
     // this.currentScoreTextField.text = "";
    //  this.currentScore.text = "";
      float moveSpeed = 2f;




      return;
    }

    Vector3 punch = Vector3.one;
    ScoreTween.Kill();

    if (ExtraScore > 100)
    {
      ExtraScore = ExtraScore - 10;
      OldScore = OldScore + 10;
      punch = this.PunchBig;
    }
    else
    {
      ExtraScore--;
      OldScore++;
      punch = this.PunchMinor;
    }


   // this.currentScore.text = ExtraScore.ToString();
    this.totalScore.text = OldScore.ToString();
   // ScoreTween = this.totalScore.rectTransform.DOScale(punch, 0.005f).SetEase(Ease.OutBack);

    time = time - 0.002f;
    time = Math.Max(0.005f, time);



    Game.TimerManager.Start(time, () => 
    {
      pushScore(OldScore, ExtraScore, time);
    });
    }

  //---------------------------------------------------------------------------------------------------------------
  public override void CloseSelf()
  {
    Game.StateManager.SetState(GameState.Menu);

  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnExitClick()
  {
    glowTween.Kill();
    ScoreTween.Kill();
    this.CloseSelf();
    Game.AdsManager.Show(() => { }, AdsType.VIDEO);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnSaveCatsClick()
  {
    if (this.IsSummonMagicCat)
    {
      //Debug.Log("OnSaveCatsClick, but its magic cat summoning round. Save btn shouldn't appear.");
      this.CloseSelf(); // Invisible button
      return;
    }

    glowTween.Kill();
    ScoreTween.Kill();

    //Game.UiManager.Open(PopupId.SaveKotans);

    if (Game.ActionRoot.CustomKotanStorage.PushKotan(this.UnlockedKotan.GetKotanData()))
    {
      Game.ActionRoot.CustomKotanStorage.SaveArray();
    }
    this.CloseSelf();

  }
}
