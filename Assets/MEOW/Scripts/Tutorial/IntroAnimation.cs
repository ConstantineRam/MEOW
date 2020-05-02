using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IntroAnimation : MonoBehaviour
{
  [SerializeField]
  private float InitialFadeTime;

  [SerializeField]
  private float ReturnToGameTime;

  [SerializeField]
  private float SoLonelyTime;

  [SerializeField]
  private float MrMeowFade;

  [SerializeField]
  private float CatsAppearTime;

  [SerializeField]
  private float WonderfulTime;

  [SerializeField]
  private float CatRemoveTime;

  [SerializeField]
  private float CardGiveInitiaTime;

  [SerializeField]
  private float CardGiveScondaryTime;

  [SerializeField]
  private float WakingUpTime;
  [SerializeField]
  private float LastStage;

  [SerializeField]
  private Fader CardGive;

  [SerializeField]
  private CanvasGroup Group1;
  [SerializeField]
  private CanvasGroup Group2;
  [SerializeField]
  private CanvasGroup Group3;

  [SerializeField]
  private CanvasGroup ZZZ;

  [SerializeField]
  private Zzz ZScript;

  [SerializeField]
  private CanvasGroup MrCatCanvas;

  [SerializeField]
  private MrCat MrCat;

  [SerializeField]
  private Button MrCatButton;

  [SerializeField]
  private Button OptionsButton;

  [SerializeField]
  private Button CardsCollectionButton;

  [SerializeField]
  private Button BuyButton;

  [SerializeField]
  private GameObject ScoreBtn;



  [SerializeField]
  private Fader Clouds1;
  [SerializeField]
  private Fader Clouds2;

  [SerializeField]
  private Fader NightSky;

  [SerializeField]
  private Shaker NightSkyShaker;
  [SerializeField]
  private Shaker MoonShaker;

  [SerializeField]
  private Fader MrMeowFader;

  [SerializeField]
  private Fader MrMeowChairFader;

  [SerializeField]
  private Fader MagicCatsFader;

  [SerializeField]
  private Cloud[] MagicCatsMover;

  [SerializeField]
  private Cloud[] SingleCloud;

  [SerializeField]
  private Cloud MeowGive;
  [SerializeField]
  private Cloud CatsGive;

  [SerializeField]
  private GameObject IntroObject;

  [SerializeField]
  private GameObject StartButtonShadow;

  [SerializeField]
  private GameObject CatBook;

  [SerializeField]
  private GameObject Kotans;

  [SerializeField]
  private GameObject FrontKotans;

  [SerializeField]
  private GameObject SkipBtn;

  private Action Callback;
  //---------------------------------------------------------------------------------------------------------------
  public void HideCats(float Time)
  {
    for (int i = 0; i < this.MagicCatsMover.Length; i++)
    {
      this.MagicCatsMover[i].Show(Time);
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void HideClouds(float Time)
  {
    for (int i = 0; i < this.SingleCloud.Length; i++)
    {
      this.SingleCloud[i].Hide(Time);
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ShowClouds(float Time)
  {
    for (int i = 0; i < this.SingleCloud.Length; i++)
    {
      this.SingleCloud[i].Show(Time);
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  public void StartAnimation(Action callback)
  {
    this.Callback = callback;
    this.MrCatButton.interactable = false;
    this.OptionsButton.gameObject.SetActive(false);
    this.CardsCollectionButton.gameObject.SetActive(false);
    this.BuyButton.gameObject.SetActive(false);
    this.ScoreBtn.gameObject.SetActive(false);
    this.StartButtonShadow.SetActive(false);
    this.CatBook.SetActive(false);
    this.Group2.DOFade(0, 0.1f);
    this.Kotans.SetActive(false);
    this.FrontKotans.SetActive(false);
    this.SkipBtn.SetActive(true);

    Game.AudioManager.PlayMusic(AudioId.MusicIntro, false, 0.4f, true);
    Game.MenuRoot.MrCat.ChangeState(4);
    this.ShowClouds(this.InitialFadeTime * 2);


    Game.TimerManager.Start(this.InitialFadeTime /3, () =>
    { Game.MenuRoot.MrCat.ChangeState(5); }

    );

      Game.TimerManager.Start(this.InitialFadeTime / 2, () => 
    {

      this.Group1.DOFade(0, this.InitialFadeTime* 1.5f);

      this.Group3.DOFade(0, this.InitialFadeTime * 1.5f);

      this.NightSky.Show(this.InitialFadeTime * 2.2f);
      this.NightSkyShaker.StartAnimation();
      this.MoonShaker.StartAnimation();
    });
    



    



    //this.Clouds1.Show(this.InitialFadeTime);
    //this.Clouds2.Show(this.InitialFadeTime* 1.5f);

    

    Game.TimerManager.Start(this.InitialFadeTime * 1.4f, Phase1);
  }
  //---------------------------------------------------------------------------------------------------------------
  private void Phase1()
  {
    this.ZZZ.DOFade(1, this.SoLonelyTime/ 2);
    this.ZScript.StartAnimation();

    this.MrMeowChairFader.Hide(this.SoLonelyTime * 1.5f);
    this.MrMeowFader.Hide(this.SoLonelyTime * 1.5f);


    Game.MenuRoot.MrCat.ChangeState(1);



      Game.TimerManager.Start(this.SoLonelyTime, Phase2);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Phase2()
  {

    this.ZZZ.DOFade(0, this.MrMeowFade);

    this.MagicCatsFader.Show(this.CatsAppearTime / 6);

    this.HideClouds(this.CatsAppearTime * 1.4f);





    //  this.SoLonely.DOFade(0, this.SoLonelyTime / 2);
    Game.TimerManager.Start(this.CatsAppearTime, Phase3);
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Phase3()
  {
    this.MagicCatsFader.Show(this.CatsAppearTime/6);
    
     this.HideClouds(this.CatsAppearTime * 1.4f); 

     

    Game.TimerManager.Start(this.WonderfulTime, () =>
    {
      Phase4();
    });

  }
  //---------------------------------------------------------------------------------------------------------------
  private void Phase4()
  {

    this.HideCats(this.CatRemoveTime);

    Game.TimerManager.Start(this.CatRemoveTime/4, () =>
    {
      this.MagicCatsFader.Hide(this.CatRemoveTime);
      Phase5();
    });
  }


  //---------------------------------------------------------------------------------------------------------------
  private void Phase5()
  {
    this.MeowGive.Show(this.CardGiveInitiaTime);
    this.CatsGive.Show(this.CardGiveInitiaTime);

    Game.TimerManager.Start(this.CardGiveInitiaTime, () =>
    {
      Vector3 MeowMove = new Vector3(-482, -896,0);
      Vector3 CatMove = new Vector3(512, 352, 0);

      this.MeowGive.SetShowCoords(MeowMove);
      this.CatsGive.SetShowCoords(CatMove);
      this.MeowGive.Show(10);
      this.CatsGive.Show(10);


    });

      Game.TimerManager.Start(this.CardGiveInitiaTime /2, () =>
    {
      this.ShowClouds(this.CardGiveInitiaTime * 1.5f);
    });

    Game.TimerManager.Start(this.CardGiveInitiaTime * 1.5f, () =>
    {
      Phase6();
    });
  
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Phase6()
  {
    this.CardGive.Hide(this.WakingUpTime);

    this.MrMeowChairFader.Show(this.WakingUpTime);
    this.MrMeowFader.Show(this.WakingUpTime);
    this.Group1.DOFade(1, this.WakingUpTime *1.3f);

    this.Group3.DOFade(1, this.WakingUpTime * 1.3f);
    this.NightSky.Hide(this.WakingUpTime);
    this.NightSkyShaker.EndAnimation();
    this.MoonShaker.EndAnimation();


    Game.TimerManager.Start(this.WakingUpTime, () =>
    {
      this.HideClouds(this.WakingUpTime *2);
      Phase7();
    });

  }
  //---------------------------------------------------------------------------------------------------------------
  private void Phase7()
  {
    Game.MenuRoot.MrCat.ChangeState(3);

    Game.TimerManager.Start(this.LastStage, () =>
    {
      EndAnimation();
    });

  }
  //---------------------------------------------------------------------------------------------------------------
  public float GetTotalTime()
  {
    return 75f;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void SkipAnimation()
  {
    this.SkipBtn.SetActive(false);
    if (Callback != null)
    {
      Callback.Invoke();
    }
    else
    {
      Game.MenuRoot.RestartMenuScene();
    }

  }

  //---------------------------------------------------------------------------------------------------------------
  private void EndAnimation()
  {
    if (Callback != null)
    {
      Callback.Invoke();
    }

    Game.MenuRoot.MrCat.ChangeState(0);
    this.MrCatButton.interactable = true;
 //   this.OptionsButton.gameObject.SetActive(true);
 //   this.CardsCollectionButton.gameObject.SetActive(true);
 //   this.BuyButton.gameObject.SetActive(true);
 //   this.ScoreBtn.gameObject.SetActive(true);
 //   this.StartButtonShadow.SetActive(true);
  //  this.CatBook.SetActive(true);
    this.Group2.DOFade(1, 0);

    this.ZZZ.DOFade(0, 0.1f);
    this.ZScript.EndAnimation();
    this.SkipBtn.SetActive(false);


    
  }

  //---------------------------------------------------------------------------------------------------------------
  public void DisableAll()
  {
    this.IntroObject.SetActive(false);
  }
}
