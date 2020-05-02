using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class UniqueKotanStorage : MonoBehaviour
{

  private int[] KotanUnlockScore;
  private int[] BonusIncs;
  private int[] Unlocked;
  private bool[] CardUnlocksShown;
  private string[] Names;
  private string[][] Quotes;

  public const int LockedSlot = -1;
  public const int NothingUnlocked = -1;
  public const int AllUnlocked = -2;
  private const string SaveFileNameCats = "UniqueCats.s";
  private const string SaveFileNameCardUnlocks = "CardUnlocks.s";

  public struct CustomKotan
  {
    string name;
    int head;
    int tail;
    int Body1;
    int Body2;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool IsItTimeToBuyGame()
  {
    if (Game.Settings.IsPremium)
    {
      return false;
    }

    if (Game.Settings.FreeCats >= this.NextToUnlock())
    {
      return false;
    }

    return true;

  }

  //---------------------------------------------------------------------------------------------------------------
  public int NextToUnlock()
  {

    for (int i = 0; i < this.Unlocked.Length; i++)
    {
      if (this.Unlocked[i] == LockedSlot)
      {
        return i + 1;
      }
    }

    return AllUnlocked;
  }

  //---------------------------------------------------------------------------------------------------------------
  public int GetScore()
  {
    return Game.Settings.BestScore;
  }

  //---------------------------------------------------------------------------------------------------------------
  public void SetScore(int newScore)
  {
    Game.Settings.BestScore = newScore;
  }

  //---------------------------------------------------------------------------------------------------------------
  public int GetBonusPointsForLevel(int level)
  {
    int result = 1;
    for (int i = 0; i < this.BonusIncs.Length; i++)
    {
      result++;
      if (this.BonusIncs[i] == level)
      {
        return result;
      }
    }

    return result;
  }

  //---------------------------------------------------------------------------------------------------------------
  void Start()
  {
    BonusIncs = new int[6];
    BonusIncs[0] = 2;
    BonusIncs[1] = 5;
    BonusIncs[2] = 8;
    BonusIncs[3] = 11;
    BonusIncs[4] = 14;
    BonusIncs[5] = 16;

    KotanUnlockScore = new int[18];
    KotanUnlockScore[0] = 5;
    KotanUnlockScore[1] = 50;
    KotanUnlockScore[2] = 200;
    KotanUnlockScore[3] = 350;
    KotanUnlockScore[4] = 600;

    KotanUnlockScore[5] = 900;
    KotanUnlockScore[6] = 1200;
    KotanUnlockScore[7] = 2000;
    KotanUnlockScore[8] = 3000;
    KotanUnlockScore[9] = 5000;

    KotanUnlockScore[10] = 6000;
    KotanUnlockScore[11] = 7000;
    KotanUnlockScore[12] = 8000;
    KotanUnlockScore[13] = 10000;
    KotanUnlockScore[14] = 12000;

    KotanUnlockScore[15] = 14000;
    KotanUnlockScore[16] = 18000;
    KotanUnlockScore[17] = 20000;




    Names = new string[18];
    this.Quotes = new string[18][];
    Names[0] = "Kittycorn";
    this.Quotes[0] = new string[3];
    this.Quotes[0][0] = "It's.. Magic!";
    this.Quotes[0][1] = "Fly with us, human!";
    this.Quotes[0][2] = "Follow the friendship!";
    Names[1] = "Tobby";
    this.Quotes[1] = new string[4];
    this.Quotes[1][0] = "Woof-woof.. err. Meow!";
    this.Quotes[1][1] = "How do ye?";
    this.Quotes[1][2] = "Howdy!";
    this.Quotes[1][3] = "Hey Y'all Doon?";
    
    Names[2] = "Ludwig";
    this.Quotes[2] = new string[4];
    this.Quotes[2][0] = "Sanguine Sancta";
    this.Quotes[2][1] = "Sanguis sanctum casta era";
    this.Quotes[2][2] = "Ama va nihilo";
    this.Quotes[2][3] = "E sanguine maledictus";

    Names[3] = "Spaicy";
    this.Quotes[3] = new string[4];
    this.Quotes[3][0] = "We weave out mesh: a blanket to warm the soul";
    this.Quotes[3][1] = "Now We Are Alone";
    this.Quotes[3][2] = "Follow path of Transcend.";
    this.Quotes[3][3] = "Transcendence is our goal.";

    Names[4] = "Curio";
    this.Quotes[4] = new string[3];
    this.Quotes[4][0] = "Interesting!";
    this.Quotes[4][1] = "Fascinating!";
    this.Quotes[4][2] = "I am interested! Please continue.";

    Names[5] = "Malibu";
    this.Quotes[5] = new string[4];
    this.Quotes[5][0] = "Sun and the beach, all I need.";
    this.Quotes[5][1] = "Surf and sand, friend.";
    this.Quotes[5][2] = "Home is where the salt air is.";
    this.Quotes[5][3] = "Live in the Sunshine";


    Names[6] = "Princess";
    this.Quotes[6] = new string[4];
    this.Quotes[6][0] = "Where is my masseuse?";
    this.Quotes[6][1] = "Bring me chocolate!";
    this.Quotes[6][2] = "I want it NOW!";
    this.Quotes[6][3] = "Are you my new servant?";

    Names[7] = "Chocomint";
    this.Quotes[7] = new string[4];
    this.Quotes[7][0] = "Peace and chocolate..";
    this.Quotes[7][1] = "Relax, friend..";
    this.Quotes[7][2] = "Let's catnap..";
    this.Quotes[7][3] = "Chill, kitty.. Let's relax.";
    

    Names[8] = "Tigre";
    this.Quotes[8] = new string[5];
    this.Quotes[8][0] = "Rrrrr.";
    this.Quotes[8][1] = "Rrrrr. meow.";
    this.Quotes[8][2] = "Meow. Purrrrr.";
    this.Quotes[8][3] = "Purrrrr.";
    this.Quotes[8][4] = "I am tiger, you know.";


    Names[9] = "Catman";
    this.Quotes[9] = new string[3];
    this.Quotes[9][0] = "For the Catkind!";
    this.Quotes[9][1] = "I have no weakness.";
    this.Quotes[9][2] = "My battle never ends!";

    Names[10] = "Grumpy";
    this.Quotes[10] = new string[4];
    this.Quotes[10][0] = "So what?";
    this.Quotes[10][1] = "Yeah, yeah.";
    this.Quotes[10][2] = "Oh. That's you..";
    this.Quotes[10][3] = "Just.. ah, never mind.";

    Names[11] = "He-He";
    this.Quotes[11] = new string[4];
    this.Quotes[11][0] = "Is it party time?";
    this.Quotes[11][1] = "Let's party!";
    this.Quotes[11][2] = "Wanna dance?";
    this.Quotes[11][3] = "Trust me, you can dance!";

    Names[12] = "Sir Watkyn";
    this.Quotes[12] = new string[3];
    this.Quotes[12][0] = "Where is my milk with tea?";
    this.Quotes[12][1] = "I am, indeed, a cat.";
    this.Quotes[12][2] = "Kindly do not address me in that familiar way.";
        

    Names[13] = "Malitia";
    this.Quotes[13] = new string[3];
    this.Quotes[13][0] = "Muhahahaha.";
    this.Quotes[13][1] = "I am The Evil Kitty!";
    this.Quotes[13][2] = "Tremble, human!";

    Names[14] = "Mysterio";
    this.Quotes[14] = new string[4];
    this.Quotes[14][0] = "We know the secrets";
    this.Quotes[14][1] = "We know..";
    this.Quotes[14][2] = "We will see..";
    this.Quotes[14][3] = "We see all..";

    Names[15] = "Goldie";
    this.Quotes[15] = new string[5];
    this.Quotes[15][0] = "Did I tell you I like tofuru?";
    this.Quotes[15][1] = "I like tofuru!";
    this.Quotes[15][2] = "The clever cat eats cheese.";
    this.Quotes[15][3] = "Moon is made of cheese.";
    this.Quotes[15][4] = "Don't eat grilled cheese. Not worth it.";
    

    Names[16] = "Meowsimmo";
    this.Quotes[16] = new string[5];
    this.Quotes[16][0] = "To our dreams!";
    this.Quotes[16][1] = "Stay strong, friend!";
    this.Quotes[16][2] = "Nothing can stand our way!";
    this.Quotes[16][3] = "We are the Champions!";
    this.Quotes[16][4] = "We will do it!";

    Names[17] = "XX121";
    this.Quotes[17] = new string[3];
    this.Quotes[17][0] = "WOEM WOEM WOEM";
    this.Quotes[17][1] = "...";
    this.Quotes[17][2] = "HAK HAK HAK";

    int PrevAmount = 0;
    for (int i = 0; i < KotanUnlockScore.Length; i++)
    {
      if (this.KotanUnlockScore[i] <= PrevAmount)
      {
        Debug.LogError("Wrong amount in Unique Kotan Storage requirments!");
      }

      PrevAmount = this.KotanUnlockScore[i];
    }

    this.Unlocked = new int[this.KotanUnlockScore.Length];
    this.CardUnlocksShown = new bool[this.KotanUnlockScore.Length];

    for (int i = 0; i < CardUnlocksShown.Length; i++)
    {
      this.CardUnlocksShown[i] = false;
    }

    for (int i = 0; i < Unlocked.Length; i++)
    {
      this.Unlocked[i] = LockedSlot;
    }

    if (Game.Settings.IsFirstLaunch) //|| Game.IsDebug
    {
      SaveCats();
      this.SaveCardUnlocks();
    }
 

    this.LoadCats();
    this.LoadCardUnlocks();
  }

  //---------------------------------------------------------------------------------------------------------------
  private void SaveCardUnlocks()
  {
    string filePath = Path.Combine(Application.persistentDataPath, SaveFileNameCardUnlocks);
    BinaryFormatter binary = new BinaryFormatter();
    FileStream saveFile = File.Create(filePath);

    binary.Serialize(saveFile, this.CardUnlocksShown);

    saveFile.Close();
    Debug.Log("Saved file " + SaveFileNameCardUnlocks + ".");
  }

  //---------------------------------------------------------------------------------------------------------------
  private void LoadCardUnlocks()
  {
    string filePath = Path.Combine(Application.persistentDataPath, SaveFileNameCardUnlocks);

    if (!File.Exists(filePath))
    {

      SaveCardUnlocks();
      return;
    }

    BinaryFormatter binary = new BinaryFormatter();
    FileStream saveFile = File.Open(filePath, FileMode.Open);
    this.CardUnlocksShown = (bool[]) binary.Deserialize(saveFile);
    saveFile.Close();

    Debug.Log("Loaded file " + SaveFileNameCardUnlocks + ".");

  }

  //---------------------------------------------------------------------------------------------------------------
  private void SaveCats()
  {
    string filePath = Path.Combine(Application.persistentDataPath, SaveFileNameCats);
    BinaryFormatter binary = new BinaryFormatter();
    FileStream saveFile = File.Create(filePath);

    binary.Serialize(saveFile, this.Unlocked);

    saveFile.Close();
    Debug.Log("Saved file " + SaveFileNameCats + ".");
  }

  //---------------------------------------------------------------------------------------------------------------
  private void DebugUnlockAll()
  {
    for (int i = 0; i < this.Unlocked.Length; i++)
    {
      this.Unlocked[i] = i;
    }
  }
  //---------------------------------------------------------------------------------------------------------------
  public bool HasAnythingToUnlock()
  {
    int Level = this.GetTopUnlockedKotanPosition();

    if (Level == UniqueKotanStorage.NothingUnlocked)
    {
      return false; // if nothing unlocked, nothig to show.
    }

    if (this.HasLevelBonusInc(Level))
      {
        return true;
      }


    List<ACAtCardData> LevelCards = GameData.CardStorage.GetDataForMagicCatLevel(Level);
    if (LevelCards == null)
    {
      return false;
    }

    if (LevelCards.Count == 0)
    {
      return false;
    }

    return true;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool HasLevelBonusInc(int Level)
  {
    for (int i = 0; i < BonusIncs.Length; i++)
    {
      if (Level == this.BonusIncs[i])
      {
        return true;
      }
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool AtLeastOneUnlocked()
  {
    if (this.Unlocked[0] != LockedSlot)
    {
      return true;
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  private void LoadCats()
  {
    string filePath = Path.Combine(Application.persistentDataPath, SaveFileNameCats);

    if (!File.Exists(filePath))
    {

      SaveCats();
      return;
    }

    BinaryFormatter binary = new BinaryFormatter();
    FileStream saveFile = File.Open(filePath, FileMode.Open);
    this.Unlocked = (int[]) binary.Deserialize(saveFile);
    saveFile.Close();

    Debug.Log("Loaded file " + SaveFileNameCats + ".");

  }


  //---------------------------------------------------------------------------------------------------------------
  public int FirstKotanCost()
  {
    return this.KotanUnlockScore[0];
  }


  private KotansStorage KotansStorage()
  {
    if (Game.StateManager.CurrentState == GameState.Action)
    {
      return Game.ActionRoot.KotansStorage;
    }

    if (Game.StateManager.CurrentState == GameState.Menu)
    {
      return Game.MenuRoot.KotansStorage;
    }
    return null;
  }
  //---------------------------------------------------------------------------------------------------------------
  public KotanData GetKotanData(int KotanPosition)
  {
    if (!CheckCatNum(KotanPosition))
    {
      return null;
    }

    if (this.Unlocked[KotanPosition-1] == LockedSlot)
    {
      return null;
    }

    return KotansStorage().GetKotan(this.Unlocked[KotanPosition-1] - 1); 
  }
  //---------------------------------------------------------------------------------------------------------------
  private bool isThisKotanUnlockedAlready(int checkNum)
  {
    for (int i = 0; i < this.Unlocked.Length; i++)
    {
      if (this.Unlocked[i] == checkNum+1)
      {
        return true;
      }
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public KotanData UnlockNewCat()
  {
   int NewKotan = this.NextToUnlock();
    if (NewKotan == UniqueKotanStorage.AllUnlocked)
    {
      return null;
    }

    int newKotNum = UnityEngine.Random.Range(0, this.KotanUnlockScore.Length);
    while (this.isThisKotanUnlockedAlready(newKotNum))
    {
      newKotNum = UnityEngine.Random.Range(0, this.KotanUnlockScore.Length);
    }

    this.Unlocked[NewKotan-1] = newKotNum+1;
    this.SaveCats();


    return Game.ActionRoot.KotansStorage.GetKotan(newKotNum);
  }
  //---------------------------------------------------------------------------------------------------------------
  public int MaxAmount ()
  {
    return this.KotanUnlockScore.Length;
  }

  //---------------------------------------------------------------------------------------------------------------
  public int GetTopUnlockedKotanNum()
  {
    int result = NothingUnlocked;
    for (int i = 0; i < this.Unlocked.Length; i++)
    {
      if (this.Unlocked[i] == LockedSlot)
      {

        break;
      }
      result = i;
      ;
    }

    if (result == NothingUnlocked)
    {
      return NothingUnlocked;
    }

    return Unlocked[result];

  }

  //---------------------------------------------------------------------------------------------------------------
  public void SetCardsUnlockShown()
  {
    

    if( this.WasCardsUnlockShown())
    {
      Debug.LogError("SetCardsUnlockShown was called, but nothing is unlocked for showing.");
      return; 
    }
    this.CardUnlocksShown[this.GetTopUnlockedKotanPosition()-1] = true;
    this.SaveCardUnlocks();
  }
  //---------------------------------------------------------------------------------------------------------------
  public bool WasCardsUnlockShown() // potensially a bug, if we unlock more than one cat.
  {
    int UnlockedCatPosition = this.GetTopUnlockedKotanPosition();

    if (UnlockedCatPosition == UniqueKotanStorage.NothingUnlocked)
    {
      return true; // if nothing unlocked, nothig to show.
    }



    return this.CardUnlocksShown[UnlockedCatPosition-1];
  }

  //---------------------------------------------------------------------------------------------------------------
  public int GetTopUnlockedKotanPosition()
  { int result = NothingUnlocked;
    for (int i = 0; i < this.Unlocked.Length; i++)
    {
      if (this.Unlocked[i] != LockedSlot)
      {
        result++;
        continue;
      }

      break;
    }

    if (result != NothingUnlocked)
    {
      return result+1;
    }

    return result;

  }

  //---------------------------------------------------------------------------------------------------------------
  public bool CheckCatNum(int CatNum)
  {
    if (CatNum < 1)
    {
      return false;
    }

    if (CatNum > KotanUnlockScore.Length)
    {
      return false;
    }

    return true;

  }

  //---------------------------------------------------------------------------------------------------------------
  public int GetPosByNum(int CatNum)
  {
    for (int i = 0; i < this.MaxAmount(); i++)

    {
      if (this.Unlocked[i] == CatNum)
      {
        return i + 1;
      }

    }

    return UniqueKotanPopUp.UniqueKotanPopupData.NO_DATA_SET;
  }

  //---------------------------------------------------------------------------------------------------------------
  public int GetNumByPos(int CatPos)
  {
    if (!this.CheckCatNum(CatPos))
    {
      return LockedSlot;
    }

    return this.Unlocked[CatPos - 1];
  }

  //---------------------------------------------------------------------------------------------------------------
  public bool isUnlocked(int CatNum)
  {
    if (!this.CheckCatNum(CatNum))
    {
      return false;
    }

    if (this.Unlocked[CatNum - 1] == LockedSlot)
    {
      return false;
    }

    return true;
  }


  //---------------------------------------------------------------------------------------------------------------
  public string GetQuoteNum(int CatNum)
  {
    if (!this.CheckCatNum(CatNum))
    {
      return "Meow! It's an error!";
    }

    return this.Quotes[CatNum - 1][UnityEngine.Random.Range(0, this.Quotes[CatNum - 1].Length)];
  }

    //---------------------------------------------------------------------------------------------------------------
    public string GetNameNum(int CatNum)
  {
    if (!this.CheckCatNum(CatNum))
    {
      return "ERROR";
    }

    return this.Names[CatNum - 1];
  }
   //---------------------------------------------------------------------------------------------------------------
    public int Cost(int CatNum)
  {
    if (!this.CheckCatNum(CatNum))
    {
      return int.MaxValue;
    }

    return this.KotanUnlockScore[CatNum-1];
  }


}
