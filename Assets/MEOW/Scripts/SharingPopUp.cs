using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class SharingPopUp : GenericPopup
{
  [SerializeField]
  private RectTransform ScreenshotArea;

  [SerializeField]
  private string filename = "catshot";

  private bool sharingInProcess = false;
  public bool SharingInProcess { get { return sharingInProcess; } }

  private float myX1;
  private float myX2;
  private float myY1;
  private float myY2;
  private float X_size;
  private float Y_size;


  protected virtual void Awake()
  {
  //  base.Awake();
    Game.Events.ScreenshotTaken.Listen(ProcessSharingScreen);
  }

  //---------------------------------------------------------------------------------------------------------------
  protected virtual void BeforeSharingStarts()
  {
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnSharePressed()
  {
    this.MakeScreenshot();
  }
  //---------------------------------------------------------------------------------------------------------------
  private void ProcessSharingScreen()
  {

    string path = System.IO.Path.Combine(Application.persistentDataPath, filename+ ".png");
  
    Sharing.ShareImage(path, "M.E.O.W.");
    this.sharingInProcess = false;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void MakeScreenshot()
  {
    if (this.SharingInProcess)
    {
      return;
    }

    this.sharingInProcess = true;
    StartCoroutine(SaveScreenshot());
  }

  //---------------------------------------------------------------------------------------------------------------
  IEnumerator SaveScreenshot()
  {
    this.CalculateMyCoords();
    yield return new WaitForEndOfFrame();
    string path = Sharing.SaveScreenshot(this.myX1, this.myY1, this.X_size, this.Y_size, filename);
    Game.Events.ScreenshotTaken.Invoke();
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void Activate(object data)
  {

    if (this.ScreenshotArea == null)
    {
      Debug.LogError("No object with RectTRansform was set for a screenshot area.");
    }

    this.CalculateMyCoords();

    
  }

  //---------------------------------------------------------------------------------------------------------------
  private float GetScaleFactor()
  {
    if (Game.StateManager.CurrentState == GameState.Action)
    {
      return Game.ActionRoot.GetScaleFactor;
    }

    if (Game.StateManager.CurrentState == GameState.Menu)
    {
      return Game.MenuRoot.GetScaleFactor;
    }
    return 0f;
  }
  //---------------------------------------------------------------------------------------------------------------
  private void CalculateMyCoords()
  {
    this.myX1 = this.ScreenshotArea.position.x + (this.ScreenshotArea.rect.xMin * GetScaleFactor());
    this.myX2 = this.ScreenshotArea.position.x + (this.ScreenshotArea.rect.xMax * GetScaleFactor());
    this.myY1 = this.ScreenshotArea.position.y + (this.ScreenshotArea.rect.yMin * GetScaleFactor());
    this.myY2 = this.ScreenshotArea.position.y + (this.ScreenshotArea.rect.yMax * GetScaleFactor());
    this.X_size = this.myX2 - this.myX1;
    this.Y_size = this.myY2 - this.myY1;
  }

  //---------------------------------------------------------------------------------------------------------------
  public override void CloseSelf()
  {
    base.CloseSelf();

  }


}
