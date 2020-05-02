// don't add custom index to enum. Tutorial Manager will serialize the total amount of stages.
public enum TutorialId
{
  // reccurent stages
 // [Description("Tutorial1_Greetings")]
//  Stage1_IsItEmpty,
  [Description("Tutorial2")]
  Stage2_PressStart,

  [Description("Tutorial3")]
  Stage3_ActionStart0,

 // [Description("Tutorial4")]
//  Stage4_ActionStart1,

  [Description("Tutorial5")]
  Stage5_ActionStart2,

  [Description("Tutorial6")]
  Stage6_ActionStart3,

  [Description("Tutorial7")]
  Stage7_ActionStart4,


  // first Cat
  [Description("Tutorial8")]
  Stage8_FirstCatSummoned,

  [Description("Tutorial9")]
  Stage9_FirstUniqueCatUnlocked,

  [Description("Tutorial10")]
  Stage10_TalkToMagicCat,

  //Independent stages
  [Description("Tutorial101_SameCards")]
  Tutorial101_SameCards,

  [Description("Tutorial102_SideMods")]
  Tutorial102_SideMods,

  [Description("Tutorial103_JokerMods")]
  Tutorial103_JokerMods,

  [Description("Tutorial104_Black_Card_0")]
  Tutorial104_BlackCard_0,

  [Description("Tutorial104_Black_Card_1")]
  Tutorial104_BlackCard_1,

  [Description("Tutorial105_BuyGame")]
  Tutorial105_BuyGame,
}
