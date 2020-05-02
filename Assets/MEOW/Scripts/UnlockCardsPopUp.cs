using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UnlockCardsPopUp : GenericPopup
{
  [SerializeField]
  private RectTransform GlowImage;
  [SerializeField]
  private Image BoosterImage;
  [SerializeField]
  private Sprite LockedBoosterImage;
  [SerializeField]
  private Sprite UnlockedBoosterImage;
  [SerializeField]
  private Image CardBackGlow;
  [SerializeField]
  private GameObject hint;
  [SerializeField]
  private RectTransform BoosterRect;
  [SerializeField]
  private float TimeToOpen;
  [SerializeField]
  private Vector3 BoosterBagDissolveScale;
  [SerializeField]
  private float TimeBoosterBagDisolve;
  [SerializeField]
  private float TimeShowCardAnimation;
  [SerializeField]
  private CanvasGroup BoosterBagCanvas;
  [SerializeField]
  private RectTransform CardsContainer;
  [SerializeField]
  private float CardNormalizeRotation = 17;
  [SerializeField]
  private CanvasGroup GlowCanvas;
  private Stack<UnlockCardsAnimationObject> animationObjects;

  private Tween BooterShaker;
  private Tween GlowScaler;
  private Tween GlowRotation;
  private bool BoosterClicked = false;

  private TimerManager.Timer FlyTimer;
  private UnlockCardsAnimationObject LastUsedObject;
  //---------------------------------------------------------------------------------------------------------------
  public sealed override void Activate(object data)
  {
    if (Game.MenuRoot.UniqueKotanStorage.WasCardsUnlockShown())
    {
      Debug.LogError("show unlock called, but it was already shown.");
      return;
    }
    animationObjects = new Stack<UnlockCardsAnimationObject>();
    this.AddCards();

    this.BooterShaker = this.BoosterRect.DOShakePosition(30, 10f, 10,80, false, false).SetLoops(-1);
    this.GlowScaler = this.CardBackGlow.transform.DOShakePosition(20, 5f, 10, 80, false, false).SetLoops(-1);
    Vector3 rotation = new Vector3(0, 0, 900);
    this.GlowRotation = this.GlowImage.transform.DORotate(rotation,130f, RotateMode.FastBeyond360).SetLoops(-1);
  }
  //---------------------------------------------------------------------------------------------------------------
  private void AddCards()
  {
    int usedPosition = Game.MenuRoot.UniqueKotanStorage.GetTopUnlockedKotanPosition();
    Quaternion resetRotation = new Quaternion();
    resetRotation.Set(0, 0, 0, 0);

    if (Game.MenuRoot.UniqueKotanStorage.HasLevelBonusInc(usedPosition))
    {

      UnlockCardsAnimationObject NewAObject = (UnlockCardsAnimationObject) Game.PoolManager.Pop(ObjectPoolName.AUnlockObject, this.CardsContainer);
      if (NewAObject != null)
      {
        NewAObject.MakeBonusInc(usedPosition);
        this.animationObjects.Push(NewAObject);
        NewAObject.transform.localPosition = Vector3.zero;
        NewAObject.rectTransform.localRotation = resetRotation;
      }
    }

    List<ACAtCardData> LevelCards = GameData.CardStorage.GetDataForMagicCatLevel(usedPosition);
    if (LevelCards == null)
    {
      return;
    }

    if (LevelCards.Count == 0)
    {
      return;
    }

    foreach (ACAtCardData CatCard in LevelCards)
    {
      UnlockCardsAnimationObject NewAObject = (UnlockCardsAnimationObject) Game.PoolManager.Pop(ObjectPoolName.AUnlockObject, this.CardsContainer);
      NewAObject.transform.localPosition = Vector3.zero;
      NewAObject.rectTransform.localRotation = resetRotation;
      if (NewAObject == null)
      {
        continue;
      }

      NewAObject.SetCard(CatCard);
      this.animationObjects.Push(NewAObject);

    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnBoosterClicked()
  {
    if (this.BoosterClicked)
    {
      this.SkipPressed(); 
      return;
    }
    this.hint.gameObject.SetActive(false);
    Debug.Log("Booster clicked");
    this.BoosterClicked = true;
    this.BooterShaker.Kill();

    this.BooterShaker = this.BoosterRect.DOShakePosition(20, 15f, 10, 80, false, false).SetLoops(-1);

    Game.TimerManager.Start(this.TimeToOpen, this.StartOpeningBooster);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void StartOpeningBooster()
  {
    this.BoosterImage.sprite = this.UnlockedBoosterImage;
    this.BoosterImage.transform.DOScale(this.BoosterBagDissolveScale, this.TimeBoosterBagDisolve);
    this.BoosterBagCanvas.DOFade(0, this.TimeBoosterBagDisolve);

    this.FlyTimer = Game.TimerManager.Start(this.TimeBoosterBagDisolve-0.4f, this.CardsFlight);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void CardsFlight()
  {
    this.FlyTimer = null;
    this.BooterShaker.Kill();
    this.FlyTimer = null;
    if (this.animationObjects.Count == 0)
    {
      this.EndOpening();  
      return;
    }

    if (this.LastUsedObject != null)
    {
      if (!this.LastUsedObject.IsPooled)
      {
        this.LastUsedObject.ReturnToPool();
        this.LastUsedObject = null;
      }
    }
    


    UnlockCardsAnimationObject UsedObject = this.animationObjects.Pop();
    UsedObject.Animate(this.TimeShowCardAnimation, this.CardNormalizeRotation);
    this.LastUsedObject = UsedObject;
    if (this.animationObjects.Count == 0)
    {
      this.GlowScaler.Kill();
      this.CardBackGlow.gameObject.SetActive(false);
    }

    Game.TimerManager.Start(this.TimeShowCardAnimation -1.4f, CardsFlight);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void EndOpening()
  {
    float CloseTime = this.TimeShowCardAnimation / 3;
    this.GlowCanvas.DOFade(0, CloseTime);
    Game.MenuRoot.UniqueKotanStorage.SetCardsUnlockShown();
    if (Game.MenuRoot.UniqueKotanStorage.HasLevelBonusInc(Game.MenuRoot.UniqueKotanStorage.GetTopUnlockedKotanPosition()))
    {
      Game.Settings.BonusValue += 1;
    }

    Game.TimerManager.Start(CloseTime, this.CloseSelf);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void SkipPressed()
  {
    if (this.FlyTimer != null)
    {
      this.FlyTimer.StopAndExecute();
    }
  }
}
