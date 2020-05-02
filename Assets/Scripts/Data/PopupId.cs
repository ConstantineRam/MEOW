using System;

public enum PopupId
{
  NoPopUp = -1,

  first,
  [Description("Prefabs/UI/Popups/YesNoPopup")]
  YesNo,
  [Description("Prefabs/UI/Popups/OkPopup")]
  Ok,
  [Description("Prefabs/UI/Popups/PausePopup")]
  GameMenu,
  [Description("Prefabs/UI/Popups/GameOver")]
  GameOver,
  [Description("Prefabs/UI/Popups/GameWon")]
  GameWon,
  [Description("Prefabs/UI/Popups/Tutorial PopUp")]
  Tutorial,
  [Description("Prefabs/UI/Popups/SaveFileMissing")]
  SaveFileMissing,


  // MEOW

  [Description("Prefabs/UI/Popups/UnlockCardsPopUp")]
  UnlockCardsPopUp,

  [Description("Prefabs/UI/Popups/MenuDeck")]
  MenuDeck,

  [Description("Prefabs/UI/Popups/CatBook")]
  CatBook,

  [Description("Prefabs/UI/Popups/HintScore")]
  HintScore,

  [Description("Prefabs/UI/Popups/BuyGame")]
  BuyGame,

  [Description("Prefabs/UI/Popups/ThankYou")]
  ThankYou,

  [Description("Prefabs/UI/Popups/PurchaseFailed")]
  PurchaiseFailed,

  [Description("Prefabs/UI/Popups/ShareKotan")]
  CatCard,

  [Description("Prefabs/UI/Popups/UniqueKotans")]
  UniqueKotans,
  [Description("Prefabs/UI/Popups/CustomKotanCollectionPopUp")]
  CustomKotanCollectionPopUp,

  [Description("Prefabs/UI/Popups/SaveKotans")]
  SaveKotans,

  [Description("Prefabs/UI/Popups/HintHelpPopUp")]
  HintHelpPopUp,

  [Description("Prefabs/UI/Popups/NextTurnHintPopUp")]
  NextTurnHintPopUp,

  [Description("Prefabs/UI/Popups/HintHandScore")]
  HintHandScore,

  [Description("Prefabs/UI/Popups/Letter")]
  LetterFromMeow,

  // MEOW Tutorial
  [Description("Prefabs/UI/Popups/Tutorial/Tutorial1_Greetings")]
  Tutorial1_Greetings,
  [Description("Prefabs/UI/Popups/Tutorial/Tutorial2")]
  Tutorial2,

  [Description("Prefabs/UI/Popups/Tutorial/Tutorial3")]
  Tutorial3,
  [Description("Prefabs/UI/Popups/Tutorial/Tutorial4")]
  Tutorial4,
  [Description("Prefabs/UI/Popups/Tutorial/Tutorial5")]
  Tutorial5,
  [Description("Prefabs/UI/Popups/Tutorial/Tutorial6")]
  Tutorial6,

  [Description("Prefabs/UI/Popups/Tutorial/Tutorial7")]
  Tutorial7,

  [Description("Prefabs/UI/Popups/Tutorial/Tutorial8")]
  Tutorial8,

  [Description("Prefabs/UI/Popups/Tutorial/Tutorial9")]
  Tutorial9,

  [Description("Prefabs/UI/Popups/Tutorial/Tutorial10")]
  Tutorial10,

  [Description("Prefabs/UI/Popups/Tutorial/Tutorial101_SameCards")]
  Tutorial101_SameCards,

  [Description("Prefabs/UI/Popups/Tutorial/Tutorial102_SideMods")]
  Tutorial102_SideMods,

  [Description("Prefabs/UI/Popups/Tutorial/Tutorial103_JokerMods")]
  Tutorial103_JokerMods,

  [Description("Prefabs/UI/Popups/Tutorial/Tutorial104_Black_Card_0")]
  Tutorial104_Black_Card_0,

  [Description("Prefabs/UI/Popups/Tutorial/Tutorial104_Black_Card_1")]
  Tutorial104_Black_Card_1,

  [Description("Prefabs/UI/Popups/Tutorial/Tutorial105_BuyGame")]
  Tutorial105_BuyGame,

  lastPlusOne,
}



