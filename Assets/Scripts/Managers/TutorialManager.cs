using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Utils.ExtensionMethods;
using UnityEngine.Scripting;

public class TutorialManager : MonoBehaviour
{

    [Preserve] public bool IsActive { get { return Game.Settings.IsTutorialActive; } }
    [Preserve] public int GetLastStage { get { return Game.Settings.TutorialStage; } }

    [Preserve] private const string TutorialFileName = "TutorialStages.s";
    [Preserve] private bool[] stagesDone;
    [Preserve] private bool IOSmaphore = false;
  //---------------------------------------------------------------------------------------------------------------
  void Awake()
  {
        DontDestroyOnLoad(gameObject);
        this.stagesDone = new bool[this.GetStagesArraySize()];

    if (Game.Settings.IsFirstLaunch)
    {
      this.Reset();
      return;
    }

    this.LoadArray();
  }


  //---------------------------------------------------------------------------------------------------------------
  private int GetStagesArraySize ()
  {
    return  Enum.GetValues(typeof(TutorialId)).Length;
  }

  //---------------------------------------------------------------------------------------------------------------
    private void Reset()
  {
    Debug.Log("Reseting tutorial.");
    this.SaveArray();

  }

  //---------------------------------------------------------------------------------------------------------------
  private PopupId GetPopID(TutorialId stage)
  {
    string PopIdString = stage.GetDesc();

    for (int i = (int) PopupId.first; i < (int) PopupId.lastPlusOne; i++)
    {
      PopupId check = (PopupId) i;
      if (check.ToString() == PopIdString)
      {
        return check;
      }
    }

    return PopupId.NoPopUp;
  }

 //---------------------------------------------------------------------------------------------------------------
 public bool WasStageShown(TutorialId stage)
  {
    return this.stagesDone[(int) stage];
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ResetFromStage(TutorialId stage)
    {

        this.stagesDone = new bool[this.GetStagesArraySize()];
        Game.Settings.TutorialStage = (int) stage; //here was tutorial bug. As you can see I have cut old code, but forget to refresh last tutorial stage. Also, we don't need -1 here, bc we have only "less" check.
        this.SaveArray();

        return;
        this.LoadArray();
        Debug.Log("ResetFromStage " + stage);
        Game.Settings.TutorialStage = (int) stage -1;
        Debug.Log("==1111=="); 
    int enumSize = Enum.GetNames(typeof(TutorialId)).Length;
        Debug.Log(" enumSize "+ enumSize);
        for (int i = (int) stage; i < enumSize; i++)
    {
            Debug.Log("for i "+ i + " " +this.stagesDone.Length);
            this.stagesDone[(int) i] = false;
    }
        Debug.Log("==222==");
        this.SaveArray();
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  ///  Returns true if tutorial popup associatered with stage was shown.
  /// </summary>
  /// <param name="stage"></param>
  /// <param name="stageType">independent stages doesn't change LastStage variable in Manager</param>
  /// <returns></returns>
  public bool ShowStage(TutorialId stage, TutorialStage stageType = TutorialStage.standard)
  {
    if (!this.IsActive)
    {
      return false;
    }

    if (this.WasStageShown(stage))
    {
      // its okay not to show debug MSG, so these calxulations are made on manager side, not ingame code.
     // Debug.LogError("Tutorial Stage " + stage + " was requested, but this stage was shown already."); 
      return false;
    }

    if (stageType == TutorialStage.standard)
    {
      if ((int) stage < this.GetLastStage)
      {
        // its okay not to show debug MSG, so these calculations are made on manager side, not ingame code.
        Debug.LogError(" Standard stage with index less to last stage was called. Check integrity of Tutorial.");
        return false;
      }

      this.SetLastLoadedStage(stage);
    }

    Debug.Log("Tutorial stage "+ stage +".");
    this.MarkStageDone(stage);

    PopupId tutorialPopupID = this.GetPopID(stage);

    if (tutorialPopupID == PopupId.NoPopUp)
    {
      Debug.LogError("Can't find PopUp with id "+ stage.GetDesc() + " for stage "+ stage + " in PopUp ID enum.");
      return false;
    }

    Game.UiManager.Open(tutorialPopupID);
    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void SetLastLoadedStage(TutorialId stage)
  {
    Game.Settings.TutorialStage = (int) stage;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void MarkStageDone(TutorialId stage)
  {
    this.stagesDone[(int) stage] = true;
    this.SaveArray();
    Game.AnalyticsManager.OnTutorialStep(stage.ToString());
  }

  //---------------------------------------------------------------------------------------------------------------
  private void SaveArray()
  {
    if (this.IOSmaphore)
    {
      Debug.LogError("SaveArray ws called when IO Semaphore was active."); // maybe we can set up times to retry.
      return;
    }

    this.IOSmaphore = true;
     SaveRoutines(this.stagesDone);
    this.IOSmaphore = false;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void LoadArray()
  {
    if (this.IOSmaphore)
    {
      Debug.LogError("LoadArray ws called when IO Semaphore was active.");
      return;
    }

    this.IOSmaphore = true;
     this.stagesDone = LoadRoutines();
    this.IOSmaphore = false;
  }

  //---------------------------------------------------------------------------------------------------------------
  private static bool[] LoadRoutines()
  {
    string filePath = Path.Combine(Application.persistentDataPath, TutorialFileName);

    if (!File.Exists(filePath))
    {

      return null;
    }
    

    BinaryFormatter binary = new BinaryFormatter();
    FileStream saveFile = File.Open(filePath, FileMode.Open);
    bool[] result = (bool[]) binary.Deserialize(saveFile);
    saveFile.Close();

    Debug.Log("Loaded file " + TutorialFileName + ".");

    return result;
  }
  //---------------------------------------------------------------------------------------------------------------
  private static void SaveRoutines(bool[] DataToSave)
  {
    string filePath = Path.Combine(Application.persistentDataPath, TutorialFileName);
    BinaryFormatter binary = new BinaryFormatter();
    FileStream saveFile = File.Create(filePath);

    binary.Serialize(saveFile, DataToSave);

    saveFile.Close();
    Debug.Log("Saved file " + TutorialFileName + ".");
  }
}

public enum TutorialStage
{
  standard = 0,
  independent = 1
}
