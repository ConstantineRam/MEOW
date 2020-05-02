using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class CustomKotanStorage : MonoBehaviour
{
  List<CustomKotamData> CustomKotans;

  private int maxSlots;
  public int MaxSlots { get { return maxSlots; } }

  private int SlotsFree = 12;
  public int FremiumAmount { get { return SlotsFree; } }
  private int SlotPremium = 200;
  public int PremiumAmount { get { return SlotPremium; } }

  private const string SaveFileName = "CustomKotans.s";



  //---------------------------------------------------------------------------------------------------------------
  private void Awake()
  {
    this.UpdatePremium();
    CustomKotans = new List<CustomKotamData>();
  }

  //---------------------------------------------------------------------------------------------------------------
  void Start ()
  {
    this.UpdatePremium();

    if (Game.Settings.IsFirstLaunch && Game.IsDebug)
    {
      SaveArray();
      return;
    }
   

    this.LoadArray();
    if (this.SlotPremium <= this.SlotsFree)
    {
      Debug.LogError("SlotPremium <= SlotsFree");
    }

	}


  //---------------------------------------------------------------------------------------------------------------
  public void UpdatePremium()
  {
    this.maxSlots = this.SlotsFree;
    if (Game.Settings.IsPremium)
    {
      this.maxSlots = this.SlotPremium;
    }

  }

  //---------------------------------------------------------------------------------------------------------------
  public int AmountOfKotans()
  {
    return this.CustomKotans.Count;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasAtLeastOne()
  {
    if (this.CustomKotans.Count < 1)
    {
      return false;
    }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasEmptySlots()
  {
    if (this.CustomKotans.Count >= this.maxSlots)
    {
      return false;
    }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  public CustomKotamData GetKotan(int KotanNum)
  {
    KotanNum--;

    if (KotanNum < 0)
    {
      Debug.LogError("GetKotan rquestedd num is too low. "+ KotanNum);
      return null;
    }

    if (KotanNum > this.CustomKotans.Count-1)
    {
      Debug.LogError("GetKotan rquestedd num is too big.");
      return null;
    }

    return this.CustomKotans.ElementAt(KotanNum);
  }
  
  //---------------------------------------------------------------------------------------------------------------
    public bool HasKotan(CustomKotamData CheckKotan)
  {
    if (this.CustomKotans.Contains(CheckKotan))
    {
      return true;
    }

    return false;

  }
  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// /Saves Kotan as standard KotanData by transforming it into CustomKotanData before save.
  ///  Pushed KotanData into array saved in memory. You still need to call SaveArray to save it to disc.
  /// </summary>
  /// <param name="newKotan"></param>
  /// <returns></returns>
  public bool PushKotan(KotanData newKotan)
  {
    CustomKotamData CustomNewKotan = new CustomKotamData();
    CustomNewKotan.Clone(newKotan);
    return this.PushKotan(CustomNewKotan);

  }
  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Pushed KotanData into array saved in memory. You still need to call SaveArray to save it to disc.
  /// </summary>
  /// <param name="newKotan"></param>
  /// <returns></returns>
    public bool PushKotan(CustomKotamData newKotan)
  {
    if (!this.HasEmptySlots())
    {
      return false;
    }

    if (this.CustomKotans.Contains(newKotan))
    {
      this.CustomKotans.Remove(newKotan);
    }

    this.CustomKotans.Add(newKotan);
    // save array was removed. At first we push all we need and then call save once.
    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void DeleteKotan(CustomKotamData removeKotan)
  {
    if (!this.CustomKotans.Contains(removeKotan))
    {
      Debug.LogError("remove Cat called, but storage hasn't that cat.");
      return;
    }

    this.CustomKotans.Remove(removeKotan);
    this.SaveArray();
    
  }


  #region Save/ Load

  //---------------------------------------------------------------------------------------------------------------
  [Serializable]
  public class SerializedData
  {
    public CustomKotamData.SerialisedKotanData[] savedData;
  }

  //---------------------------------------------------------------------------------------------------------------
  private static SerializedData LoadRoutines()
  {
    string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);

    if (!File.Exists(filePath))
    {

      return null;
    }
    SerializedData result = new SerializedData();

    BinaryFormatter binary = new BinaryFormatter();
    FileStream saveFile = File.Open(filePath, FileMode.Open);
    result.savedData = (CustomKotamData.SerialisedKotanData[]) binary.Deserialize(saveFile);
    saveFile.Close();

    Debug.Log("Loaded file " + SaveFileName + ".");

    return result;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void LoadArray()
  {
    SerializedData LoadeData = CustomKotanStorage.LoadRoutines();
    if (LoadeData == null)
    {
      return;
    }

    if (LoadeData.savedData.Length < 1)
    {
   //   Debug.LogError("Data was loaded, but no elements found."); its okay. It may happen, if player saved some cats before, but then cleared them all.
      return;
    }

    CustomKotans.Clear();

    for (int i = 0; i < LoadeData.savedData.Length; i++)
    {
      CustomKotamData LoadedCotan = new CustomKotamData();
      LoadedCotan.DeSerialiseKotan(LoadeData.savedData[i]);
      CustomKotans.Add(LoadedCotan);
    }
  }

  //---------------------------------------------------------------------------------------------------------------
  private static void SaveRoutines(SerializedData dataToSave)
  {
    string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
    BinaryFormatter binary = new BinaryFormatter();
    FileStream saveFile = File.Create(filePath);

    binary.Serialize(saveFile, dataToSave.savedData);

    saveFile.Close();
    Debug.Log("Saved file " + SaveFileName + ".");
  }

  //---------------------------------------------------------------------------------------------------------------
  public void SaveArray()
  {
    //if (this.CustomKotans.Count < 1)
    //{
    //  return;
    //}

    SerializedData dataToSave = new SerializedData();
    dataToSave.savedData = new CustomKotamData.SerialisedKotanData[this.CustomKotans.Count];

    int i = -1;
    foreach (CustomKotamData Cat in this.CustomKotans)
    {
      i++;
      if (Cat == null)
      {
        Debug.Log("Unexpected Error Cat #" + i + " == null.");
        continue;
      }
      dataToSave.savedData[i] = Cat.SerialiseKotan();
    }

    CustomKotanStorage.SaveRoutines(dataToSave);
  }

  #endregion

}
