using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SessionResult : MonoBehaviour
{
  [Tooltip("Amount of buttons also determines the lenght of a session.")]
  [SerializeField]
  private SessionResultButton[] resultButtons;
  [SerializeField]
  private ScorePanelController scorePanel;
  [SerializeField]
  private RectTransform FlightToObject;
  
  

  private const int NoEmpty = -1;

  //---------------------------------------------------------------------------------------------------------------
  void Start ()
  {
    if (this.resultButtons.Length != Game.ActionRoot.GameSessionManager.SessionLenght)
    {
      Debug.LogError("Result buttons amount not Equal to session lenght!");
    }
	}

  //---------------------------------------------------------------------------------------------------------------
  public void Clear()
  {
    foreach (SessionResultButton btn in resultButtons )
    {
      btn.ClearAndClose();
    }

  }
  //---------------------------------------------------------------------------------------------------------------
  public void FlyBtnsForSummoning()
  {
    float moveTime = 0.8f;
    foreach (SessionResultButton btn in this.resultButtons)
    {
      btn.StopMoveAnimation();
      btn.transform.DOMove(this.FlightToObject.transform.position, moveTime);
      btn.RectTransform.DOScale(Vector3.zero, moveTime);
    }

    Game.TimerManager.Start(moveTime, () => 
    {
      foreach (SessionResultButton btn in this.resultButtons)
      {

        btn.gameObject.SetActive(false);
      }
    });
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasCatsToSave()
  {
    foreach (SessionResultButton btn in this.resultButtons)
    {
      CustomKotamData kotanData = btn.GetKotanData();
      if (kotanData == null)
      {
        Debug.Log("Unexpected error, btn has no result.");
        continue;
      }

      if (!Game.ActionRoot.CustomKotanStorage.HasKotan(kotanData))
      {
        return true;
      }
      
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public List<CustomKotamData> GetUnsavedCats()
  {
    List<CustomKotamData> result = new List<CustomKotamData>();

    foreach (SessionResultButton btn in this.resultButtons)
    {
      CustomKotamData kotanData = btn.GetKotanData();
      if (kotanData == null)
      {
        Debug.Log("Unexpected error, btn has no result.");
        continue;
      }

      if (Game.ActionRoot.CustomKotanStorage.HasKotan(kotanData))
      {
        continue;
      }

      result.Add(kotanData);
    }

    return result;
  }

  //---------------------------------------------------------------------------------------------------------------
  public int GetStage()
  {
    int result = 1;
    for (int i = 0; i < this.resultButtons.Length; i++)
    {
      if (this.resultButtons[i].HasKotanData())
      {
        result++;
      }

    }
   // Debug.Log("stage " + result);
    return result;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasEmpty()
  {
    for (int i = 0; i < this.resultButtons.Length; i++)
    {
      if (!this.resultButtons[i].HasKotanData())
      {
        return true;
      }

    }

    return false;
  }


  //---------------------------------------------------------------------------------------------------------------
  private int GetEmpty()
  {
    for (int i = 0; i < this.resultButtons.Length;i++)
    {
      if (!this.resultButtons[i].HasKotanData())
      {
        return i;
      }

    }

    return NoEmpty;
  }

  //---------------------------------------------------------------------------------------------------------------
  public int MaxResults()
  {
    return this.resultButtons.Length;
  }

  //---------------------------------------------------------------------------------------------------------------
  public KotanData GetData( int kotanNum)
  {
    if (kotanNum < 0)
    {
      Debug.LogError("Session Result got wrong reques Kotan Num < 0");
      return null;
    }

    if (kotanNum > this.resultButtons.Length-1)
    {
      Debug.LogError("Session Result got wrong reques Kotan Num this.resultButtons.Length-1");
      return null;
    }

   return this.resultButtons[kotanNum].GetKotanData();
  }

  //---------------------------------------------------------------------------------------------------------------
  public Vector3 Push(KotanData kotanData, float delay, int Score)
  {
    int newNum = this.GetEmpty();
    if (newNum == NoEmpty)
    {
      Debug.LogError("Unexpected Error, can't find place for new Kotan result.");
      return new Vector3(0, 0, 0);
    }



    Game.TimerManager.Start(delay, () =>
    {
      this.resultButtons[newNum].InitAndOpen(kotanData, Score);
      if (Score > 0) // its a debug check Before we implemented end session.
      {
        this.scorePanel.PushPoints(Score);
      }
      

      if (newNum == this.resultButtons.Length - 1)
      {
        Game.Events.SessionOverAllTasksDone.Invoke();
      }
    });

    //Game.TutorialManager.ShowStage(TutorialId.Stage8_FirstCatSummoned, TutorialStage.standard);
    return new Vector3(this.resultButtons[newNum].transform.position.x, this.resultButtons[newNum].transform.position.y, this.resultButtons[newNum].transform.position.z);


  }
}
