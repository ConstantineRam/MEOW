using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScorePanelController : MonoBehaviour //, IPointerClickHandler
{
  private CanvasGroup canvasGroup;
  [SerializeField]
  private Text ScoreText;
	private int PointsToProcess = 0;
  private int totalPoints = 0;
  private int PointsOnScreen = 0;
  public int TotalPoints { get { return totalPoints; } }

  private float punchTimer        = 0.01f;
  private float punchRecoverTimer = 0.008f;
  private bool CanAddScore = true;
  private Vector3 punch;

  //---------------------------------------------------------------------------------------------------------------
  void Awake ()
  {
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    this.punch = new Vector3(1.05f, 1.05f,1);

    this.Reset();
	}
  //---------------------------------------------------------------------------------------------------------------
  void Update()
  {
    if (this.PointsToProcess == 0)
    {
      return;
    }

    if (!this.CanAddScore)
    {
      return;
    }
    this.CanAddScore = false;

    if (PointsToProcess > 0)
    {
      this.PointsToProcess--;
      this.PointsOnScreen++;
    }

    if (PointsToProcess < 0)
    {
      this.PointsToProcess++;
      this.PointsOnScreen--;
    }
    this.ShowPoints();
    this.Punch();

    Game.TimerManager.Start(this.punchTimer, () => { this.CanAddScore = true; });
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnHandHintClick()
  {
    Game.UiManager.Open(PopupId.HintHandScore);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnUniqueKotansClick()
  {
    Game.ActionRoot.UniqueKotanBook();
  }

//---------------------------------------------------------------------------------------------------------------
public void Reset()
  {
    this.totalPoints = 0;
    this.PointsToProcess = 0;
    this.PointsOnScreen = 0;
    this.ShowPoints();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void Punch()
  {
   Tween pt = ScoreText.transform.DOPunchScale(this.punch, this.punchRecoverTimer);
    Game.TimerManager.Start(punchRecoverTimer+0.01f, () => { pt.Kill(); } );

  }
  //---------------------------------------------------------------------------------------------------------------
  private void ShowPoints()
  {
    this.ScoreText.text = PointsOnScreen.ToString();
    if (PointsOnScreen < 100 && PointsOnScreen > 10)
    {
      this.ScoreText.text = " " + PointsOnScreen.ToString()+ " ";
      return;
    } 
    if (PointsOnScreen < 10)
    {
      this.ScoreText.text = "  " + PointsOnScreen.ToString()+ "  ";
    }


  }
  //---------------------------------------------------------------------------------------------------------------
  //
  public void UpdatePoints(int NewPointsToProcess)
  {
    
    int newPoints = this.totalPoints + (NewPointsToProcess - this.totalPoints);
    this.PushPoints(newPoints);
  }
  //---------------------------------------------------------------------------------------------------------------
  public void PushPoints(int NewPointsToProcess)
  {
    this.PointsToProcess += NewPointsToProcess;
    this.totalPoints += NewPointsToProcess;
  }
}
